using KitSchmidt.DAL.Models;

namespace KitSchmidt.Common.DAL
{
    public interface IMessageDataService
    {
        void SaveMessage(Message message);
    }
}