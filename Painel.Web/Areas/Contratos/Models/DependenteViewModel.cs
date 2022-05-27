using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;


namespace Painel.Web.Areas.Contratos.Models
{
    public class DependenteViewModel
    {
        public long? id { get; set; }
        public long? idplano { get; set; }
       
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome")]
        public string NOMEDep { get; set; }

        [Display(Name = "Idade")]
        public string IDADEDep { get; set; }
        public string guidDependente { get; set; }

        [Display(Name = "Valor")]
        public string VALORDep { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "CPF")]
        public string CPFDep { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Sexo")]
        public string SEXODep { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string IDSEXODep { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Parentesco")]
        public string PARENTESCODep { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string IDPARENTESCODep { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nacionalidade")]
        public string NACIONALIDADEDep { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome da Mãe")]
        public string NOMEDAMAEDep { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Estado Civil")]
        public string ESTADOCIVILDep { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string IDESTADOCIVILDep { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Cartão Nacional de Saúde")]
        public string CARTAODESAUDEDep { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Data de Nascimento")]
        public DateTime DATANASCIMENTODep { get; set; }
        public DateTime DATACADASTRO { get; set; }
        public DateTime DATAATUALIZACAO { get; set; }
        public string USUARIOCADASTRO { get; set; }
        public string USUARIOATUALIZACAO { get; set; }

    }
}