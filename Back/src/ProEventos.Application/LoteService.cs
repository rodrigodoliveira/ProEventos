using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contracts;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application
{
    public class LoteService : ILoteService
    {
        private readonly IGeralPersistence _geralPersistence;
        private readonly ILotePersistence _lotePersistence;
        private readonly IMapper _mapper;

        public LoteService(IGeralPersistence geralPersistence, ILotePersistence lotePersistence, IMapper mapper)
        {
            this._mapper = mapper;
            this._lotePersistence = lotePersistence;
            this._geralPersistence = geralPersistence;

        }

        public async Task<LoteDto[]> SaveLotesAsync(int eventoId, LoteDto[] models)
        {
            var lotes = await _lotePersistence.GetLotesByEventoId(eventoId);
            if (lotes == null) return null;

            foreach (var model in models)
            {
                if (model.Id == 0)
                {
                    await AddLote(eventoId, model);
                }
                else
                {
                    var lote = lotes.FirstOrDefault(l => l.Id == model.Id);
                    model.EventoId = eventoId;
                    
                    _mapper.Map(model, lote);

                    _geralPersistence.Update<Lote>(lote);

                    await _geralPersistence.SaveChangesAsync();
                }
            }

            var lotesRetorno = await _lotePersistence.GetLotesByEventoId(eventoId);

            return  _mapper.Map<LoteDto[]>(lotesRetorno);
        }

        public async Task AddLote(int eventoId, LoteDto lote)
        {
            var loteSave = _mapper.Map<Lote>(lote);
            loteSave.EventoId = eventoId;

            _geralPersistence.Add<Lote>(loteSave);

            await _geralPersistence.SaveChangesAsync();

        }

        public async Task<bool> Delete(int eventoId, int id)
        {

            var lote = await _lotePersistence.GetLoteByIds(eventoId, id);

            if (lote == null) throw new ArgumentException("Lote para delete não encontrado");

            _geralPersistence.Delete<Lote>(lote);
            return await _geralPersistence.SaveChangesAsync();

        }


        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {

            var lotes = await _lotePersistence.GetLotesByEventoId(eventoId);
            if (lotes == null || !lotes.Any()) return null;

            var resultado = _mapper.Map<LoteDto[]>(lotes);

            return resultado;
        }

        public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int id)
        {
            var lote = await _lotePersistence.GetLoteByIds(eventoId, id);
            if (lote == null) return null;

            var resultado = _mapper.Map<LoteDto>(lote);
            return resultado;
        }


    }
}