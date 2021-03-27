using System;
using System.ComponentModel.DataAnnotations;

namespace web_advert_web.Models.Contas
{
    public class Cadastrar
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(6, ErrorMessage = "Senha deve ser pelo menos 6 caracteres!")]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "Confirmacao de senha nao confere.")]
        public string ConfirmarSenha { get; set; }

        public Cadastrar()
        {
        }
    }
}
