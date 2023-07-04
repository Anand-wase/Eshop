   
﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OnlineShopping.Utility;
using OnlineShopping.WebApp.Models;
using OnlineShopping.WebApp.Models.Dto;
using OnlineShopping.WebApp.Services;
using OnlineShopping.WebApp.Services.IServices;

namespace OnlineShopping.WebApp.Controllers
{
    public class UserManagerController : Controller
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IMapper _mapper;
        public  UserManagerController(IUserManagerService userManagerService, IMapper mapper)
        {
            _userManagerService = userManagerService;
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
		public async Task<IActionResult> IndexUsers(string SearchText = "", int pg = 1, int pageSize = 5)
		{

			List<UserManagerDto> list = new();

			var response = await _userManagerService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<UserManagerDto>>(Convert.ToString(response.Result));

				if (pg < 1) pg = 1;




				if (SearchText != "" && SearchText != null)
				{
					List<UserManagerDto> userList = new List<UserManagerDto>();
					foreach (var user in list)
					{
                        if (user.Name.ToLower().Contains(SearchText.ToLower()) || user.Email.ToLower().Contains(SearchText.ToLower())|| user.Role.ToLower().Contains(SearchText.ToLower()))
							userList.Add(user);
					}
					list = userList;
				}
				int recsCount = list.Count();



				int recSkip = (pg - 1) * pageSize;
				var data = list.Skip(recSkip).Take(pageSize).ToList();



				Pager SearchPager = new Pager(recsCount, pg, pageSize) { Action = "Indexusers", Controller = "UserManager", SearchText = SearchText };
				ViewBag.SearchPager = SearchPager;



				this.ViewBag.PageSizes = GetPageSizes(pageSize);



				return View(data.ToList());
			}
			return View(list);
		}

		[Authorize(Roles = "Admin")]
		//public async Task<IActionResult> IndexUsers()
  //      {
  //          List<UserManagerDto> list = new();

  //          var response = await _userManagerService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
  //          if (response != null && response.IsSuccess)
  //          {
  //              list = JsonConvert.DeserializeObject<List<UserManagerDto>>(Convert.ToString(response.Result));
  //          }
  //          return View(list);
  //      }
       
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int Id)
        {
            var response = await _userManagerService.GetAsync<APIResponse>(Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                UserManagerDto model = JsonConvert.DeserializeObject<UserManagerDto>(Convert.ToString((response.Result)));
                return View(_mapper.Map<UserManagerDto>(model));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(UserManagerDto model)
        {
            var response = await _userManagerService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "User updated successfully";

                return RedirectToAction(nameof(IndexUsers));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        
        
    }
}