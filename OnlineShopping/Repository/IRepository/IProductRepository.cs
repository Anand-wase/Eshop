using OnlineShopping.WebApi.Models;
using System.Linq.Expressions;

namespace OnlineShopping.WebApi.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
 
    {
        Task<Product> UpdateAsync(Product entity);
    }
}