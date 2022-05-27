using Dapper;
using Oracle.ManagedDataAccess.Client;
using Painel.Models.Cadastro;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

namespace Painel.Repositories.Cadastro
{
    public class EmpresasRepository : OracleRepositoryBase<Empresas>
    {
        //public EmpresasRepository() : base("servicos_oracle_hom")
        //{
        //}
        public EmpresasRepository() : base("servicos_oracle")
        {
        }

        public List<Empresas> GetAllEmpresssas()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_EMPRESAS";
                return db.Query<Empresas>(q).ToList();
            }
        }

        public List<Empresas> GetEmpresaByCNPJ(string cnpj)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_EMPRESAS WHERE CNPJ = '" + cnpj + "' ";
                return db.Query<Empresas>(q).ToList();
            }
        }

        public virtual bool UpdateEmpresa(Empresas dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: dto.ID, direction: ParameterDirection.Input);
                param.Add(name: "RAZAOSOCIAL", value: dto.RAZAOSOCIAL, direction: ParameterDirection.Input);
                param.Add(name: "NOMEFANTASIA", value: dto.NOMEFANTASIA, direction: ParameterDirection.Input);
                param.Add(name: "INSCRICAOESTADUAL", value: dto.INSCRICAOESTADUAL, direction: ParameterDirection.Input);
                param.Add(name: "ISS", value: dto.ISS, direction: ParameterDirection.Input);
                param.Add(name: "ENDERECO", value: dto.ENDERECO, direction: ParameterDirection.Input);
                param.Add(name: "NUMERO", value: dto.NUMERO, direction: ParameterDirection.Input);
                param.Add(name: "COMPLEMENTO", value: dto.COMPLEMENTO, direction: ParameterDirection.Input);
                param.Add(name: "BAIRRO", value: dto.BAIRRO, direction: ParameterDirection.Input);
                param.Add(name: "CIDADE", value: dto.CIDADE, direction: ParameterDirection.Input);
                param.Add(name: "UF", value: dto.UF, direction: ParameterDirection.Input);
                param.Add(name: "CEP", value: dto.CEP, direction: ParameterDirection.Input);
                param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                param.Add(name: "EMAIL", value: dto.EMAIL, direction: ParameterDirection.Input);
                param.Add(name: "REPRESENTANTELEGAL", value: dto.REPRESENTANTELEGAL, direction: ParameterDirection.Input);
                param.Add(name: "CPFREPRESENTANTE", value: dto.CPFREPRESENTANTE, direction: ParameterDirection.Input);
                param.Add(name: "RGREPRESENTANTE", value: dto.RGREPRESENTANTE, direction: ParameterDirection.Input);
                param.Add(name: "CONTATO", value: dto.CONTATO, direction: ParameterDirection.Input);
                param.Add(name: "TELEFONE", value: dto.TELEFONE, direction: ParameterDirection.Input);
                param.Add(name: "TELEFONE2", value: dto.TELEFONE2, direction: ParameterDirection.Input);
                param.Add(name: "DATAATUALIZACAO", value: dto.DATAATUALIZACAO, direction: ParameterDirection.Input);
                param.Add(name: "USUARIOATUALIZACAO", value: dto.USUARIOATUALIZACAO, direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_EMPRESAS SET USUARIOATUALIZACAO=:USUARIOATUALIZACAO, DATAATUALIZACAO=:DATAATUALIZACAO, TELEFONE2=:TELEFONE2, TELEFONE=:TELEFONE, CONTATO=:CONTATO, STATUS=:STATUS, EMAIL=:EMAIL, REPRESENTANTELEGAL=:REPRESENTANTELEGAL, CPFREPRESENTANTE=:CPFREPRESENTANTE, RGREPRESENTANTE=:RGREPRESENTANTE, CEP=:CEP, UF=:UF, CIDADE=:CIDADE, BAIRRO=:BAIRRO, COMPLEMENTO=:COMPLEMENTO, NUMERO=:NUMERO, ENDERECO=:ENDERECO, ISS=:ISS, INSCRICAOESTADUAL=:INSCRICAOESTADUAL, RAZAOSOCIAL=:RAZAOSOCIAL, NOMEFANTASIA=:NOMEFANTASIA WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }
        }

        public void InserirEmpresa(ref Empresas dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "CNPJ", value: dto.CNPJ, direction: ParameterDirection.Input);
                    param.Add(name: "RAZAOSOCIAL", value: dto.RAZAOSOCIAL, direction: ParameterDirection.Input);
                    param.Add(name: "NOMEFANTASIA", value: dto.NOMEFANTASIA, direction: ParameterDirection.Input);
                    param.Add(name: "INSCRICAOESTADUAL", value: dto.INSCRICAOESTADUAL, direction: ParameterDirection.Input);
                    param.Add(name: "ISS", value: dto.ISS, direction: ParameterDirection.Input);
                    param.Add(name: "ENDERECO", value: dto.ENDERECO, direction: ParameterDirection.Input);
                    param.Add(name: "NUMERO", value: dto.NUMERO, direction: ParameterDirection.Input);
                    param.Add(name: "COMPLEMENTO", value: dto.COMPLEMENTO, direction: ParameterDirection.Input);
                    param.Add(name: "BAIRRO", value: dto.BAIRRO, direction: ParameterDirection.Input);
                    param.Add(name: "CIDADE", value: dto.CIDADE, direction: ParameterDirection.Input);
                    param.Add(name: "UF", value: dto.UF, direction: ParameterDirection.Input);
                    param.Add(name: "CEP", value: dto.CEP, direction: ParameterDirection.Input);
                    param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                    param.Add(name: "CONTATO", value: dto.CONTATO, direction: ParameterDirection.Input);
                    param.Add(name: "TELEFONE", value: dto.TELEFONE, direction: ParameterDirection.Input);
                    param.Add(name: "TELEFONE2", value: dto.TELEFONE2, direction: ParameterDirection.Input);
                    param.Add(name: "EMAIL", value: dto.EMAIL, direction: ParameterDirection.Input);
                    param.Add(name: "REPRESENTANTELEGAL", value: dto.REPRESENTANTELEGAL, direction: ParameterDirection.Input);
                    param.Add(name: "CPFREPRESENTANTE", value: dto.CPFREPRESENTANTE, direction: ParameterDirection.Input);
                    param.Add(name: "RGREPRESENTANTE", value: dto.RGREPRESENTANTE, direction: ParameterDirection.Input);
                    param.Add(name: "DATACADASTRO", value: dto.DATACADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOCADASTRO", value: dto.USUARIOCADASTRO, direction: ParameterDirection.Input);
                    db.Open();

                    db.Execute("INSERT INTO SERVICOS.COM_EMPRESAS (CNPJ, RAZAOSOCIAL, NOMEFANTASIA, INSCRICAOESTADUAL, ISS, ENDERECO, NUMERO, COMPLEMENTO, BAIRRO, CIDADE, EMAIL, UF, CEP, STATUS, CONTATO, TELEFONE, TELEFONE2, REPRESENTANTELEGAL, CPFREPRESENTANTE, RGREPRESENTANTE, DATACADASTRO, USUARIOCADASTRO) " +
                        " VALUES (:CNPJ, :RAZAOSOCIAL, :NOMEFANTASIA, :INSCRICAOESTADUAL, :ISS, :ENDERECO, :NUMERO, :COMPLEMENTO, :BAIRRO, :CIDADE, :EMAIL, :UF, :CEP, :STATUS, :CONTATO, :TELEFONE, :TELEFONE2, :REPRESENTANTELEGAL, :CPFREPRESENTANTE, :RGREPRESENTANTE, :DATACADASTRO, :USUARIOCADASTRO)", param);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                }
            }
        }

        public Empresas GetByIdEmpresa(long? ID)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: ID, direction: ParameterDirection.Input);
                db.Open();

                return db.QueryFirstOrDefault<Empresas>("SELECT * FROM SERVICOS.COM_EMPRESAS WHERE ID=:ID", param);
            }
        }

        public List<Empresas> GetByCNPJ(string cpnj)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "CNPJ", value: cpnj, direction: ParameterDirection.Input);
                db.Open();

                return db.Query<Empresas>("SELECT * FROM SERVICOS.COM_EMPRESAS WHERE CNPJ=:CNPJ", param).ToList();
            }
        }

        public List<Empresas> GetByCnpjID(string cnpj, long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "CNPJ", value: cnpj, direction: ParameterDirection.Input);
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                db.Open();

                return db.Query<Empresas>("SELECT * FROM SERVICOS.COM_EMPRESAS WHERE CNPJ=:CNPJ and ID !=:ID", param).ToList(); ;
            }
        }

    }
}