using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopping.WebApp.Models;
namespace OnlineShopping.WebApp.Models.VM
{
    public class ProductCreateVM
    {
        public ProductCreateVM()
        {
            Product = new CreateProductDto();
        }
        public CreateProductDto Product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? CategoryList { get; set; } 
        

    }
}
