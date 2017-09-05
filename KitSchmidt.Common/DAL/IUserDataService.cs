using KitSchmidt.Common.DAL.Models;

namespace KitSchmidt.Common.DAL
{
    public interface IUserDataService
    {
        User GetUser(string userId);
    }
}