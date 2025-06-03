using Microsoft.AspNetCore.Mvc;
using SpongPopBakery.DTOs;
using SpongPopBakery.Models;
using SpongPopBakery.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace SpongPopBakery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly BakeryDbContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<ContactController> _logger;

        public ContactController(
            BakeryDbContext context,
            IConfiguration config,
            ILogger<ContactController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Submits a new contact message
        /// </summary>
        /// <param name="contactDto">Contact message data</param>
        /// <returns>Success or error response</returns>
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ContactMessageDto contactDto)
        {
            try
            {
                // Validate model
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid form data",
                        errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                // Additional validation
                var validationResult = ValidateContactMessage(contactDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Validation failed",
                        errors = validationResult.Errors
                    });
                }

                // Create and save message
                var contactMessage = new ContactMessage
                {
                    Name = contactDto.Name.Trim(),
                    Email = contactDto.Email.Trim().ToLower(),
                    Subject = contactDto.Subject?.Trim(),
                    Message = contactDto.Message.Trim(),
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };

                _context.ContactMessages.Add(contactMessage);
                await _context.SaveChangesAsync();

                // Send email notification if enabled
                if (_config.GetValue<bool>("ContactSettings:EnableEmailNotifications"))
                {
                    await SendEmailNotification(contactMessage);
                }

                _logger.LogInformation("New contact message received from {Name} ({Email})",
                    contactMessage.Name, contactMessage.Email);

                return Ok(new
                {
                    success = true,
                    message = "Your message has been sent successfully!",
                    messageId = contactMessage.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing contact message");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing your message. Please try again later."
                });
            }
        }

        /// <summary>
        /// Gets all contact messages (admin only)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _context.ContactMessages
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            return Ok(messages);
        }

        /// <summary>
        /// Marks a message as read (admin only)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/mark-as-read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            message.IsRead = true;
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        /// <summary>
        /// Deletes a contact message (admin only)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.ContactMessages.Remove(message);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        #region Helper Methods

        private ValidationResult ValidateContactMessage(ContactMessageDto contactDto)
        {
            var result = new ValidationResult { IsValid = true };

            // Name validation
            if (string.IsNullOrWhiteSpace(contactDto.Name))
            {
                result.Errors.Add("Name is required");
                result.IsValid = false;
            }
            else if (contactDto.Name.Length > 100)
            {
                result.Errors.Add("Name cannot exceed 100 characters");
                result.IsValid = false;
            }

            // Email validation
            if (string.IsNullOrWhiteSpace(contactDto.Email))
            {
                result.Errors.Add("Email is required");
                result.IsValid = false;
            }
            else if (!IsValidEmail(contactDto.Email))
            {
                result.Errors.Add("Invalid email format");
                result.IsValid = false;
            }
            else if (contactDto.Email.Length > 100)
            {
                result.Errors.Add("Email cannot exceed 100 characters");
                result.IsValid = false;
            }

            // Subject validation
            if (!string.IsNullOrWhiteSpace(contactDto.Subject) && contactDto.Subject.Length > 200)
            {
                result.Errors.Add("Subject cannot exceed 200 characters");
                result.IsValid = false;
            }

            // Message validation
            if (string.IsNullOrWhiteSpace(contactDto.Message))
            {
                result.Errors.Add("Message is required");
                result.IsValid = false;
            }
            else if (contactDto.Message.Length > 2000)
            {
                result.Errors.Add("Message cannot exceed 2000 characters");
                result.IsValid = false;
            }

            return result;
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Simple regex for basic email validation
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private async Task SendEmailNotification(ContactMessage message)
        {
            var adminEmail = _config["ContactSettings:AdminEmail"];
            if (string.IsNullOrEmpty(adminEmail))
            {
                _logger.LogWarning("Admin email not configured - skipping email notification");
                return;
            }

            try
            {
                using (var smtpClient = new SmtpClient(_config["SmtpSettings:Host"]))
                {
                    smtpClient.Port = _config.GetValue<int>("SmtpSettings:Port");
                    smtpClient.Credentials = new NetworkCredential(
                        _config["SmtpSettings:Username"],
                        _config["SmtpSettings:Password"]);
                    smtpClient.EnableSsl = _config.GetValue<bool>("SmtpSettings:EnableSsl");

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(
                            _config["SmtpSettings:FromEmail"],
                            _config["SmtpSettings:FromName"]),
                        Subject = $"New Contact Message: {message.Subject ?? "No Subject"}",
                        Body = $@"
                            <h2>New Contact Message Received</h2>
                            <p><strong>From:</strong> {message.Name} ({message.Email})</p>
                            <p><strong>Subject:</strong> {message.Subject ?? "None"}</p>
                            <p><strong>Message:</strong></p>
                            <div style='white-space: pre-line;'>{message.Message}</div>
                            <p><small>Message ID: {message.Id}</small></p>
                        ",
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(adminEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                    _logger.LogInformation("Email notification sent for message ID {MessageId}", message.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email notification for message ID {MessageId}", message.Id);
            }
        }

        private class ValidationResult
        {
            public bool IsValid { get; set; }
            public List<string> Errors { get; set; } = new List<string>();
        }

        #endregion
    }
}