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
    public class PlanosRepository : OracleRepositoryBase<Empresas>
    {
        //public PlanosRepository() : base("servicos_oracle_hom")
        //{
        //}

        public PlanosRepository() : base("servicos_oracle")
        {
        }

        public List<Planos> GetAllPlanos()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_PLANOS";
                return db.Query<Planos>(q).ToList();
            }
        }


        public virtual bool UpdateEmpresa(Planos dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: dto.id, direction: ParameterDirection.Input);
                param.Add(name: "PLANO", value: dto.PLANO, direction: ParameterDirection.Input);
                param.Add(name: "CODIGO", value: dto.CODIGO, direction: ParameterDirection.Input);
                param.Add(name: "DESCRICAO", value: dto.DESCRICAO, direction: ParameterDirection.Input);
                param.Add(name: "ABRANGENCIA", value: dto.ABRANGENCIA, direction: ParameterDirection.Input);
                param.Add(name: "ACOMODACAO", value: dto.ACOMODACAO, direction: ParameterDirection.Input);
                param.Add(name: "MODALIDADE", value: dto.MODALIDADE, direction: ParameterDirection.Input);
                param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                param.Add(name: "TIPOCONTRATO", value: dto.TIPOCONTRATO, direction: ParameterDirection.Input);
                param.Add(name: "TIPOCONTRATACAO", value: dto.TIPOCONTRATACAO, direction: ParameterDirection.Input);
                param.Add(name: "FORMACAOPRECO", value: dto.FORMACAOPRECO, direction: ParameterDirection.Input);
                param.Add(name: "NOMEARQUIVO", value: dto.NOMEARQUIVO, direction: ParameterDirection.Input);
                param.Add(name: "DATAATUALIZACAO", value: dto.DATAATUALIZACAO, direction: ParameterDirection.Input);
                param.Add(name: "USUARIOATUALIZACAO", value: dto.USUARIOATUALIZACAO, direction: ParameterDirection.Input);
                param.Add(name: "NUMEROCONTRATO", value: dto.NUMEROCONTRATO, direction: ParameterDirection.Input);
                db.Open();
                
                var update = db.Execute("UPDATE SERVICOS.COM_PLANOS SET PLANO=:PLANO, CODIGO=:CODIGO, DESCRICAO=:DESCRICAO, ABRANGENCIA=:ABRANGENCIA, ACOMODACAO=:ACOMODACAO, MODALIDADE=:MODALIDADE, STATUS=:STATUS, TIPOCONTRATO=:TIPOCONTRATO, TIPOCONTRATACAO=:TIPOCONTRATACAO, FORMACAOPRECO=:FORMACAOPRECO, NOMEARQUIVO=:NOMEARQUIVO, DATAATUALIZACAO=:DATAATUALIZACAO, USUARIOATUALIZACAO=:USUARIOATUALIZACAO, NUMEROCONTRATO=:NUMEROCONTRATO WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }
        }


        public virtual bool UpdateEmpresa2(Planos dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: dto.id, direction: ParameterDirection.Input);
                param.Add(name: "PLANO", value: dto.PLANO, direction: ParameterDirection.Input);
                param.Add(name: "CODIGO", value: dto.CODIGO, direction: ParameterDirection.Input);
                param.Add(name: "DESCRICAO", value: dto.DESCRICAO, direction: ParameterDirection.Input);
                param.Add(name: "ABRANGENCIA", value: dto.ABRANGENCIA, direction: ParameterDirection.Input);
                param.Add(name: "ACOMODACAO", value: dto.ACOMODACAO, direction: ParameterDirection.Input);
                param.Add(name: "TIPOCONTRATO", value: dto.TIPOCONTRATO, direction: ParameterDirection.Input);
                param.Add(name: "MODALIDADE", value: dto.MODALIDADE, direction: ParameterDirection.Input);
                param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                param.Add(name: "DATAATUALIZACAO", value: dto.DATAATUALIZACAO, direction: ParameterDirection.Input);
                param.Add(name: "USUARIOATUALIZACAO", value: dto.USUARIOATUALIZACAO, direction: ParameterDirection.Input);
                param.Add(name: "NUMEROCONTRATO", value: dto.NUMEROCONTRATO, direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_PLANOS SET PLANO=:PLANO, CODIGO=:CODIGO, DESCRICAO=:DESCRICAO, ABRANGENCIA=:ABRANGENCIA, ACOMODACAO=:ACOMODACAO, TIPOCONTRATO=:TIPOCONTRATO, MODALIDADE=:MODALIDADE, STATUS=:STATUS, DATAATUALIZACAO=:DATAATUALIZACAO, USUARIOATUALIZACAO=:USUARIOATUALIZACAO, NUMEROCONTRATO=:NUMEROCONTRATO WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }
        }

        public void InserirPlano(ref Planos dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "PLANO", value: dto.PLANO, direction: ParameterDirection.Input);
                    param.Add(name: "CODIGO", value: dto.CODIGO, direction: ParameterDirection.Input);
                    param.Add(name: "DESCRICAO", value: dto.DESCRICAO, direction: ParameterDirection.Input);
                    param.Add(name: "ABRANGENCIA", value: dto.ABRANGENCIA, direction: ParameterDirection.Input);
                    param.Add(name: "ACOMODACAO", value: dto.ACOMODACAO, direction: ParameterDirection.Input);
                    param.Add(name: "MODALIDADE", value: dto.MODALIDADE, direction: ParameterDirection.Input);
                    param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                    param.Add(name: "TIPOCONTRATO", value: dto.TIPOCONTRATO, direction: ParameterDirection.Input);
                    param.Add(name: "TIPOCONTRATACAO", value: dto.TIPOCONTRATACAO, direction: ParameterDirection.Input);
                    param.Add(name: "FORMACAOPRECO", value: dto.FORMACAOPRECO, direction: ParameterDirection.Input);
                    param.Add(name: "NOMEARQUIVO", value: dto.NOMEARQUIVO, direction: ParameterDirection.Input);
                    param.Add(name: "DATACADASTRO", value: dto.DATACADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOCADASTRO", value: dto.USUARIOCADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "NUMEROCONTRATO", value: dto.NUMEROCONTRATO, direction: ParameterDirection.Input);
                    db.Open();

                    db.Execute("INSERT INTO SERVICOS.COM_PLANOS (PLANO, CODIGO, DESCRICAO, ABRANGENCIA, ACOMODACAO, MODALIDADE, STATUS, TIPOCONTRATO, TIPOCONTRATACAO, FORMACAOPRECO, NOMEARQUIVO, DATACADASTRO, USUARIOCADASTRO, NUMEROCONTRATO) " +
                        " VALUES (:PLANO, :CODIGO, :DESCRICAO, :ABRANGENCIA, :ACOMODACAO, :MODALIDADE, :STATUS, :TIPOCONTRATO, :TIPOCONTRATACAO, :FORMACAOPRECO, :NOMEARQUIVO, :DATACADASTRO, :USUARIOCADASTRO, :NUMEROCONTRATO)", param);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                }
            }
        }

        public Planos GetByIdPlano(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                db.Open();

                return db.QueryFirstOrDefault<Planos>("SELECT * FROM SERVICOS.COM_PLANOS WHERE ID=:ID", param);
            }
        }

        public List<Planos> GetPlanosBynome(String plano)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "PLANO", value: plano, direction: ParameterDirection.Input);
                db.Open();

                return db.Query<Planos>("SELECT * FROM SERVICOS.COM_PLANOS WHERE PLANO=:PLANO", param).ToList();
            }
        }

        public List<Planos> GetPlanosByCodigo(String codigo)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "CODIGO", value: codigo, direction: ParameterDirection.Input);
                db.Open();

                return db.Query<Planos>("SELECT * FROM SERVICOS.COM_PLANOS WHERE CODIGO=:CODIGO", param).ToList();
            }
        }

        public List<Planos> GetPlanosBynomeId(String plano, long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "PLANO", value: plano, direction: ParameterDirection.Input);
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                db.Open();

                return db.Query<Planos>("SELECT * FROM SERVICOS.COM_PLANOS WHERE PLANO=:PLANO and ID !=:ID", param).ToList();
            }
        }

    }
}