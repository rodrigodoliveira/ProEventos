using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ProEventos.Application.Contracts;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralPersistence _geralPersistence;
        private readonly IEventoPersistence _eventoPersistence;
        private readonly ILogger<EventoService> _logger;
        private readonly IMapper _mapper;
        public EventoService(IGeralPersistence geralPersistence, IEventoPersistence eventoPersistence, IMapper mapper, ILogger<EventoService> logger)
        {
            this._mapper = mapper;
            this._eventoPersistence = eventoPersistence;
            this._geralPersistence = geralPersistence;
            this._logger = logger;

        }
        public async Task<EventoDto> Add(int userId, EventoDto model)
        {
            var evento = _mapper.Map<Evento>(model);
            evento.UserId = userId;

            _geralPersistence.Add(evento);

            if (await _geralPersistence.SaveChangesAsync())
            {
                var eventoReturn = await _eventoPersistence.GetAllEventoByIdAsync(userId, evento.Id);

                return _mapper.Map<EventoDto>(eventoReturn);
            }

            return null;

        }

        public async Task<EventoDto> Update(int userId, int id, EventoDto model)
        {
            var evento = await _eventoPersistence.GetAllEventoByIdAsync(userId, id);
            if (evento == null) throw new ArgumentException($"Evento com id {id} não econtrado");

            model.Id = evento.Id;
            model.UserId = userId;

            _mapper.Map(model, evento);

            _geralPersistence.Update<Evento>(evento);

            if (await _geralPersistence.SaveChangesAsync())
            {
                var eventoAdded = await _eventoPersistence.GetAllEventoByIdAsync(userId, evento.Id, includePalestrantes: false);
                return _mapper.Map<EventoDto>(eventoAdded);
            }

            return null;

        }

        public async Task<bool> Remove(int userId, int id)
        {
            var eventoRemove = await _eventoPersistence.GetAllEventoByIdAsync(userId, id);
            if (eventoRemove == null) throw new ArgumentException($"Evento com id {id} para delete não encontrado");


            _geralPersistence.Delete(eventoRemove);
            return await _geralPersistence.SaveChangesAsync();
        }

        public async Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
        {
            var eventos = await _eventoPersistence.GetAllEventosAsync(userId, includePalestrantes);
            if (eventos == null) return null;

            return _mapper.Map<EventoDto[]>(eventos);
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
        {
            var eventos = await _eventoPersistence.GetAllEventosByTemaAsync(userId, tema, includePalestrantes);
            if (eventos == null) return null;

            return _mapper.Map<EventoDto[]>(eventos);
        }

        public async Task<EventoDto> GetEventoByIdAsync(int userId, int id, bool includePalestrantes = false)
        {
            var evento = await _eventoPersistence.GetAllEventoByIdAsync(userId, id, includePalestrantes);
            if (evento == null) return null;

            return _mapper.Map<EventoDto>(evento);
        }

    }
}