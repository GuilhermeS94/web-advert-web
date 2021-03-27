using System;
using System.ComponentModel.DataAnnotations;

namespace web_advert_web.Models.Contas
{
    public class Login
    {
        public Login()
        {
        }

        [Required(ErrorMessage = "Email obrigatorio")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha obrigatorio")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Display(Name = "Lembrar-me")]
        public bool LembrarMe { get; set; }
    }
}
