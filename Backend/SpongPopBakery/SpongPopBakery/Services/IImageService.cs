namespace SpongPopBakery.Services
{
    public interface IImageService
    {
        Task<string> SaveImage(IFormFile imageFile, string subDirectory);
        void DeleteImage(string imagePath);
        string GetImageUrl(string imagePath);
    }
}
