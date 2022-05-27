using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;


namespace Painel.Web.Areas.Contratos.Models
{
    public class CustoExibirViewModel
    {
      
        public long? IDPLANO { get; set; }
        public long? IDCONTRATO { get; set; }


        [Display(Name = "Faixa Etária")]
        public string NOM1 { get; set; }

        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDA1 { get; set; }

        [Display(Name = "Valor Individual")]
        public string VALORCUST1 { get; set; }

        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUA1 { get; set; }



        [Display(Name = "Faixa Etária")]
        public string NOM2 { get; set; }

        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDA2 { get; set; }

        [Display(Name = "Valor Individual")]
        public string VALORCUST2 { get; set; }

        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUA2 { get; set; }


        [Display(Name = "Faixa Etária")]
        public string NOM3 { get; set; }

        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDA3 { get; set; }

        [Display(Name = "Valor Individual")]
        public string VALORCUST3 { get; set; }

        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUA3 { get; set; }


        [Display(Name = "Faixa Etária")]
        public string NOM4 { get; set; }

        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDA4 { get; set; }

        [Display(Name = "Valor Individual")]
        public string VALORCUST4 { get; set; }

        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUA4 { get; set; }


        [Display(Name = "Faixa Etária")]
        public string NOM5 { get; set; }

        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDA5 { get; set; }

        [Display(Name = "Valor Individual")]
        public string VALORCUST5 { get; set; }

        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUA5 { get; set; }


        [Display(Name = "Faixa Etária")]
        public string NOM6 { get; set; }

        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDA6 { get; set; }

        [Display(Name = "Valor Individual")]
        public string VALORCUST6 { get; set; }

        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUA6 { get; set; }


        [Display(Name = "Faixa Etária")]
        public string NOM7 { get; set; }

        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDA7 { get; set; }

        [Display(Name = "Valor Individual")]
        public string VALORCUST7 { get; set; }

        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUA7 { get; set; }

        [Display(Name = "Faixa Etária")]
        public string NOM8 { get; set; }

        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDA8 { get; set; }

        [Display(Name = "Valor Individual")]
        public string VALORCUST8 { get; set; }

        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUA8 { get; set; }


        [Display(Name = "Faixa Etária")]
        public string NOM9 { get; set; }

        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDA9 { get; set; }

        [Display(Name = "Valor Individual")]
        public string VALORCUST9 { get; set; }

        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUA9 { get; set; }


        [Display(Name = "Faixa Etária")]
        public string NOM10 { get; set; }

        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDA10 { get; set; }

        [Display(Name = "Valor Individual")]
        public string VALORCUST10 { get; set; }

        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUA10 { get; set; }


        [Display(Name = "Faixa Etária")]
        public string NOM11 { get; set; }

        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDA11 { get; set; }

        [Display(Name = "Valor Individual")]
        public string VALORCUST11 { get; set; }

        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUA11 { get; set; }


        [Display(Name = "Faixa Etária")]
        public string NOM12 { get; set; }

        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDA12 { get; set; }

        [Display(Name = "Valor Individual")]
        public string VALORCUST12 { get; set; }

        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUA12 { get; set; }


        [Display(Name = "Faixa Etária")]
        public string NOM13 { get; set; }

        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDA13 { get; set; }

        [Display(Name = "Valor Individual")]
        public string VALORCUST13 { get; set; }

        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUA13 { get; set; }


        [Display(Name = "Faixa Etária")]
        public string NOM14 { get; set; }

        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDA14 { get; set; }

        [Display(Name = "Valor Individual")]
        public string VALORCUST14 { get; set; }

        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUA14 { get; set; }


        [Display(Name = "Faixa Etária")]
        public string NOM15 { get; set; }

        [Display(Name = "Quantidade de vidas")]
        public long? QNTDVIDA15 { get; set; }

        [Display(Name = "Valor Individual")]
        public string VALORCUST15 { get; set; }

        [Display(Name = "Valor Total")]
        public string VALORINDIVIDUA15 { get; set; }


        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Início")]
        public long? INICIO { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Até")]
        public long? FIM { get; set; }

        public string guidCustoValor { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Total")]
        public string QNTDEFAIXAS { get; set; }







        public string guidDependente { get; set; }
        public DateTime DATACADASTRO { get; set; }
        public DateTime DATAATUALIZACAO { get; set; }
        public string USUARIOCADASTRO { get; set; }
        public string USUARIOATUALIZACAO { get; set; }

    }
}