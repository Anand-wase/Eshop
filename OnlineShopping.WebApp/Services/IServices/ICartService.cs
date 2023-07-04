using OnlineShopping.WebApp.Models.Dto;

namespace OnlineShopping.WebApp.Services.IServices
{
    public interface ICartService
    {
        Task<T> GetAsync<T>(string userId);
        Task<T> CreateAsync<T>(CartDto cartDto);

        Task<T> CreateAsync<T>(int cartDetailsId);


    }
}
