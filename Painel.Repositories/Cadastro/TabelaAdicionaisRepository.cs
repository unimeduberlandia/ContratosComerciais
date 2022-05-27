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
    public class TabelaAdicionaisRepository : OracleRepositoryBase<TabelaAdicionais>
    {
        //public TabelaAdicionaisRepository() : base("servicos_oracle_hom")
        //{
        //}

        public TabelaAdicionaisRepository() : base("servicos_oracle")
        {
        }

        public List<TabelaAdicionais> GetAllAdicionais()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_SERVICOS_ADICIONAIS";


                return db.Query<TabelaAdicionais>(q).ToList();
            }
        }

        public virtual bool UpdateAdicional2(TabelaAdicionais dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "ID", value: dto.ID, direction: ParameterDirection.Input);
                    param.Add(name: "NOME", value: dto.NOME, direction: ParameterDirection.Input);
                    param.Add(name: "GRUPO", value: dto.GRUPO, direction: ParameterDirection.Input);
                    param.Add(name: "VALOR", value: dto.VALOR, direction: ParameterDirection.Input);
                    param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                    param.Add(name: "DATAATUALIZACAO", value: dto.DATAATUALIZACAO, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOATUALIZACAO", value: dto.USUARIOATUALIZACAO, direction: ParameterDirection.Input);
                    db.Open();

                    var update = db.Execute("UPDATE SERVICOS.COM_SERVICOS_ADICIONAIS SET NOME=:NOME, GRUPO=:GRUPO, VALOR=:VALOR, STATUS=:STATUS, DATAATUALIZACAO=:DATAATUALIZACAO, USUARIOATUALIZACAO=:USUARIOATUALIZACAO WHERE ID=:ID", param);

                    scope.Complete();

                    return update > 0;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public virtual bool UpdateAdicional(TabelaAdicionais dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "ID", value: dto.ID, direction: ParameterDirection.Input);
                    param.Add(name: "NOME", value: dto.NOME, direction: ParameterDirection.Input);
                    param.Add(name: "GRUPO", value: dto.GRUPO, direction: ParameterDirection.Input);
                    param.Add(name: "VALOR", value: dto.VALOR, direction: ParameterDirection.Input);
                    param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                    param.Add(name: "NOMEARQUIVO", value: dto.NOMEARQUIVO, direction: ParameterDirection.Input);
                    param.Add(name: "TEXTOCONTRATO", value: dto.TEXTOCONTRATO, direction: ParameterDirection.Input);
                    param.Add(name: "DATAATUALIZACAO", value: dto.DATAATUALIZACAO, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOATUALIZACAO", value: dto.USUARIOATUALIZACAO, direction: ParameterDirection.Input);
                    db.Open();

                    var update = db.Execute("UPDATE SERVICOS.COM_SERVICOS_ADICIONAIS SET NOME=:NOME, GRUPO=:GRUPO, VALOR=:VALOR, STATUS=:STATUS, TEXTOCONTRATO=:TEXTOCONTRATO, NOMEARQUIVO=:NOMEARQUIVO, DATAATUALIZACAO=:DATAATUALIZACAO, USUARIOATUALIZACAO=:USUARIOATUALIZACAO WHERE ID=:ID", param);

                    scope.Complete();

                    return update > 0;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public void InserirAdicional(ref TabelaAdicionais dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "NOME", value: dto.NOME, direction: ParameterDirection.Input);
                    param.Add(name: "GRUPO", value: dto.GRUPO, direction: ParameterDirection.Input);
                    param.Add(name: "VALOR", value: dto.VALOR, direction: ParameterDirection.Input);
                    param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                    param.Add(name: "NOMEARQUIVO", value: dto.NOMEARQUIVO, direction: ParameterDirection.Input);
                    param.Add(name: "DATACADASTRO", value: dto.DATACADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOCADASTRO", value: dto.USUARIOCADASTRO, direction: ParameterDirection.Input);
                    db.Open();

                    db.Execute("INSERT INTO SERVICOS.COM_SERVICOS_ADICIONAIS (NOME, GRUPO, VALOR, STATUS, NOMEARQUIVO, DATACADASTRO, USUARIOCADASTRO) " +
                        " VALUES (:NOME, :GRUPO, :VALOR, :STATUS, :NOMEARQUIVO, :DATACADASTRO, :USUARIOCADASTRO)", param);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public TabelaAdicionais GetByIdAdicional(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                db.Open();

                return db.QueryFirstOrDefault<TabelaAdicionais>("SELECT * FROM SERVICOS.COM_SERVICOS_ADICIONAIS WHERE ID=:ID", param);
            }
        }

        public List<TabelaAdicionais> GetAdicionalByNomeId(long? id, string nome)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                param.Add(name: "NOME", value: nome, direction: ParameterDirection.Input);
                db.Open();

                return db.Query<TabelaAdicionais>("SELECT * FROM SERVICOS.COM_SERVICOS_ADICIONAIS WHERE ID !=:ID and NOME =:NOME", param).ToList();
            }
        }

        public List<TabelaAdicionais> GetAdicionalByNome(string nome)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "NOME", value: nome, direction: ParameterDirection.Input);
                db.Open();

                return db.Query<TabelaAdicionais>("SELECT * FROM SERVICOS.COM_SERVICOS_ADICIONAIS WHERE NOME =:NOME", param).ToList();
            }
        }

    }
}