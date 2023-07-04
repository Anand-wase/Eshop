using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OnlineShopping.Utility;
using OnlineShopping.WebApp.Models;
using OnlineShopping.WebApp.Models.VM;
using OnlineShopping.WebApp.Services;
using OnlineShopping.WebApp.Services.IServices;
using System.Collections.Generic;
using System.Data;

namespace OnlineShopping.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, IMapper mapper, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _mapper = mapper;
        }

        private List<SelectListItem> GetPageSizes(int selectedPageSize = 5)
        {
            var pagesSizes = new List<SelectListItem>();



            if (selectedPageSize == 5)
                pagesSizes.Add(new SelectListItem("5", "5", true));
            else
                pagesSizes.Add(new SelectListItem("5", "5"));



            for (int lp = 10; lp <= 100; lp += 10)
            {
                if (lp == selectedPageSize)
                { pagesSizes.Add(new SelectListItem(lp.ToString(), lp.ToString(), true)); }
                else
                    pagesSizes.Add(new SelectListItem(lp.ToString(), lp.ToString()));
            }



            return pagesSizes;
        }
        public async Task<IActionResult> IndexProduct(string SearchText = "", int pg = 1, int pageSize = 5)
        {
            List<ProductDto> list = new();
            var response = await _productService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
                if (pg < 1) pg = 1;




                if (SearchText != "" && SearchText != null)
                {
                    List<ProductDto> productList = new List<ProductDto>();
                    foreach (var product in list)
                    {
                        if (product.Category.CategoryName.ToLower().Contains(SearchText.ToLower()) || product.ProductName.ToLower().Contains(SearchText.ToLower()))
                            productList.Add(product);
                    }
                    list = productList;
                }
                int recsCount = list.Count();



                int recSkip = (pg - 1) * pageSize;
                var data = list.Skip(recSkip).Take(pageSize).ToList();



                Pager SearchPager = new Pager(recsCount, pg, pageSize) { Action = "IndexProduct", Controller = "Product", SearchText = SearchText };
                ViewBag.SearchPager = SearchPager;



                this.ViewBag.PageSizes = GetPageSizes(pageSize);



                return View(data.ToList());
            }
            return View(list);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct()
        {
            ProductCreateVM productVM = new();
            var response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken)) ;
            if (response != null && response.IsSuccess)
            {
                   
                    productVM.CategoryList = JsonConvert.DeserializeObject<List<CategoryDto>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.CategoryName,
                        Value = i.CategoryId.ToString()
                    }); ;
            }
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct(ProductCreateVM model)
        {

            if (model.Product.Price <= 0 )
            {
                TempData["error"] = "Price should be greater than zero.";
                return RedirectToAction(nameof(CreateProduct));

            }
            if (model.Product.AvailableQuantity < 0)
            {
                TempData["error"] = "AvailableQuantity should not be negative.";
                return RedirectToAction(nameof(CreateProduct));

            }

            var response = await _productService.CreateAsync<APIResponse>(model.Product, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                
                TempData["success"] = "Product created successfully";
                return RedirectToAction(nameof(IndexProduct));
            }
            else
            {
                if (response.ErrorMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                }
            }
            var resp = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (resp != null && resp.IsSuccess)
            {
                model.CategoryList = JsonConvert.DeserializeObject<List<CategoryDto>>
                    (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                    {
                        Text = i.CategoryName,
                        Value = i.CategoryId.ToString()
                    }); ;
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int productId)
        {
            
            ProductUpdateVM productVM = new();
            var response = await _productService.GetAsync<APIResponse>(productId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString((response.Result)));
                productVM.Product = _mapper.Map<UpdateProductDto>(model);
            }
            response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                productVM.CategoryList = JsonConvert.DeserializeObject<List<CategoryDto>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.CategoryName,
                        Value = i.CategoryId.ToString()
                    }); ;
                return View(productVM);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(ProductUpdateVM model)
        {
            if (model.Product.Price <= 0)
            {
                TempData["error"] = "Price should be greater than zero.";
                return RedirectToAction(nameof(CreateProduct));

            }
            if (model.Product.AvailableQuantity < 0)
            {
                TempData["error"] = "AvailableQuantity should not be negative.";
                return RedirectToAction(nameof(CreateProduct));

            }
            if (ModelState.IsValid)
            {
                var response = await _productService.UpdateAsync<APIResponse>(model.Product,HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction(nameof(IndexProduct));
                }
                else if(response.ErrorMessages!=null)
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            var resp = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (resp != null && resp.IsSuccess)
            {
                model.CategoryList = JsonConvert.DeserializeObject<List<CategoryDto>>
                    (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                    {
                        Text = i.CategoryName,
                        Value = i.CategoryId.ToString()
                    }); ;
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int productId)

        {
            ProductDeleteVM productVM = new();
            var response = await _productService.GetAsync<APIResponse>(productId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString((response.Result)));
                productVM.Product = model;
            }
            response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                productVM.CategoryList = JsonConvert.DeserializeObject<List<CategoryDto>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.CategoryName,
                        Value = i.CategoryId.ToString()
                    }); ;
                return View(productVM);
            }
            return NotFound();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(ProductDeleteVM model)
        {
            
            var response = await _productService.DeleteAsync<APIResponse>(model.Product.ProductId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product delete successfully";
                return RedirectToAction(nameof(IndexProduct));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
    }
}
