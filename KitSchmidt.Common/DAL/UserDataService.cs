using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KitSchmidt.Common.DAL.Models;
using KitSchmidt.DAL;

namespace KitSchmidt.Common.DAL
{
    public class UserDataService : IUserDataService
    {
        private KitContext _dbContext;

        public UserDataService(KitContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User GetUser(string userId)
        {
            return _dbContext.Users.FirstOrDefault(u => u.UserId == userId);
        }

        public async Task AddUser(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
