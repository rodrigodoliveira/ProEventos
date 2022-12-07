using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} é obrigatório"),
        StringLength(50, MinimumLength = 3, ErrorMessage = "intervalo permitidos de 3 a 50 caractere")]
        public string Tema { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Local { get; set; }

        [Display(Name = "Quantidade Pessoas")]
        [Required(ErrorMessage = "{0} é obrigatório"),
        Range(1, 120000, ErrorMessage = "intervalo permitido para {0} é de 1 a 120000")]
        public int QtdPessoas { get; set; }

        [Required(ErrorMessage = "{0} é orbrigatorio")]
        public string DataEvento { get; set; }

        [Display(Name = "Imagem")]
        [RegularExpression(@"[^\\s]+(.*?)\\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$", ErrorMessage = "Imagem inválida. Tipo aceitos: jpg,jpeg,png,gif")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "{0} é orbrigatorio"),
        Phone(ErrorMessage = "{0} é inválido")]
        public string Telefone { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "{0} é inválido"), Required(ErrorMessage = "{0} é obrigatório")]
        public string Email { get; set; }

        public IEnumerable<LoteDto> Lotes { get; set; }
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
        public IEnumerable<PalestranteDto> Palestrantes { get; set; }
    }
}