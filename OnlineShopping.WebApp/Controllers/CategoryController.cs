
﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OnlineShopping.Utility;
using OnlineShopping.WebApp.Models;
using OnlineShopping.WebApp.Services;
using OnlineShopping.WebApp.Services.IServices;

namespace OnlineShopping.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        //[Authorize]

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
        public async Task<IActionResult> IndexCategory(string SearchText = "", int pg = 1, int pageSize = 5)
        {
            List<CategoryDto> list = new();

            var response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(response.Result));

                if (pg < 1) pg = 1;




                if (SearchText != "" && SearchText != null)
                {
                    List<CategoryDto> categoryList = new List<CategoryDto>();
                    foreach (var category in list)
                    {
                        if (category.CategoryName.ToLower().Contains(SearchText.ToLower()))
                            categoryList.Add(category);
                    }
                    list = categoryList;
                }
                int recsCount = list.Count();



                int recSkip = (pg - 1) * pageSize;
                var data = list.Skip(recSkip).Take(pageSize).ToList();



                Pager SearchPager = new Pager(recsCount, pg, pageSize) { Action = "IndexCategory", Controller = "Category", SearchText = SearchText };
                ViewBag.SearchPager = SearchPager;



                this.ViewBag.PageSizes = GetPageSizes(pageSize);



                return View(data.ToList());
            }
            return View(list);
        }
        //public async Task<IActionResult> IndexCategory()
        //{
        //    List<CategoryDto> list = new();

        //    var response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        list = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(response.Result));
        //    }
        //    return View(list);
        //}
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory(CategoryDto model)
        {
            List<CategoryDto> list = new();

            var response = await _categoryService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Category created successfully";
                return RedirectToAction(nameof(IndexCategory));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int categoryId)
        {
            var response = await _categoryService.GetAsync<APIResponse>(categoryId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                CategoryDto model = JsonConvert.DeserializeObject<CategoryDto>(Convert.ToString((response.Result)));
                return View(_mapper.Map<CategoryDto>(model));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(CategoryDto model)
        {
            var response = await _categoryService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Category updated successfully";

                return RedirectToAction(nameof(IndexCategory));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int categoryId)

        {
            var response = await _categoryService.GetAsync<APIResponse>(categoryId, HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSuccess)

            {
                CategoryDto model = JsonConvert.DeserializeObject<CategoryDto>(Convert.ToString((response.Result)));

                return View(model);

            }
            return NotFound();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(CategoryDto model)

        {

            var response = await _categoryService.DeleteAsync<APIResponse>(model.CategoryId, HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSuccess)

            {

                TempData["success"] = "Category deleted successfully";
                return RedirectToAction(nameof(IndexCategory));
            }
            TempData["error"] = "Error encountered.";
            return View(model);

        }
    }
}