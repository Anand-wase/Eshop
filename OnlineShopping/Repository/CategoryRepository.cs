using OnlineShopping.WebApi.Data;
using OnlineShopping.WebApi.Models;
using OnlineShopping.WebApi.Repository.IRepository;

namespace OnlineShopping.WebApi.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Category> UpdateAsync(Category entity)
        {
            _db.Categories.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
