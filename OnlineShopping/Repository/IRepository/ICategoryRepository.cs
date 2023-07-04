using OnlineShopping.WebApi.Models;

namespace OnlineShopping.WebApi.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>

    {
        Task<Category> UpdateAsync(Category entity);
    }
}
