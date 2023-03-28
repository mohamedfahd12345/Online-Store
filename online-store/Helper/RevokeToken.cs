using System.ComponentModel.DataAnnotations;

namespace online_store.Helper
{
    public class RevokeToken
    {
        [Required]
        public string? Token { get; set; }
    }
}
