using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence
{
    public class EventoPersistence : IEventoPersistence
    {
        private readonly ProEventosContext _context;

        public EventoPersistence(ProEventosContext context)
        {
            this._context = context;
        }

        public async Task<Evento> GetAllEventoByIdAsync(int userId, int id, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            query = includePalestrantes
                ? query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante)
                : query;

            return await query.AsNoTracking()
                              .Where(e => e.Id == id && e.UserId == userId)
                              .FirstOrDefaultAsync();
        }

        public async Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos;
            query = query
                .Include(evento => evento.Lotes)
                .Include(evento => evento.RedesSociais)
                .Include(evento => evento.User);


            query = includePalestrantes
                ? query.Include(evento => evento.PalestrantesEventos)
                       .ThenInclude(palestranteEvento => palestranteEvento.Palestrante)
                : query;


            return await query.AsNoTracking()
                              .Where(evento => evento.UserId == userId)
                              .OrderBy(evento => evento.Id).ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);


            query = includePalestrantes
                    ? query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante)
                    : query;

            return await query.AsNoTracking()
                              .Where(x => x.Tema.ToLower().Contains(tema.ToLower()) && x.UserId == userId)
                              .OrderBy(x => x.Id)
                              .ToArrayAsync();
        }


    }
}