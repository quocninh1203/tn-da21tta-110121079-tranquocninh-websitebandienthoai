using Microsoft.AspNetCore.Hosting;

namespace Shop.Application.Services.FileUpload
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _env;

        public FileUploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string SaveImage(string base64, string folderName, int entityId)
        {
            if (string.IsNullOrWhiteSpace(base64))
                throw new ArgumentException("Base64 string is null or empty", nameof(base64));

            string prefix = base64.Split(',')[0].ToLower();

            string extension;

            if (prefix.StartsWith("data:image/"))
            {
                extension = ".jpg";
            }
            else if (prefix.StartsWith("data:video/"))
            {
                extension = ".mp4";
            }
            else
            {
                extension = ".jpg"; // mặc định ảnh
            }

            var base64Data = base64.Contains(",") ? base64.Split(',')[1] : base64;
            var bytes = Convert.FromBase64String(base64Data);

            var fileName = $"{Guid.NewGuid().ToString("N").Substring(0, 8)}_{entityId}{extension}";
            var folderPath = Path.Combine(_env.WebRootPath, "uploads", folderName);

            Directory.CreateDirectory(folderPath);
            var fullPath = Path.Combine(folderPath, fileName);
            File.WriteAllBytes(fullPath, bytes);

            return $"/uploads/{folderName}/{fileName}";
        }

        public void RemoveImage(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return;

            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }
    }
}
