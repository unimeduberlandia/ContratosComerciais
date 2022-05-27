using Painel.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Painel.Web.Areas.Configuracao.Models
{
    public class CadastroUsuarioViewModel
    {
        public long? Id { get; set; }
        public long? IdEmpresa { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Empresa")]
        public string Empresa { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Confirme a Senha")]
        public string Senha2 { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "CPF")]
        public string CPF { get; set; }

        [Display(Name = "Status Ativo")]
        public string userstatus { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Telefone Fixo")]
        public string Telefone1 { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Telefone Celular")]
        public string Telefone2 { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }


    }
}