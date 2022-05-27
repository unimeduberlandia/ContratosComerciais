using Dapper;
using Oracle.ManagedDataAccess.Client;
using Painel.Models.Contratos;
using Painel.Models.Cadastro;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

namespace Painel.Repositories.Contratos
{
    public class PessoaFisicaRepository : OracleRepositoryBase<PessoaFisica>
    {
        //public PessoaFisicaRepository() : base("servicos_oracle_hom")
        //{
        //}

        public PessoaFisicaRepository() : base("servicos_oracle")
        {
        }

        public List<PessoaFisica> GetAllContratos(string dataInicial, string dataFinal)
        {
            var datai = Convert.ToDateTime(dataInicial);
            var dataf = Convert.ToDateTime(dataFinal);
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT cf.ID, cf.TITULAR, cf.CPFTITULAR, cf.TOTALVIDAS, cf.VALORTOTAL, cf.QNTOPCIONAIS, cf.DATAATENDIMENTO, cf.DATACADASTRO, cn.PLANO, cn.MODALIDADE FROM SERVICOS.COM_CONTRATOS_PF cf INNER JOIN SERVICOS.COM_PLANOS cn ON cn.ID = cf.IDPLANO where cf.DATACADASTRO BETWEEN '" + datai + "' AND '" + dataf + "'";
                return db.Query<PessoaFisica>(q).ToList();
            }
        }

