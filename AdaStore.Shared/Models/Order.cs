using AdaStore.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdaStore.Shared.Models
{
    public class Order : ModelBase
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        public OrderStatuses Status { get; set; }
        public List<CartItem> CartItems { get; set; }

        [NotMapped]
        public double Total { get; set; }
       
        [NotMapped]
        public string UserName
        {
            get
            {
                return User != null ? User.Name : string.Empty;
            }
        }
    }
}
