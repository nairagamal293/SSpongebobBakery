﻿
namespace SpongPopBakery.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; } = "User"; // "Admin" or "User"
        public DateTime CreatedAt { get; internal set; }
    }
}
