using System.Text.Json.Serialization;

namespace nous_backend_upload.Models
{
    public class UploadFile
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? File { get; set; }

    }
}
