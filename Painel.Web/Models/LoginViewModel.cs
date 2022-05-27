using System.ComponentModel.DataAnnotations;

namespace Painel.Web.Models
{
    public class LoginViewModel
    {
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Usuário")]
        public string Usuario { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Display(Name = "Me manter conectado")]
        public bool ManterConectado { get; set; }
    }
}