
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Domain;

namespace ProEventos.Application.Helpers
{
    public class ProEventosMapperProfile : Profile
    {
        public ProEventosMapperProfile()
        {
            this.CreateMap<Evento, EventoDto>().ReverseMap();
            this.CreateMap<Lote, LoteDto>().ReverseMap();
            this.CreateMap<Palestrante, PalestranteDto>().ReverseMap();
            this.CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
        }
    }
}