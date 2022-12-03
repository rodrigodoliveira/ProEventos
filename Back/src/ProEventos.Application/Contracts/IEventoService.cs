using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Application.Contracts
{
    public interface IEventoService
    {
        Task<Evento> Add(Evento model);
        Task<Evento> Update(int id, Evento model);
        Task<bool> Remove(int id);

        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int id, bool includePalestrantes = false);

    }
}