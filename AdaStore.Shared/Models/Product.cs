using System.ComponentModel.DataAnnotations;

namespace AdaStore.Shared.Models
{
    public class Product : ModelBase
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(80, ErrorMessage = "80 caracteres máximo")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(500, ErrorMessage = "500 caracteres máximo")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad mínima es 0")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad mínima es 0")]
        public double Price { get; set; }

        public string ImageUrl { get; set; }
    }
}
