using OnlineShopping.Utility;
using OnlineShopping.WebApp.Models.Dto;
using OnlineShopping.WebApp.Models;
using OnlineShopping.WebApp.Services.IServices;
using OnlineShopping.WebApp.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly IHttpClientFactory _clientFactory;
    private string villaUrl;

    public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
    {
        _clientFactory = clientFactory;
        villaUrl = configuration.GetValue<string>("ServiceUrls:WebAPI");

    }

    public Task<T> LoginAsync<T>(LoginRequestDTO obj)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = obj,
            Url = villaUrl + "/api/UsersAuth/login"
        });
    }

    public Task<T> RegisterAsync<T>(RegisterationRequestDTO obj)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = obj,
            Url = villaUrl + "/api/UsersAuth/register"
        });
    }
}