using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;


namespace Painel.Web.Areas.Cadastro.Models
{
    public class TabelaPrecosViewModel
    {
    
        public long? IDPLANO { get; set; }

        public long? id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Escolha o plano")]
        public string PLANO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Idade Inicial")]
        public int INICIO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Faixa Idade Final")]
        public string FIM { get; set; }

        public string novo { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Preço")]
        public string VALOR { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Ativo")]
        public string STATUS { get; set; }
        public string GUID { get; set; }
        public DateTime DATACADASTRO { get; set; }
        public DateTime DATAATUALIZACAO { get; set; }
        public string USUARIOCADASTRO { get; set; }
        public string USUARIOATUALIZACAO { get; set; }

    }
}