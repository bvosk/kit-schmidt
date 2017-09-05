using KitSchmidt.Common.DAL.Models;

namespace KitSchmidt.Common.DAL
{
    public interface IEventDataService
    {
        int SaveEvent(Event newEvent);
    }
}