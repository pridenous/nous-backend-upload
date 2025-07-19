using nous_backend_upload.DTO;
using nous_backend_upload.Models;

namespace nous_backend_upload.Services
{
    public interface IUploadService
    {
        Task<string?> GetPathFileFromDb(string id);
        Task<List<UploadFile>> GetListFiles();

        Task<int> UploadFiles(RequestUpload request);
        Task<int> DeleteFiles(string id);
        Task<int> EditFiles(RequestEdit request);
    }
}
