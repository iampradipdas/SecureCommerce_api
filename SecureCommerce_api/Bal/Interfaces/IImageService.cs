namespace SecureCommerce_api.Bal.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file, string folder);
        void DeleteImage(string imageUrl);
    }
}
