using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contracts
{
    public interface ILoteService
    {
        Task<LoteDto[]> SaveLotesAsync(int eventoId, LoteDto[] models);
        Task<bool> Delete(int eventoId, int id);
        Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId);
        Task<LoteDto> GetLoteByIdsAsync(int eventoId, int id);
    }
}