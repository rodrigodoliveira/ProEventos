using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence
{
    public class LotePersistence : ILotePersistence
    {
        private readonly ProEventosContext _context;
        public LotePersistence(ProEventosContext context)
        {
            this._context = context;

        }
        public async Task<Lote[]> GetLotesByEventoId(int eventoId)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query
                .AsNoTracking()
                .Where(lote => lote.EventoId == eventoId);

            return await query.ToArrayAsync();

        }

        public async Task<Lote> GetLoteByIds(int eventoId, int id)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query
                .AsNoTracking()
                .Where(lote => lote.EventoId == eventoId && lote.Id == id);

            return await query.FirstOrDefaultAsync();

        }
    }
}