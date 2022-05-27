using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;


namespace Painel.Web.Areas.Cadastro.Models
{
    public class TermosContratuaisViewModel
    {
        public long? id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome")]
        public string NOME { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Tipo")]
        public string GRUPO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string IDGRUPO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Preço")]
        public string VALOR { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Status")]
        public string STATUS { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Arquivo")]
        public string NOMEARQUIVO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Mensagem:")]
        public string mensagem { get; set; }

        public DateTime DATACADASTRO { get; set; }
        public DateTime DATAATUALIZACAO { get; set; }
        public string USUARIOCADASTRO { get; set; }
        public string USUARIOATUALIZACAO { get; set; }

    }
}