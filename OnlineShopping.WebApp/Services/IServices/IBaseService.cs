using OnlineShopping.WebApp.Models;

namespace OnlineShopping.WebApp.Services.IServices
{
    public interface IBaseService
    {
        APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}