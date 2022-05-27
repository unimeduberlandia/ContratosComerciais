using Dapper;
using Oracle.ManagedDataAccess.Client;
using Painel.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Painel.Repositories
{
    public class UsuarioRepository : OracleRepositoryBase<Usuario>
    {
        //public UsuarioRepository() : base("servicos_oracle_hom")
        //{
        //}

        public UsuarioRepository() : base("servicos_oracle")
        {
        }

        public List<Usuario> GetAll()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT ");
                sb.Append("  U.ID,");
                sb.Append("  U.LOGIN,");
                sb.Append("  U.NOME,      ");
                sb.Append("  U.DATAATUALIZADO,");
                sb.Append("  U.DATACADASTRO,");
                sb.Append("  U.DATAULTLOGIN,");
                sb.Append("  (CUE.NOMEFANTASIA) AS Empresa,");
                sb.Append("  U.USERSTATUS,");
                sb.Append("  COUNT(UP.PERMISSAOID) AS NPERMISSOES,");
                sb.Append(" COUNT(UPA.PERMISSAOID) AS ADMIN ");
                sb.Append(" FROM SERVICOS.COM_USUARIOS U LEFT JOIN SERVICOS.COM_USUARIOS_PERMISSOES UP ON U.ID = UP.USUARIOID ");
                sb.Append(" LEFT JOIN SERVICOS.COM_USUARIOS_PERMISSOES UPA ON U.ID = UPA.USUARIOID AND UP.PERMISSAOID = 1  ");
                sb.Append(" INNER JOIN SERVICOS.COM_EMPRESAS CUE ON CUE.ID = U.IDEMPRESA  ");
                sb.Append(" GROUP BY ");
                sb.Append("  U.ID,");
                sb.Append("  U.LOGIN,");
                sb.Append("  U.NOME,      ");
                sb.Append("  U.DATAATUALIZADO,");
                sb.Append("  U.DATACADASTRO,");
                sb.Append("  U.DATAULTLOGIN,");
                sb.Append("  CUE.NOMEFANTASIA,");
                sb.Append("  U.USERSTATUS");

                return db.Query<Usuario>(sb.ToString()).ToList();
            }
        }

        public List<Empresas> GetAllEmpresasAtivas()
        {
            try
            {
                using (var db = new OracleConnection(ConnectionString))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(" SELECT * FROM SERVICOS.COM_EMPRESAS  where STATUS = 'Sim' ORDER BY id DESC");
                    return db.Query<Empresas>(sb.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
     

        public Usuario GetById(long id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                db.Open();

                return db.QueryFirstOrDefault<Usuario>("SELECT * FROM SERVICOS.COM_USUARIOS WHERE ID=:ID", param);
            }
        }

        public Usuario GetByLogin(string login, string senha)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "LOGIN", value: login, direction: ParameterDirection.Input);
                param.Add(name: "SENHA", value: senha, direction: ParameterDirection.Input);
                db.Open();

                return db.QueryFirstOrDefault<Usuario>("SELECT * FROM SERVICOS.COM_USUARIOS WHERE LOGIN=:LOGIN AND SENHA=:SENHA AND USERSTATUS='Ativo'", param);
            }
        }

        public Usuario GetByusuario(string login)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "LOGIN", value: login, direction: ParameterDirection.Input);

                db.Open();

                return db.QueryFirstOrDefault<Usuario>("SELECT * FROM SERVICOS.COM_USUARIOS WHERE LOGIN=:LOGIN", param);
            }
        }

        public List<Usuario> getbycpf(string cpf)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();

                db.Open();

                StringBuilder sb = new StringBuilder();

                sb.Append(" SELECT * FROM SERVICOS.COM_USUARIOS WHERE CPF='" + cpf + "'");

                return db.Query<Usuario>(sb.ToString()).ToList();

            }
        }

        public virtual bool Update(Usuario dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "LOGIN", value: dto.Login, direction: ParameterDirection.Input);
                param.Add(name: "DEPARTAMENTO", value: dto.Departamento, direction: ParameterDirection.Input);
                param.Add(name: "NOME", value: dto.Nome, direction: ParameterDirection.Input);
                param.Add(name: "DATACADASTRO", value: dto.DataCadastro, direction: ParameterDirection.Input);
                param.Add(name: "DATAATUALIZADO", value: dto.DataAtualizado, direction: ParameterDirection.Input);
                param.Add(name: "DATAULTLOGIN", value: dto.DataUltLogin, direction: ParameterDirection.Input);
                param.Add(name: "STATUS", value: dto.Status, direction: ParameterDirection.Input);
                param.Add(name: "ID", value: dto.Id, direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_USUARIOS SET DATAULTLOGIN=:DATAULTLOGIN WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }
        }

        public virtual bool UpdateSenha(Usuario dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "LOGIN", value: dto.Login, direction: ParameterDirection.Input);
                param.Add(name: "SENHA", value: dto.senha, direction: ParameterDirection.Input);
                param.Add(name: "ID", value: dto.idusuario, direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_USUARIOS SET SENHA=:SENHA WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }
        }

        public NovoUsuario getemail(long id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                db.Open();

                return db.QueryFirstOrDefault<NovoUsuario>("SELECT * FROM SERVICOS.COM_USUARIOS WHERE ID=:ID", param);
            }
        }

        public NovoUsuario getemailCpf(string cpf)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "CPF", value: cpf, direction: ParameterDirection.Input);
                db.Open();

                return db.QueryFirstOrDefault<NovoUsuario>("SELECT * FROM SERVICOS.COM_USUARIOS WHERE CPF=:CPF", param);
            }
        }

        public virtual bool Resetsenha(NovoUsuario dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "SENHA", value: dto.senha, direction: ParameterDirection.Input);
                param.Add(name: "ID", value: dto.idusuario, direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_USUARIOS SET SENHA=:SENHA WHERE ID=:ID", param);

                scope.Complete();

                return true;
            }
        }

        //public virtual void Insert(ref Usuario dto)
        //{
        //    using (TransactionScope scope = new TransactionScope())
        //    using (var db = new OracleConnection(ConnectionString))
        //    {
        //        try
        //        {
        //            var param = new DynamicParameters();
        //            param.Add(name: "LOGIN", value: dto.Login, direction: ParameterDirection.Input);
        //            param.Add(name: "DATACADASTRO", value: dto.DataCadastro, direction: ParameterDirection.Input);
        //            param.Add(name: "STATUS", value: dto.Status, direction: ParameterDirection.Input);
        //            param.Add(name: "ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
        //            db.Open();

        //            db.Execute("INSERT INTO SERVICOS.CEN_USUARIOS (LOGIN,DATACADASTRO,STATUS) " +
        //                " VALUES (:LOGIN,:DATACADASTRO,:STATUS) RETURNING ID INTO :ID", param);

        //            var id = param.Get<int>("ID");

        //            dto = GetById(id);

        //            scope.Complete();
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //}

        public virtual bool Updateusuario(NovoUsuario dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "ID", value: dto.id, direction: ParameterDirection.Input);
                    param.Add(name: "IDEMPRESA", value: dto.IdEmpresa, direction: ParameterDirection.Input);
                    param.Add(name: "LOGIN", value: dto.Login, direction: ParameterDirection.Input);
                    param.Add(name: "CPF", value: dto.CPF, direction: ParameterDirection.Input);
                    param.Add(name: "NOME", value: dto.Nome, direction: ParameterDirection.Input);
                    param.Add(name: "TELEFONE1", value: dto.Telefone1, direction: ParameterDirection.Input);
                    param.Add(name: "TELEFONE2", value: dto.Telefone2, direction: ParameterDirection.Input);
                    param.Add(name: "EMAIL", value: dto.Email, direction: ParameterDirection.Input);
                    param.Add(name: "USERSTATUS", value: dto.userstatus, direction: ParameterDirection.Input);
                    param.Add(name: "DATAATUALIZADO", value: dto.dataatualizado, direction: ParameterDirection.Input);
                    db.Open();

                    var update = db.Execute("UPDATE SERVICOS.COM_USUARIOS SET IDEMPRESA=:IDEMPRESA, LOGIN=:LOGIN, CPF=:CPF, NOME=:NOME, TELEFONE1=:TELEFONE1, TELEFONE2=:TELEFONE2, EMAIL=:EMAIL, USERSTATUS=:USERSTATUS, DATAATUALIZADO=:DATAATUALIZADO WHERE ID=:ID", param);
                    scope.Complete();

                    return update > 0;

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        public virtual void InsertUsuario(ref NovoUsuario dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "IDEMPRESA", value: dto.IdEmpresa, direction: ParameterDirection.Input);
                    param.Add(name: "LOGIN", value: dto.Login, direction: ParameterDirection.Input);
                    param.Add(name: "CPF", value: dto.CPF, direction: ParameterDirection.Input);
                    param.Add(name: "SENHA", value: dto.senha, direction: ParameterDirection.Input);
                    param.Add(name: "NOME", value: dto.Nome, direction: ParameterDirection.Input);
                    param.Add(name: "TELEFONE1", value: dto.Telefone1, direction: ParameterDirection.Input);
                    param.Add(name: "TELEFONE2", value: dto.Telefone2, direction: ParameterDirection.Input);
                    param.Add(name: "EMAIL", value: dto.Email, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOCADASTRO", value: dto.usuariocadastro, direction: ParameterDirection.Input);
                    param.Add(name: "DATACADASTRO", value: dto.datacadastro, direction: ParameterDirection.Input);
                    param.Add(name: "USERSTATUS", value: dto.userstatus, direction: ParameterDirection.Input);
                    param.Add(name: "ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    db.Open();

                    db.Execute("INSERT INTO SERVICOS.COM_USUARIOS (IDEMPRESA,LOGIN, CPF, SENHA, NOME, TELEFONE1, TELEFONE2, EMAIL, USUARIOCADASTRO, DATACADASTRO, USERSTATUS) " +
                        " VALUES (:IDEMPRESA,:LOGIN,:CPF,:SENHA,:NOME,:TELEFONE1,:TELEFONE2,:EMAIL,:USUARIOCADASTRO,:DATACADASTRO,:USERSTATUS) RETURNING ID INTO :ID", param);

                    var id = param.Get<int>("ID");

                    dto.idusuario = (id);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}