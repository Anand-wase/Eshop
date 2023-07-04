using OnlineShopping.WebApp.Models;
using OnlineShopping.WebApp.Models.Dto;

namespace OnlineShopping.WebApp.Services.IServices
{
    public interface IUserManagerService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> UpdateAsync<T>(UserManagerDto dto, string token);
    }
}