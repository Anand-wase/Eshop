using OnlineShopping.WebApi.Models;

namespace OnlineShopping.WebApi.Repository.IRepository
{
    public interface IUserManagerRepository : IRepository<LocalUser>

    {
        Task<LocalUser> UpdateAsync(LocalUser entity);
    }
}
