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
    public class TaxaInscricaoRepository : OracleRepositoryBase<TaxaInscricao>
    {
        //public TaxaInscricaoRepository() : base("servicos_oracle_hom")
        //{
        //}
        public TaxaInscricaoRepository() : base("servicos_oracle")
        {
        }
        public List<TaxaInscricao> GetAllTaxas()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_TAXA_INSCRICAO";


                return db.Query<TaxaInscricao>(q).ToList();
            }
        }

        public List<TaxaInscricao> GetAllTaxasAtivas()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_TAXA_INSCRICAO WHERE STATUS = 'Sim'";


                return db.Query<TaxaInscricao>(q).ToList();
            }
        }

        public virtual bool UpdateTaxaInscricao(TaxaInscricao dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "ID", value: dto.ID, direction: ParameterDirection.Input);
                    param.Add(name: "NOME", value: dto.NOME, direction: ParameterDirection.Input);
                    param.Add(name: "VALOR", value: dto.VALOR, direction: ParameterDirection.Input);
                    param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                    param.Add(name: "DATAATUALIZACAO", value: dto.DATAATUALIZACAO, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOATUALIZACAO", value: dto.USUARIOATUALIZACAO, direction: ParameterDirection.Input);
                    db.Open();

                    var update = db.Execute("UPDATE SERVICOS.COM_TAXA_INSCRICAO SET NOME=:NOME, VALOR=:VALOR, STATUS=:STATUS,  DATAATUALIZACAO=:DATAATUALIZACAO, USUARIOATUALIZACAO=:USUARIOATUALIZACAO WHERE ID=:ID", param);

                    scope.Complete();

                    return update > 0;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public void InserirTaxaInscricao(ref TaxaInscricao dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "NOME", value: dto.NOME, direction: ParameterDirection.Input);
                    param.Add(name: "VALOR", value: dto.VALOR, direction: ParameterDirection.Input);
                    param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                    param.Add(name: "DATACADASTRO", value: dto.DATACADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOCADASTRO", value: dto.USUARIOCADASTRO, direction: ParameterDirection.Input);
                    db.Open();

                    db.Execute("INSERT INTO SERVICOS.COM_TAXA_INSCRICAO (NOME, VALOR, STATUS, DATACADASTRO, USUARIOCADASTRO) " +
                        " VALUES (:NOME, :VALOR, :STATUS, :DATACADASTRO, :USUARIOCADASTRO)", param);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public TaxaInscricao GetByTaxaId(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                db.Open();

                return db.QueryFirstOrDefault<TaxaInscricao>("SELECT * FROM SERVICOS.COM_TAXA_INSCRICAO WHERE ID=:ID", param);
            }
        }

        public List<TaxaInscricao> GetByTaxaDiferenteId(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                db.Open();

                return db.Query<TaxaInscricao>("SELECT * FROM SERVICOS.COM_TAXA_INSCRICAO WHERE ID !=:ID AND STATUS = 'Sim'", param).ToList();
            }
        }

    }
}