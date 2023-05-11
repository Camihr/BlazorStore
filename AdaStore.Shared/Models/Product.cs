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
        [Range(0, 100000000, ErrorMessage = "La cantidad debe ser entre 0 y 100000000")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Range(0, 100000000, ErrorMessage = "El precio debe ser entre 0 y 100000000")]
        public double Price { get; set; }

        public string ImageUrl { get; set; }
    }
}
