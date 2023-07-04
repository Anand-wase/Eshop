using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopping.WebApp.Models;
namespace OnlineShopping.WebApp.Models.VM
{
    public class ProductUpdateVM
    {
        public ProductUpdateVM()
        {
            Product = new UpdateProductDto();
        }
        public UpdateProductDto Product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? CategoryList { get; set; } 
        

    }
}
