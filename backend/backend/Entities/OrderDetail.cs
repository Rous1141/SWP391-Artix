using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ArtWorkID { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public double Price { get; set; }
        public Order Order { get; set; }
        public string? PurchaseConfirmationImage { get; set; }
        [NotMapped]
        public string email { get; set; }

    }
}
