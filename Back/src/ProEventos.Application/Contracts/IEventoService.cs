using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contracts
{
    public interface IEventoService
    {
        Task<EventoDto> Add(EventoDto model);
        Task<EventoDto> Update(int id, EventoDto model);
        Task<bool> Remove(int id);

        Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false);
        Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
        Task<EventoDto> GetEventoByIdAsync(int id, bool includePalestrantes = false);

    }
}