using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface IPalestrantePersistence
    {
        //PALESTRANTES
        Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includEvento);
        Task<Palestrante[]> GetAllPalestrantesAsync(bool includEvento);
        Task<Palestrante> GetAllPalestranteByIdAsync(int id, bool includEvento);
    }
}