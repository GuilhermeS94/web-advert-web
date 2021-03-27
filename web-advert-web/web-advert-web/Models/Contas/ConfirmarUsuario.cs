using System;
using System.ComponentModel.DataAnnotations;

namespace web_advert_web.Models.Contas
{
    public class ConfirmarUsuario
    {
        public ConfirmarUsuario()
        {
        }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required (ErrorMessage = "Código obrigatório")]
        public string Codigo { get; set; }
    }
}
