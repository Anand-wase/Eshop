using OnlineShopping.Utility;
using OnlineShopping.WebApp.Models;
using OnlineShopping.WebApp.Services.IServices;
using OnlineShopping.WebApp.Services;
using OnlineShopping.WebApp.Models.Dto;

namespace OnlineShopping.WebApp.Services
{
    public class UserManagerService : BaseService, IUserManagerService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string UserManagerUrl;

        public UserManagerService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            UserManagerUrl = configuration.GetValue<string>("ServiceUrls:WebAPI");

        }
        

       

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = UserManagerUrl + "/api/usermanagerAPI",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = UserManagerUrl + "/api/usermanagerAPI/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(UserManagerDto dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = UserManagerUrl + "/api/usermanagerAPI/" + dto.Id,
                Token = token
            });
        }
    }
}