using Painel.Models;
using System;

namespace Painel.Models.Contratos
{
    public class PessoaJuridica : BaseEntity
    {
        public long? ID { get; set; }
        public long? IDPLANO { get; set; }
        public long? IDEMPRESA { get; set; }
        public string CNPJ { get; set; }
        public string PLANO { get; set; }
        public string NOMEFANTASIA { get; set; }
        public string ISS { get; set; }
        public string CONTATOINTERNO { get; set; }
        public string INSCRICAOESTADUAL { get; set; }
        public string NUMEROCONTRATO { get; set; }
        public string NUMEROPROPOSTA { get; set; }
        public string GRUPOECONOMICO { get; set; }
        public string RAZAOSOCIAL { get; set; }
        public long? IDTAXAINSCRICAO { get; set; }
        public string TAXADEINSCRICAO { get; set; }
        public string TABELAPRECO { get; set; }
        public string PARTICIPACAOFINANCEIRA { get; set; }
        public DateTime DATAVIGENCIACONTRATO { get; set; }
        public string VALORTOTAL { get; set; }
        public string VALORPRIMEIRAFATURA { get; set; }
        public string TOTALDEVIDAS { get; set; }
        public string DIAVENCIMENTO { get; set; }
        public string ABRANGENCIA { get; set; }
        public string ACOMODACAO { get; set; }
        public string DATAVENCIMENTOMENSAL { get; set; }
        public string MODALIDADE { get; set; }
        public string segundoendereco { get; set; }
        public string Idsegundoendereco { get; set; }
        public string EMAIL { get; set; }
        public string ENDERECO { get; set; }
        public string NUMERO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string BAIRRO { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
        public string idUF { get; set; }
        public string CEP { get; set; }
        public string ENDERECOCOBRANCA { get; set; }
        public string NUMEROCOBRANCA { get; set; }
        public DateTime DATAPRIMEIRAFATURA { get; set; }
        public DateTime DATAATENDIMENTO { get; set; }
        public string COMPLEMENTOCOBRANCA { get; set; }
        public string BAIRROCOBRANCA { get; set; }
        public string UFCOBRANCA { get; set; }
        public string IdUFCOBRANCA { get; set; }
        public string CIDADECOBRANCA { get; set; }
        public string CEPCOBRANCA { get; set; }
        public string TELEFONE1 { get; set; }
        public string TELEFONE2 { get; set; }
        public string QNTOPCIONAIS { get; set; }
        public string QNTFAIXAS { get; set; }
        public string OBSERVACAO { get; set; }
        public DateTime DATACADASTRO { get; set; }
        public DateTime DATAATUALIZACAO { get; set; }
        public string USUARIOCADASTRO { get; set; }
        public string USUARIOATUALIZACAO { get; set; }
    }
}
