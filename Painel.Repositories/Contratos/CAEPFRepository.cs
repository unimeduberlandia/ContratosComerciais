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
    public class CAEPFRepository : OracleRepositoryBase<CAEPF>
    {
        //public CAEPFRepository() : base("servicos_oracle_hom")
        //{
        //}
        public CAEPFRepository() : base("servicos_oracle")
        {
        }

        public List<PDFCAEPF> GetIdAssinatura(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = "select pj.CONTATOINTERNO, pj.DATAVIGENCIACONTRATO, e.RAZAOSOCIAL from servicos.COM_CONTRATOS_CAEPF pj inner join servicos.com_empresas e on e.id = pj.idempresa where pj.id = '" + id + "'";

                    return db.Query<PDFCAEPF>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public virtual bool UpdateAssinaturaCAEPF(long? id)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {

                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                param.Add(name: "STATUSASSINATURA", value: "Gerado", direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_CONTRATOS_CAEPF SET STATUSASSINATURA=:STATUSASSINATURA WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }

        }

        public List<PDFCAEPF> GetIdQualificacao(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = "select p.plano,(p.codigo)as CODIGOANS,p.tipocontratacao,(p.descricao) as SEGMENTACAO,p.acomodacao,p.abrangencia,p.formacaopreco,p.modalidade,e.cnpj,e.endereco,e.numero,e.complemento,e.bairro,e.cidade,e.uf,e.RAZAOSOCIAL,e.NOMEFANTASIA,e.TELEFONE,e.email,e.REPRESENTANTELEGAL,e.CPFREPRESENTANTE,e.CONTATO,e.RGREPRESENTANTE,e.INSCRICAOESTADUAL,pj.PARTICIPACAOFINANCEIRA,pj.numerocontrato from servicos.COM_CONTRATOS_CAEPF pj inner join servicos.COM_PLANOS p on p.id = pj.idplano inner join servicos.COM_EMPRESAS e on e.id = pj.idempresa where pj.id = '" + id + "'";

                    return db.Query<PDFCAEPF>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public virtual bool UpdateQualificacaoCAEPF(long? id)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {

                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                param.Add(name: "STATUSQUALIFICACAO", value: "Gerado", direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_CONTRATOS_CAEPF SET STATUSQUALIFICACAO=:STATUSQUALIFICACAO WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }

        }

        public List<CAEPF> GetAllContratosCAEPF(string dataInicial, string dataFinal)
        {
            var datai = Convert.ToDateTime(dataInicial);
            var dataf = Convert.ToDateTime(dataFinal);
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "select cp.ID, ce.CNPJ,  ce.RAZAOSOCIAL, pl.PLANO, pl.MODALIDADE, cp.TOTALDEVIDAS, cp.VALORTOTAL, cp.TAXADEINSCRICAO, cp.VALORPRIMEIRAFATURA, cp.QNTOPCIONAIS, cp.DATACADASTRO FROM SERVICOS.com_contratos_CAEPF cp INNER JOIN SERVICOS.COM_EMPRESAS ce ON ce.ID = cp.IDEMPRESA INNER JOIN SERVICOS.COM_PLANOS pl ON pl.ID = cp.IDPLANO where cp.DATACADASTRO BETWEEN '" + datai + "' AND '" + dataf + "'";
                return db.Query<CAEPF>(q).ToList();

            }
        }

        public List<CAEPF> GetAllContratosCAEPFUsuario(string usuario, string dataInicial, string dataFinal)
        {
            var datai = Convert.ToDateTime(dataInicial);
            var dataf = Convert.ToDateTime(dataFinal);
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "select cp.ID, ce.CNPJ, ce.RAZAOSOCIAL, pl.PLANO, pl.MODALIDADE, cp.TOTALDEVIDAS, cp.VALORTOTAL, cp.TAXADEINSCRICAO, cp.VALORPRIMEIRAFATURA, cp.QNTOPCIONAIS, cp.DATACADASTRO FROM SERVICOS.com_contratos_CAEPF cp INNER JOIN SERVICOS.COM_EMPRESAS ce ON ce.ID = cp.IDEMPRESA INNER JOIN SERVICOS.COM_PLANOS pl ON pl.ID = cp.IDPLANO where cp.USUARIOCADASTRO = '" + usuario + "' and cp.DATACADASTRO BETWEEN '" + datai + "' AND '" + dataf + "'";
                return db.Query<CAEPF>(q).ToList();
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

        public List<CAEPF> GetContratoCAEPFByID(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_CONTRATOS_CAEPF where ID ='" + id + "' ";
                return db.Query<CAEPF>(q).ToList();
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

        public List<Opcional> GetOpcionalByPlano(long? idplano)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_OPCIONAIS_CAEPF where IDCONTRATO = '" + idplano + "'";
                return db.Query<Opcional>(q).ToList();
            }
        }

        public List<Dependente> GetFaixasByPlano(long? idplano)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_VIDAS_CAPF where IDCONTRATO = '" + idplano + "'";
                return db.Query<Dependente>(q).ToList();
            }
        }

        public List<PDFCAEPF> GetallTermosAtivos()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = "select (nome)as NOMETERMO, (nomearquivo)as ARQUIVOTERMO from servicos.COM_TERMOS_CONTRATUAIS where STATUS = 'Sim'";

                    return db.Query<PDFCAEPF>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public PDFCAEPF GetUsuariobyCPF(string cpf)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add(name: "CPF", value: cpf, direction: ParameterDirection.Input);
                db.Open();

                return db.QueryFirstOrDefault<PDFCAEPF>("SELECT (EMAIL)as EMAILVENDEDOR FROM SERVICOS.COM_USUARIOS WHERE CPF=:CPF", param);
            }
        }
        public List<PDFCAEPF> Getemailbyid(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = "select pj.contatointerno, pc.email, pj.usuariocadastro, pj.usuarioatualizacao from SERVICOS.COM_CONTRATOS_CAEPF pj inner join SERVICOS.COM_EMPRESAS pc on pc.id = pj.idempresa where pj.id = '" + id + "'";

                    return db.Query<PDFCAEPF>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public List<PDFCAEPF> GetallopcionaisAtivos(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = "select pf.qntopcionais,op.idadicionais,ad.nomearquivo from servicos.com_contratos_CAEPF pf inner join servicos.COM_OPCIONAIS_CAEPF op on op.idcontrato = pf.id inner join servicos.COM_SERVICOS_ADICIONAIS ad on ad.id = op.idadicionais where pf.id = '" + id + "'";

                    return db.Query<PDFCAEPF>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public List<PDFCAEPF> GetIdPlano(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = " select pj.id,pj.contatointerno,pj.telefone1,pj.telefone2,pj.abrangencia,pj.acomodacao,pj.modalidade,pj.taxadeinscricao," +
                        "pj.totaldevidas,pj.datavigenciacontrato,pj.diavencimento,pj.valortotal,pj.valorprimeirafatura,pj.dataprimeirafatura,pj.dataatendimento," +
                        "pj.qntopcionais,pj.qntfaixas,pj.enderecocobranca,pj.participacaofinanceira,pj.numerocobranca,pj.complementocobranca,pj.bairrocobranca,pj.cidadecobranca," +
                        "pj.ufcobranca,pj.cepcobranca,pj.statuscontrato,pj.numeroproposta,pj.observacao,p.plano,p.codigo,p.NOMEARQUIVO,ep.cnpj,ep.razaosocial,ep.nomefantasia,ep.inscricaoestadual," +
                        "ep.iss,ep.email,ep.endereco,ep.numero,ep.complemento,pj.NUMEROCONTRATO,pj.GRUPOECONOMICO,ep.bairro,ep.cidade,ep.uf,ep.cep,ep.contato,ep.representantelegal," +
                        "vp.qntdfaixas,vp.nome1,vp.qntdvidas1,vp.valorcusto1,vp.valorindividual1,vp.nome2,vp.qntdvidas2,vp.valorcusto2,vp.valorindividual2,vp.nome3,vp.qntdvidas3,vp.valorcusto3,vp.valorindividual3," +
                        "vp.nome4,vp.qntdvidas4,vp.valorcusto4,vp.valorindividual4,vp.nome5,vp.qntdvidas5,vp.valorcusto5,vp.valorindividual5,vp.nome6,vp.qntdvidas6,vp.valorcusto6,vp.valorindividual6,vp.nome7,vp.qntdvidas7," +
                        "vp.valorcusto7,vp.valorindividual7,vp.nome8,vp.qntdvidas8,vp.valorcusto8,vp.valorindividual8,vp.nome9,vp.qntdvidas9,vp.valorcusto9,vp.valorindividual9,vp.nome10,vp.qntdvidas10,vp.valorcusto10,vp.valorindividual10," +
                        "vp.nome11,vp.qntdvidas11,vp.valorcusto11,vp.valorindividual11,vp.nome12,vp.qntdvidas12,vp.valorcusto12,vp.valorindividual12,vp.nome13,vp.qntdvidas13,vp.valorcusto13,vp.valorindividual13,vp.nome14,vp.qntdvidas14," +
                        "vp.valorcusto14,vp.valorindividual14,vp.nome15,vp.qntdvidas15,vp.valorcusto15,vp.valorindividual15,ep.cpfrepresentante,ep.rgrepresentante" +
                        " from servicos.COM_CONTRATOS_CAEPF pj inner join servicos.COM_PLANOS p on p.id=pj.idplano inner join servicos.COM_EMPRESAS ep on ep.id=pj.idempresa inner join servicos.COM_VIDAS_CAPF vp on vp.idcontrato=pj.id where pj.id='" + id + "'"; ;

                    return db.Query<PDFCAEPF>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public List<PDFCAEPF> GetIdQualificãcoes(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = " select * from servicos.COM_CONTRATOS_CAEPF  where id='" + id + "'"; ;

                    return db.Query<PDFCAEPF>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public List<PDFCAEPF> GetEspelho(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                try
                {
                    db.Open();

                    string q = " select STATUSESPELHO FROM SERVICOS.COM_CONTRATOS_CAEPF where id='" + id + "'"; ;

                    return db.Query<PDFCAEPF>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }


        public List<Planos> GetAllPlanosCAEPFAtivos()
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_PLANOS where STATUS = 'Sim' AND TIPOCONTRATO = 'PJ' ORDER BY PLANO ";
                return db.Query<Planos>(q).ToList();
            }
        }


        public List<CustoValor> GetAllPrecosByPlano(long? idplano)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_PLANOS_PRECOS where STATUS = 'Sim' AND IDPLANO = '" + idplano + "' order by id asc";
                return db.Query<CustoValor>(q).ToList();
            }
        }

        public List<TabelaPrecos> GetPrecosByID(long? idplano)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_PLANOS_PRECOS where ID = '" + idplano + "'";
                return db.Query<TabelaPrecos>(q).ToList();
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

        public List<Opcional> GetOpcionaisByIDContratoCAEPF(long? idcontrato)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_OPCIONAIS_CAEPF where IDCONTRATO ='" + idcontrato + "' ";
                return db.Query<Opcional>(q).ToList();
            }
        }

        public List<CustoValor> GetValoresbyIDVidasCAEPF(long? idcontrato)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_VIDAS_CAPF where IDCONTRATO ='" + idcontrato + "' ";
                return db.Query<CustoValor>(q).ToList();
            }
        }

        public List<Empresas> GetEmpresaByID(long? ID)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                string q = "SELECT * FROM SERVICOS.COM_EMPRESAS WHERE ID = '" + ID + "' ";
                return db.Query<Empresas>(q).ToList();
            }
        }

        public virtual void InsertContratoCAEPF(ref CAEPF dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "IDTAXAINSCRICAO", value: dto.IDTAXAINSCRICAO, direction: ParameterDirection.Input);
                    param.Add(name: "IDEMPRESA", value: dto.IDEMPRESA, direction: ParameterDirection.Input);
                    param.Add(name: "CONTATOINTERNO", value: dto.CONTATOINTERNO, direction: ParameterDirection.Input);
                    param.Add(name: "TELEFONE1", value: dto.TELEFONE1, direction: ParameterDirection.Input);
                    param.Add(name: "TELEFONE2", value: dto.TELEFONE2, direction: ParameterDirection.Input);
                    param.Add(name: "IDPLANO", value: dto.IDPLANO, direction: ParameterDirection.Input);
                    param.Add(name: "NUMEROCONTRATO", value: dto.NUMEROCONTRATO, direction: ParameterDirection.Input);
                    param.Add(name: "NUMEROPROPOSTA", value: dto.NUMEROPROPOSTA, direction: ParameterDirection.Input);
                    param.Add(name: "GRUPOECONOMICO", value: dto.GRUPOECONOMICO, direction: ParameterDirection.Input);
                    param.Add(name: "ABRANGENCIA", value: dto.ABRANGENCIA, direction: ParameterDirection.Input);
                    param.Add(name: "ACOMODACAO", value: dto.ACOMODACAO, direction: ParameterDirection.Input);
                    param.Add(name: "MODALIDADE", value: dto.MODALIDADE, direction: ParameterDirection.Input);
                    param.Add(name: "CEPCOBRANCA", value: dto.CEPCOBRANCA, direction: ParameterDirection.Input);
                    param.Add(name: "ENDERECOCOBRANCA", value: dto.ENDERECOCOBRANCA, direction: ParameterDirection.Input);
                    param.Add(name: "NUMEROCOBRANCA", value: dto.NUMEROCOBRANCA, direction: ParameterDirection.Input);
                    param.Add(name: "BAIRROCOBRANCA", value: dto.BAIRROCOBRANCA, direction: ParameterDirection.Input);
                    param.Add(name: "COMPLEMENTOCOBRANCA", value: dto.COMPLEMENTOCOBRANCA, direction: ParameterDirection.Input);
                    param.Add(name: "CIDADECOBRANCA", value: dto.CIDADECOBRANCA, direction: ParameterDirection.Input);
                    param.Add(name: "UFCOBRANCA", value: dto.UFCOBRANCA, direction: ParameterDirection.Input);
                    param.Add(name: "TAXADEINSCRICAO", value: dto.TAXADEINSCRICAO, direction: ParameterDirection.Input);
                    param.Add(name: "TOTALDEVIDAS", value: dto.TOTALDEVIDAS, direction: ParameterDirection.Input);
                    param.Add(name: "DIAVENCIMENTO", value: dto.DIAVENCIMENTO, direction: ParameterDirection.Input);
                    param.Add(name: "DATAPRIMEIRAFATURA", value: dto.DATAPRIMEIRAFATURA, direction: ParameterDirection.Input);
                    param.Add(name: "VALORTOTAL", value: dto.VALORTOTAL, direction: ParameterDirection.Input);
                    param.Add(name: "VALORPRIMEIRAFATURA", value: dto.VALORPRIMEIRAFATURA, direction: ParameterDirection.Input);
                    param.Add(name: "PARTICIPACAOFINANCEIRA", value: dto.PARTICIPACAOFINANCEIRA, direction: ParameterDirection.Input);
                    param.Add(name: "OBSERVACAO", value: dto.OBSERVACAO, direction: ParameterDirection.Input);
                    param.Add(name: "DATAVIGENCIACONTRATO", value: dto.DATAVIGENCIACONTRATO, direction: ParameterDirection.Input);
                    param.Add(name: "DATAATENDIMENTO", value: dto.DATAATENDIMENTO, direction: ParameterDirection.Input);
                    param.Add(name: "QNTOPCIONAIS", value: dto.QNTOPCIONAIS, direction: ParameterDirection.Input);
                    param.Add(name: "QNTFAIXAS", value: dto.QNTFAIXAS, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOCADASTRO", value: dto.USUARIOCADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "DATACADASTRO", value: dto.DATACADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    db.Open();

                    db.Execute("INSERT INTO SERVICOS.COM_CONTRATOS_CAEPF (IDTAXAINSCRICAO,IDEMPRESA,CONTATOINTERNO,TELEFONE1,TELEFONE2,IDPLANO,GRUPOECONOMICO,NUMEROCONTRATO,NUMEROPROPOSTA,ABRANGENCIA,ACOMODACAO,MODALIDADE,CEPCOBRANCA,ENDERECOCOBRANCA,NUMEROCOBRANCA,BAIRROCOBRANCA,COMPLEMENTOCOBRANCA,CIDADECOBRANCA,UFCOBRANCA,TAXADEINSCRICAO,TOTALDEVIDAS,DIAVENCIMENTO,DATAPRIMEIRAFATURA,VALORTOTAL,VALORPRIMEIRAFATURA,PARTICIPACAOFINANCEIRA,OBSERVACAO,DATAVIGENCIACONTRATO,DATAATENDIMENTO,QNTOPCIONAIS,QNTFAIXAS,USUARIOCADASTRO,DATACADASTRO) " +
                        " VALUES (:IDTAXAINSCRICAO,:IDEMPRESA,:CONTATOINTERNO,:TELEFONE1,:TELEFONE2,:IDPLANO,:GRUPOECONOMICO,:NUMEROCONTRATO,:NUMEROPROPOSTA,:ABRANGENCIA,:ACOMODACAO,:MODALIDADE,:CEPCOBRANCA,:ENDERECOCOBRANCA,:NUMEROCOBRANCA,:BAIRROCOBRANCA,:COMPLEMENTOCOBRANCA,:CIDADECOBRANCA,:UFCOBRANCA,:TAXADEINSCRICAO,:TOTALDEVIDAS,:DIAVENCIMENTO,:DATAPRIMEIRAFATURA,:VALORTOTAL,:VALORPRIMEIRAFATURA,:PARTICIPACAOFINANCEIRA,:OBSERVACAO,:DATAVIGENCIACONTRATO,:DATAATENDIMENTO,:QNTOPCIONAIS,:QNTFAIXAS,:USUARIOCADASTRO,:DATACADASTRO) RETURNING ID INTO :ID", param);

                    dto.ID = param.Get<int>("ID");

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public void InserirEmpresaCAEPF(ref Empresas dto)
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
                    param.Add(name: "INICIOATIVIDADE", value: dto.INICIOATIVIDADE, direction: ParameterDirection.Input);
                    param.Add(name: "ENDERECO", value: dto.ENDERECO, direction: ParameterDirection.Input);
                    param.Add(name: "NUMERO", value: dto.NUMERO, direction: ParameterDirection.Input);
                    param.Add(name: "COMPLEMENTO", value: dto.COMPLEMENTO, direction: ParameterDirection.Input);
                    param.Add(name: "BAIRRO", value: dto.BAIRRO, direction: ParameterDirection.Input);
                    param.Add(name: "CIDADE", value: dto.CIDADE, direction: ParameterDirection.Input);
                    param.Add(name: "EMAIL", value: dto.EMAIL, direction: ParameterDirection.Input);
                    param.Add(name: "UF", value: dto.UF, direction: ParameterDirection.Input);
                    param.Add(name: "CEP", value: dto.CEP, direction: ParameterDirection.Input);
                    param.Add(name: "STATUS", value: dto.STATUS, direction: ParameterDirection.Input);
                    param.Add(name: "CONTATO", value: dto.CONTATO, direction: ParameterDirection.Input);
                    param.Add(name: "TELEFONE", value: dto.TELEFONE, direction: ParameterDirection.Input);
                    param.Add(name: "TELEFONE2", value: dto.TELEFONE2, direction: ParameterDirection.Input);
                    param.Add(name: "REPRESENTANTELEGAL", value: dto.REPRESENTANTELEGAL, direction: ParameterDirection.Input);
                    param.Add(name: "CPFREPRESENTANTE", value: dto.CPFREPRESENTANTE, direction: ParameterDirection.Input);
                    param.Add(name: "RGREPRESENTANTE", value: dto.RGREPRESENTANTE, direction: ParameterDirection.Input);
                    param.Add(name: "DATACADASTRO", value: dto.DATACADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOCADASTRO", value: dto.USUARIOCADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    db.Open();

                    db.Execute("INSERT INTO SERVICOS.COM_EMPRESAS (CNPJ, RAZAOSOCIAL, NOMEFANTASIA, INSCRICAOESTADUAL, INICIOATIVIDADE, ENDERECO, NUMERO, COMPLEMENTO, BAIRRO, CIDADE, EMAIL, UF, CEP, STATUS, CONTATO, TELEFONE, TELEFONE2, REPRESENTANTELEGAL, CPFREPRESENTANTE, RGREPRESENTANTE, DATACADASTRO, USUARIOCADASTRO) " +
                        " VALUES (:CNPJ, :RAZAOSOCIAL, :NOMEFANTASIA, :INSCRICAOESTADUAL, :INICIOATIVIDADE, :ENDERECO, :NUMERO, :COMPLEMENTO, :BAIRRO, :CIDADE, :EMAIL, :UF, :CEP, :STATUS, :CONTATO, :TELEFONE, :TELEFONE2, :REPRESENTANTELEGAL, :CPFREPRESENTANTE, :RGREPRESENTANTE, :DATACADASTRO, :USUARIOCADASTRO) RETURNING ID INTO :ID", param);

                    dto.ID = param.Get<int>("ID");

                    scope.Complete();
                }
                catch (Exception ex)
                {
                }
            }
        }

        public virtual void InsertOpcionaisCAEPF(ref Opcional dto)
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

                    db.Execute("INSERT INTO SERVICOS.COM_OPCIONAIS_CAEPF (IDCONTRATO,IDADICIONAIS,NOME,GUID,GRUPO,VALOR,DATACADASTRO,USUARIOCADASTRO) " +
                        " VALUES (:IDCONTRATO,:IDADICIONAIS,:NOME,:GUID,:GRUPO,:VALOR,:DATACADASTRO,:USUARIOCADASTRO)", param);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public virtual void InserCustoCAEPF(ref CustoValor dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "IDTABELAPRECO", value: dto.IDTABELAPRECO, direction: ParameterDirection.Input);
                    param.Add(name: "IDCONTRATO", value: dto.IDCONTRATO, direction: ParameterDirection.Input);
                    param.Add(name: "VALOR", value: dto.VALOR, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDFAIXAS", value: dto.QNTDFAIXAS, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS", value: dto.QNTDVIDAS, direction: ParameterDirection.Input);
                    param.Add(name: "NOME1", value: dto.NOME1, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS1", value: dto.QNTDVIDAS1, direction: ParameterDirection.Input);
                    param.Add(name: "VALORCUSTO1", value: dto.VALORCUSTO1, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL1", value: dto.VALORINDIVIDUAL1, direction: ParameterDirection.Input);
                    param.Add(name: "NOME2", value: dto.NOME2, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS2", value: dto.QNTDVIDAS2, direction: ParameterDirection.Input);
                    param.Add(name: "VALORCUSTO2", value: dto.VALORCUSTO2, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL2", value: dto.VALORINDIVIDUAL2, direction: ParameterDirection.Input);
                    param.Add(name: "NOME3", value: dto.NOME3, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS3", value: dto.QNTDVIDAS3, direction: ParameterDirection.Input);
                    param.Add(name: "VALORCUSTO3", value: dto.VALORCUSTO3, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL3", value: dto.VALORINDIVIDUAL3, direction: ParameterDirection.Input);
                    param.Add(name: "NOME4", value: dto.NOME4, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS4", value: dto.QNTDVIDAS4, direction: ParameterDirection.Input);
                    param.Add(name: "VALORCUSTO4", value: dto.VALORCUSTO4, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL4", value: dto.VALORINDIVIDUAL4, direction: ParameterDirection.Input);
                    param.Add(name: "NOME5", value: dto.NOME5, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS5", value: dto.QNTDVIDAS5, direction: ParameterDirection.Input);
                    param.Add(name: "VALORCUSTO5", value: dto.VALORCUSTO5, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL5", value: dto.VALORINDIVIDUAL5, direction: ParameterDirection.Input);
                    param.Add(name: "NOME6", value: dto.NOME6, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS6", value: dto.QNTDVIDAS6, direction: ParameterDirection.Input);
                    param.Add(name: "VALORCUSTO6", value: dto.VALORCUSTO6, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL6", value: dto.VALORINDIVIDUAL6, direction: ParameterDirection.Input);
                    param.Add(name: "NOME7", value: dto.NOME7, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS7", value: dto.QNTDVIDAS7, direction: ParameterDirection.Input);
                    param.Add(name: "VALORCUSTO7", value: dto.VALORCUSTO7, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL7", value: dto.VALORINDIVIDUAL7, direction: ParameterDirection.Input);
                    param.Add(name: "NOME8", value: dto.NOME8, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS8", value: dto.QNTDVIDAS8, direction: ParameterDirection.Input);
                    param.Add(name: "VALORCUSTO8", value: dto.VALORCUSTO8, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL8", value: dto.VALORINDIVIDUAL8, direction: ParameterDirection.Input);
                    param.Add(name: "NOME9", value: dto.NOME9, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS9", value: dto.QNTDVIDAS9, direction: ParameterDirection.Input);
                    param.Add(name: "VALORCUSTO9", value: dto.VALORCUSTO9, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL9", value: dto.VALORINDIVIDUAL9, direction: ParameterDirection.Input);
                    param.Add(name: "NOME10", value: dto.NOME10, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS10", value: dto.QNTDVIDAS10, direction: ParameterDirection.Input);
                    param.Add(name: "VALORCUSTO10", value: dto.VALORCUSTO10, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL10", value: dto.VALORINDIVIDUAL10, direction: ParameterDirection.Input);
                    param.Add(name: "NOME11", value: dto.NOME11, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS11", value: dto.QNTDVIDAS11, direction: ParameterDirection.Input);
                    param.Add(name: "VALORCUSTO11", value: dto.VALORCUSTO11, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL11", value: dto.VALORINDIVIDUAL11, direction: ParameterDirection.Input);
                    param.Add(name: "NOME12", value: dto.NOME12, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS12", value: dto.QNTDVIDAS12, direction: ParameterDirection.Input);
                    param.Add(name: "VALORCUSTO12", value: dto.VALORCUSTO12, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL12", value: dto.VALORINDIVIDUAL12, direction: ParameterDirection.Input);
                    param.Add(name: "NOME13", value: dto.NOME13, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS13", value: dto.QNTDVIDAS13, direction: ParameterDirection.Input);
                    param.Add(name: "VALORCUSTO13", value: dto.VALORCUSTO13, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL13", value: dto.VALORINDIVIDUAL13, direction: ParameterDirection.Input);
                    param.Add(name: "NOME14", value: dto.NOME14, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS14", value: dto.QNTDVIDAS14, direction: ParameterDirection.Input);
                    param.Add(name: "VALORCUSTO14", value: dto.VALORCUSTO14, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL14", value: dto.VALORINDIVIDUAL14, direction: ParameterDirection.Input);
                    param.Add(name: "NOME15", value: dto.NOME15, direction: ParameterDirection.Input);
                    param.Add(name: "QNTDVIDAS15", value: dto.QNTDVIDAS15, direction: ParameterDirection.Input);
                    param.Add(name: "VALORCUSTO15", value: dto.VALORCUSTO15, direction: ParameterDirection.Input);
                    param.Add(name: "VALORINDIVIDUAL15", value: dto.VALORINDIVIDUAL15, direction: ParameterDirection.Input);
                    param.Add(name: "USUARIOCADASTRO", value: dto.USUARIOCADASTRO, direction: ParameterDirection.Input);
                    param.Add(name: "DATACADASTRO", value: dto.DATACADASTRO, direction: ParameterDirection.Input);
                    db.Open();

                    db.Execute("INSERT INTO SERVICOS.COM_VIDAS_CAPF (IDTABELAPRECO,IDCONTRATO,VALOR,QNTDFAIXAS,QNTDVIDAS,NOME1,QNTDVIDAS1,VALORCUSTO1,VALORINDIVIDUAL1,NOME2,QNTDVIDAS2,VALORCUSTO2,VALORINDIVIDUAL2,NOME3,QNTDVIDAS3,VALORCUSTO3,VALORINDIVIDUAL3,NOME4,QNTDVIDAS4,VALORCUSTO4,VALORINDIVIDUAL4,NOME5,QNTDVIDAS5,VALORCUSTO5,VALORINDIVIDUAL5,NOME6,QNTDVIDAS6,VALORCUSTO6,VALORINDIVIDUAL6,NOME7,QNTDVIDAS7,VALORCUSTO7,VALORINDIVIDUAL7,NOME8,QNTDVIDAS8,VALORCUSTO8,VALORINDIVIDUAL8,NOME9,QNTDVIDAS9,VALORCUSTO9,VALORINDIVIDUAL9,NOME10,QNTDVIDAS10,VALORCUSTO10,VALORINDIVIDUAL10,NOME11,QNTDVIDAS11,VALORCUSTO11,VALORINDIVIDUAL11,NOME12,QNTDVIDAS12,VALORCUSTO12,VALORINDIVIDUAL12,NOME13,QNTDVIDAS13,VALORCUSTO13,VALORINDIVIDUAL13,NOME14,QNTDVIDAS14,VALORCUSTO14,VALORINDIVIDUAL14,NOME15,QNTDVIDAS15,VALORCUSTO15,VALORINDIVIDUAL15,USUARIOCADASTRO,DATACADASTRO ) " +
                        " VALUES (:IDTABELAPRECO,:IDCONTRATO,:VALOR,:QNTDFAIXAS,:QNTDVIDAS,:NOME1,:QNTDVIDAS1,:VALORCUSTO1,:VALORINDIVIDUAL1,:NOME2,:QNTDVIDAS2,:VALORCUSTO2,:VALORINDIVIDUAL2,:NOME3,:QNTDVIDAS3,:VALORCUSTO3,:VALORINDIVIDUAL3,:NOME4,:QNTDVIDAS4,:VALORCUSTO4,:VALORINDIVIDUAL4,:NOME5,:QNTDVIDAS5,:VALORCUSTO5,:VALORINDIVIDUAL5,:NOME6,:QNTDVIDAS6,:VALORCUSTO6,:VALORINDIVIDUAL6,:NOME7,:QNTDVIDAS7,:VALORCUSTO7,:VALORINDIVIDUAL7,:NOME8,:QNTDVIDAS8,:VALORCUSTO8,:VALORINDIVIDUAL8,:NOME9,:QNTDVIDAS9,:VALORCUSTO9,:VALORINDIVIDUAL9,:NOME10,:QNTDVIDAS10,:VALORCUSTO10,:VALORINDIVIDUAL10,:NOME11,:QNTDVIDAS11,:VALORCUSTO11,:VALORINDIVIDUAL11,:NOME12,:QNTDVIDAS12,:VALORCUSTO12,:VALORINDIVIDUAL12,:NOME13,:QNTDVIDAS13,:VALORCUSTO13,:VALORINDIVIDUAL13,:NOME14,:QNTDVIDAS14,:VALORCUSTO14,:VALORINDIVIDUAL14,:NOME15,:QNTDVIDAS15,:VALORCUSTO15,:VALORINDIVIDUAL15,:USUARIOCADASTRO,:DATACADASTRO )", param);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw;
                }
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
                param.Add(name: "INICIOATIVIDADE", value: dto.INICIOATIVIDADE, direction: ParameterDirection.Input);
                param.Add(name: "ENDERECO", value: dto.ENDERECO, direction: ParameterDirection.Input);
                param.Add(name: "EMAIL", value: dto.EMAIL, direction: ParameterDirection.Input);
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
                param.Add(name: "REPRESENTANTELEGAL", value: dto.REPRESENTANTELEGAL, direction: ParameterDirection.Input);
                param.Add(name: "CPFREPRESENTANTE", value: dto.CPFREPRESENTANTE, direction: ParameterDirection.Input);
                param.Add(name: "RGREPRESENTANTE", value: dto.RGREPRESENTANTE, direction: ParameterDirection.Input);
                param.Add(name: "DATAATUALIZACAO", value: dto.DATAATUALIZACAO, direction: ParameterDirection.Input);
                param.Add(name: "USUARIOATUALIZACAO", value: dto.USUARIOATUALIZACAO, direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_EMPRESAS SET USUARIOATUALIZACAO=:USUARIOATUALIZACAO, DATAATUALIZACAO=:DATAATUALIZACAO, TELEFONE2=:TELEFONE2, TELEFONE=:TELEFONE, REPRESENTANTELEGAL=:REPRESENTANTELEGAL, CPFREPRESENTANTE=:CPFREPRESENTANTE, RGREPRESENTANTE=:RGREPRESENTANTE, CONTATO=:CONTATO, EMAIL=:EMAIL, STATUS=:STATUS, CEP=:CEP, UF=:UF, CIDADE=:CIDADE, BAIRRO=:BAIRRO, COMPLEMENTO=:COMPLEMENTO, NUMERO=:NUMERO, ENDERECO=:ENDERECO, INICIOATIVIDADE=:INICIOATIVIDADE, INSCRICAOESTADUAL=:INSCRICAOESTADUAL, RAZAOSOCIAL=:RAZAOSOCIAL, NOMEFANTASIA=:NOMEFANTASIA WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
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
                    db.Execute("DELETE FROM SERVICOS.COM_OPCIONAIS_CAEPF WHERE IDCONTRATO=:IDCONTRATO", param);
                    scope.Complete();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool ExcluirCustoValor(long? idcontrato)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add(name: "IDCONTRATO", value: idcontrato, direction: ParameterDirection.Input);
                    db.Open();
                    db.Execute("DELETE FROM SERVICOS.COM_VIDAS_CAPF WHERE IDCONTRATO=:IDCONTRATO", param);
                    scope.Complete();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public virtual bool UpdateContratoCAEPF(CAEPF dto)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {

                var param = new DynamicParameters();
                param.Add(name: "ID", value: dto.ID, direction: ParameterDirection.Input);
                param.Add(name: "IDTAXAINSCRICAO", value: dto.IDTAXAINSCRICAO, direction: ParameterDirection.Input);
                param.Add(name: "IDEMPRESA", value: dto.IDEMPRESA, direction: ParameterDirection.Input);
                param.Add(name: "CONTATOINTERNO", value: dto.CONTATOINTERNO, direction: ParameterDirection.Input);
                param.Add(name: "TELEFONE1", value: dto.TELEFONE1, direction: ParameterDirection.Input);
                param.Add(name: "TELEFONE2", value: dto.TELEFONE2, direction: ParameterDirection.Input);
                param.Add(name: "IDPLANO", value: dto.IDPLANO, direction: ParameterDirection.Input);
                param.Add(name: "NUMEROCONTRATO", value: dto.NUMEROCONTRATO, direction: ParameterDirection.Input);
                param.Add(name: "NUMEROPROPOSTA", value: dto.NUMEROPROPOSTA, direction: ParameterDirection.Input);
                param.Add(name: "GRUPOECONOMICO", value: dto.GRUPOECONOMICO, direction: ParameterDirection.Input);
                param.Add(name: "ABRANGENCIA", value: dto.ABRANGENCIA, direction: ParameterDirection.Input);
                param.Add(name: "ACOMODACAO", value: dto.ACOMODACAO, direction: ParameterDirection.Input);
                param.Add(name: "MODALIDADE", value: dto.MODALIDADE, direction: ParameterDirection.Input);
                param.Add(name: "CEPCOBRANCA", value: dto.CEPCOBRANCA, direction: ParameterDirection.Input);
                param.Add(name: "ENDERECOCOBRANCA", value: dto.ENDERECOCOBRANCA, direction: ParameterDirection.Input);
                param.Add(name: "NUMEROCOBRANCA", value: dto.NUMEROCOBRANCA, direction: ParameterDirection.Input);
                param.Add(name: "BAIRROCOBRANCA", value: dto.BAIRROCOBRANCA, direction: ParameterDirection.Input);
                param.Add(name: "COMPLEMENTOCOBRANCA", value: dto.COMPLEMENTOCOBRANCA, direction: ParameterDirection.Input);
                param.Add(name: "CIDADECOBRANCA", value: dto.CIDADECOBRANCA, direction: ParameterDirection.Input);
                param.Add(name: "UFCOBRANCA", value: dto.UFCOBRANCA, direction: ParameterDirection.Input);
                param.Add(name: "TAXADEINSCRICAO", value: dto.TAXADEINSCRICAO, direction: ParameterDirection.Input);
                param.Add(name: "TOTALDEVIDAS", value: dto.TOTALDEVIDAS, direction: ParameterDirection.Input);
                param.Add(name: "DIAVENCIMENTO", value: dto.DIAVENCIMENTO, direction: ParameterDirection.Input);
                param.Add(name: "DATAPRIMEIRAFATURA", value: dto.DATAPRIMEIRAFATURA, direction: ParameterDirection.Input);
                param.Add(name: "VALORTOTAL", value: dto.VALORTOTAL, direction: ParameterDirection.Input);
                param.Add(name: "VALORPRIMEIRAFATURA", value: dto.VALORPRIMEIRAFATURA, direction: ParameterDirection.Input);
                param.Add(name: "PARTICIPACAOFINANCEIRA", value: dto.PARTICIPACAOFINANCEIRA, direction: ParameterDirection.Input);
                param.Add(name: "OBSERVACAO", value: dto.OBSERVACAO, direction: ParameterDirection.Input);
                param.Add(name: "DATAVIGENCIACONTRATO", value: dto.DATAVIGENCIACONTRATO, direction: ParameterDirection.Input);
                param.Add(name: "DATAATENDIMENTO", value: dto.DATAATENDIMENTO, direction: ParameterDirection.Input);
                param.Add(name: "QNTOPCIONAIS", value: dto.QNTOPCIONAIS, direction: ParameterDirection.Input);
                param.Add(name: "QNTFAIXAS", value: dto.QNTFAIXAS, direction: ParameterDirection.Input);
                param.Add(name: "DATAATUALIZACAO", value: dto.DATAATUALIZACAO, direction: ParameterDirection.Input);
                param.Add(name: "USUARIOATUALIZACAO", value: dto.USUARIOATUALIZACAO, direction: ParameterDirection.Input);
                param.Add(name: "STATUSESPELHO", value: "", direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_CONTRATOS_CAEPF SET IDTAXAINSCRICAO=:IDTAXAINSCRICAO,IDEMPRESA=:IDEMPRESA,CONTATOINTERNO=:CONTATOINTERNO,TELEFONE1=:TELEFONE1,TELEFONE2=:TELEFONE2,IDPLANO=:IDPLANO,NUMEROCONTRATO=:NUMEROCONTRATO,NUMEROPROPOSTA=:NUMEROPROPOSTA,GRUPOECONOMICO=:GRUPOECONOMICO,ABRANGENCIA=:ABRANGENCIA,ACOMODACAO=:ACOMODACAO,MODALIDADE=:MODALIDADE,CEPCOBRANCA=:CEPCOBRANCA,ENDERECOCOBRANCA=:ENDERECOCOBRANCA,NUMEROCOBRANCA=:NUMEROCOBRANCA,BAIRROCOBRANCA=:BAIRROCOBRANCA,COMPLEMENTOCOBRANCA=:COMPLEMENTOCOBRANCA,CIDADECOBRANCA=:CIDADECOBRANCA,UFCOBRANCA=:UFCOBRANCA,TAXADEINSCRICAO=:TAXADEINSCRICAO,TOTALDEVIDAS=:TOTALDEVIDAS,DIAVENCIMENTO=:DIAVENCIMENTO,DATAPRIMEIRAFATURA=:DATAPRIMEIRAFATURA,VALORTOTAL=:VALORTOTAL,VALORPRIMEIRAFATURA=:VALORPRIMEIRAFATURA,OBSERVACAO=:OBSERVACAO,DATAVIGENCIACONTRATO=:DATAVIGENCIACONTRATO,DATAATENDIMENTO=:DATAATENDIMENTO,PARTICIPACAOFINANCEIRA=:PARTICIPACAOFINANCEIRA,QNTOPCIONAIS=:QNTOPCIONAIS,QNTFAIXAS=:QNTFAIXAS,USUARIOATUALIZACAO=:USUARIOATUALIZACAO,DATAATUALIZACAO=:DATAATUALIZACAO,STATUSESPELHO=:STATUSESPELHO WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }

        }

        public virtual bool UpdateStatusCAEPF(long? id)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {

                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                param.Add(name: "STATUSESPELHO", value: "Gerado", direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_CONTRATOS_CAEPF SET STATUSESPELHO=:STATUSESPELHO WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }

        }

        public virtual bool UpdateStatusContratoCAEPF(long? id)
        {
            using (TransactionScope scope = new TransactionScope())
            using (var db = new OracleConnection(ConnectionString))
            {

                var param = new DynamicParameters();
                param.Add(name: "ID", value: id, direction: ParameterDirection.Input);
                param.Add(name: "STATUSCONTRATO", value: "Gerado", direction: ParameterDirection.Input);
                db.Open();

                var update = db.Execute("UPDATE SERVICOS.COM_CONTRATOS_CAEPF SET STATUSCONTRATO=:STATUSCONTRATO WHERE ID=:ID", param);

                scope.Complete();

                return update > 0;
            }

        }

        public List<CAEPF> GetPlanoByID(long? id)
        {
            using (var db = new OracleConnection(ConnectionString))
            {
                try
                {
                    string q = "SELECT PLANO, ABRANGENCIA, ACOMODACAO, MODALIDADE FROM SERVICOS.COM_PLANOS WHERE ID ='" + id + "'";
                    return db.Query<CAEPF>(q).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

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


    }
}