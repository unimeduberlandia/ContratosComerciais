using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;


namespace Painel.Web.Areas.Cadastro.Models
{
    public class PlanosViewModel
    {
        public long? id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código do plano")]
        public string codigoconsulta { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome do Plano")]
        public string PLANO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código Plano")]
        public string CODIGOPLANO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código ANS")]
        public string CODIGO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Tipo de Contratação")]
        public string TIPOCONTRATACAO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Formação de Preço")]
        public string FORMACAOPRECO { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Descrição do Plano")]
        public string DESCRICAO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Abrangência")]
        public string ABRANGENCIA { get; set; }
        public string idABRANGENCIA { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Acomodação")]
        public string ACOMODACAO { get; set; }
        public string idACOMODACAO { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Modalidade")]
        public string MODALIDADE { get; set; }
        public string idMODALIDADE { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Cadastro Ativo")]
        public string STATUS { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Contrato")]
        public string NOMEARQUIVO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nº Contrato")]
        public string NUMEROCONTRATO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Tipo de Contrato")]
        public string tipocontrato { get; set; }

    }
}