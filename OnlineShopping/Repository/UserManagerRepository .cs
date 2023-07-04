using OnlineShopping.WebApi.Data;
using OnlineShopping.WebApi.Models;
using OnlineShopping.WebApi.Repository.IRepository;

namespace OnlineShopping.WebApi.Repository
{
    public class UserManagerRepository : Repository<LocalUser>, IUserManagerRepository
    {
        private readonly ApplicationDbContext _db;
        public UserManagerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<LocalUser> UpdateAsync(LocalUser entity)
        {
            _db.LocalUsers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        
    }
}