        public List<PessoaFisica> GetContratoslogin(string usuario, string dataInicial, string dataFinal)
        {

            var datai = Convert.ToDateTime(dataInicial);
            var dataf = Convert.ToDateTime(dataFinal);

            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT cf.ID, cf.TITULAR, cf.CPFTITULAR, cf.TOTALVIDAS, cf.VALORTOTAL, cf.QNTOPCIONAIS, cf.DATAATENDIMENTO, cf.DATACADASTRO, cn.PLANO, cn.MODALIDADE FROM SERVICOS.COM_CONTRATOS_PF cf INNER JOIN SERVICOS.COM_PLANOS cn ON cn.ID = cf.IDPLANO where cf.USUARIOCADASTRO = '" + usuario + "'  and cf.DATACADASTRO BETWEEN '" + datai + "' AND '" + dataf + "'";
                return db.Query<PessoaFisica>(q).ToList();
            }
        }


        public List<PessoaFisica> GetDadosTitular(string cpftitular)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_CONTRATOS_PF WHERE CPFTITULAR = '" + cpftitular + "' ";
                return db.Query<PessoaFisica>(q).ToList();
            }
        }

        public List<PessoaFisica> GetValorPlano(long? id, int idade)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    string q = "SELECT (pp.VALOR) as VALORINDIVIDUAL, cp.ABRANGENCIA, cp.ACOMODACAO, cp.MODALIDADE, cp.NUMEROCONTRATO " +
                               "FROM SERVICOS.COM_PLANOS_PRECOS pp " +
                               "INNER JOIN SERVICOS.COM_PLANOS cp ON cp.id = pp.IDPLANO " +
                               "WHERE pp.IDPLANO = '" + id + "' and '" + idade + "' BETWEEN pp.INICIO AND pp.FIM";
                    return db.Query<PessoaFisica>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public List<TabelaAdicionais> GetValorAdicional(long? idadicional)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {

                    string q = "SELECT NOME, GRUPO, VALOR FROM SERVICOS.COM_SERVICOS_ADICIONAIS WHERE ID = '" + idadicional + "' ";
                    return db.Query<TabelaAdicionais>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public List<Planos> GetAllPlanosPFAtivos()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_PLANOS where STATUS = 'Sim' AND TIPOCONTRATO = 'PF' ORDER BY PLANO ";
                return db.Query<Planos>(q).ToList();
            }
        }

        public List<PDFPessoaFisica> GetEspelho(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = " select STATUSESPELHO FROM SERVICOS.COM_CONTRATOS_PF where id='" + id + "'"; ;

                    return db.Query<PDFPessoaFisica>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public PDFPessoaFisica GetUsuariobyCPF(string cpf)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "CPF", value: cpf, direction: ParameterDirection.Input);
                db.Open();

                return db.QueryFirstOrDefault<PDFPessoaFisica>("SELECT (EMAIL)as EMAILVENDEDOR FROM SERVICOS.COM_USUARIOS WHERE CPF=:CPF", param);
            }
        }

        public virtual bool UpdateStatusPF(long? id)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {

                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                param.Add(name: "STATUSESPELHO", value: "Gerado", direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_CONTRATOS_PF SET STATUSESPELHO=:STATUSESPELHO WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }

        }

        public virtual bool UpdateStatusContratoPF(long? id)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {

                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                param.Add(name: "STATUSCONTRATO", value: "Gerado", direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_CONTRATOS_PF SET STATUSCONTRATO=:STATUSCONTRATO WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }

        }
        public virtual bool UpdateQualificacaoPF(long? id)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {

                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                param.Add(name: "STATUSQUALIFICACAO", value: "Gerado", direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_CONTRATOS_PF SET STATUSQUALIFICACAO=:STATUSQUALIFICACAO WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }

        }

        public virtual bool UpdateAssinaturaPF(long? id)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {

                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                param.Add(name: "STATUSASSINATURA", value: "Gerado", direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_CONTRATOS_PF SET STATUSASSINATURA=:STATUSASSINATURA WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }

        }

        public List<PDFPessoaFisica> GetIdPlano(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = "select pf.datavigenciacontrato,pf.numerocontrato,pf.numeroproposta,pf.diavencimento,pf.valorindividual,pf.valortotal,pf.valorprimeirafatura,pf.titular,pf.abrangencia,pf.acomodacao,pf.modalidade,pf.taxainscricao " +
                                ",pf.totalvidas, pf.dataatendimento,pf.responsavelfinanceiro,pf.cpfresponsavel,pf.dataprimeirafatura,pf.cpftitular,pf.datanascimento,pf.idade,pf.qntopcionais,pf.qntdependetes,pf.statuscontrato,pf.cartaosaude,pf.sexo" +
                                ",pf.rg,pf.orgaoexpedidor,pf.dataexpedicao,pf.nomedamae,pf.email,pf.enderecocobranca,pf.numerocobranca,pf.complementocobranca" +
                                ",pf.bairrocobranca,pf.cidadecobranca,pf.ufcobranca,pf.cepcobranca,pf.telefone1,pf.telefone2,pf.observacao,p.plano,(p.codigo)as CODIGO, p.NOMEARQUIVO  from servicos.COM_CONTRATOS_PF pf inner join servicos.COM_PLANOS p on p.id = pf.idplano where pf.id = '" + id + "'";

                    return db.Query<PDFPessoaFisica>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public List<PDFPessoaFisica> GetIdQualificãcoes(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = " select * from servicos.COM_CONTRATOS_PF  where id='" + id + "'"; ;

                    return db.Query<PDFPessoaFisica>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public List<PDFPessoaFisica> GetIdQualificacao(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = "select p.plano,(p.codigo)as CODIGOANS,p.tipocontratacao,(p.descricao) as SEGMENTACAO,p.acomodacao,p.abrangencia,p.formacaopreco,p.modalidade,pf.titular,pf.endereco,pf.numero,pf.complemento,pf.bairro,pf.cidade,pf.uf,pf.cpftitular,pf.rg,pf.telefone1,pf.email,pf.responsavelfinanceiro,pf.numerocontrato,pf.cpfresponsavel from servicos.COM_CONTRATOS_PF pf inner join servicos.COM_PLANOS p on p.id = pf.idplano where pf.id = '" + id + "'";

                    return db.Query<PDFPessoaFisica>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }


        public List<PDFPessoaFisica> GetIdAssinatura(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = "select p.plano,(p.codigo)as CODIGOANS,p.tipocontratacao,(p.descricao) as SEGMENTACAO,p.acomodacao,p.abrangencia,p.formacaopreco,p.modalidade,pf.DATAVIGENCIACONTRATO,pf.titular,pf.endereco,pf.numero,pf.complemento,pf.bairro,pf.cidade,pf.uf,pf.cpftitular,pf.rg,pf.telefone1,pf.email,pf.responsavelfinanceiro,pf.numerocontrato,pf.cpfresponsavel from servicos.COM_CONTRATOS_PF pf inner join servicos.COM_PLANOS p on p.id = pf.idplano where pf.id = '" + id + "'";

                    return db.Query<PDFPessoaFisica>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }
        public List<PDFPessoaFisica> GetallopcionaisAtivos(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = "select pf.qntopcionais,op.idadicionais,ad.nomearquivo from servicos.com_contratos_pf pf inner join servicos.COM_OPCIONAIS_PF op on op.idcontrato = pf.id inner join servicos.COM_SERVICOS_ADICIONAIS ad on ad.id = op.idadicionais where pf.id = '" + id + "'";

                    return db.Query<PDFPessoaFisica>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public List<PDFPessoaFisica> Getemailbyid(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = "select  email, usuariocadastro, USUARIOATUALIZACAO from SERVICOS.COM_CONTRATOS_PF where id = '" + id + "'";

                    return db.Query<PDFPessoaFisica>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public List<PDFPessoaFisica> GetallTermosAtivos()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = "select (nome)as NOMETERMO, (nomearquivo)as ARQUIVOTERMO from servicos.COM_TERMOS_CONTRATUAIS where STATUS = 'Sim'";

                    return db.Query<PDFPessoaFisica>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public List<PDFPessoaFisica> GetIdPlanoOpcionais(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = "select pf.QNTOPCIONAIS,pf.IDPLANO,pf.NUMEROCONTRATO,pf.NUMEROPROPOSTA,op.nome,op.grupo,op.valor,sa.textocontrato FROM Servicos.COM_CONTRATOS_PF pf inner join Servicos.COM_OPCIONAIS_PF op on op.IDCONTRATO = pf.id left join Servicos.COM_SERVICOS_ADICIONAIS sa on sa.id = op.idadicionais where pf.id = '" + id + "'";

                    return db.Query<PDFPessoaFisica>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public Planos GetPlanoByID(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                db.Open();

                return db.QueryFirstOrDefault<Planos>("SELECT * FROM SERVICOS.COM_PLANOS WHERE ID=:ID", param);
            }
        }

        public List<Opcional> GetOpcionalByPlano(long? idplano)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_OPCIONAIS_PF where IDCONTRATO = '" + idplano + "'";
                return db.Query<Opcional>(q).ToList();
            }
        }

        public List<Dependente> GetDependenteByPlano(long? idplano)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_DEPENDENTES_PF where IDCONTRATO = '" + idplano + "'";
                return db.Query<Dependente>(q).ToList();
            }
        }

        public List<TabelaAdicionais> GetAllAdicionaisAtivos()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT (ID)AS IDADICIONAIS, NOME, GRUPO, VALOR FROM SERVICOS.COM_SERVICOS_ADICIONAIS where STATUS = 'Sim' ORDER BY NOME";
                return db.Query<TabelaAdicionais>(q).ToList();
            }
        }

        public List<TaxaInscricao> GetTaxaAtiva()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT ID, VALOR FROM SERVICOS.COM_TAXA_INSCRICAO where STATUS = 'Sim'";
                return db.Query<TaxaInscricao>(q).ToList();
            }
        }

        public List<PessoaFisica> GetContratoByID(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_CONTRATOS_PF where ID ='" + id + "' ";
                return db.Query<PessoaFisica>(q).ToList();
            }
        }

        public List<Opcional> GetOpcionaisByIDContrato(long? idcontrato)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_OPCIONAIS_PF where IDCONTRATO ='" + idcontrato + "' ";
                return db.Query<Opcional>(q).ToList();
            }
        }

        public List<Dependente> GetDependentesByIDContrato(long? idcontrato)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_DEPENDENTES_PF where IDCONTRATO ='" + idcontrato + "' ";
                return db.Query<Dependente>(q).ToList();
            }
        }

        public PDFPessoaJuridica GetUsuariobysCPF(string cpf)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "CPF", value: cpf, direction: ParameterDirection.Input);
                db.Open();

                return db.QueryFirstOrDefault<PDFPessoaJuridica>("SELECT (EMAIL)as EMAILVENDEDOR, (NOME) as USUARIOCADASTRO FROM SERVICOS.COM_USUARIOS WHERE CPF=:CPF", param);
            }
        }

        public virtual void InsertContratoPF(ref PessoaFisica dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "IDTAXAINSCRICAO", value: dto.IDTAXAINSCRICAO, direction: ParameterDirection.Input);
                    param.Add(name: "CPFTITULAR", value: dto.CPFTITULAR, direction: ParameterDirection.Input);
                    param.Add(name: "TITULAR", value: dto.TITULAR, direction: ParameterDirection.Input);
                    param.Add(name: "DATANASCIMENTO", value: dto.DATANASCIMENTO, direction: ParameterDirection.Input);
                    param.Add(name: "SEXO", value: dto.SEXO, direction: ParameterDirection.Input);
                    param.Add(name: "NOMEDAMAE", value: dto.NOMEDAMAE, direction: ParameterDirection.Input);
                    param.Add(name: "CARTAOSAUDE", value: dto.CARTAOSAUDE, direction: ParameterDirection.Input);
                    param.Add(name: "RG", value: dto.RG, direction: ParameterDirection.Input);
                    param.Add(name: "ORGAOEXPEDIDOR", value: dto.ORGAOEXPEDIDOR, direction: ParameterDirection.Input);
                    param.Add(name: "DATAEXPEDICAO", value: dto.DATAEXPEDICAO, direction: ParameterDirection.Input);
                    param.Add(name: "EMAIL", value: dto.EMAIL, direction: ParameterDirection.Input);
                    param.Add(name: "TELEFONE1", value: dto.TELEFONE1, direction: ParameterDirection.Input);
                    param.Add(name: "TELEFONE2", value: dto.TELEFONE2, direction: ParameterDirection.Input);
                    param.Add(name: "RESPONSAVELFINANCEIRO", value: dto.RESPONSAVELFINANCEIRO, direction: ParameterDirection.Input);
                    param.Add(name: "CPFRESPONSAVEL", value: dto.CPFRESPONSAVEL, direction: ParameterDirection.Input);
                    param.Add(name: "IDADE", value: dto.IDADE, direction: ParameterDirection.Input);
                    param.Add(name: "IDPLANO", value: dto.IDPLANO, direction: ParameterDirection.Input);
                    param.Add(name: "NUMEROCONTRATO", value: dto.NUMEROCONTRATO, direction: ParameterDirection.Input);
                    param.Add(name: "NUMEROPROPOSTA", value: dto.NUMEROPROPOSTA, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL", value: dto.VALORINDIVIDUAL, direction: ParameterDirection.Input);
                    param.Add(name: "ABRANGENCIA", value: dto.ABRANGENCIA, direction: ParameterDirection.Input);
                    param.Add(name: "ACOMODACAO", value: dto.ACOMODACAO, direction: ParameterDirection.Input);
                    param.Add(name: "MODALIDADE", value: dto.MODALIDADE, direction: ParameterDirection.Input);
                    param.Add(name: "CEP", value: dto.CEP, direction: ParameterDirection.Input);
                    param.Add(name: "ENDERECO", value: dto.ENDERECO, direction: ParameterDirection.Input);
                    param.Add(name: "NUMERO", value: dto.NUMERO, direction: ParameterDirection.Input);
                    param.Add(name: "BAIRRO", value: dto.BAIRRO, direction: ParameterDirection.Input);
                    param.Add(name: "COMPLEMENTO", value: dto.COMPLEMENTO, direction: ParameterDirection.Input);
                    param.Add(name: "CIDADE", value: dto.CIDADE, direction: ParameterDirection.Input);
                    param.Add(name: "UF", value: dto.UF, direction: ParameterDirection.Input);
                    param.Add(name: "CEPCOBRANCA", value: dto.CEPCOBRANCA, direction: ParameterDirection.Input);
                    param.Add(name: "ENDERECOCOBRANCA", value: dto.ENDERECOCOBRANCA, direction: ParameterDirection.Input);
                    param.Add(name: "NUMEROCOBRANCA", value: dto.NUMEROCOBRANCA, direction: ParameterDirection.Input);
                    param.Add(name: "BAIRROCOBRANCA", value: dto.BAIRROCOBRANCA, direction: ParameterDirection.Input);
                    param.Add(name: "COMPLEMENTOCOBRANCA", value: dto.COMPLEMENTOCOBRANCA, direction: ParameterDirection.Input);
                    param.Add(name: "CIDADECOBRANCA", value: dto.CIDADECOBRANCA, direction: ParameterDirection.Input);
                    param.Add(name: "UFCOBRANCA", value: dto.UFCOBRANCA, direction: ParameterDirection.Input);
                    param.Add(name: "TAXAINSCRICAO", value: dto.TAXAINSCRICAO, direction: ParameterDirection.Input);
                    param.Add(name: "TOTALVIDAS", value: dto.TOTALVIDAS, direction: ParameterDirection.Input);
                    param.Add(name: "DIAVENCIMENTO", value: dto.DIAVENCIMENTO, direction: ParameterDirection.Input);
                    param.Add(name: "DATAPRIMEIRAFATURA", value: dto.DATAPRIMEIRAFATURA, direction: ParameterDirection.Input);
                    param.Add(name: "VALORTOTAL", value: dto.VALORTOTAL, direction: ParameterDirection.Input);
                    param.Add(name: "VALORPRIMEIRAFATURA", value: dto.VALORPRIMEIRAFATURA, direction: ParameterDirection.Input);
                    param.Add(name: "OBSERVACAO", value: dto.OBSERVACAO, direction: ParameterDirection.Input);
                    param.Add(name: "DATAVIGENCIACONTRATO", value: dto.DATAVIGENCIACONTRATO, direction: ParameterDirection.Input);
                    param.Add(name: "DATAATENDIMENTO", value: dto.DATAATENDIMENTO, direction: ParameterDirection.Input);
                    param.Add(name: "QNTOPCIONAIS", value: dto.QNTOPCIONAIS, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDEPENDETES", value: dto.QNTDEPENDETES, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOCADASTRO", value: dto.USUARIOCADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "DATACADASTRO", value: dto.DATACADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    db.Open();

                    db.Execute("INSERT INTO SERVICOS.COM_CONTRATOS_PF (IDTAXAINSCRICAO,CPFTITULAR,TITULAR,DATANASCIMENTO,SEXO,NOMEDAMAE,CARTAOSAUDE,RG,ORGAOEXPEDIDOR,DATAEXPEDICAO,EMAIL,TELEFONE1,TELEFONE2,RESPONSAVELFINANCEIRO,CPFRESPONSAVEL,IDADE,IDPLANO,NUMEROCONTRATO,NUMEROPROPOSTA,VALORINDIVIDUAL,ABRANGENCIA,ACOMODACAO,MODALIDADE,CEP,ENDERECO,NUMERO,BAIRRO,COMPLEMENTO,CIDADE,UF,CEPCOBRANCA,ENDERECOCOBRANCA,NUMEROCOBRANCA,BAIRROCOBRANCA,COMPLEMENTOCOBRANCA,CIDADECOBRANCA,UFCOBRANCA,TAXAINSCRICAO,TOTALVIDAS,DIAVENCIMENTO,DATAPRIMEIRAFATURA,VALORTOTAL,VALORPRIMEIRAFATURA,OBSERVACAO,DATAVIGENCIACONTRATO ,DATAATENDIMENTO,QNTOPCIONAIS,QNTDEPENDETES,USUARIOCADASTRO,DATACADASTRO) " +
                        " VALUES (:IDTAXAINSCRICAO,:CPFTITULAR,:TITULAR,:DATANASCIMENTO,:SEXO,:NOMEDAMAE,:CARTAOSAUDE,:RG,:ORGAOEXPEDIDOR,:DATAEXPEDICAO,:EMAIL,:TELEFONE1,:TELEFONE2,:RESPONSAVELFINANCEIRO,:CPFRESPONSAVEL,:IDADE,:IDPLANO,:NUMEROCONTRATO,:NUMEROPROPOSTA,:VALORINDIVIDUAL,:ABRANGENCIA,:ACOMODACAO,:MODALIDADE,:CEP,:ENDERECO,:NUMERO,:BAIRRO,:COMPLEMENTO,:CIDADE,:UF,:CEPCOBRANCA,:ENDERECOCOBRANCA,:NUMEROCOBRANCA,:BAIRROCOBRANCA,:COMPLEMENTOCOBRANCA,:CIDADECOBRANCA,:UFCOBRANCA,:TAXAINSCRICAO,:TOTALVIDAS,:DIAVENCIMENTO,:DATAPRIMEIRAFATURA,:VALORTOTAL,:VALORPRIMEIRAFATURA,:OBSERVACAO,:DATAVIGENCIACONTRATO,:DATAATENDIMENTO,:QNTOPCIONAIS,:QNTDEPENDETES,:USUARIOCADASTRO,:DATACADASTRO) RETURNING ID INTO :ID", param);

                    dto.ID = param.Get<int>("ID");

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public virtual bool UpdateContratoPF(PessoaFisica dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {

                var param = new DynamicParameters();
                param.Add(name: "ID", value: dto.ID, direction: ParameterDirection.Input);
                param.Add(name: "CPFTITULAR", value: dto.CPFTITULAR, direction: ParameterDirection.Input);
                param.Add(name: "TITULAR", value: dto.TITULAR, direction: ParameterDirection.Input);
                param.Add(name: "DATANASCIMENTO", value: dto.DATANASCIMENTO, direction: ParameterDirection.Input);
                param.Add(name: "SEXO", value: dto.SEXO, direction: ParameterDirection.Input);
                param.Add(name: "NOMEDAMAE", value: dto.NOMEDAMAE, direction: ParameterDirection.Input);
                param.Add(name: "CARTAOSAUDE", value: dto.CARTAOSAUDE, direction: ParameterDirection.Input);
                param.Add(name: "RG", value: dto.RG, direction: ParameterDirection.Input);
                param.Add(name: "ORGAOEXPEDIDOR", value: dto.ORGAOEXPEDIDOR, direction: ParameterDirection.Input);
                param.Add(name: "DATAEXPEDICAO", value: dto.DATAEXPEDICAO, direction: ParameterDirection.Input);
                param.Add(name: "EMAIL", value: dto.EMAIL, direction: ParameterDirection.Input);
                param.Add(name: "TELEFONE1", value: dto.TELEFONE1, direction: ParameterDirection.Input);
                param.Add(name: "TELEFONE2", value: dto.TELEFONE2, direction: ParameterDirection.Input);
                param.Add(name: "RESPONSAVELFINANCEIRO", value: dto.RESPONSAVELFINANCEIRO, direction: ParameterDirection.Input);
                param.Add(name: "CPFRESPONSAVEL", value: dto.CPFRESPONSAVEL, direction: ParameterDirection.Input);
                param.Add(name: "IDADE", value: dto.IDADE, direction: ParameterDirection.Input);
                param.Add(name: "IDPLANO", value: dto.IDPLANO, direction: ParameterDirection.Input);
                param.Add(name: "NUMEROCONTRATO", value: dto.NUMEROCONTRATO, direction: ParameterDirection.Input);
                param.Add(name: "NUMEROPROPOSTA", value: dto.NUMEROPROPOSTA, direction: ParameterDirection.Input);
                param.Add(name: "VALORINDIVIDUAL", value: dto.VALORINDIVIDUAL, direction: ParameterDirection.Input);
                param.Add(name: "ABRANGENCIA", value: dto.ABRANGENCIA, direction: ParameterDirection.Input);
                param.Add(name: "ACOMODACAO", value: dto.ACOMODACAO, direction: ParameterDirection.Input);
                param.Add(name: "MODALIDADE", value: dto.MODALIDADE, direction: ParameterDirection.Input);
                param.Add(name: "CEP", value: dto.CEP, direction: ParameterDirection.Input);
                param.Add(name: "ENDERECO", value: dto.ENDERECO, direction: ParameterDirection.Input);
                param.Add(name: "NUMERO", value: dto.NUMERO, direction: ParameterDirection.Input);
                param.Add(name: "BAIRRO", value: dto.BAIRRO, direction: ParameterDirection.Input);
                param.Add(name: "COMPLEMENTO", value: dto.COMPLEMENTO, direction: ParameterDirection.Input);
                param.Add(name: "CIDADE", value: dto.CIDADE, direction: ParameterDirection.Input);
                param.Add(name: "UF", value: dto.UF, direction: ParameterDirection.Input);
                param.Add(name: "CEPCOBRANCA", value: dto.CEPCOBRANCA, direction: ParameterDirection.Input);
                param.Add(name: "ENDERECOCOBRANCA", value: dto.ENDERECOCOBRANCA, direction: ParameterDirection.Input);
                param.Add(name: "NUMEROCOBRANCA", value: dto.NUMEROCOBRANCA, direction: ParameterDirection.Input);
                param.Add(name: "BAIRROCOBRANCA", value: dto.BAIRROCOBRANCA, direction: ParameterDirection.Input);
                param.Add(name: "COMPLEMENTOCOBRANCA", value: dto.COMPLEMENTOCOBRANCA, direction: ParameterDirection.Input);
                param.Add(name: "CIDADECOBRANCA", value: dto.CIDADECOBRANCA, direction: ParameterDirection.Input);
                param.Add(name: "UFCOBRANCA", value: dto.UFCOBRANCA, direction: ParameterDirection.Input);
                param.Add(name: "TAXAINSCRICAO", value: dto.TAXAINSCRICAO, direction: ParameterDirection.Input);
                param.Add(name: "TOTALVIDAS", value: dto.TOTALVIDAS, direction: ParameterDirection.Input);
                param.Add(name: "DIAVENCIMENTO", value: dto.DIAVENCIMENTO, direction: ParameterDirection.Input);
                param.Add(name: "DATAPRIMEIRAFATURA", value: dto.DATAPRIMEIRAFATURA, direction: ParameterDirection.Input);
                param.Add(name: "VALORTOTAL", value: dto.VALORTOTAL, direction: ParameterDirection.Input);
                param.Add(name: "VALORPRIMEIRAFATURA", value: dto.VALORPRIMEIRAFATURA, direction: ParameterDirection.Input);
                param.Add(name: "OBSERVACAO", value: dto.OBSERVACAO, direction: ParameterDirection.Input);
                param.Add(name: "DATAVIGENCIACONTRATO", value: dto.DATAVIGENCIACONTRATO, direction: ParameterDirection.Input);
                param.Add(name: "DATAATENDIMENTO", value: dto.DATAATENDIMENTO, direction: ParameterDirection.Input);
                param.Add(name: "QNTOPCIONAIS", value: dto.QNTOPCIONAIS, direction: ParameterDirection.Input);
                param.Add(name: "QNTDEPENDETES", value: dto.QNTDEPENDETES, direction: ParameterDirection.Input);
                param.Add(name: "USUARIOATUALIZACAO", value: dto.USUARIOATUALIZACAO, direction: ParameterDirection.Input);
                param.Add(name: "DATAATUALIZACAO", value: dto.DATAATUALIZACAO, direction: ParameterDirection.Input);
                param.Add(name: "STATUSESPELHO", value: "", direction: ParameterDirection.Input);
                db.Open();
                var update = db.Execute("UPDATE SERVICOS.COM_CONTRATOS_PF SET CPFTITULAR=:CPFTITULAR,TITULAR=:TITULAR,DATANASCIMENTO=:DATANASCIMENTO,SEXO=:SEXO,NOMEDAMAE=:NOMEDAMAE,CARTAOSAUDE=:CARTAOSAUDE,RG=:RG,ORGAOEXPEDIDOR=:ORGAOEXPEDIDOR,DATAEXPEDICAO=:DATAEXPEDICAO,EMAIL=:EMAIL,TELEFONE1=:TELEFONE1,TELEFONE2=:TELEFONE2,RESPONSAVELFINANCEIRO=:RESPONSAVELFINANCEIRO,CPFRESPONSAVEL=:CPFRESPONSAVEL,IDADE=:IDADE,IDPLANO=:IDPLANO,NUMEROCONTRATO=:NUMEROCONTRATO,NUMEROPROPOSTA=:NUMEROPROPOSTA,VALORINDIVIDUAL=:VALORINDIVIDUAL,ABRANGENCIA=:ABRANGENCIA,ACOMODACAO=:ACOMODACAO,MODALIDADE=:MODALIDADE,CEP=:CEP,ENDERECO=:ENDERECO,NUMERO=:NUMERO,BAIRRO=:BAIRRO,COMPLEMENTO=:COMPLEMENTO,CIDADE=:CIDADE,UF=:UF,CEPCOBRANCA=:CEPCOBRANCA,ENDERECOCOBRANCA=:ENDERECOCOBRANCA,NUMEROCOBRANCA=:NUMEROCOBRANCA,BAIRROCOBRANCA=:BAIRROCOBRANCA,COMPLEMENTOCOBRANCA=:COMPLEMENTOCOBRANCA,CIDADECOBRANCA=:CIDADECOBRANCA,UFCOBRANCA=:UFCOBRANCA,TAXAINSCRICAO=:TAXAINSCRICAO,TOTALVIDAS=:TOTALVIDAS,DIAVENCIMENTO=:DIAVENCIMENTO,DATAPRIMEIRAFATURA=:DATAPRIMEIRAFATURA,VALORTOTAL=:VALORTOTAL,VALORPRIMEIRAFATURA=:VALORPRIMEIRAFATURA,OBSERVACAO=:OBSERVACAO,DATAVIGENCIACONTRATO=:DATAVIGENCIACONTRATO,DATAATENDIMENTO=:DATAATENDIMENTO,QNTOPCIONAIS=:QNTOPCIONAIS,QNTDEPENDETES=:QNTDEPENDETES,USUARIOATUALIZACAO=:USUARIOATUALIZACAO,DATAATUALIZACAO=:DATAATUALIZACAO,STATUSESPELHO=:STATUSESPELHO WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }
        }


        public virtual void InsertDependetesPF(ref Dependente dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "IDCONTRATO", value: dto.IDCONTRATO, direction: ParameterDirection.Input);
                    param.Add(name: "NOMEDep", value: dto.NOMEDep, direction: ParameterDirection.Input);
                    param.Add(name: "GUIDDEPENDENTE", value: dto.guidDependente, direction: ParameterDirection.Input);
                    param.Add(name: "IDADEDep", value: dto.IDADEDep, direction: ParameterDirection.Input);
                    param.Add(name: "CPFDep", value: dto.CPFDep, direction: ParameterDirection.Input);
                    param.Add(name: "CARTAODESAUDEDep", value: dto.CARTAODESAUDEDep, direction: ParameterDirection.Input);
                    param.Add(name: "DATANASCIMENTODep", value: dto.DATANASCIMENTODep, direction: ParameterDirection.Input);
                    param.Add(name: "SEXODep", value: dto.SEXODep, direction: ParameterDirection.Input);
                    param.Add(name: "VALORDep", value: dto.VALORDep, direction: ParameterDirection.Input);
                    param.Add(name: "PARENTESCODep", value: dto.PARENTESCODep, direction: ParameterDirection.Input);
                    param.Add(name: "NACIONALIDADEDep", value: dto.NACIONALIDADEDep, direction: ParameterDirection.Input);
                    param.Add(name: "NOMEDAMAEDep", value: dto.NOMEDAMAEDep, direction: ParameterDirection.Input);
                    param.Add(name: "ESTADOCIVILDep", value: dto.ESTADOCIVILDep, direction: ParameterDirection.Input);
                    param.Add(name: "DATACADASTRO", value: dto.DATACADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOCADASTRO", value: dto.USUARIOCADASTRO, direction: ParameterDirection.Input);
                    db.Open();

                    db.Execute("INSERT INTO SERVICOS.COM_DEPENDENTES_PF (IDCONTRATO,NOMEDep,GUIDDEPENDENTE,IDADEDep,CPFDep,CARTAODESAUDEDep,DATANASCIMENTODep,SEXODep,VALORDep,PARENTESCODep,NACIONALIDADEDep,NOMEDAMAEDep,ESTADOCIVILDep,DATACADASTRO,USUARIOCADASTRO) " +
                        " VALUES (:IDCONTRATO,:NOMEDep,:GUIDDEPENDENTE,:IDADEDep,:CPFDep,:CARTAODESAUDEDep,:DATANASCIMENTODep,:SEXODep,:VALORDep,:PARENTESCODep,:NACIONALIDADEDep,:NOMEDAMAEDep,:ESTADOCIVILDep,:DATACADASTRO,:USUARIOCADASTRO)", param);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        public bool ExcluirDepente(long? idcontrato)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "IDCONTRATO", value: idcontrato, direction: ParameterDirection.Input);
                    db.Open();
                    db.Execute("DELETE FROM SERVICOS.COM_DEPENDENTES_PF WHERE IDCONTRATO=:IDCONTRATO", param);
                    scope.Complete();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool ExcluirOpcionais(long? idcontrato)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "IDCONTRATO", value: idcontrato, direction: ParameterDirection.Input);
                    db.Open();
                    db.Execute("DELETE FROM SERVICOS.COM_OPCIONAIS_PF WHERE IDCONTRATO=:IDCONTRATO", param);
                    scope.Complete();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public virtual void InsertOpcionaisPF(ref Opcional dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "IDCONTRATO", value: dto.IDCONTRATO, direction: ParameterDirection.Input);
                    param.Add(name: "IDADICIONAIS", value: dto.IDADICIONAIS, direction: ParameterDirection.Input);
                    param.Add(name: "NOME", value: dto.NOME, direction: ParameterDirection.Input);
                    param.Add(name: "GUID", value: dto.guid, direction: ParameterDirection.Input);
                    param.Add(name: "GRUPO", value: dto.GRUPO, direction: ParameterDirection.Input);
                    param.Add(name: "VALOR", value: dto.VALOR, direction: ParameterDirection.Input);
                    param.Add(name: "DATACADASTRO", value: dto.DATACADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOCADASTRO", value: dto.USUARIOCADASTRO, direction: ParameterDirection.Input);
                    db.Open();

                    db.Execute("INSERT INTO SERVICOS.COM_OPCIONAIS_PF (IDCONTRATO,IDADICIONAIS,NOME,GUID,GRUPO,VALOR,DATACADASTRO,USUARIOCADASTRO) " +
                        " VALUES (:IDCONTRATO,:IDADICIONAIS,:NOME,:GUID,:GRUPO,:VALOR,:DATACADASTRO,:USUARIOCADASTRO)", param);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }



    }
}