using System.ComponentModel.DataAnnotations;

namespace Painel.Web.Models
{
    public class TrocarSenhaViewModel
    {
        public long? idusuario { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo é obrigatório")]
        [Display(Name = "Senha Antiga")]
        public string senha { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo é obrigatório")]
        [Display(Name = "Nova Senha")]
        public string novasenha { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo é obrigatório")]
        [Display(Name = "Repitir Nova Senha")]
        public string novasenha2 { get; set; }
    }
}