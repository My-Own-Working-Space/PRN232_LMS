using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.API.Models.Requests
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "RefreshToken is required")]
        public string RefreshToken { get; set; }
    }
}
