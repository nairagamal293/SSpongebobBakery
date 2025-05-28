namespace SpongPopBakery.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _uploadBasePath;

        public ImageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;

            // Set base path - use WebRootPath if available, otherwise fallback
            _uploadBasePath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            if (string.IsNullOrEmpty(_uploadBasePath))
            {
                throw new InvalidOperationException("Could not determine the upload base path");
            }
        }

        public async Task<string> SaveImage(IFormFile imageFile, string subDirectory)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("Image file is empty");
            }

            // Validate subDirectory to prevent path traversal attacks
            if (string.IsNullOrWhiteSpace(subDirectory) || subDirectory.Any(c => Path.GetInvalidPathChars().Contains(c)))
            {
                throw new ArgumentException("Invalid subdirectory name");
            }

            var uploadsFolder = Path.Combine(_uploadBasePath, "uploads", subDirectory);

            // Ensure directory exists
            Directory.CreateDirectory(uploadsFolder);

            // Sanitize filename
            var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
            var extension = Path.GetExtension(imageFile.FileName);
            var safeFileName = $"{Guid.NewGuid()}_{fileName}{extension}";

            var filePath = Path.Combine(uploadsFolder, safeFileName);

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return Path.Combine("uploads", subDirectory, safeFileName);
        }

        public void DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;

            var fullPath = Path.Combine(_env.WebRootPath, imagePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public string GetImageUrl(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return null;

            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}/{imagePath.Replace("\\", "/")}";
        }
    }
}
