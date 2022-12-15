using System.Threading.Tasks;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence
{
    public class GeralPersistence : IGeralPersistence
    {
        private readonly ProEventosContext _context;

        public GeralPersistence(ProEventosContext context)
        {
            this._context = context;

        }
        public virtual void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public virtual void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public virtual void DeleteRange<T>(T[] entities) where T : class
        {
            _context.RemoveRange(entities);
        }

        public virtual void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

    }
}