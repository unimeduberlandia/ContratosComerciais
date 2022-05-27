using Painel.Models;
using System;

namespace Painel.Models.Cadastro
{
    public class Planos : BaseEntity
    {
        public long? id { get; set; }             
        public string PLANO { get; set; }     
        public string CODIGO { get; set; }     
        public string DESCRICAO { get; set; }      
        public string ABRANGENCIA { get; set; }
        public string ACOMODACAO { get; set; }    
        public string MODALIDADE { get; set; }
        public string TIPOCONTRATACAO { get; set; }
        public string FORMACAOPRECO { get; set; }
        public string TIPOCONTRATO { get; set; }
        public string NOMEARQUIVO { get; set; }
        public string NUMEROCONTRATO { get; set; }
        public string STATUS { get; set; }   
        public DateTime DATACADASTRO { get; set; }
        public DateTime DATAATUALIZACAO { get; set; }
        public string USUARIOCADASTRO { get; set; }
        public string USUARIOATUALIZACAO { get; set; }
    }
}
