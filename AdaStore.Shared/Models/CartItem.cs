using System.ComponentModel.DataAnnotations.Schema;

namespace AdaStore.Shared.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        [ForeignKey ("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey ("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [NotMapped]
        public double Total { get; set; }
    }
}
