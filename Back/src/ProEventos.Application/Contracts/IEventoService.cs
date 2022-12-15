using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contracts
{
    public interface IEventoService
    {
        Task<EventoDto> Add(int userId, EventoDto model);
        Task<EventoDto> Update(int userId, int id, EventoDto model);
        Task<bool> Remove(int userId, int id);

        Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);
        Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);
        Task<EventoDto> GetEventoByIdAsync(int userId, int id, bool includePalestrantes = false);

    }
}