using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;


namespace Painel.Web.Areas.Contratos.Models
{
    public class OpcionalViewModel
    {
        public long? id { get; set; }
        public long? idplano { get; set; }
        public long? IDADICIONAIS { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Nome")]
        public string NOME { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Grupo")]
        public string GRUPO { get; set; }

        public string guid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Valor")]
        public string VALOR { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Observações")]
        public string OBSERVACAO { get; set; }

        public DateTime DATACADASTRO { get; set; }
        public DateTime DATAATUALIZACAO { get; set; }
        public string USUARIOCADASTRO { get; set; }
        public string USUARIOATUALIZACAO { get; set; }

    }
}