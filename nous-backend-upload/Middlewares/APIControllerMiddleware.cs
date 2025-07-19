using System.Text.Json;

namespace nous_backend_upload.Middlewares
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Backup original response body
            var originalBodyStream = context.Response.Body;

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var bodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            object? finalBody = null;
            var contentType = context.Response.ContentType;

            if (!string.IsNullOrWhiteSpace(bodyText) &&
                contentType != null &&
                contentType.Contains("application/json"))
            {
                try
                {
                    var parsedData = JsonSerializer.Deserialize<object>(bodyText, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    finalBody = new
                    {
                        statusCode = context.Response.StatusCode,
                        message = context.Response.StatusCode == 200 ? "Success" : "Error",
                        data = parsedData
                    };
                }
                catch
                {
                    // If not JSON, fallback
                    finalBody = new
                    {
                        statusCode = context.Response.StatusCode,
                        message = "Non-JSON Response",
                        data = bodyText
                    };
                }

                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(finalBody));
            }
            else
            {
                // Forward as is if not JSON
                context.Response.Body = originalBodyStream;
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
    }

}
