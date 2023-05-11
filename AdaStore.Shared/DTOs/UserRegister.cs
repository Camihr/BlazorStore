using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdaStore.Shared.DTOs
{
    public class UserRegister
    {
        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(12, ErrorMessage = "12 caracteres máximo")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(30, ErrorMessage = "30 caracteres máximo")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$", ErrorMessage = "El formato es incorrecto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(16, ErrorMessage = "16 caracteres máximo")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(60, ErrorMessage = "60 caracteres máximo")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(20, ErrorMessage = "20 caracteres máximo")]
        public string Document { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Campo obligatorio")]
        [MinLength(6, ErrorMessage = "6 caracteres máximo")]
        public string Password { get; set; }
    }
}
