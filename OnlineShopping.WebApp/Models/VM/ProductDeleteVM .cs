using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopping.WebApp.Models;
namespace OnlineShopping.WebApp.Models.VM
{
    public class ProductDeleteVM
    {
        public ProductDeleteVM()
        {
            Product = new ProductDto();
        }
        public ProductDto Product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? CategoryList { get; set; } 
        

    }
}
