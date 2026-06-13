
namespace EKemit.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode , string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForSatusCode(StatusCode);
        }

        private string? GetDefaultMessageForSatusCode(int? statusCode)
        {
            // 500 => Internal Server Error
            // 400 => Bad Request 
            // 401 => UnAthorized
            // 404 => Not Found

            return StatusCode switch
            {
                400 => "Bad Request",
                401 => "You are not Authorized",
                404 => "Resource Not Found",
                500 => "internal Server Error",
                _ => null //Default 
            };
        }
    }
}
