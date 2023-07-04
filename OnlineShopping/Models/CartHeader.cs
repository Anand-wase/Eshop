using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopping.WebApi.Models
{
    public class CartHeader
    {

        [Key]
        public int CartHeaderId { get; set; }
        public string? UserId { get; set; }

        [NotMapped]
        public double CartTotal { get; set; }
    }
}
