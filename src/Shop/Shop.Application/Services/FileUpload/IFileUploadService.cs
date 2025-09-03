namespace Shop.Application.Services.FileUpload
{
    public interface IFileUploadService
    {
        string SaveImage(string base64, string folderName, int entityId);
        void RemoveImage(string relativePath);
    }
}
