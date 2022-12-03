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
        
        public async Task<Evento> GetAllEventoByIdAsync(int id, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            query = includePalestrantes
                ? query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante)
                : query;

            return await query
                    .Where(e => e.Id == id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos;
            query = query
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            query = includePalestrantes
                ? query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante)
                : query;


            return await query
                .AsNoTracking()
                .OrderBy(e => e.Id).ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);


            query = includePalestrantes
                    ? query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante)
                    : query;

            return await query
                .OrderBy(x => x.Id)
                .AsNoTracking()
                .Where(x => x.Tema.ToLower().Contains(tema.ToLower()))
                .ToArrayAsync();
        }


    }
}