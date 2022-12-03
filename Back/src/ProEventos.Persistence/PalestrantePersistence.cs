using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence
{
    public class PalestrantePersistence : IPalestrantePersistence
    {
        private readonly ProEventosContext _context;

        public PalestrantePersistence(ProEventosContext context)
        {
            this._context = context;

        }

        public async Task<Palestrante> GetAllPalestranteByIdAsync(int id, bool includEvento = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                    .Include(p => p.RedesSociais);
            query = includEvento
                ? query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento)
                : query;

            return await query.OrderBy(p => p.Id)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includEvento = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes;
            query = includEvento
                ? query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento)
                : query;

            return await query
                .OrderBy(x => x.Id)
                .ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includEvento = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes;
            query = includEvento
                ? query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento)
                : query;

            return await query.OrderBy(x => x.Id)
                .Where(p => p.Nome.ToLower().Contains(nome.ToLower()))
                .ToArrayAsync();
        }

    }
}