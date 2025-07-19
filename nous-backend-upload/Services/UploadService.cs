using nous_backend_upload.Core;
using nous_backend_upload.DTO;
using nous_backend_upload.Models;
using System.IO;
using System.Linq;

namespace nous_backend_upload.Services
{
    public class UploadService:IUploadService
    {
        private readonly IDatabaseService _db;

        public UploadService(IDatabaseService db)
        {
            _db = db;
        }
        public async Task<string?> GetPathFileFromDb(string id)
        {
            string sql = "select path from uploaded_file where id = @id;";
            return await _db.ExecuteScalarAsync<string>(sql, new { id = Convert.ToInt64(id) });
        }

        public async Task<List<UploadFile>> GetListFiles()
        {
            string sql = "select id,filename,description,createddate from uploaded_file order by 1 desc";
            return await _db.ExecuteTableAsync<UploadFile>(sql);
        }

        public async Task<int> UploadFiles(RequestUpload request)
        {
            var path = await UploadToLocal(request.filename, request.content);
            string sql = "insert into uploaded_file (filename, description, createddate, path) values (@filename, @description, now(), @path)";
            return await _db.ExecuteNonQueryAsync(sql, new { filename = request.filename, description = request.description, path = path });
        }

        public async Task<int> EditFiles(RequestEdit request)
        {
            var pathOldFile = await GetPathFileFromDb(request.id);
            DeleteLocalFile(pathOldFile);

            var newPath = await UploadToLocal(request.filename, request.content);
            string sql = "update uploaded_file set filename = @filename, description = @description, path = @path where id = @id";
            var recordCount = await _db.ExecuteNonQueryAsync(sql, new { id = Convert.ToInt64(request.id),filename = request.filename, description = request.description, path = newPath });
            return recordCount;
        }

        public async Task<int> DeleteFiles(string id)
        {
            var path = await GetPathFileFromDb(id);
            string sql = "delete from uploaded_file where id = @id";
            var recordCount = await _db.ExecuteNonQueryAsync(sql, new { id = Convert.ToInt64(id) });
            DeleteLocalFile(path);
            return recordCount;
        }

        private async Task<string> UploadToLocal(string filename, string content)
        {
            var ext = Path.GetExtension(filename).ToLowerInvariant();

            var uploadDir = @"D:\uploads";
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var path = Path.Combine(uploadDir, fileName);

            try
            {
                byte[] fileBytes = Convert.FromBase64String(content);
                await System.IO.File.WriteAllBytesAsync(path, fileBytes);
                return path;
            }
            catch (FormatException)
            {
                throw;
            }
        }

        private void DeleteLocalFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            else
            {
                // Optional: log atau lempar exception kalau file tidak ditemukan
                Console.WriteLine("File not found: " + path);
                // throw new FileNotFoundException("File not found", path);
            }
        }
    }
}
