using Dapper;
using Oracle.ManagedDataAccess.Client;
using Painel.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

namespace Painel.Repositories
{
    public class PermissaoRepository : OracleRepositoryBase<Permissao>
    {
        //public PermissaoRepository() : base("servicos_oracle_hom")
        //{
        //}
        public PermissaoRepository() : base("servicos_oracle")
        {
        }

        public List<Permissao> GetAll()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT " +
                            "  pe.ID," +
                            "  pe.DESCRICAO," +
                            "  pe.REGRAS," +
                            "  count(upe.USUARIOID) as usuarios " +
                            " FROM " +
                            "  SERVICOS.COM_PERMISSOES pe " +
                            "  left join SERVICOS.COM_USUARIOS_PERMISSOES upe on pe.ID=upe.PERMISSAOID " +
                            " group by " +
                            "  pe.ID,pe.DESCRICAO,pe.REGRAS";

                return db.Query<Permissao>(q).ToList();
            }
        }


        public virtual bool Update(Permissao dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "DESCRICAO", value: dto.Descricao, direction: ParameterDirection.Input);
                param.Add(name: "REGRAS", value: dto.Regras, direction: ParameterDirection.Input);
                param.Add(name: "ID", value: dto.Id, direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_PERMISSOES SET DESCRICAO=:DESCRICAO, REGRAS=:REGRAS WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }
        }

        public bool Apagar(int id)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                    db.Open();

                    db.Execute("DELETE FROM SERVICOS.COM_PERMISSOES WHERE ID=:ID", param);

                    scope.Complete();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public void Insert(ref Permissao dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "Regras", value: dto.Regras, direction: ParameterDirection.Input);
                    param.Add(name: "Descricao", value: dto.Descricao, direction: ParameterDirection.Input);
                    db.Open();

                    db.Execute("INSERT INTO SERVICOS.COM_PERMISSOES (descricao,regras) " +
                        " VALUES (:Descricao,:Regras)", param);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                }
            }
        }

        public Permissao GetById(long id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                db.Open();

                return db.QueryFirstOrDefault<Permissao>("SELECT * FROM SERVICOS.COM_PERMISSOES WHERE ID=:ID", param);
            }
        }

        public void InsertToUser(long usuarioid, int permissaoid)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "USUARIOID", value: usuarioid, direction: ParameterDirection.Input);
                    param.Add(name: "PERMISSAOID", value: permissaoid, direction: ParameterDirection.Input);
                    db.Open();

                    db.Execute("INSERT INTO SERVICOS.COM_USUARIOS_PERMISSOES (usuarioid,permissaoid) " +
                        " VALUES (:USUARIOID,:PERMISSAOID)", param);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                }
            }
        }

        public List<Permissao> GetByUsuarioid(long usuarioid)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "USUARIOID", value: usuarioid, direction: ParameterDirection.Input);
                db.Open();

                return db.Query<Permissao>("SELECT P.* FROM SERVICOS.COM_PERMISSOES P INNER JOIN SERVICOS.COM_USUARIOS_PERMISSOES UP ON P.ID=UP.PERMISSAOID WHERE UP.USUARIOID=:USUARIOID", param).ToList();
            }
        }

        public void DeleteByUsuarioid(long usuarioid)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "USUARIOID", value: usuarioid, direction: ParameterDirection.Input);
                    db.Open();

                    db.Execute("DELETE FROM SERVICOS.COM_USUARIOS_PERMISSOES WHERE USUARIOID=:USUARIOID", param);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}