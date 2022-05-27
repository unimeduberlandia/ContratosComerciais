
using System;
using System.Collections.Generic;

namespace Painel.Models
{
    public class Usuario : BaseEntity
    {
        public long? idusuario { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string senha { get; set; }
        public string Departamento { get; set; }
        public string Empresa { get; set; }
        public string CPF { get; set; }
        public DateTime? DataUltLogin { get; set; }
        public int NPermissoes { get; set; }
        public List<Permissao> Permissoes { get; set; }
        public int Admin { get; set; }
        public string userstatus { get; set; }
        public long? IdEmpresa { get; set; }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string Email { get; set; }
    }

    public class Empresas : BaseEntity
    {
        public long? id { get; set; }
        public string cnpj { get; set; }
        public string nome { get; set; }
        public string NOMEFANTASIA { get; set; }
        public string status { get; set; }
        public string telefone { get; set; }

    }

    public class NovoUsuario : BaseEntity
    {
        public long? id { get; set; }
        public long? idusuario { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string senha { get; set; }
        public string Departamento { get; set; }
        public string Empresa { get; set; }
        public string CPF { get; set; }
        public DateTime? DataUltLogin { get; set; }
        public int NPermissoes { get; set; }
        public List<Permissao> Permissoes { get; set; }
        public int Admin { get; set; }

        public string userstatus { get; set; }

        //public long? id { get; set; }
        public long? IdEmpresa { get; set; }
        public string Telefone1 { get; set; }
        public string email { get; set; }
        public string Telefone2 { get; set; }
        public string Email { get; set; }
        public DateTime? datacadastro { get; set; }
        public string usuariocadastro { get; set; }
        public DateTime? dataatualizado { get; set; }
    }

}