using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OnlineShopping.Utility;
using OnlineShopping.WebApp.Models;
using OnlineShopping.WebApp.Models.Dto;
using OnlineShopping.WebApp.Models.VM;
using OnlineShopping.WebApp.Services;
using OnlineShopping.WebApp.Services.IServices;
using System.Diagnostics;

namespace OnlineShopping.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly IUserManagerService _userManagerService;

        private readonly ICartService _cartService;
        public HomeController(IProductService productService, IMapper mapper, ICategoryService categoryService, ICartService cartService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _mapper = mapper;
            _cartService = cartService;

        }

        private List<SelectListItem> GetPageSizes(int selectedPageSize = 4)
        {
            var pagesSizes = new List<SelectListItem>();



            if (selectedPageSize == 4)
                pagesSizes.Add(new SelectListItem("4", "4", true));
            else
                pagesSizes.Add(new SelectListItem("4", "4"));



            for (int lp = 8; lp <= 104; lp += 4)
            {
                if (lp == selectedPageSize)
                { pagesSizes.Add(new SelectListItem(lp.ToString(), lp.ToString(), true)); }
                else
                    pagesSizes.Add(new SelectListItem(lp.ToString(), lp.ToString()));
            }



            return pagesSizes;
        }
        public async Task<IActionResult> Index(string SearchText = "", int pg = 1, int pageSize = 4)
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



                Pager SearchPager = new Pager(recsCount, pg, pageSize) { Action = "Index", Controller = "Home", SearchText = SearchText };
                ViewBag.SearchPager = SearchPager;



                this.ViewBag.PageSizes = GetPageSizes(pageSize);



                return View(data.ToList());
            }
            return View(list);
        }
       




        //[Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductdetailsVM productVM = new();

            var response = await _productService.GetAsync<APIResponse>(productId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString((response.Result)));
                productVM.Product = _mapper.Map<UpdateProductDto>(model);
            }
            return View(productVM);
        }
        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductdetailsVM productVM)
        {
            CartDto cartDto = new CartDto()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = HttpContext.Session.GetString(SD.SessionUserId)
                }
            };

            CartDetailsDto cartDetails = new CartDetailsDto()
            {
                Count = productVM.Count,
                ProductId = productVM.Product.ProductId,
            };

           if(cartDetails.Count <= 0)
            {
                TempData["error"] = "Error encountered.";
                return RedirectToAction("Index");

            }



            List<CartDetailsDto> cartDetailsDtos = new() { cartDetails };
                cartDto.CartDetails = cartDetailsDtos;

                APIResponse? response = await _cartService.CreateAsync<APIResponse>(cartDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Item has been added to the Shopping Cart";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                   TempData["error"] = "Error encountered.";

                }


            return View(productVM);

        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Profile()
        {
            var list = new ProfileDto()
            {
                Name = HttpContext.Session.GetString(SD.SessionEmail),
                Email = HttpContext.Session.GetString(SD.SessionUserMail),
                PhoneNumber = HttpContext.Session.GetString(SD.SessionUserPhoneNumber),
                Role = HttpContext.Session.GetString(SD.SessionUserRole),
                ImageUrl = HttpContext.Session.GetString(SD.SessionUserImage),
            };
            return View(list);


        }
    }
}