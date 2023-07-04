using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineShopping.Utility;
using OnlineShopping.WebApp.Models;
using OnlineShopping.WebApp.Models.Dto;
using OnlineShopping.WebApp.Services.IServices;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;

namespace OnlineShopping.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {

            var userId = HttpContext.Session.GetString(SD.SessionUserId);
            APIResponse? response = await _cartService.GetAsync<APIResponse>(userId);
            if (response != null & response.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }
            return new CartDto();

        }



        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = HttpContext.Session.GetString(SD.SessionUserId);

            APIResponse? response = await _cartService.CreateAsync<APIResponse>(cartDetailsId);
            if (response != null & response.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }







      




        public async Task<IActionResult> SubmitCart()

        {

            var emailId = HttpContext.Session.GetString(SD.SessionUserMail);
            var userId = HttpContext.Session.GetString(SD.SessionUserId);
            var name = HttpContext.Session.GetString(SD.SessionEmail);

            APIResponse? response = await _cartService.GetAsync<APIResponse>(userId);


            CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));

            string fromMail = "jayarambande01@gmail.com";

            string fromPassword = "pfvrbgxmtqbtqjpy";




            var s = "Hi, " + name + " Your Order has been Succesfully Placed.<br>" + "<p><b>YOUR ORDERS ARE : </b></p> ";

            var i = 1;

            double amount = 0;


            foreach (var item in cartDto.CartDetails)

            {

                s += i.ToString() + "<br>" + "Product Name : " + item.Product.ProductName + "<br>" + "Quantity : " + item.Count + "<br>" + "Price : " + (item.Product.Price * item.Count).ToString("c") + "<p>";

                i++;

                amount += item.Product.Price * item.Count;

            }


            s += "<h4> Total Amount = " + amount.ToString("c") + "</h4>" + "<p>" + "<p>" + "<h5>THANK YOU</h5>";

            MailMessage message = new MailMessage();

            message.From = new MailAddress(fromMail);

            message.Subject = "ORDER DETAILS";

            //message.To.Add(new MailAddress("198w1a0407@vrsec.ac.in"));
            message.To.Add(new MailAddress(emailId));

            message.Body = s;

            message.IsBodyHtml = true;


            var smtpClient = new SmtpClient("smtp.gmail.com")

            {

                Port = 587,

                Credentials = new NetworkCredential(fromMail, fromPassword),

                EnableSsl = true,

            };


            smtpClient.Send(message);

            TempData["success"] = "Order Placed Successfully";

            return RedirectToAction("Index", "Home");


        }
    }
}
