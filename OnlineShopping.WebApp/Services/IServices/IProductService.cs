using OnlineShopping.WebApp.Models;

namespace OnlineShopping.WebApp.Services.IServices
{
    public interface IProductService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(CreateProductDto dto, string token);
        Task<T> UpdateAsync<T>(UpdateProductDto dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
