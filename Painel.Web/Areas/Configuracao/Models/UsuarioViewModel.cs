using Painel.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Painel.Web.Areas.Configuracao.Models
{
    public class UsuarioViewModel
    {
        public long? Id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Login é obrigatório")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        public string[] PermissoesIds { get; set; }
        public IEnumerable<Permissao> TodasPermissoes { get; set; }
        public IEnumerable<Permissao> PermissoesSelecionadas { get; set; }
    }
}