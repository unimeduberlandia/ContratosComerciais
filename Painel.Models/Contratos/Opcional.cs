using Painel.Models;
using System;
using System.Collections.Generic;

namespace Painel.Models.Contratos
{
    public class Opcional : BaseEntity
    {
        public long? ID { get; set; }
        public long? IDCONTRATO { get; set; }
        public long? IDPLANO { get; set; }
        public long? IDADICIONAIS { get; set; }
        public string NOME { get; set; }
        public string GRUPO { get; set; }
        public string guid { get; set; }
        public string VALOR { get; set; }

        public DateTime DATACADASTRO { get; set; }

        public DateTime DATAATUALIZACAO { get; set; }

        public string USUARIOCADASTRO { get; set; }

        public string USUARIOATUALIZACAO { get; set; }

    }
}
