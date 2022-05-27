using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;


namespace Painel.Web.Areas.Contratos.Models
{
    public class GerarContrtoPFViewModel
    {
        public long? id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "CNPJ")]
        public string CNPJ { get; set; }


        public long? ID { get; set; }
        public long? IDPLANO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Plano")]
        public string PLANO { get; set; }

        public long? IDTAXAINSCRICAO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor Taxa de Inscrição")]
        public string TAXADEINSCRICAO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Tabela Preço")]
        public string TABELAPRECO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Data Vigência Contrato")]
        public DateTime DATAVIGENCIACONTRATO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor do Titular")]
        public string VALORINDIVIDUAL { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor da Mensalidade (Plano + Opcionais)")]
        public string VALORTOTAL { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Valor da 1ª Fatura (Plano + Opcionais + Taxa)")]
        public string VALORPRIMEIRAFATURA { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor do Plano (Titular)")]
        public string TOTALTITULAR { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Total Dependentes")]
        public string TOTALDEPENDENTES { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Quantidade de Vidas")]
        public string TOTALDEVIDAS { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Dia do Vencimento")]
        public string DIAVENCIMENTO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Nome")]
        public string TITULAR { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*CPF")]
        public string CPFTITULAR { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Abrangência")]
        public string ABRANGENCIA { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Acomodação")]
        public string ACOMODACAO { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Dia do Vencimento Mensalidade")]
        public string DATAVENCIMENTOMENSAL { get; set; }
        

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Modalidade")]
        public string MODALIDADE { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Data de Nascimento")]
        public DateTime DATANASCIMENTO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Idade do Titular")]
        public int IDADE { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Cartão Nacional de Saúde")]
        public string CARTAOSAUDE { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Sexo")]
        public string SEXO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string IDSEXO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Esse é o endereço de cobrança?")]
        public string segundoendereco { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Idsegundoendereco { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*RG")]
        public string RG { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Orgão Expedidor")]
        public string ORGAOEXPEDIDOR { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Data Expedição")]
        public DateTime DATAEXPEDICAO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Nome da Mãe")]
        public string NOMEDAMAE { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*E-mail")]
        public string EMAIL { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Endereço")]
        public string ENDERECO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Número")]
        public string NUMERO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Complemento")]
        public string COMPLEMENTO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Bairro")]
        public string BAIRRO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Cidade")]
        public string CIDADE { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*UF")]
        public string UF { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*UF")]
        public string idUF { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Cep")]
        public string CEP { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Endereço Cobrança")]
        public string ENDERECOCOBRANCA { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Número")]
        public string NUMEROCOBRANCA { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Data Vencimento 1º Fatura")]
        public DateTime DATAVENCIMENTOPRIMEIRAFATURA { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Data Vigência")]
        public DateTime DATAFIMVIGENCIA { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Data Atendimento")]
        public DateTime DATAATENDIMENTO { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Complemento")]
        public string COMPLEMENTOCOBRANCA { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Bairro")]
        public string BAIRROCOBRANCA { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "UF")]
        public string UFCOBRANCA { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "UF")]
        public string IdUFCOBRANCA { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Cidade")]
        public string CIDADECOBRANCA { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Cep")]
        public string CEPCOBRANCA { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Telefone Celular")]
        public string TELEFONE1 { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "*Telefone Fixo")]
        public string TELEFONE2 { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Display(Name = "Observações")]
        public string OBSERVACAO { get; set; }
        public DateTime DATACADASTRO { get; set; }
        public DateTime DATAATUALIZACAO { get; set; }
        public string USUARIOCADASTRO { get; set; }
        public string USUARIOATUALIZACAO { get; set; }

        public string teste { get; set; }
    }
}