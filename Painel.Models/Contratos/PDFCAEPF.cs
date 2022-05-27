using Painel.Models;
using System;
using System.Collections.Generic;

namespace Painel.Models.Contratos
{
    public class PDFCAEPF : BaseEntity
    {
        public long? ID { get; set; }
        public long? IDPLANO { get; set; }
        public long? IDEMPRESA { get; set; }
        public string CNPJ { get; set; }
        public string ARQUIVOTERMO { get; set; }
        public int QNTDTERMOS { get; set; }
        public string RAZAOSOCIAL { get; set; }
        public string NOMEFANTASIA { get; set; }
        public string INSCRICAOESTADUAL { get; set; }
        public string ISS { get; set; }
        public string EMAILVENDEDOR { get; set; }
        public string EMAIL { get; set; }
        public string ENDERECO { get; set; }
        public string NUMERO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string BAIRRO { get; set; }
        public string STATUSESPELHO { get; set; }
        public string STATUSQUALIFICACAO { get; set; }
        public string STATUSASSINATURA { get; set; }
        public string STATUSCONTRATO { get; set; }
        public string REPRESENTANTELEGAL { get; set; }
        public string CPFREPRESENTANTE { get; set; }
        public string RGREPRESENTANTE { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string STATUS { get; set; }
        public string CONTATO { get; set; }
        public string TELEFONE { get; set; }
        public string TELEFONE2 { get; set; }
        public string CODIGOANS { get; set; }

        public string SEGMENTACAO { get; set; }
        public string TIPOCONTRATACAO { get; set; }
        public string PLANO { get; set; }
        public string NOMEARQUIVO { get; set; }
        public string CODIGO { get; set; }
        public string CONTATOINTERNO { get; set; }
        public string NUMEROCONTRATO { get; set; }
        public string NUMEROPROPOSTA { get; set; }
        public string GRUPOECONOMICO { get; set; }
        public long? IDTAXAINSCRICAO { get; set; }
        public string TAXADEINSCRICAO { get; set; }
        public string TABELAPRECO { get; set; }
        public DateTime DATAVIGENCIACONTRATO { get; set; }
        public string VALORTOTAL { get; set; }
        public string PARTICIPACAOFINANCEIRA { get; set; }
        public string VALORPRIMEIRAFATURA { get; set; }
        public string TOTALDEVIDAS { get; set; }
        public string DIAVENCIMENTO { get; set; }
        public string ABRANGENCIA { get; set; }
        public string FORMACAOPRECO { get; set; }
        public string ACOMODACAO { get; set; }
        public string DATAVENCIMENTOMENSAL { get; set; }
        public string MODALIDADE { get; set; }
        public string segundoendereco { get; set; }
        public string Idsegundoendereco { get; set; }
        public string idUF { get; set; }
        public string ENDERECOCOBRANCA { get; set; }
        public string NUMEROCOBRANCA { get; set; }
        public DateTime DATAPRIMEIRAFATURA { get; set; }
        public string COMPLEMENTOCOBRANCA { get; set; }
        public string BAIRROCOBRANCA { get; set; }
        public string UFCOBRANCA { get; set; }
        public string IdUFCOBRANCA { get; set; }
        public string CIDADECOBRANCA { get; set; }
        public string CEPCOBRANCA { get; set; }
        public string TELEFONE1 { get; set; }
        public int QNTOPCIONAIS { get; set; }
        public string QNTFAIXAS { get; set; }
        public string OBSERVACAO { get; set; }

        public long? IDTABELAPRECO { get; set; }
        public long? IDCONTRATO { get; set; }
        public string NOME { get; set; }
        public long? INICIO { get; set; }
        public string VALOR { get; set; }
        public string QNTDFAIXAS { get; set; }
        public long? FIM { get; set; }
        public string guidCustoValor { get; set; }
        public string VALORINDIVIDUAL { get; set; }
        public string VALORCUSTO { get; set; }
        public long? QNTDVIDAS { get; set; }
        public DateTime DATANASCIMENTODep { get; set; }
        public DateTime DATACADASTRO { get; set; }
        public DateTime DATAATUALIZACAO { get; set; }
        public string USUARIOCADASTRO { get; set; }
        public string USUARIOATUALIZACAO { get; set; }

        public string NOME1 { get; set; }
        public long? QNTDVIDAS1 { get; set; }
        public string VALORCUSTO1 { get; set; }
        public string VALORINDIVIDUAL1 { get; set; }

        public string NOME2 { get; set; }
        public long? QNTDVIDAS2 { get; set; }
        public string VALORCUSTO2 { get; set; }
        public string VALORINDIVIDUAL2 { get; set; }

        public string NOME3 { get; set; }
        public long? QNTDVIDAS3 { get; set; }
        public string VALORCUSTO3 { get; set; }
        public string VALORINDIVIDUAL3 { get; set; }

        public string NOME4 { get; set; }
        public long? QNTDVIDAS4 { get; set; }
        public string VALORCUSTO4 { get; set; }
        public string VALORINDIVIDUAL4 { get; set; }

        public string NOME5 { get; set; }
        public long? QNTDVIDAS5 { get; set; }
        public string VALORCUSTO5 { get; set; }
        public string VALORINDIVIDUAL5 { get; set; }

        public string NOME6 { get; set; }
        public long? QNTDVIDAS6 { get; set; }
        public string VALORCUSTO6 { get; set; }
        public string VALORINDIVIDUAL6 { get; set; }

        public string NOME7 { get; set; }
        public long? QNTDVIDAS7 { get; set; }
        public string VALORCUSTO7 { get; set; }
        public string VALORINDIVIDUAL7 { get; set; }

        public string NOME8 { get; set; }
        public long? QNTDVIDAS8 { get; set; }
        public string VALORCUSTO8 { get; set; }
        public string VALORINDIVIDUAL8 { get; set; }

        public string NOME9 { get; set; }
        public long? QNTDVIDAS9 { get; set; }
        public string VALORCUSTO9 { get; set; }
        public string VALORINDIVIDUAL9 { get; set; }

        public string NOME10 { get; set; }
        public long? QNTDVIDAS10 { get; set; }
        public string VALORCUSTO10 { get; set; }
        public string VALORINDIVIDUAL10 { get; set; }

        public string NOME11 { get; set; }
        public long? QNTDVIDAS11 { get; set; }
        public string VALORCUSTO11 { get; set; }
        public string VALORINDIVIDUAL11 { get; set; }

        public string NOME12 { get; set; }
        public long? QNTDVIDAS12 { get; set; }
        public string VALORCUSTO12 { get; set; }
        public string VALORINDIVIDUAL12 { get; set; }

        public string NOME13 { get; set; }
        public long? QNTDVIDAS13 { get; set; }
        public string VALORCUSTO13 { get; set; }
        public string VALORINDIVIDUAL13 { get; set; }

        public string NOME14 { get; set; }
        public long? QNTDVIDAS14 { get; set; }
        public string VALORCUSTO14 { get; set; }
        public string VALORINDIVIDUAL14 { get; set; }

        public string NOME15 { get; set; }
        public long? QNTDVIDAS15 { get; set; }
        public string VALORCUSTO15 { get; set; }
        public string VALORINDIVIDUAL15 { get; set; }

    }
}
