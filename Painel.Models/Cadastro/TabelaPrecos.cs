using Painel.Models;
using System;

namespace Painel.Models.Cadastro
{
    public class TabelaPrecos : BaseEntity
    {
        public long? ID { get; set; }
        public long? IDPLANO { get; set; }
        public string PLANO { get; set; }
        public string NOME { get; set; }
        public int INICIO { get; set; }
        public string GUID { get; set; }
        public string CODIGO { get; set; }
        public string TIPOCONTRATO { get; set; }
        public int FIM { get; set; }
        public string VALOR { get; set; }
        public string STATUS { get; set; }
        public DateTime DATACADASTRO { get; set; }
        public DateTime DATAATUALIZACAO { get; set; }
        public string USUARIOCADASTRO { get; set; }
        public string USUARIOATUALIZACAO { get; set; }
    }
}
