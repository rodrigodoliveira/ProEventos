
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Domain.Identity;

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

            this.CreateMap<User, UserDto>().ReverseMap();
            this.CreateMap<User, UserLoginDto>().ReverseMap();
            this.CreateMap<User, UserUpdateDto>().ReverseMap();
        }
    }
}