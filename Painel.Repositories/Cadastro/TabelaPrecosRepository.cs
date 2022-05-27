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
    public class TabelaPrecosRepository : OracleRepositoryBase<TabelaPrecos>
    {
        //public TabelaPrecosRepository() : base("servicos_oracle_hom")
        //{
        //}

        public TabelaPrecosRepository() : base("servicos_oracle")
        {
        }

        public List<TabelaPrecos> GetAllPrecos()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT pp.ID, pp.INICIO, pp.FIM, pp.VALOR, pp.STATUS, pc.CODIGO, pc.TIPOCONTRATO, pc.PLANO FROM SERVICOS.COM_PLANOS_PRECOS pp INNER JOIN SERVICOS.COM_PLANOS pc ON pp.IDPLANO = pc.ID ORDER BY  pc.PLANO,  pp.FIM";


                return db.Query<TabelaPrecos>(q).ToList();
            }
        }

        public List<Planos> GetAllPlanosAtivos()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_PLANOS where STATUS = 'Sim'";
                return db.Query<Planos>(q).ToList();
            }
        }


        public virtual bool UpdatePreco(TabelaPrecos dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "ID", value: dto.ID, direction: ParameterDirection.Input);
                    param.Add(name: "IDPLANO", value: dto.IDPLANO, direction: ParameterDirection.Input);
                    param.Add(name: "NOME", value: dto.NOME, direction: ParameterDirection.Input);
                    param.Add(name: "INICIO", value: dto.INICIO, direction: ParameterDirection.Input);
                    param.Add(name: "FIM", value: dto.FIM, direction: ParameterDirection.Input);
                    param.Add(name: "VALOR", value: dto.VALOR, direction: ParameterDirection.Input);
                    param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                    param.Add(name: "DATAATUALIZACAO", value: dto.DATAATUALIZACAO, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOATUALIZACAO", value: dto.USUARIOATUALIZACAO, direction: ParameterDirection.Input);
                    db.Open();

                    var update = db.Execute("UPDATE SERVICOS.COM_PLANOS_PRECOS SET IDPLANO=:IDPLANO, NOME=:NOME, INICIO=:INICIO, FIM=:FIM, VALOR=:VALOR, STATUS=:STATUS,  DATAATUALIZACAO=:DATAATUALIZACAO, USUARIOATUALIZACAO=:USUARIOATUALIZACAO WHERE ID=:ID", param);

                    scope.Complete();

                    return update > 0;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public void InserirPreco(ref TabelaPrecos dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "IDPLANO", value: dto.IDPLANO, direction: ParameterDirection.Input);
                    param.Add(name: "NOME", value: dto.NOME, direction: ParameterDirection.Input);
                    param.Add(name: "INICIO", value: dto.INICIO, direction: ParameterDirection.Input);
                    param.Add(name: "FIM", value: dto.FIM, direction: ParameterDirection.Input);
                    param.Add(name: "VALOR", value: dto.VALOR, direction: ParameterDirection.Input);
                    param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                    param.Add(name: "DATACADASTRO", value: dto.DATACADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOCADASTRO", value: dto.USUARIOCADASTRO, direction: ParameterDirection.Input);
                    db.Open();

                    db.Execute("INSERT INTO SERVICOS.COM_PLANOS_PRECOS (IDPLANO, NOME, INICIO, FIM, VALOR, STATUS, DATACADASTRO, USUARIOCADASTRO) " +
                        " VALUES (:IDPLANO, :NOME, :INICIO, :FIM, :VALOR, :STATUS, :DATACADASTRO, :USUARIOCADASTRO)", param);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        public List<TabelaPrecos> GetByIdPlano(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_PLANOS_PRECOS where IDPLANO ='" + id+"'";
                return db.Query<TabelaPrecos>(q).ToList();
            }
        }

  
        public TabelaPrecos GetByIdPreco(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                db.Open();

                return db.QueryFirstOrDefault<TabelaPrecos>("SELECT * FROM SERVICOS.COM_PLANOS_PRECOS WHERE ID=:ID", param);
            }
        }

        public List<Planos> GetTabelaBynomevalor(long? idplano, int inicial, int final)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "IDPLANO", value: idplano, direction: ParameterDirection.Input);
                param.Add(name: "INICIO", value: inicial, direction: ParameterDirection.Input);
                param.Add(name: "FIM", value: final, direction: ParameterDirection.Input);
                db.Open();

                return db.Query<Planos>("SELECT * FROM SERVICOS.COM_PLANOS_PRECOS WHERE IDPLANO=:IDPLANO and INICIO=:INICIO and FIM =:FIM and STATUS = 'Sim'", param).ToList();
            }
        }

        public List<Planos> GetTabelaByfaixa1(long? idplano, int inicial, int final)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "IDPLANO", value: idplano, direction: ParameterDirection.Input);
                param.Add(name: "INICIO", value: inicial, direction: ParameterDirection.Input);
                param.Add(name: "FIM", value: final, direction: ParameterDirection.Input);
                db.Open();

                string q = " SELECT * FROM SERVICOS.COM_PLANOS_PRECOS ";
                q += " WHERE (:INICIO BETWEEN INICIO AND FIM AND IDPLANO=:IDPLANO and STATUS = 'Sim') ORDER BY ID DESC";
                return db.Query<Planos>(q, param).ToList();
            }
        }

        public List<Planos> GetTabelaByfaixa2(long? idplano, int inicial, int final)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "IDPLANO", value: idplano, direction: ParameterDirection.Input);
                param.Add(name: "INICIO", value: inicial, direction: ParameterDirection.Input);
                param.Add(name: "FIM", value: final, direction: ParameterDirection.Input);
                db.Open();

                string q = " SELECT * FROM SERVICOS.COM_PLANOS_PRECOS ";
                q += " WHERE (FIM BETWEEN :INICIO AND :FIM AND IDPLANO=:IDPLANO and STATUS = 'Sim') ORDER BY ID DESC";
                return db.Query<Planos>(q, param).ToList();
            }
        }

        public List<Planos> GetTabelaByfaixa3(long? id, long? idplano, int inicial, int final)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "IDPLANO", value: idplano, direction: ParameterDirection.Input);
                param.Add(name: "INICIO", value: inicial, direction: ParameterDirection.Input);
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                param.Add(name: "FIM", value: final, direction: ParameterDirection.Input);
                db.Open();

                string q = " SELECT * FROM SERVICOS.COM_PLANOS_PRECOS ";
                q += " WHERE (:INICIO BETWEEN INICIO AND FIM AND IDPLANO=:IDPLANO AND ID !=:ID) ORDER BY ID DESC";
                return db.Query<Planos>(q, param).ToList();
            }
        }

        public List<Planos> GetTabelaByfaixa4(long? id, long? idplano, int inicial, int final)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "IDPLANO", value: idplano, direction: ParameterDirection.Input);
                param.Add(name: "INICIO", value: inicial, direction: ParameterDirection.Input);
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                param.Add(name: "FIM", value: final, direction: ParameterDirection.Input);
                db.Open();

                string q = " SELECT * FROM SERVICOS.COM_PLANOS_PRECOS ";
                q += " WHERE (FIM BETWEEN :INICIO AND :FIM AND IDPLANO=:IDPLANO AND ID !=:ID) ORDER BY ID DESC";
                return db.Query<Planos>(q, param).ToList();
            }
        }



        public List<TabelaPrecos> GetTabelaBynomevalorid(long? id, long? idplano, int inicial, int final)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                param.Add(name: "IDPLANO", value: idplano, direction: ParameterDirection.Input);
                param.Add(name: "INICIO", value: inicial, direction: ParameterDirection.Input);
                param.Add(name: "FIM", value: final, direction: ParameterDirection.Input);
                db.Open();

                return db.Query<TabelaPrecos>("SELECT * FROM SERVICOS.COM_PLANOS_PRECOS WHERE ID !=:ID and IDPLANO =:IDPLANO and INICIO =:INICIO and FIM =:FIM", param).ToList();
            }
        }

    }
}