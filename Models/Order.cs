using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public  int UserId { get; set; }


    }
}
