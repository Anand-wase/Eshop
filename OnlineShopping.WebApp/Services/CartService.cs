using OnlineShopping.Utility;
using OnlineShopping.WebApp.Models.Dto;
using OnlineShopping.WebApp.Models;
using OnlineShopping.WebApp.Services.IServices;

namespace OnlineShopping.WebApp.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string cartUrl;

        public CartService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            cartUrl = configuration.GetValue<string>("ServiceUrls:WebAPI");

        }

        public Task<T> GetAsync<T>(string userId)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = cartUrl + "/api/cart/GetCart/" + userId

            });
        }

        public Task<T> CreateAsync<T>(CartDto cartDto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = cartUrl + "/api/cart/CartUpsert"

            });

        }



        public Task<T> CreateAsync<T>(int cartDetailsId)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDetailsId,
                Url = cartUrl + "/api/cart/RemoveCart"

            });
        } 
        
        
      

    }
}
