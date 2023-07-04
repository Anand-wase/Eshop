using OnlineShopping.WebApp.Models.Dto;

namespace OnlineShopping.WebApp.Models.Dto
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
    }
}
