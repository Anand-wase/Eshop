using OnlineShopping.WebApi.Models.Dto;
using OnlineShopping.WebApi.Models;

namespace OnlineShopping.WebApi.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string Email);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
