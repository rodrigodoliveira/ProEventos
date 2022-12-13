using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface ILotePersistence
    {
        /// <summary>
        /// Get all lotes by evento
        /// </summary>
        /// <param name="eventoId">id do evento</param>
        /// <returns>Array Lotes</returns>
        Task<Lote[]> GetLotesByEventoId(int eventoId);

        /// <summary>
        /// Get lote by id
        /// </summary>
        /// <param name="eventoId">Id do evento</param>
        /// <param name="id">Id do lote</param>
        /// <returns>Lote</returns>
        Task<Lote> GetLoteByIds(int eventoId, int id);
    }
}