using System;
using KitSchmidt.Common.DAL.Models;
using KitSchmidt.DAL;

namespace KitSchmidt.Common.DAL
{
    public class EventDataService : IEventDataService
    {
        private KitContext _dbContext;

        public EventDataService(KitContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int SaveEvent(Event newEvent)
        {
            _dbContext.Events.Add(newEvent);
            _dbContext.SaveChanges();

            return newEvent.Id;
        }
    }
}
