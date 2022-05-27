using Painel.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Painel.Web.Areas.Configuracao.Models
{
    public class PermissaoViewModel
    {
        public long? Id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Descrição é obrigatório")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Descrição é obrigatório")]
        [Display(Name = "Regras")]
        public string Regras { get; set; }
    }
}