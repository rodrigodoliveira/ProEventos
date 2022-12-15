using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence
{
    public class UserPersistence : GeralPersistence, IUserPersistence
    {
        private readonly ProEventosContext _context;
        public UserPersistence(ProEventosContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users
            .FindAsync(id);
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users
            .SingleOrDefaultAsync(user => user.UserName.ToLower() == userName.ToLower());
        }

        public override void Delete<User>(User entity)
        {
            // throw new ApplicationException("Corporação não permite deleção de usuario");
            throw new SystemException("Corporação não permite deleção de usuários");
        }

        
    }
}