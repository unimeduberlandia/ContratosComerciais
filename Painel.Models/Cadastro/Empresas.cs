using Painel.Models;
using System;

namespace Painel.Models.Cadastro
{
    public class Empresas : BaseEntity
    {
        public long? ID { get; set; }
        public string CNPJ { get; set; }
        public string RAZAOSOCIAL { get; set; }
        public string NOMEFANTASIA { get; set; }
        public string INSCRICAOESTADUAL { get; set; }
        public string ISS { get; set; }
        public string EMAIL { get; set; }
        public string ENDERECO { get; set; }
        public string NUMERO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string BAIRRO { get; set; }

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
        public DateTime DATACADASTRO { get; set; }
        public DateTime DATAATUALIZACAO { get; set; }
        public DateTime INICIOATIVIDADE { get; set; }

        public string USUARIOCADASTRO { get; set; }
        public string USUARIOATUALIZACAO { get; set; }
    }
}
