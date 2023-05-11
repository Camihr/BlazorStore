using System.ComponentModel.DataAnnotations;

namespace AdaStore.Shared.DTOs
{
    public class LoginCredentials
    {
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
    }
}
