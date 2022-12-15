using System.Collections.Generic;
using System.Threading.Tasks;
using ProEventos.Domain.Identity;

namespace ProEventos.Persistence.Contracts
{
    public interface IUserPersistence : IGeralPersistence
    {
        Task<IEnumerable<User>> GetUsersAsync();        
        Task<User> GetUserByIdAsync(int id);        
        Task<User> GetUserByUserNameAsync(string userName); 
        
    }
}