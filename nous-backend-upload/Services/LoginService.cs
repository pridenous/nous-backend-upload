using nous_backend_upload.DTO;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using nous_backend_upload.Models;

namespace nous_backend_upload.Services
{
    public class LoginService(IConfiguration _config):ILoginService
    {

        public async Task<string> ProcessLogin(string token)
        {
            return "";
        }

        public async Task<LoginResp> ProcessLoginByGoogle(string token)
        {
            var loginResp = new LoginResp();
            try
            {
                var httpClient = new HttpClient();

                var googleApi = $"https://oauth2.googleapis.com/tokeninfo?id_token={token}";

                var response = await httpClient.GetAsync(googleApi);

                if (!response.IsSuccessStatusCode)
                {
                    loginResp.statuscode = Convert.ToInt16(HttpStatusCode.Unauthorized);
                    loginResp.message = "Invalid Google token";
                    return loginResp;
                }

                var payload = await response.Content.ReadFromJsonAsync<GoogleResp>();

                // Cek email valid, dan terverifikasi
                if (payload == null || payload.EmailVerified != "true")
                {
                    loginResp.statuscode = Convert.ToInt16(HttpStatusCode.Unauthorized);
                    loginResp.message = "Invalid Google token";
                    return loginResp;
                }

                //var user = FindOrCreateUser(payload);

                // Buat JWT token
                //var jwtToken = GenerateJwt(user);

                loginResp.statuscode = Convert.ToInt16(HttpStatusCode.OK);
                loginResp.message = "Success";
                loginResp.token = "";
                return loginResp;
            }
            catch (Exception ex)
            {
                loginResp.statuscode = Convert.ToInt16(HttpStatusCode.InternalServerError);
                loginResp.message = ex.Message;
                return loginResp;
            }
        }

        private string GenerateJwt(AppUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.email),
                new Claim("uid", user.id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
