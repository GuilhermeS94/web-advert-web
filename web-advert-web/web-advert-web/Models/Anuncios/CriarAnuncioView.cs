using System;
using System.ComponentModel.DataAnnotations;

namespace web_advert_web.Models.Anuncios
{
    public class CriarAnuncioView
    {
        public CriarAnuncioView()
        {
        }

        [Required(ErrorMessage = "Titulo deve ser informado")]
        public string Titulo { get; set; }

        public string Descricao { get; set; }

        [Required(ErrorMessage = "Preco deve ser informado")]
        [DataType(DataType.Currency)]
        public double Preco { get; set; }
    }
}
