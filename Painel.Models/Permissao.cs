using Painel.Models;
using System;

namespace Painel.Models
{
    public class Permissao : BaseEntity
    {
        public string Descricao { get; set; }
        public string Regras { get; set; }
        public int? usuarios { get; set; }
    }
}
