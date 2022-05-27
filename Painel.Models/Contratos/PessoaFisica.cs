using Painel.Models;
using System;
using System.Collections.Generic;

namespace Painel.Models.Contratos
{
    public class PessoaFisica : BaseEntity
    {
        public long? ID { get; set; }
        public long? IDPLANO { get; set; }
        public string PLANO { get; set; }
        public long? IDTAXAINSCRICAO { get; set; }
        public string TAXAINSCRICAO { get; set; }
        public long? IDTABELAPRECO { get; set; }
        public DateTime DATAVIGENCIACONTRATO { get; set; }
        public string VALORINDIVIDUAL { get; set; }
        public string NUMEROCONTRATO { get; set; }
        public string NUMEROPROPOSTA { get; set; }
        public string VALORTOTAL { get; set; }
        public string TOTALVIDAS { get; set; }
        public string DIAVENCIMENTO { get; set; }
        public DateTime DATAPRIMEIRAFATURA { get; set; }
        public string VALORPRIMEIRAFATURA { get; set; }
        public string TITULAR { get; set; }
        public string ABRANGENCIA { get; set; }
        public string ACOMODACAO { get; set; }
        public string MODALIDADE { get; set; }
        public string CPFTITULAR { get; set; }
        public DateTime DATANASCIMENTO { get; set; }
        public string RESPONSAVELFINANCEIRO { get; set; }
        public string CPFRESPONSAVEL { get; set; }
        public int IDADE { get; set; }
        public int QNTDEPENDETES { get; set; }               
        public int QNTOPCIONAIS { get; set; }
        public string CARTAOSAUDE { get; set; }
        public string SEXO { get; set; }
        public string RG { get; set; }
        public string ORGAOEXPEDIDOR { get; set; }
        public DateTime DATAEXPEDICAO { get; set; }
        public DateTime DATAATENDIMENTO { get; set; }
        public string NOMEDAMAE { get; set; }
        public string EMAIL { get; set; }
        public string ENDERECO { get; set; }
        public string NUMERO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string BAIRRO { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string ENDERECOCOBRANCA { get; set; }
        public string NUMEROCOBRANCA { get; set; }
        public string COMPLEMENTOCOBRANCA { get; set; }
        public string BAIRROCOBRANCA { get; set; }
        public string CIDADECOBRANCA { get; set; }
        public string UFCOBRANCA { get; set; }
        public string CEPCOBRANCA { get; set; }
        public string TELEFONE1 { get; set; }
        public string TELEFONE2 { get; set; }
        public string OBSERVACAO { get; set; }
        public DateTime DATACADASTRO { get; set; }
        public DateTime DATAATUALIZACAO { get; set; }
        public string USUARIOCADASTRO { get; set; }
        public string USUARIOATUALIZACAO { get; set; }
        public List<Opcional> opcionais { get; set; }

    }
}
    