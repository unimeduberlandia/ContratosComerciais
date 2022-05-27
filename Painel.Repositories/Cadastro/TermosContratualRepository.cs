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
    public class TermosContratualRepository : OracleRepositoryBase<TermosContratual>
    {
        //public TermosContratualRepository() : base("servicos_oracle_hom")
        //{
        //}

        public TermosContratualRepository() : base("servicos_oracle")
        {
        }

        public List<TermosContratual> GetAllTermos()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_TERMOS_CONTRATUAIS";


                return db.Query<TermosContratual>(q).ToList();
            }
        }

        public List<TermosContratual> GetTermoByNome(string nome)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "NOME", value: nome, direction: ParameterDirection.Input);
                db.Open();

                return db.Query<TermosContratual>("SELECT * FROM SERVICOS.COM_TERMOS_CONTRATUAIS WHERE NOME =:NOME", param).ToList();
            }
        }

        public List<TermosContratual> GetTermoByNomeId(long? id, string nome)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                param.Add(name: "NOME", value: nome, direction: ParameterDirection.Input);
                db.Open();

                return db.Query<TermosContratual>("SELECT * FROM SERVICOS.COM_TERMOS_CONTRATUAIS WHERE ID !=:ID and NOME =:NOME", param).ToList();
            }
        }

        public virtual bool UpdateTermo(TermosContratual dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "ID", value: dto.ID, direction: ParameterDirection.Input);
                    param.Add(name: "NOME", value: dto.NOME, direction: ParameterDirection.Input);
                    param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                    param.Add(name: "NOMEARQUIVO", value: dto.NOMEARQUIVO, direction: ParameterDirection.Input);
                    param.Add(name: "DATAATUALIZACAO", value: dto.DATAATUALIZACAO, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOATUALIZACAO", value: dto.USUARIOATUALIZACAO, direction: ParameterDirection.Input);
                    db.Open();

                    var update = db.Execute("UPDATE SERVICOS.COM_TERMOS_CONTRATUAIS SET NOME=:NOME, STATUS=:STATUS, NOMEARQUIVO=:NOMEARQUIVO, DATAATUALIZACAO=:DATAATUALIZACAO, USUARIOATUALIZACAO=:USUARIOATUALIZACAO WHERE ID=:ID", param);

                    scope.Complete();

                    return update > 0;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public virtual bool UpdateTermo2(TermosContratual dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "ID", value: dto.ID, direction: ParameterDirection.Input);
                    param.Add(name: "NOME", value: dto.NOME, direction: ParameterDirection.Input);
                    param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                    param.Add(name: "DATAATUALIZACAO", value: dto.DATAATUALIZACAO, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOATUALIZACAO", value: dto.USUARIOATUALIZACAO, direction: ParameterDirection.Input);
                    db.Open();

                    var update = db.Execute("UPDATE SERVICOS.COM_TERMOS_CONTRATUAIS SET NOME=:NOME, STATUS=:STATUS, DATAATUALIZACAO=:DATAATUALIZACAO, USUARIOATUALIZACAO=:USUARIOATUALIZACAO WHERE ID=:ID", param);

                    scope.Complete();

                    return update > 0;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public void InserirTermo(ref TermosContratual dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "NOME", value: dto.NOME, direction: ParameterDirection.Input);
                    param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                    param.Add(name: "NOMEARQUIVO", value: dto.NOMEARQUIVO, direction: ParameterDirection.Input);
                    param.Add(name: "DATACADASTRO", value: dto.DATACADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOCADASTRO", value: dto.USUARIOCADASTRO, direction: ParameterDirection.Input);
                    db.Open();

                    db.Execute("INSERT INTO SERVICOS.COM_TERMOS_CONTRATUAIS (NOME, STATUS, NOMEARQUIVO, DATACADASTRO, USUARIOCADASTRO) " +
                        " VALUES (:NOME, :STATUS, :NOMEARQUIVO, :DATACADASTRO, :USUARIOCADASTRO)", param);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public TermosContratual GetByIdTermo(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                db.Open();

                return db.QueryFirstOrDefault<TermosContratual>("SELECT * FROM SERVICOS.COM_TERMOS_CONTRATUAIS WHERE ID=:ID", param);
            }
        }

    }
}