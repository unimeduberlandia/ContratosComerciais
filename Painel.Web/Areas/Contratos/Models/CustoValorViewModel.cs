using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;


namespace Painel.Web.Areas.Contratos.Models
{
    public class CustoValorViewModel
    {
        public long? ID1 { get; set; }
        public long? ID2 { get; set; }
        public long? ID3 { get; set; }
        public long? ID4 { get; set; }
        public long? ID5 { get; set; }
        public long? ID6 { get; set; }
        public long? ID7 { get; set; }
        public long? ID8 { get; set; }
        public long? ID9 { get; set; }
        public long? ID10 { get; set; }
        public long? ID11 { get; set; }
        public long? ID12 { get; set; }
        public long? ID13 { get; set; }
        public long? ID14 { get; set; }
        public long? ID15 { get; set; }
        //public long? ID { get; set; }
        public long? IDPLANO { get; set; }
        public long? IDCONTRATO { get; set; }
        public long? IDNOME { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME1 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME2 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME3 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME4 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME5 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME6 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME7 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME8 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME9 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME10 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME11 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME12 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME13 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME14 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Faixa Etária")]
        public string NOME15 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS1 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS2 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS3 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS4 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS5 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS6 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS7 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS8 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS9 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS10 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS11 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS12 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS13 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS14 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDAS15 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public long? VALORINDIVIDUAL { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUAL1 { get; set; }
        //public long? VALORINDIVIDUAL1 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUAL2 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUAL3 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUAL4 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUAL5 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUAL6 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUAL7 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUAL8 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUAL9 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUAL10 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUAL11 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUAL12 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUAL13 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUAL14 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUAL15 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO1 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO2 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO3 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO4 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO5 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO6 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO7 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO8 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO9 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO10 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO11 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO12 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO13 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO14 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Individual")]
        public string VALORCUSTO15 { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Início")]
        public long? INICIO { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Até")]
        public long? FIM { get; set; }

        public string guidCustoValor { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string QNTDFAIXAS { get; set; }





        public string guidDependente { get; set; }
        public DateTime DATACADASTRO { get; set; }
        public DateTime DATAATUALIZACAO { get; set; }
        public string USUARIOCADASTRO { get; set; }
        public string USUARIOATUALIZACAO { get; set; }

    }
}