using nous_backend_upload.DTO;

namespace nous_backend_upload.Services
{
    public interface ILoginService
    {
        Task<string> ProcessLogin(string token);
        Task<LoginResp> ProcessLoginByGoogle(string token);
    }
}
