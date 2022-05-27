using Painel.Models;
using System;

namespace Painel.Models.Cadastro
{
    public class TabelaAdicionais : BaseEntity
    {
        public long? ID { get; set; }
        public long? IDADICIONAIS { get; set; }
        public string NOME { get; set; }
        public string IDGRUPO { get; set; }
        public string GRUPO { get; set; }
        public string VALOR { get; set; }
        public string STATUS { get; set; }
        public string TEXTOCONTRATO { get; set; }
        public string NOMEARQUIVO { get; set; }
        public DateTime DATACADASTRO { get; set; }
        public DateTime DATAATUALIZACAO { get; set; }
        public string USUARIOCADASTRO { get; set; }
        public string USUARIOATUALIZACAO { get; set; }
    }
}
