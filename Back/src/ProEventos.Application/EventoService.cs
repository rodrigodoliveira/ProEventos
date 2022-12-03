using System;
using System.Threading.Tasks;
using ProEventos.Application.Contracts;
using ProEventos.Domain;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralPersistence _geralPersistence;
        private readonly IEventoPersistence _eventoPersistence;
        public EventoService(IGeralPersistence geralPersistence, IEventoPersistence eventoPersistence)
        {
            this._eventoPersistence = eventoPersistence;
            this._geralPersistence = geralPersistence;

        }
        public async Task<Evento> Add(Evento model)
        {
            try
            {
                _geralPersistence.Add(model);
                if (await _geralPersistence.SaveChangesAsync())
                {
                    return await _eventoPersistence.GetAllEventoByIdAsync(model.Id);
                }

                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao tentar adicionar evento", ex);
            }
        }

        public async Task<Evento> Update(int id, Evento model)
        {
            try
            {
                var evento = await _eventoPersistence.GetAllEventoByIdAsync(id);
                if (evento == null) throw new ArgumentException($"Evento com id {id} não econtrado");

                model.Id = evento.Id;

                _geralPersistence.Update(model);
                if (await _geralPersistence.SaveChangesAsync())
                {
                    return await _eventoPersistence.GetAllEventoByIdAsync(model.Id, includePalestrantes: false);
                }

                return null;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar evento", ex);
            }
        }

        public async Task<bool> Remove(int id)
        {
            try
            {
                var eventoRemove = await _eventoPersistence.GetAllEventoByIdAsync(id);
                if (eventoRemove == null)
                {
                    throw new ArgumentException($"Evento com id {id} para delete não encontrado");
                }

                _geralPersistence.Delete(eventoRemove);
                return await _geralPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao remover evento", ex);
            }
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                return await _eventoPersistence.GetAllEventosAsync(includePalestrantes);
            }
            catch (System.Exception ex)
            {
                throw new Exception("Erro ao recuperar eventos", ex);
            }
        }

        public async Task<Evento> GetEventoByIdAsync(int id, bool includePalestrantes = false)
        {
            try
            {
                return await _eventoPersistence.GetAllEventoByIdAsync(id, includePalestrantes);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao recuperar evento", ex);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                return await _eventoPersistence.GetAllEventosByTemaAsync(tema, includePalestrantes);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao recuperar eventos", ex);
            }
        }

    }
}