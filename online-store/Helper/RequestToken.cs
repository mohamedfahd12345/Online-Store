using System.ComponentModel.DataAnnotations;

namespace online_store.Helper
{
    public class RequestToken
    {
        [Required]
        public string jwtToken { get; set; }
        [Required]
        public string refreshToken { get; set; }
    }
}
