using Painel.Core;
using Painel.Models.Contratos;
using Painel.Models.Cadastro;
using Painel.Repositories.Contratos;
using Painel.Web.Areas.Contratos.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Rotativa;
using Rotativa.Options;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace Painel.Web.Areas.Contratos.Controllers
{
    public class CAEPFController : Controller
    {
        CAEPFRepository consulta = new CAEPFRepository();

        private void FormularioViewBags()
        {
            ViewBag.Uf = new[] {
                new { value = "AC", text = "Acre"  },
                new { value = "AL", text = "Alagoas"  },
                new { value = "AP", text = "Amapá"  },
                new { value = "AM", text = "Amazonas"  },
                new { value = "BA", text = "Bahia"  },
                new { value = "CE", text = "Ceará"  },
                new { value = "DF", text = "Distrito Federal"  },
                new { value = "ES", text = "Espírito Santo"  },
                new { value = "GO", text = "Goiás"  },
                new { value = "MA", text = "Maranhão"  },
                new { value = "MT", text = "Mato Grosso"  },
                new { value = "MS", text = "Mato Grosso do Sul"  },
                new { value = "MG", text = "Minas Gerais"  },
                new { value = "PA", text = "Pará"  },
                new { value = "PB", text = "Paraíba"  },
                new { value = "PR", text = "Paraná"  },
                new { value = "PE", text = "Pernambuco"  },
                new { value = "PI", text = "Piauí"  },
                new { value = "RJ", text = "RJ"  },
                new { value = "RN", text = "Rio Grande do Norte"  },
                new { value = "RS", text = "Rio Grande do Sul"  },
                new { value = "RO", text = "Rondônia"  },
                new { value = "RR", text = "Roraima"  },
                new { value = "SC", text = "Santa Catarina"  },
                new { value = "SP", text = "São Paulo"  },
                new { value = "SE", text = "Sergipe"  },
                new { value = "TO", text = "Tocantins"  }
            };

            ViewBag.segundoendereco = new[] {

                new { value = "Sim", text = "Sim"  },
                new { value = "Nao", text = "Não"  }
            };

            var planos = consulta.GetAllPlanosCAEPFAtivos();
            var planosSelecionaveis = new List<SelectListItem>();
            foreach (var item in planos)
            {
                planosSelecionaveis.Add(new SelectListItem
                {
                    Value = item.id.ToString(),
                    Text = item.PLANO
                });
            }
            ViewBag.planos = planosSelecionaveis;




            var opcionais = consulta.GetAllAdicionaisAtivos();
            var opcionaisSelecionaveis = new List<SelectListItem>();
            foreach (var item in opcionais)
            {
                opcionaisSelecionaveis.Add(new SelectListItem
                {
                    Value = item.IDADICIONAIS.ToString(),
                    Text = item.NOME
                });
            }
            ViewBag.opcionais = opcionaisSelecionaveis;
        }

        public async Task<JsonResult> BuscaEmpresa(string cnpj)
        {
            try
            {
                var dto = consulta.GetEmpresaByCNPJ(cnpj);
                return Json(new
                {
                    razaosocial = dto[0].RAZAOSOCIAL,
                    inicioatividade = dto[0].INICIOATIVIDADE,
                    inscricaoestadual = dto[0].INSCRICAOESTADUAL,
                    iss = dto[0].ISS,
                    contatointerno = dto[0].CONTATO,
                    email = dto[0].EMAIL,
                    telefone1 = dto[0].TELEFONE,
                    telefone2 = dto[0].TELEFONE2,
                    cep = dto[0].CEP,
                    endereco = dto[0].ENDERECO,
                    numero = dto[0].NUMERO,
                    bairro = dto[0].BAIRRO,
                    complemento = dto[0].COMPLEMENTO,
                    cidade = dto[0].CIDADE,
                    uf = dto[0].UF,
                    representante = dto[0].REPRESENTANTELEGAL,
                    cpfrepresentante = dto[0].CPFREPRESENTANTE,
                    rgrepresentante = dto[0].RGREPRESENTANTE

                }, JsonRequestBehavior.AllowGet); ; ;
            }
            catch (System.Exception)
            {
                return Json(false);
            }
        }

        public async Task<JsonResult> ValidaCNPJ(string cnpj)
        {
            if (!Validacoes.ValidaCNPJ(cnpj))
            {
                return Json(new
                {
                    cnpj = "F"
                });
            }
            else
            {
                return Json(new
                {
                    cnpj = "V"
                });
            }
        }

        public async Task<JsonResult> BuscaPlano(long? idplano)
        {
            var consultavalor = consulta.GetPlanoByID(idplano);
            {
                if (consultavalor.Count > 0)
                {
                    Session["valores"] = null;
                    PartialValores();

                    return Json(new
                    {
                        abrangencia = consultavalor[0].ABRANGENCIA,
                        acomodacao = consultavalor[0].ACOMODACAO,
                        modalidade = consultavalor[0].MODALIDADE,

                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false);
                }
            }
        }

        public async Task<JsonResult> BuscaValorPlano(string valorcusto, long? vidas)
        {
            try
            {
                var custo = Convert.ToDouble(valorcusto);
                var pessoas = Convert.ToDouble(vidas);
                var custototal = custo * pessoas;
                var valores = Convert.ToDouble(custototal).ToString("C");


                return Json(new
                {
                    valor = valores

                }, JsonRequestBehavior.AllowGet);

            }
            catch (System.Exception)
            {
                return Json(false);
            }

        }

        public async Task<JsonResult> Buscaplanosindividual(long? idplano)
        {
            var consultavalor = consulta.GetPrecosByID(idplano);
            var custo = Convert.ToDouble(consultavalor[0].VALOR);
            var valores = Convert.ToDouble(custo).ToString("C");

            if (consultavalor.Count > 0)
            {
                return Json(new
                {
                    valorindividual = valores

                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false);
            }

        }

        public async Task<JsonResult> BuscaCep2(string cep)
        {
            using (var ws = new Correios.AtendeClienteClient())
            {
                try
                {
                    var resultado = ws.consultaCEP(cep.Replace("-", ""));
                    return Json(new
                    {
                        bairro = resultado.bairro,
                        cidade = resultado.cidade,
                        complemento = resultado.complemento,
                        complemento2 = resultado.complemento2,
                        end = resultado.end,
                        uf = resultado.uf
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (System.Exception)
                {
                    return Json(false);
                }
            }
        }

        public async Task<JsonResult> BuscaCep(string cep)
        {
            using (var ws = new Correios.AtendeClienteClient())
            {
                try
                {
                    var resultado = ws.consultaCEP(cep.Replace("-", ""));
                    return Json(new
                    {
                        bairro = resultado.bairro,
                        cidade = resultado.cidade,
                        complemento = resultado.complemento,
                        complemento2 = resultado.complemento2,
                        end = resultado.end,
                        uf = resultado.uf
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (System.Exception)
                {
                    return Json(false);
                }
            }
        }



        #region NovoContrato

        [Autorizado]
        public ActionResult Index()
        {
            //var dto = new List<CAEPF>();
            //var tipo1 = "";
            //var tipo2 = "";
            //SessionManager secao = SessionManager.GetInstance();

            //if (SegurancaManager.usuario.Permissoes.Count == 2)
            //{
            //    tipo1 = SegurancaManager.usuario.Permissoes[0].Descricao;
            //    tipo2 = SegurancaManager.usuario.Permissoes[1].Descricao;
            //}

            //if (SegurancaManager.usuario.Permissoes.Count == 1)
            //{
            //    tipo1 = SegurancaManager.usuario.Permissoes[0].Descricao;
            //}

            //if (tipo1 == "Vendedor")
            //{
            //    var usuario = secao.Usuario.Login;
            //    Session["opcionaisCAEPF"] = null;
            //    Session["valores"] = null;

            //    dto = consulta.GetAllContratosCAEPFUsuario(usuario);
            //}
            //if (tipo1 == "Administrador")
            //{
            //    Session["opcionaisCAEPF"] = null;
            //    Session["valores"] = null;

            //    dto = consulta.GetAllContratosCAEPF();
            //}


            return View();
                      
        }

        [Autorizado]
        [HttpPost]
        public ActionResult PartialRelatorio(string datainicial, string datafinal)
        {

            var dataini = "";
            var datafim = "";
            if (datainicial == null)
            {
                var inicio = DateTime.Now;
                var fim = inicio.AddDays(+15);
                dataini = inicio.ToString("yyyy-MM-dd");
                datafim = fim.ToString("yyyy-MM-dd");
            }
            else
            {
                var b = Convert.ToDateTime(datainicial);
                var e = Convert.ToDateTime(datafinal);
                e = e.AddDays(+1);
                dataini = b.ToString("yyyy-MM-dd");
                datafim = e.ToString("yyyy-MM-dd");
            }

            var dto = new List<CAEPF>();
            var tipo1 = "";
            var tipo2 = "";
            SessionManager secao = SessionManager.GetInstance();

            if (SegurancaManager.usuario.Permissoes.Count == 2)
            {
                tipo1 = SegurancaManager.usuario.Permissoes[0].Descricao;
                tipo2 = SegurancaManager.usuario.Permissoes[1].Descricao;
            }

            if (SegurancaManager.usuario.Permissoes.Count == 1)
            {
                tipo1 = SegurancaManager.usuario.Permissoes[0].Descricao;
            }

            if (tipo1 == "Vendedor")
            {
                var usuario = secao.Usuario.Login;
                Session["opcionaisCAEPF"] = null;
                Session["valores"] = null;

                dto = consulta.GetAllContratosCAEPFUsuario(usuario, dataini, datafim);
            }
            if (tipo1 == "Administrador")
            {
                Session["opcionaisCAEPF"] = null;
                Session["valores"] = null;

                dto = consulta.GetAllContratosCAEPF(dataini, datafim);
            }


            return View("~/Areas/Contratos/Views/CAEPF/Partial/_PartialConsulta.cshtml", dto);

        }

        [Autorizado]
        public ActionResult NovoContrato()
        {
            Session["opcionaisCAEPF"] = null;
            Session["valores"] = null;
            SessionManager secao = SessionManager.GetInstance();

            var taxainscricao = consulta.GetTaxaAtiva();
            double OutTaxa;
            double.TryParse(taxainscricao[0].VALOR, out OutTaxa);

            var taxas = Convert.ToDouble(OutTaxa).ToString("C");

            FormularioViewBags();
            var dto = new CAEPFViewModel();
            dto.TAXADEINSCRICAO = taxas.ToString();
            dto.DATAVENCIMENTOMENSAL = "10";
            dto.DATAVIGENCIACONTRATO = DateTime.Now;
            dto.INICIOATIVIDADE = DateTime.Now;
            dto.Idsegundoendereco = "Sim";
            var datavencimento = DateTime.Now;
            var mesfatura = datavencimento.AddMonths(1);
            dto.USUARIOCADASTRO = secao.Usuario.Nome;
            dto.DATACADASTRO = DateTime.Now;
            dto.statuscadastro = "nao";

            dto.DATAVENCIMENTOPRIMEIRAFATURA = new DateTime(mesfatura.Year, mesfatura.Month, 10);

            return View("~/Areas/Contratos/Views/CAEPF/Partial/_NovoContratoCAEPF.cshtml", dto);
        }


        [Autorizado]
        public ActionResult EditarContratoCAEPF(long? ID)
        {
            var dto = new CAEPFViewModel();
            var editar = consulta.GetContratoCAEPFByID(ID);
            var empresa = consulta.GetEmpresaByID(editar[0].IDEMPRESA);

            dto.ID = ID;
            dto.IDPLANO = editar[0].IDPLANO;
            dto.IDTAXAINSCRICAO = editar[0].IDTAXAINSCRICAO;

            var consultasr = consulta.GetUsuariobysCPF(editar[0].USUARIOCADASTRO);

            dto.USUARIOCADASTRO = consultasr.USUARIOCADASTRO;
            dto.DATACADASTRO = editar[0].DATACADASTRO;
            dto.statuscadastro = "nao";

            if (editar[0].USUARIOATUALIZACAO != null)
            {

                var consultass = consulta.GetUsuariobysCPF(editar[0].USUARIOATUALIZACAO);
                dto.statuscadastro = "sim";
                dto.USUARIOATUALIZACAO = consultass.USUARIOCADASTRO;
                dto.DATAATUALIZACAO = editar[0].DATAATUALIZACAO;

            }


            dto.CNPJ = empresa[0].CNPJ;
            dto.RAZAOSOCIAL = empresa[0].RAZAOSOCIAL;
            dto.NOMEFANTASIA = empresa[0].NOMEFANTASIA;
            dto.INSCRICAOESTADUAL = empresa[0].INSCRICAOESTADUAL;
            dto.INICIOATIVIDADE = empresa[0].INICIOATIVIDADE;
            dto.CONTATOINTERNO = empresa[0].CONTATO;
            dto.EMAIL = empresa[0].EMAIL;
            dto.CPFREPRESENTANTE = empresa[0].CPFREPRESENTANTE;
            dto.REPRESENTANTELEGAL = empresa[0].REPRESENTANTELEGAL;
            dto.RGREPRESENTANTE = empresa[0].RGREPRESENTANTE;
            dto.TELEFONE1 = editar[0].TELEFONE1;
            dto.TELEFONE2 = editar[0].TELEFONE2;
            dto.IDPLANO = editar[0].IDPLANO;
            dto.NUMEROCONTRATO = editar[0].NUMEROCONTRATO;
            dto.NUMEROPROPOSTA = editar[0].NUMEROPROPOSTA;
            dto.GRUPOECONOMICO = editar[0].GRUPOECONOMICO;
            dto.ACOMODACAO = editar[0].ACOMODACAO;
            dto.MODALIDADE = editar[0].MODALIDADE;
            dto.ABRANGENCIA = editar[0].ABRANGENCIA;
            dto.TAXADEINSCRICAO = editar[0].TAXADEINSCRICAO;
            dto.CEP = empresa[0].CEP;
            dto.ENDERECO = empresa[0].ENDERECO;
            dto.NUMERO = empresa[0].NUMERO;
            dto.BAIRRO = empresa[0].BAIRRO;
            dto.COMPLEMENTO = empresa[0].COMPLEMENTO;
            dto.CIDADE = empresa[0].CIDADE;
            dto.idUF = empresa[0].UF;
            dto.CEPCOBRANCA = editar[0].CEPCOBRANCA;
            dto.ENDERECOCOBRANCA = editar[0].ENDERECOCOBRANCA;
            dto.NUMEROCOBRANCA = editar[0].NUMEROCOBRANCA;
            dto.BAIRROCOBRANCA = editar[0].BAIRROCOBRANCA;
            dto.COMPLEMENTOCOBRANCA = editar[0].COMPLEMENTOCOBRANCA;
            dto.CIDADECOBRANCA = editar[0].CIDADECOBRANCA;
            dto.IdUFCOBRANCA = editar[0].UFCOBRANCA;
            dto.VALORTOTAL = editar[0].VALORTOTAL;
            dto.VALORPRIMEIRAFATURA = editar[0].VALORPRIMEIRAFATURA;
            dto.TOTALDEVIDAS = editar[0].TOTALDEVIDAS;
            dto.DATAVENCIMENTOMENSAL = editar[0].DIAVENCIMENTO;
            dto.DATAVIGENCIACONTRATO = editar[0].DATAVIGENCIACONTRATO;
            dto.DATAVENCIMENTOPRIMEIRAFATURA = editar[0].DATAPRIMEIRAFATURA;
            dto.PARTICIPACAOFINANCEIRA = editar[0].PARTICIPACAOFINANCEIRA;
            dto.OBSERVACAO = editar[0].OBSERVACAO;


            var listaopcionais = consulta.GetOpcionaisByIDContratoCAEPF(ID);

            if (listaopcionais.Count > 0)
                Session["opcionaisCAEPF"] = listaopcionais;

            var listavalores = consulta.GetValoresbyIDVidasCAEPF(editar[0].ID);

            if (listavalores.Count > 0)
                Session["valores"] = listavalores;

            FormularioViewBags();

            return View("~/Areas/Contratos/Views/CAEPF/Partial/_novoContratoCAEPF.cshtml", dto);
        }

        #endregion

        #region opcionais

        public async Task<JsonResult> BuscaValorAdicionalCAEPF(long? idadicional)
        {
            var consultavalor = consulta.GetValorAdicional(idadicional);
            {
                if (consultavalor.Count > 0)
                {
                    return Json(new
                    {
                        valor = consultavalor[0].VALOR,
                        grupo = consultavalor[0].GRUPO
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false);
                }
            }
        }


        [Autorizado]
        public ActionResult PartialOpcionais()
        {
            var lista = new List<Opcional>();

            if (Session["opcionaisCAEPF"] != null)
                lista = (List<Opcional>)Session["opcionaisCAEPF"];

            return View("~/Areas/Contratos/Views/CAEPF/Partial/_OpcionalCAEPF.cshtml", lista);
        }



        [Autorizado]
        public ActionResult AdicionarOpcional(string guid)
        {
            if (guid != null)
            {
                var list = new List<Opcional>();

                if (Session["opcionaisCAEPF"] != null)
                {
                    list = (List<Opcional>)Session["opcionaisCAEPF"];
                    var dep = list.FirstOrDefault(q => q.guid == guid);
                    var dto = new OpcionalViewModel();
                    FormularioViewBags();

                    return View("~/Areas/Contratos/Views/CAEPF/Partial/_NovoOpcionalCAEPF.cshtml", dto);

                }
                else
                {
                    var dto = new OpcionalViewModel();
                    dto.guid = Guid.NewGuid().ToString().Replace("-", "");
                    FormularioViewBags();

                    return View("~/Areas/Contratos/Views/CAEPF/Partial/_NovoOpcionalCAEPF.cshtml", dto);

                }
            }
            else
            {
                var dto = new OpcionalViewModel();
                FormularioViewBags();

                return View("~/Areas/Contratos/Views/CAEPF/Partial/_NovoOpcionalCAEPF.cshtml", dto);
            }
        }


        [Autorizado]
        public ActionResult PostopcionaisCAEPF(OpcionalViewModel dto)
        {
            if (dto.IDADICIONAIS != null)
            {
                var sel = new Opcional();

                var list = new List<Opcional>();

                if (Session["opcionaisCAEPF"] != null)
                    list = (List<Opcional>)Session["opcionaisCAEPF"];

                if (!string.IsNullOrEmpty(dto.guid))
                {
                    sel = list.FirstOrDefault(d => d.guid == dto.guid);
                    list.Remove(sel);
                }

                var consultavalor = consulta.GetValorAdicional(dto.IDADICIONAIS);

                sel.IDADICIONAIS = dto.IDADICIONAIS;
                sel.NOME = consultavalor[0].NOME;
                sel.GRUPO = dto.GRUPO;
                sel.VALOR = dto.VALOR;
                sel.guid = Guid.NewGuid().ToString().Replace("-", "");

                list.Add(sel);

                Session["opcionaisCAEPF"] = list;

                return Json(new
                {
                    success = true,
                    dep = false,
                    msg = "Adicionado com sucesso"
                });
            }

            else
            {
                FormularioViewBags();
                return View("~/Areas/Contratos/Views/CAEPF/Partial/_NovoOpcionalCAEPF.cshtml", dto);
            }
        }



        [Autorizado]
        public ActionResult ExcluirOpcionalCAEPF(string guid)
        {
            if (!String.IsNullOrEmpty(guid))
            {
                var list = new List<Opcional>();

                if (Session["opcionaisCAEPF"] != null)
                    list = (List<Opcional>)Session["opcionaisCAEPF"];

                var dep = list.FirstOrDefault(q => q.guid == guid);

                list.Remove(dep);

                Session["opcionaisCAEPF"] = list;

                return Json(new
                {
                    success = true,
                    msg = "Serviço excluído"
                });
            }
            else
            {
                return Json(new
                {
                    success = false
                });
            }
        }


        #endregion

        #region Custo/Faixas

        [Autorizado]
        public ActionResult PartialValores()
        {
            var dto = new CustoExibirViewModel();

            if (Session["valores"] != null)
            {
                var lista = new List<CustoValor>();
                lista = (List<CustoValor>)Session["valores"];

                dto.IDPLANO = lista[0].IDPLANO;
                dto.QNTDEFAIXAS = lista[0].QNTDFAIXAS;

                dto.NOM1 = lista[0].NOME1;
                dto.QNTDVIDA1 = lista[0].QNTDVIDAS1;
                dto.VALORCUST1 = lista[0].VALORCUSTO1;
                dto.VALORINDIVIDUA1 = lista[0].VALORINDIVIDUAL1;

                dto.NOM2 = lista[0].NOME2;
                dto.QNTDVIDA2 = lista[0].QNTDVIDAS2;
                dto.VALORCUST2 = lista[0].VALORCUSTO2;
                dto.VALORINDIVIDUA2 = lista[0].VALORINDIVIDUAL2;

                dto.NOM3 = lista[0].NOME3;
                dto.QNTDVIDA3 = lista[0].QNTDVIDAS3;
                dto.VALORCUST3 = lista[0].VALORCUSTO3;
                dto.VALORINDIVIDUA3 = lista[0].VALORINDIVIDUAL3;

                dto.NOM4 = lista[0].NOME4;
                dto.QNTDVIDA4 = lista[0].QNTDVIDAS4;
                dto.VALORCUST4 = lista[0].VALORCUSTO4;
                dto.VALORINDIVIDUA4 = lista[0].VALORINDIVIDUAL4;

                dto.NOM5 = lista[0].NOME5;
                dto.QNTDVIDA5 = lista[0].QNTDVIDAS5;
                dto.VALORCUST5 = lista[0].VALORCUSTO5;
                dto.VALORINDIVIDUA5 = lista[0].VALORINDIVIDUAL5;

                dto.NOM6 = lista[0].NOME6;
                dto.QNTDVIDA6 = lista[0].QNTDVIDAS6;
                dto.VALORCUST6 = lista[0].VALORCUSTO6;
                dto.VALORINDIVIDUA6 = lista[0].VALORINDIVIDUAL6;

                dto.NOM7 = lista[0].NOME7;
                dto.QNTDVIDA7 = lista[0].QNTDVIDAS7;
                dto.VALORCUST7 = lista[0].VALORCUSTO7;
                dto.VALORINDIVIDUA7 = lista[0].VALORINDIVIDUAL7;

                dto.NOM8 = lista[0].NOME8;
                dto.QNTDVIDA8 = lista[0].QNTDVIDAS8;
                dto.VALORCUST8 = lista[0].VALORCUSTO8;
                dto.VALORINDIVIDUA8 = lista[0].VALORINDIVIDUAL8;

                dto.NOM9 = lista[0].NOME9;
                dto.QNTDVIDA9 = lista[0].QNTDVIDAS9;
                dto.VALORCUST9 = lista[0].VALORCUSTO9;
                dto.VALORINDIVIDUA9 = lista[0].VALORINDIVIDUAL9;

                dto.NOM10 = lista[0].NOME10;
                dto.QNTDVIDA10 = lista[0].QNTDVIDAS10;
                dto.VALORCUST10 = lista[0].VALORCUSTO10;
                dto.VALORINDIVIDUA10 = lista[0].VALORINDIVIDUAL10;

                dto.NOM11 = lista[0].NOME11;
                dto.QNTDVIDA11 = lista[0].QNTDVIDAS11;
                dto.VALORCUST11 = lista[0].VALORCUSTO11;
                dto.VALORINDIVIDUA11 = lista[0].VALORINDIVIDUAL11;

                dto.NOM12 = lista[0].NOME12;
                dto.QNTDVIDA12 = lista[0].QNTDVIDAS12;
                dto.VALORCUST12 = lista[0].VALORCUSTO12;
                dto.VALORINDIVIDUA12 = lista[0].VALORINDIVIDUAL12;

                dto.NOM13 = lista[0].NOME13;
                dto.QNTDVIDA13 = lista[0].QNTDVIDAS13;
                dto.VALORCUST13 = lista[0].VALORCUSTO13;
                dto.VALORINDIVIDUA13 = lista[0].VALORINDIVIDUAL13;

                dto.NOM14 = lista[0].NOME14;
                dto.QNTDVIDA14 = lista[0].QNTDVIDAS14;
                dto.VALORCUST14 = lista[0].VALORCUSTO14;
                dto.VALORINDIVIDUA14 = lista[0].VALORINDIVIDUAL14;

                dto.NOM15 = lista[0].NOME15;
                dto.QNTDVIDA15 = lista[0].QNTDVIDAS15;
                dto.VALORCUST15 = lista[0].VALORCUSTO15;
                dto.VALORINDIVIDUA15 = lista[0].VALORINDIVIDUAL15;
            }
            else
            {
                dto.QNTDEFAIXAS = "0";
                ViewBag.qntdefaixas = "0";
            }

            return View("~/Areas/Contratos/Views/CAEPF/Partial/_CustoValor.cshtml", dto);
        }

        [Autorizado]
        public ActionResult AdicionarValor(long? ID)
        {
            var dto = new CustoValorViewModel();

            if (Session["valores"] == null)
            {
                var precos = consulta.GetAllPrecosByPlano(ID);

                dto.QNTDFAIXAS = precos.Count.ToString();

                for (int i = 0; i <= precos.Count - 1; i++)
                {
                    var dep = precos[i];

                    if (i == 0)
                    {
                        dto.ID1 = dep.ID;
                        dto.NOME1 = dep.NOME;
                        dto.VALORCUSTO1 = dep.VALOR;
                    }
                    if (i == 1)
                    {
                        dto.ID2 = dep.ID;
                        dto.NOME2 = dep.NOME;
                        dto.VALORCUSTO2 = dep.VALOR;
                    }
                    if (i == 2)
                    {
                        dto.ID3 = dep.ID;
                        dto.NOME3 = dep.NOME;
                        dto.VALORCUSTO3 = dep.VALOR;
                    }
                    if (i == 3)
                    {
                        dto.ID4 = dep.ID;
                        dto.NOME4 = dep.NOME;
                        dto.VALORCUSTO4 = dep.VALOR;
                    }
                    if (i == 4)
                    {
                        dto.ID5 = dep.ID;
                        dto.NOME5 = dep.NOME;
                        dto.VALORCUSTO5 = dep.VALOR;
                    }
                    if (i == 5)
                    {
                        dto.ID6 = dep.ID;
                        dto.NOME6 = dep.NOME;
                        dto.VALORCUSTO6 = dep.VALOR;
                    }
                    if (i == 6)
                    {
                        dto.ID7 = dep.ID;
                        dto.NOME7 = dep.NOME;
                        dto.VALORCUSTO7 = dep.VALOR;
                    }
                    if (i == 7)
                    {
                        dto.ID8 = dep.ID;
                        dto.NOME8 = dep.NOME;
                        dto.VALORCUSTO8 = dep.VALOR;
                    }
                    if (i == 8)
                    {
                        dto.ID9 = dep.ID;
                        dto.NOME9 = dep.NOME;
                        dto.VALORCUSTO9 = dep.VALOR;
                    }
                    if (i == 9)
                    {
                        dto.ID10 = dep.ID;
                        dto.NOME10 = dep.NOME;
                        dto.VALORCUSTO10 = dep.VALOR;
                    }
                    if (i == 10)
                    {
                        dto.ID11 = dep.ID;
                        dto.NOME11 = dep.NOME;
                        dto.VALORCUSTO11 = dep.VALOR;
                    }
                    if (i == 11)
                    {
                        dto.ID12 = dep.ID;
                        dto.NOME12 = dep.NOME;
                        dto.VALORCUSTO12 = dep.VALOR;
                    }
                    if (i == 12)
                    {
                        dto.ID13 = dep.ID;
                        dto.NOME13 = dep.NOME;
                        dto.VALORCUSTO13 = dep.VALOR;
                    }
                    if (i == 13)
                    {
                        dto.ID14 = dep.ID;
                        dto.NOME14 = dep.NOME;
                        dto.VALORCUSTO14 = dep.VALOR;
                    }
                    if (i == 14)
                    {
                        dto.ID15 = dep.ID;
                        dto.NOME15 = dep.NOME;
                        dto.VALORCUSTO15 = dep.VALOR;
                    }
                }
                dto.IDPLANO = ID;
            }
            else
            {
                var lista = new List<CustoValor>();
                lista = (List<CustoValor>)Session["valores"];

                dto.IDPLANO = lista[0].IDPLANO;
                dto.QNTDFAIXAS = lista[0].QNTDFAIXAS;

                dto.NOME1 = lista[0].NOME1;
                dto.QNTDVIDAS1 = lista[0].QNTDVIDAS1;
                dto.VALORCUSTO1 = lista[0].VALORCUSTO1;
                dto.VALORINDIVIDUAL1 = lista[0].VALORINDIVIDUAL1;

                dto.NOME2 = lista[0].NOME2;
                dto.QNTDVIDAS2 = lista[0].QNTDVIDAS2;
                dto.VALORCUSTO2 = lista[0].VALORCUSTO2;
                dto.VALORINDIVIDUAL2 = lista[0].VALORINDIVIDUAL2;

                dto.NOME3 = lista[0].NOME3;
                dto.QNTDVIDAS3 = lista[0].QNTDVIDAS3;
                dto.VALORCUSTO3 = lista[0].VALORCUSTO3;
                dto.VALORINDIVIDUAL3 = lista[0].VALORINDIVIDUAL3;

                dto.NOME4 = lista[0].NOME4;
                dto.QNTDVIDAS4 = lista[0].QNTDVIDAS4;
                dto.VALORCUSTO4 = lista[0].VALORCUSTO4;
                dto.VALORINDIVIDUAL4 = lista[0].VALORINDIVIDUAL4;

                dto.NOME5 = lista[0].NOME5;
                dto.QNTDVIDAS5 = lista[0].QNTDVIDAS5;
                dto.VALORCUSTO5 = lista[0].VALORCUSTO5;
                dto.VALORINDIVIDUAL5 = lista[0].VALORINDIVIDUAL5;

                dto.NOME6 = lista[0].NOME6;
                dto.QNTDVIDAS6 = lista[0].QNTDVIDAS6;
                dto.VALORCUSTO6 = lista[0].VALORCUSTO6;
                dto.VALORINDIVIDUAL6 = lista[0].VALORINDIVIDUAL6;

                dto.NOME7 = lista[0].NOME7;
                dto.QNTDVIDAS7 = lista[0].QNTDVIDAS7;
                dto.VALORCUSTO7 = lista[0].VALORCUSTO7;
                dto.VALORINDIVIDUAL7 = lista[0].VALORINDIVIDUAL7;

                dto.NOME8 = lista[0].NOME8;
                dto.QNTDVIDAS8 = lista[0].QNTDVIDAS8;
                dto.VALORCUSTO8 = lista[0].VALORCUSTO8;
                dto.VALORINDIVIDUAL8 = lista[0].VALORINDIVIDUAL8;

                dto.NOME9 = lista[0].NOME9;
                dto.QNTDVIDAS9 = lista[0].QNTDVIDAS9;
                dto.VALORCUSTO9 = lista[0].VALORCUSTO9;
                dto.VALORINDIVIDUAL9 = lista[0].VALORINDIVIDUAL9;

                dto.NOME10 = lista[0].NOME10;
                dto.QNTDVIDAS10 = lista[0].QNTDVIDAS10;
                dto.VALORCUSTO10 = lista[0].VALORCUSTO10;
                dto.VALORINDIVIDUAL10 = lista[0].VALORINDIVIDUAL10;

                dto.NOME11 = lista[0].NOME11;
                dto.QNTDVIDAS11 = lista[0].QNTDVIDAS11;
                dto.VALORCUSTO11 = lista[0].VALORCUSTO11;
                dto.VALORINDIVIDUAL11 = lista[0].VALORINDIVIDUAL11;

                dto.NOME12 = lista[0].NOME12;
                dto.QNTDVIDAS12 = lista[0].QNTDVIDAS12;
                dto.VALORCUSTO12 = lista[0].VALORCUSTO12;
                dto.VALORINDIVIDUAL12 = lista[0].VALORINDIVIDUAL12;

                dto.NOME13 = lista[0].NOME13;
                dto.QNTDVIDAS13 = lista[0].QNTDVIDAS13;
                dto.VALORCUSTO13 = lista[0].VALORCUSTO13;
                dto.VALORINDIVIDUAL13 = lista[0].VALORINDIVIDUAL13;

                dto.NOME14 = lista[0].NOME14;
                dto.QNTDVIDAS14 = lista[0].QNTDVIDAS14;
                dto.VALORCUSTO14 = lista[0].VALORCUSTO14;
                dto.VALORINDIVIDUAL14 = lista[0].VALORINDIVIDUAL14;

                dto.NOME15 = lista[0].NOME15;
                dto.QNTDVIDAS15 = lista[0].QNTDVIDAS15;
                dto.VALORCUSTO15 = lista[0].VALORCUSTO15;
                dto.VALORINDIVIDUAL15 = lista[0].VALORINDIVIDUAL15;
            }

            FormularioViewBags();

            return View("~/Areas/Contratos/Views/CAEPF/Partial/_NovoCustoValor.cshtml", dto);

        }


        [Autorizado]
        public ActionResult InserirCustoCAEPF(CustoValorViewModel dto)
        {
            Session["valores"] = null;

            var sel = new CustoValor();
            var list = new List<CustoValor>();

            int vidas = Convert.ToInt32(dto.QNTDVIDAS1) + Convert.ToInt32(dto.QNTDVIDAS2) + Convert.ToInt32(dto.QNTDVIDAS3) + Convert.ToInt32(dto.QNTDVIDAS4) + Convert.ToInt32(dto.QNTDVIDAS5) + Convert.ToInt32(dto.QNTDVIDAS6) + Convert.ToInt32(dto.QNTDVIDAS7) + Convert.ToInt32(dto.QNTDVIDAS8) + Convert.ToInt32(dto.QNTDVIDAS9) + Convert.ToInt32(dto.QNTDVIDAS10) + Convert.ToInt32(dto.QNTDVIDAS11) + Convert.ToInt32(dto.QNTDVIDAS12) + Convert.ToInt32(dto.QNTDVIDAS13) + Convert.ToInt32(dto.QNTDVIDAS14) + Convert.ToInt32(dto.QNTDVIDAS15);

            sel.QNTDVIDAS = vidas;
            sel.IDPLANO = dto.IDPLANO;
            sel.QNTDFAIXAS = dto.QNTDFAIXAS;

            double valortotal = 0;

            sel.NOME1 = dto.NOME1;
            sel.QNTDVIDAS1 = dto.QNTDVIDAS1;
            sel.VALORCUSTO1 = dto.VALORCUSTO1;
            var pessoas1 = Convert.ToDouble(dto.QNTDVIDAS1);
            var custo1 = Convert.ToDouble(dto.VALORCUSTO1);
            var custototal1 = custo1 * pessoas1;
            valortotal = valortotal + custototal1;
            sel.VALORINDIVIDUAL1 = Convert.ToDouble(custototal1).ToString("C");

            sel.NOME2 = dto.NOME2;
            sel.QNTDVIDAS2 = dto.QNTDVIDAS2;
            sel.VALORCUSTO2 = dto.VALORCUSTO2;
            var pessoas2 = Convert.ToDouble(dto.QNTDVIDAS2);
            var custo2 = Convert.ToDouble(dto.VALORCUSTO2);
            var custototal2 = custo2 * pessoas2;
            valortotal = valortotal + custototal2;
            sel.VALORINDIVIDUAL2 = Convert.ToDouble(custototal2).ToString("C");

            sel.NOME3 = dto.NOME3;
            sel.QNTDVIDAS3 = dto.QNTDVIDAS3;
            sel.VALORCUSTO3 = dto.VALORCUSTO3;
            var pessoas3 = Convert.ToDouble(dto.QNTDVIDAS3);
            var custo3 = Convert.ToDouble(dto.VALORCUSTO3);
            var custototal3 = custo3 * pessoas3;
            valortotal = valortotal + custototal3;
            sel.VALORINDIVIDUAL3 = Convert.ToDouble(custototal3).ToString("C");

            sel.NOME4 = dto.NOME4;
            sel.QNTDVIDAS4 = dto.QNTDVIDAS4;
            sel.VALORCUSTO4 = dto.VALORCUSTO4;
            var pessoas4 = Convert.ToDouble(dto.QNTDVIDAS4);
            var custo4 = Convert.ToDouble(dto.VALORCUSTO4);
            var custototal4 = custo4 * pessoas4;
            valortotal = valortotal + custototal4;
            sel.VALORINDIVIDUAL4 = Convert.ToDouble(custototal4).ToString("C");

            sel.NOME5 = dto.NOME5;
            sel.QNTDVIDAS5 = dto.QNTDVIDAS5;
            sel.VALORCUSTO5 = dto.VALORCUSTO5;
            var pessoas5 = Convert.ToDouble(dto.QNTDVIDAS5);
            var custo5 = Convert.ToDouble(dto.VALORCUSTO5);
            var custototal5 = custo5 * pessoas5;
            valortotal = valortotal + custototal5;
            sel.VALORINDIVIDUAL5 = Convert.ToDouble(custototal5).ToString("C");

            sel.NOME6 = dto.NOME6;
            sel.QNTDVIDAS6 = dto.QNTDVIDAS6;
            sel.VALORCUSTO6 = dto.VALORCUSTO6;
            var pessoas6 = Convert.ToDouble(dto.QNTDVIDAS6);
            var custo6 = Convert.ToDouble(dto.VALORCUSTO6);
            var custototal6 = custo6 * pessoas6;
            valortotal = valortotal + custototal6;
            sel.VALORINDIVIDUAL6 = Convert.ToDouble(custototal6).ToString("C");

            sel.NOME7 = dto.NOME7;
            sel.QNTDVIDAS7 = dto.QNTDVIDAS7;
            sel.VALORCUSTO7 = dto.VALORCUSTO7;
            var pessoas7 = Convert.ToDouble(dto.QNTDVIDAS7);
            var custo7 = Convert.ToDouble(dto.VALORCUSTO7);
            var custototal7 = custo7 * pessoas7;
            valortotal = valortotal + custototal7;
            sel.VALORINDIVIDUAL7 = Convert.ToDouble(custototal7).ToString("C");

            sel.NOME8 = dto.NOME8;
            sel.QNTDVIDAS8 = dto.QNTDVIDAS8;
            sel.VALORCUSTO8 = dto.VALORCUSTO8;
            var pessoas8 = Convert.ToDouble(dto.QNTDVIDAS8);
            var custo8 = Convert.ToDouble(dto.VALORCUSTO8);
            var custototal8 = custo8 * pessoas8;
            valortotal = valortotal + custototal8;
            sel.VALORINDIVIDUAL8 = Convert.ToDouble(custototal8).ToString("C");

            sel.NOME9 = dto.NOME9;
            sel.QNTDVIDAS9 = dto.QNTDVIDAS9;
            sel.VALORCUSTO9 = dto.VALORCUSTO9;
            var pessoas9 = Convert.ToDouble(dto.QNTDVIDAS9);
            var custo9 = Convert.ToDouble(dto.VALORCUSTO9);
            var custototal9 = custo9 * pessoas9;
            valortotal = valortotal + custototal9;
            sel.VALORINDIVIDUAL9 = Convert.ToDouble(custototal9).ToString("C");

            sel.NOME10 = dto.NOME10;
            sel.QNTDVIDAS10 = dto.QNTDVIDAS10;
            sel.VALORCUSTO10 = dto.VALORCUSTO10;
            var pessoas10 = Convert.ToDouble(dto.QNTDVIDAS10);
            var custo10 = Convert.ToDouble(dto.VALORCUSTO10);
            var custototal10 = custo10 * pessoas10;
            valortotal = valortotal + custototal10;
            sel.VALORINDIVIDUAL10 = Convert.ToDouble(custototal10).ToString("C");

            sel.NOME11 = dto.NOME11;
            sel.QNTDVIDAS11 = dto.QNTDVIDAS11;
            sel.VALORCUSTO11 = dto.VALORCUSTO11;
            var pessoas11 = Convert.ToDouble(dto.QNTDVIDAS11);
            var custo11 = Convert.ToDouble(dto.VALORCUSTO11);
            var custototal11 = custo11 * pessoas11;
            valortotal = valortotal + custototal11;
            sel.VALORINDIVIDUAL11 = Convert.ToDouble(custototal11).ToString("C");

            sel.NOME12 = dto.NOME12;
            sel.QNTDVIDAS12 = dto.QNTDVIDAS12;
            sel.VALORCUSTO12 = dto.VALORCUSTO12;
            var pessoas12 = Convert.ToDouble(dto.QNTDVIDAS12);
            var custo12 = Convert.ToDouble(dto.VALORCUSTO12);
            var custototal12 = custo12 * pessoas12;
            valortotal = valortotal + custototal12;
            sel.VALORINDIVIDUAL12 = Convert.ToDouble(custototal12).ToString("C");

            sel.NOME13 = dto.NOME13;
            sel.QNTDVIDAS13 = dto.QNTDVIDAS13;
            sel.VALORCUSTO13 = dto.VALORCUSTO13;
            var pessoas13 = Convert.ToDouble(dto.QNTDVIDAS13);
            var custo13 = Convert.ToDouble(dto.VALORCUSTO13);
            var custototal13 = custo13 * pessoas13;
            valortotal = valortotal + custototal13;
            sel.VALORINDIVIDUAL13 = Convert.ToDouble(custototal13).ToString("C");

            sel.NOME14 = dto.NOME14;
            sel.QNTDVIDAS14 = dto.QNTDVIDAS14;
            sel.VALORCUSTO14 = dto.VALORCUSTO14;
            var pessoas14 = Convert.ToDouble(dto.QNTDVIDAS14);
            var custo14 = Convert.ToDouble(dto.VALORCUSTO14);
            var custototal14 = custo14 * pessoas14;
            valortotal = valortotal + custototal14;
            sel.VALORINDIVIDUAL14 = Convert.ToDouble(custototal14).ToString("C");

            sel.NOME15 = dto.NOME15;
            sel.QNTDVIDAS15 = dto.QNTDVIDAS15;
            sel.VALORCUSTO15 = dto.VALORCUSTO15;
            var pessoas15 = Convert.ToDouble(dto.QNTDVIDAS15);
            var custo15 = Convert.ToDouble(dto.VALORCUSTO15);
            var custototal15 = custo15 * pessoas15;
            valortotal = valortotal + custototal15;
            sel.VALORINDIVIDUAL15 = Convert.ToDouble(custototal15).ToString("C");

            sel.VALOR = valortotal.ToString();

            list.Add(sel);

            Session["valores"] = list;

            return Json(new
            {
                success = true,
                dep = true,
                msg = "Adicionado com sucesso"
            });

        }

        #endregion

        #region calcula valor
        public async Task<JsonResult> CalcularValor(string valortaxa)
        {
            var taxateste2 = valortaxa.Replace("R$", "");
            var taxateste = Convert.ToDouble(taxateste2);

            var taxa = taxateste;
            var taxa1 = Convert.ToDouble(taxa).ToString("C");

            if (Session["valores"] != null)
            {
                var list = new List<CustoValor>();
                list = (List<CustoValor>)Session["valores"];

                var vidas = list[0].QNTDVIDAS;
                var valortotal = list[0].VALOR;


                var lista = new List<Opcional>();


                double OutVal;
                double.TryParse(valortotal, out OutVal);

                var primeirovalor = OutVal;

                if (Session["opcionaisCAEPF"] != null)
                    lista = (List<Opcional>)Session["opcionaisCAEPF"];

                double totalopcionalfamiliar;
                double totalopcionalpessoal;

                int qntdopcionalpessoal = 0;
                double valoropcionalpessoal = 0;
                int qntdopcionalfamiliar = 0;
                double valoropcionalfamiliar = 0;

                for (int i = 0; i <= lista.Count - 1; i++)
                {
                    var opc = lista[i];
                    double OutValores;

                    if (opc.GRUPO == "Familiar")
                    {
                        double.TryParse(opc.VALOR, out OutValores);

                        totalopcionalfamiliar = valoropcionalfamiliar + OutValores;
                        valoropcionalfamiliar = totalopcionalfamiliar;
                        qntdopcionalfamiliar += 1;
                    }
                    else
                    {
                        double.TryParse(opc.VALOR, out OutValores);

                        totalopcionalpessoal = valoropcionalpessoal + OutValores;
                        valoropcionalpessoal = totalopcionalpessoal;
                        qntdopcionalpessoal += 1;

                    }
                }

                var totalopcionalgeral = valoropcionalpessoal * vidas;
                var taxas = vidas * taxa;

                var totalprimeirafatura = primeirovalor + valoropcionalfamiliar + totalopcionalgeral + taxas;
                var total = primeirovalor + valoropcionalfamiliar + totalopcionalgeral;


                var total1 = Convert.ToDouble(total).ToString("C");
                var totalprimeirafatura1 = Convert.ToDouble(totalprimeirafatura).ToString("C");


                return Json(new
                {
                    qntvidas = vidas,
                    taxainscricao = taxa1,
                    valortotal = total1,
                    valorprimeirafatura = totalprimeirafatura1,

                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    qntvidas = 1,
                    taxainscricao = taxa1,
                    valortotal = "",
                    valorprimeirafatura = "",

                }, JsonRequestBehavior.AllowGet);
            }
        }


        [Autorizado]
        public ActionResult InserirContratoCAEPF(long? id, string cnpj, string razaosocial, string inscricaoestadual, string representante, string numeroproposta, string cpfrepresentante, string rgrepresentante, string nomefantasia, long? idplano, string grupoeconomico, string numerocontrato, string iss, string contatointerno, string email, string telefone1, string telefone2, string acomodacao, string modalidade, string abrangencia, string taxadeinscricao, string cep, string endereco, string numero, string bairro, string complemento, string cidade, string iduf, string idsegundoendereco, string cepcobranca, string enderecocobranca, string numerocobranca, string bairrocobranca, string complementocobranca, string cidadecobranca, string idufcobranca, string valortotal, string valorprimeirafatura, string totaldevidas, DateTime datavigencia, DateTime inicioatividade, string datavencimentomensal, DateTime datavencimentoprimeira, string participacaofinanceira, string observacao)
        {
            var taxainscricao = consulta.GetTaxaAtiva();
            SessionManager secao = SessionManager.GetInstance();
            CAEPF inserir = new CAEPF();

            var list = new List<CustoValor>();
            if (Session["valores"] != null)
                list = (List<CustoValor>)Session["valores"];

            var lista = new List<Opcional>();
            if (Session["opcionaisCAEPF"] != null)
                lista = (List<Opcional>)Session["opcionaisCAEPF"];

            int qntdfaixas = list.Count;
            int qntopcionais = lista.Count;

            if (id == null)
            {
                long? idempresa = 0;

                var empresacadastro = consulta.GetEmpresaByCNPJ(cnpj);
                if (empresacadastro.Count == 0)
                {
                    var permissao = new Empresas();

                    permissao.CNPJ = cnpj;
                    permissao.RAZAOSOCIAL = razaosocial;
                    permissao.NOMEFANTASIA = nomefantasia;
                    permissao.INSCRICAOESTADUAL = inscricaoestadual;
                    permissao.INICIOATIVIDADE = inicioatividade;
                    permissao.EMAIL = email;
                    permissao.ENDERECO = endereco;
                    permissao.NUMERO = numero;
                    permissao.COMPLEMENTO = complemento;
                    permissao.BAIRRO = bairro;
                    permissao.CIDADE = cidade;
                    permissao.UF = iduf;
                    permissao.CEP = cep;
                    permissao.STATUS = "Sim";
                    permissao.CONTATO = contatointerno;
                    permissao.REPRESENTANTELEGAL = representante;
                    permissao.CPFREPRESENTANTE = cpfrepresentante;
                    permissao.RGREPRESENTANTE = rgrepresentante;
                    permissao.TELEFONE = telefone1;
                    permissao.TELEFONE2 = telefone2;

                    permissao.DATACADASTRO = DateTime.Now;
                    permissao.USUARIOCADASTRO = secao.Usuario.Login;
                    consulta.InserirEmpresaCAEPF(ref permissao);
                    idempresa = permissao.ID;
                }
                else
                {
                    var permissao = new Empresas();
                    permissao.ID = empresacadastro[0].ID;
                    permissao.CNPJ = cnpj;
                    permissao.RAZAOSOCIAL = razaosocial;
                    permissao.NOMEFANTASIA = nomefantasia;
                    permissao.INSCRICAOESTADUAL = inscricaoestadual;
                    permissao.INICIOATIVIDADE = inicioatividade;
                    permissao.EMAIL = email;
                    permissao.ENDERECO = endereco;
                    permissao.NUMERO = numero;
                    permissao.COMPLEMENTO = complemento;
                    permissao.BAIRRO = bairro;
                    permissao.CIDADE = cidade;
                    permissao.UF = iduf;
                    permissao.CEP = cep;
                    permissao.STATUS = "Sim";
                    permissao.CONTATO = contatointerno;
                    permissao.TELEFONE = telefone1;
                    permissao.TELEFONE2 = telefone2;
                    permissao.REPRESENTANTELEGAL = representante;
                    permissao.CPFREPRESENTANTE = cpfrepresentante;
                    permissao.RGREPRESENTANTE = rgrepresentante;
                    permissao.DATACADASTRO = DateTime.Now;
                    permissao.USUARIOCADASTRO = secao.Usuario.Login;
                    consulta.UpdateEmpresa(permissao);

                    idempresa = empresacadastro[0].ID;
                }


                inserir.IDTAXAINSCRICAO = taxainscricao[0].ID;
                inserir.IDEMPRESA = idempresa;
                inserir.CONTATOINTERNO = contatointerno;
                inserir.TELEFONE1 = telefone1;
                inserir.TELEFONE2 = telefone2;
                inserir.IDPLANO = idplano;
                inserir.NUMEROCONTRATO = numerocontrato;
                inserir.NUMEROPROPOSTA = numeroproposta;
                inserir.GRUPOECONOMICO = grupoeconomico;
                inserir.ABRANGENCIA = abrangencia;
                inserir.ACOMODACAO = acomodacao;
                inserir.MODALIDADE = modalidade;
                inserir.CEPCOBRANCA = cepcobranca;
                inserir.ENDERECOCOBRANCA = enderecocobranca;
                inserir.NUMEROCOBRANCA = numerocobranca;
                inserir.BAIRROCOBRANCA = bairrocobranca;
                inserir.COMPLEMENTOCOBRANCA = complementocobranca;
                inserir.CIDADECOBRANCA = cidadecobranca;
                inserir.UFCOBRANCA = idufcobranca;
                inserir.TAXADEINSCRICAO = taxadeinscricao;
                inserir.TOTALDEVIDAS = totaldevidas;
                inserir.PARTICIPACAOFINANCEIRA = participacaofinanceira;
                inserir.DIAVENCIMENTO = datavencimentomensal;
                inserir.DATAPRIMEIRAFATURA = datavencimentoprimeira;
                inserir.VALORTOTAL = valortotal;
                inserir.VALORPRIMEIRAFATURA = valorprimeirafatura;
                inserir.OBSERVACAO = observacao;
                inserir.DATAVIGENCIACONTRATO = datavigencia;
                inserir.QNTOPCIONAIS = qntopcionais.ToString();
                inserir.QNTFAIXAS = qntdfaixas.ToString();
                inserir.USUARIOCADASTRO = secao.Usuario.Login;
                inserir.DATACADASTRO = DateTime.Now;

                consulta.InsertContratoCAEPF(ref inserir);

                var idcontratoCAEPF = inserir.ID;



                Opcional inseriropc = new Opcional();

                for (int i = 0; i <= lista.Count - 1; i++)
                {
                    var opc = lista[i];
                    inseriropc.IDCONTRATO = idcontratoCAEPF;
                    inseriropc.guid = opc.guid;
                    inseriropc.IDADICIONAIS = opc.IDADICIONAIS;
                    inseriropc.NOME = opc.NOME;
                    inseriropc.GRUPO = opc.GRUPO;
                    inseriropc.VALOR = opc.VALOR;
                    inseriropc.DATACADASTRO = DateTime.Now;
                    inseriropc.USUARIOCADASTRO = secao.Usuario.Login;

                    consulta.InsertOpcionaisCAEPF(ref inseriropc);
                }


                CustoValor inserircusto = new CustoValor();

                inserircusto.IDCONTRATO = idcontratoCAEPF;
                inserircusto.IDPLANO = list[0].IDPLANO;
                inserircusto.IDTABELAPRECO = list[0].IDTABELAPRECO;
                inserircusto.INICIO = list[0].INICIO;
                inserircusto.VALOR = list[0].VALOR;
                inserircusto.QNTDVIDAS = Convert.ToInt32(totaldevidas);
                inserircusto.QNTDFAIXAS = list[0].QNTDFAIXAS;

                inserircusto.NOME1 = list[0].NOME1;
                inserircusto.QNTDVIDAS1 = list[0].QNTDVIDAS1;
                inserircusto.VALORCUSTO1 = list[0].VALORCUSTO1;
                inserircusto.VALORINDIVIDUAL1 = list[0].VALORINDIVIDUAL1;
                inserircusto.NOME2 = list[0].NOME2;
                inserircusto.QNTDVIDAS2 = list[0].QNTDVIDAS2;
                inserircusto.VALORCUSTO2 = list[0].VALORCUSTO2;
                inserircusto.VALORINDIVIDUAL2 = list[0].VALORINDIVIDUAL2;
                inserircusto.NOME3 = list[0].NOME3;
                inserircusto.QNTDVIDAS3 = list[0].QNTDVIDAS3;
                inserircusto.VALORCUSTO3 = list[0].VALORCUSTO3;
                inserircusto.VALORINDIVIDUAL3 = list[0].VALORINDIVIDUAL3;
                inserircusto.NOME4 = list[0].NOME4;
                inserircusto.QNTDVIDAS4 = list[0].QNTDVIDAS4;
                inserircusto.VALORCUSTO4 = list[0].VALORCUSTO4;
                inserircusto.VALORINDIVIDUAL4 = list[0].VALORINDIVIDUAL4;
                inserircusto.NOME5 = list[0].NOME5;
                inserircusto.QNTDVIDAS5 = list[0].QNTDVIDAS5;
                inserircusto.VALORCUSTO5 = list[0].VALORCUSTO5;
                inserircusto.VALORINDIVIDUAL5 = list[0].VALORINDIVIDUAL5;
                inserircusto.NOME6 = list[0].NOME6;
                inserircusto.QNTDVIDAS6 = list[0].QNTDVIDAS6;
                inserircusto.VALORCUSTO6 = list[0].VALORCUSTO6;
                inserircusto.VALORINDIVIDUAL6 = list[0].VALORINDIVIDUAL6;
                inserircusto.NOME7 = list[0].NOME7;
                inserircusto.QNTDVIDAS7 = list[0].QNTDVIDAS7;
                inserircusto.VALORCUSTO7 = list[0].VALORCUSTO7;
                inserircusto.VALORINDIVIDUAL7 = list[0].VALORINDIVIDUAL7;
                inserircusto.NOME8 = list[0].NOME8;
                inserircusto.QNTDVIDAS8 = list[0].QNTDVIDAS8;
                inserircusto.VALORCUSTO8 = list[0].VALORCUSTO8;
                inserircusto.VALORINDIVIDUAL8 = list[0].VALORINDIVIDUAL8;
                inserircusto.NOME9 = list[0].NOME9;
                inserircusto.QNTDVIDAS9 = list[0].QNTDVIDAS9;
                inserircusto.VALORCUSTO9 = list[0].VALORCUSTO9;
                inserircusto.VALORINDIVIDUAL9 = list[0].VALORINDIVIDUAL9;
                inserircusto.NOME10 = list[0].NOME10;
                inserircusto.QNTDVIDAS10 = list[0].QNTDVIDAS10;
                inserircusto.VALORCUSTO10 = list[0].VALORCUSTO10;
                inserircusto.VALORINDIVIDUAL10 = list[0].VALORINDIVIDUAL10;
                inserircusto.NOME11 = list[0].NOME11;
                inserircusto.QNTDVIDAS11 = list[0].QNTDVIDAS11;
                inserircusto.VALORCUSTO11 = list[0].VALORCUSTO11;
                inserircusto.VALORINDIVIDUAL11 = list[0].VALORINDIVIDUAL11;
                inserircusto.NOME12 = list[0].NOME12;
                inserircusto.QNTDVIDAS12 = list[0].QNTDVIDAS12;
                inserircusto.VALORCUSTO12 = list[0].VALORCUSTO12;
                inserircusto.VALORINDIVIDUAL12 = list[0].VALORINDIVIDUAL12;
                inserircusto.NOME13 = list[0].NOME13;
                inserircusto.QNTDVIDAS13 = list[0].QNTDVIDAS13;
                inserircusto.VALORCUSTO13 = list[0].VALORCUSTO13;
                inserircusto.VALORINDIVIDUAL13 = list[0].VALORINDIVIDUAL13;
                inserircusto.NOME14 = list[0].NOME14;
                inserircusto.QNTDVIDAS14 = list[0].QNTDVIDAS14;
                inserircusto.VALORCUSTO14 = list[0].VALORCUSTO14;
                inserircusto.VALORINDIVIDUAL14 = list[0].VALORINDIVIDUAL14;
                inserircusto.NOME15 = list[0].NOME15;
                inserircusto.QNTDVIDAS15 = list[0].QNTDVIDAS15;
                inserircusto.VALORCUSTO15 = list[0].VALORCUSTO15;
                inserircusto.VALORINDIVIDUAL15 = list[0].VALORINDIVIDUAL15;
                inserircusto.DATACADASTRO = DateTime.Now;
                inserircusto.USUARIOCADASTRO = secao.Usuario.Login;

                consulta.InserCustoCAEPF(ref inserircusto);

                if (idcontratoCAEPF != null)
                {
                    return Json(new
                    {
                        success = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false
                    });
                }
            }
            else
            {
                var permissao = new Empresas();
                var empresacadastro = consulta.GetEmpresaByCNPJ(cnpj);

                permissao.ID = empresacadastro[0].ID;
                permissao.CNPJ = cnpj;
                permissao.RAZAOSOCIAL = razaosocial;
                permissao.NOMEFANTASIA = nomefantasia;
                permissao.INSCRICAOESTADUAL = inscricaoestadual;
                permissao.INICIOATIVIDADE = inicioatividade;
                permissao.EMAIL = email;
                permissao.ENDERECO = endereco;
                permissao.NUMERO = numero;
                permissao.COMPLEMENTO = complemento;
                permissao.BAIRRO = bairro;
                permissao.CIDADE = cidade;
                permissao.UF = iduf;
                permissao.CEP = cep;
                permissao.STATUS = "Sim";
                permissao.CONTATO = contatointerno;
                permissao.TELEFONE = telefone1;
                permissao.TELEFONE2 = telefone2;
                permissao.REPRESENTANTELEGAL = representante;
                permissao.CPFREPRESENTANTE = cpfrepresentante;
                permissao.RGREPRESENTANTE = rgrepresentante;
                permissao.DATACADASTRO = DateTime.Now;
                permissao.USUARIOCADASTRO = secao.Usuario.Login;
                consulta.UpdateEmpresa(permissao);

                inserir.ID = id;
                inserir.IDTAXAINSCRICAO = taxainscricao[0].ID;
                inserir.IDEMPRESA = empresacadastro[0].ID;
                inserir.CONTATOINTERNO = contatointerno;
                inserir.TELEFONE1 = telefone1;
                inserir.TELEFONE2 = telefone2;
                inserir.IDPLANO = idplano;
                inserir.NUMEROCONTRATO = numerocontrato;
                inserir.NUMEROPROPOSTA = numeroproposta;
                inserir.GRUPOECONOMICO = grupoeconomico;
                inserir.ABRANGENCIA = abrangencia;
                inserir.ACOMODACAO = acomodacao;
                inserir.MODALIDADE = modalidade;
                inserir.CEPCOBRANCA = cepcobranca;
                inserir.ENDERECOCOBRANCA = enderecocobranca;
                inserir.NUMEROCOBRANCA = numerocobranca;
                inserir.BAIRROCOBRANCA = bairrocobranca;
                inserir.COMPLEMENTOCOBRANCA = complementocobranca;
                inserir.CIDADECOBRANCA = cidadecobranca;
                inserir.UFCOBRANCA = idufcobranca;
                inserir.TAXADEINSCRICAO = taxadeinscricao;
                inserir.TOTALDEVIDAS = totaldevidas;
                inserir.DIAVENCIMENTO = datavencimentomensal;
                inserir.DATAPRIMEIRAFATURA = datavencimentoprimeira;
                inserir.VALORTOTAL = valortotal;
                inserir.VALORPRIMEIRAFATURA = valorprimeirafatura;
                inserir.PARTICIPACAOFINANCEIRA = participacaofinanceira;
                inserir.OBSERVACAO = observacao;
                inserir.DATAVIGENCIACONTRATO = datavigencia;
                inserir.QNTOPCIONAIS = qntopcionais.ToString();
                inserir.QNTFAIXAS = qntdfaixas.ToString();
                inserir.USUARIOATUALIZACAO = secao.Usuario.Login;
                inserir.DATAATUALIZACAO = DateTime.Now;
                consulta.UpdateContratoCAEPF(inserir);

                consulta.ExcluirCustoValor(id);
                consulta.ExcluirOpcionais(id);

                var idcontratoCAEPF = inserir.ID;

                Opcional inseriropc = new Opcional();

                for (int i = 0; i <= lista.Count - 1; i++)
                {
                    var opc = lista[i];
                    inseriropc.IDCONTRATO = idcontratoCAEPF;
                    inseriropc.guid = opc.guid;
                    inseriropc.IDADICIONAIS = opc.IDADICIONAIS;
                    inseriropc.NOME = opc.NOME;
                    inseriropc.GRUPO = opc.GRUPO;
                    inseriropc.VALOR = opc.VALOR;
                    inseriropc.DATACADASTRO = DateTime.Now;
                    inseriropc.USUARIOCADASTRO = secao.Usuario.Login;

                    consulta.InsertOpcionaisCAEPF(ref inseriropc);
                }

                CustoValor inserircusto = new CustoValor();

                inserircusto.IDCONTRATO = idcontratoCAEPF;
                inserircusto.IDPLANO = list[0].IDPLANO;
                inserircusto.IDTABELAPRECO = list[0].IDTABELAPRECO;
                inserircusto.INICIO = list[0].INICIO;
                inserircusto.VALOR = list[0].VALOR;
                inserircusto.QNTDFAIXAS = list[0].QNTDFAIXAS;
                inserircusto.QNTDVIDAS = Convert.ToInt32(totaldevidas);
                inserircusto.NOME1 = list[0].NOME1;
                inserircusto.QNTDVIDAS1 = list[0].QNTDVIDAS1;
                inserircusto.VALORCUSTO1 = list[0].VALORCUSTO1;
                inserircusto.VALORINDIVIDUAL1 = list[0].VALORINDIVIDUAL1;
                inserircusto.NOME2 = list[0].NOME2;
                inserircusto.QNTDVIDAS2 = list[0].QNTDVIDAS2;
                inserircusto.VALORCUSTO2 = list[0].VALORCUSTO2;
                inserircusto.VALORINDIVIDUAL2 = list[0].VALORINDIVIDUAL2;
                inserircusto.NOME3 = list[0].NOME3;
                inserircusto.QNTDVIDAS3 = list[0].QNTDVIDAS3;
                inserircusto.VALORCUSTO3 = list[0].VALORCUSTO3;
                inserircusto.VALORINDIVIDUAL3 = list[0].VALORINDIVIDUAL3;
                inserircusto.NOME4 = list[0].NOME4;
                inserircusto.QNTDVIDAS4 = list[0].QNTDVIDAS4;
                inserircusto.VALORCUSTO4 = list[0].VALORCUSTO4;
                inserircusto.VALORINDIVIDUAL4 = list[0].VALORINDIVIDUAL4;
                inserircusto.NOME5 = list[0].NOME5;
                inserircusto.QNTDVIDAS5 = list[0].QNTDVIDAS5;
                inserircusto.VALORCUSTO5 = list[0].VALORCUSTO5;
                inserircusto.VALORINDIVIDUAL5 = list[0].VALORINDIVIDUAL5;
                inserircusto.NOME6 = list[0].NOME6;
                inserircusto.QNTDVIDAS6 = list[0].QNTDVIDAS6;
                inserircusto.VALORCUSTO6 = list[0].VALORCUSTO6;
                inserircusto.VALORINDIVIDUAL6 = list[0].VALORINDIVIDUAL6;
                inserircusto.NOME7 = list[0].NOME7;
                inserircusto.QNTDVIDAS7 = list[0].QNTDVIDAS7;
                inserircusto.VALORCUSTO7 = list[0].VALORCUSTO7;
                inserircusto.VALORINDIVIDUAL7 = list[0].VALORINDIVIDUAL7;
                inserircusto.NOME8 = list[0].NOME8;
                inserircusto.QNTDVIDAS8 = list[0].QNTDVIDAS8;
                inserircusto.VALORCUSTO8 = list[0].VALORCUSTO8;
                inserircusto.VALORINDIVIDUAL8 = list[0].VALORINDIVIDUAL8;
                inserircusto.NOME9 = list[0].NOME9;
                inserircusto.QNTDVIDAS9 = list[0].QNTDVIDAS9;
                inserircusto.VALORCUSTO9 = list[0].VALORCUSTO9;
                inserircusto.VALORINDIVIDUAL9 = list[0].VALORINDIVIDUAL9;
                inserircusto.NOME10 = list[0].NOME10;
                inserircusto.QNTDVIDAS10 = list[0].QNTDVIDAS10;
                inserircusto.VALORCUSTO10 = list[0].VALORCUSTO10;
                inserircusto.VALORINDIVIDUAL10 = list[0].VALORINDIVIDUAL10;
                inserircusto.NOME11 = list[0].NOME11;
                inserircusto.QNTDVIDAS11 = list[0].QNTDVIDAS11;
                inserircusto.VALORCUSTO11 = list[0].VALORCUSTO11;
                inserircusto.VALORINDIVIDUAL11 = list[0].VALORINDIVIDUAL11;
                inserircusto.NOME12 = list[0].NOME12;
                inserircusto.QNTDVIDAS12 = list[0].QNTDVIDAS12;
                inserircusto.VALORCUSTO12 = list[0].VALORCUSTO12;
                inserircusto.VALORINDIVIDUAL12 = list[0].VALORINDIVIDUAL12;
                inserircusto.NOME13 = list[0].NOME13;
                inserircusto.QNTDVIDAS13 = list[0].QNTDVIDAS13;
                inserircusto.VALORCUSTO13 = list[0].VALORCUSTO13;
                inserircusto.VALORINDIVIDUAL13 = list[0].VALORINDIVIDUAL13;
                inserircusto.NOME14 = list[0].NOME14;
                inserircusto.QNTDVIDAS14 = list[0].QNTDVIDAS14;
                inserircusto.VALORCUSTO14 = list[0].VALORCUSTO14;
                inserircusto.VALORINDIVIDUAL14 = list[0].VALORINDIVIDUAL14;
                inserircusto.NOME15 = list[0].NOME15;
                inserircusto.QNTDVIDAS15 = list[0].QNTDVIDAS15;
                inserircusto.VALORCUSTO15 = list[0].VALORCUSTO15;
                inserircusto.VALORINDIVIDUAL15 = list[0].VALORINDIVIDUAL15;
                inserircusto.DATACADASTRO = DateTime.Now;
                inserircusto.USUARIOCADASTRO = secao.Usuario.Login;

                consulta.InserCustoCAEPF(ref inserircusto);


                if (idcontratoCAEPF != null)
                {
                    return Json(new
                    {
                        success = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false
                    });
                }


            }

        }

        #endregion

        #region envio e-mail


        [Autorizado]
        public ActionResult enviaremail(int id)
        {
            var session = SessionManager.GetInstance();
            var cadastro = consulta.Getemailbyid(id);
            var termos = consulta.GetallTermosAtivos();
            var contrato = consulta.GetIdPlano(id);
            var opcionais = consulta.GetallopcionaisAtivos(id);
            var emailsuario = "";
            var espelho = consulta.GetEspelho(id);

            if (espelho[0].STATUSESPELHO == null)
            {
                return Json(false);
            }

            if (cadastro[0].USUARIOATUALIZACAO == null)
            {
                var emailcadastros = consulta.GetUsuariobyCPF(cadastro[0].USUARIOCADASTRO);

                emailsuario = emailcadastros.EMAILVENDEDOR;
            }

            if (cadastro[0].USUARIOATUALIZACAO != null)
            {
                var emailcadastros = consulta.GetUsuariobyCPF(cadastro[0].USUARIOATUALIZACAO);

                emailsuario = emailcadastros.EMAILVENDEDOR;
            }

            var qualificacao = "Qualificacao" + id + ".pdf";
            var assinatura = "Assinatura" + id + ".pdf";
            var contratogerado = "Contrato" + id + ".pdf";
            var emailcadastro = cadastro[0].EMAIL;

            int qndtermos = termos.Count;

            var arquivoespelho = "C:/inetpub/wwwroot/contratoscomerciais/Publico/EspelhoContrato/CAEPF/EspelhoContrato" + id + ".pdf";

            var contratos = Path.Combine(Server.MapPath("/Publico/ContratosAssinados/CAEPF/"), contratogerado);

            var qualificacoes = Path.Combine(Server.MapPath("/Publico/QualificacaoContrato/CAEPF/"), qualificacao);

            int qndopcionais = opcionais.Count;


            StringBuilder emailText = new StringBuilder();
            emailText.Append("Em anexo está o seu contrato Assistencial Unimed, com as particularidades contratadas e definição do plano.");
            emailText.Append("<br/><br/>");
            emailText.Append("Caso tenha alguma dúvida pode entrar em contato pelo telefone 0800940 6900.");
            emailText.Append("<br/><br/>");
            emailText.Append("www.unimeduberlandia.coop.br");
            emailText.Append("<br/><br/><br/>");

            Email email = new Email();
            email.destinatarios.Add(emailcadastro);
            email.destinatarios.Add(emailsuario);
            email.templateFile = contratos;

            email.assunto = "Novo Contrato Unimed Uberlândia";
            email.templateText = emailText;
            email.anexos.Add(arquivoespelho);
            email.anexos.Add(qualificacoes);
            email.anexos.Add(contratos);
         

            if (qndtermos == 1)
            {

                var termos1 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/TermosContratuais/" + termos[0].ARQUIVOTERMO + "";
                email.anexos.Add(termos1);
            }
            if (qndtermos == 2)
            {
                var termos1 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/TermosContratuais/" + termos[0].ARQUIVOTERMO + "";
                email.anexos.Add(termos1);
                var termos2 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/TermosContratuais/" + termos[1].ARQUIVOTERMO + "";
                email.anexos.Add(termos2);
            }
            if (qndtermos == 3)
            {
                var termos1 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/TermosContratuais/" + termos[0].ARQUIVOTERMO + "";
                email.anexos.Add(termos1);
                var termos2 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/TermosContratuais/" + termos[1].ARQUIVOTERMO + "";
                email.anexos.Add(termos2);
                var termos3 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/TermosContratuais/" + termos[2].ARQUIVOTERMO + "";
                email.anexos.Add(termos3);
            }

            if (qndopcionais == 1)
            {
                var contratosopcionais1 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[0].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais1);
            }
            if (qndopcionais == 2)
            {
                var contratosopcionais1 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[0].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais1);
                var contratosopcionais2 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[1].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais2);
            }
            if (qndopcionais == 3)
            {
                var contratosopcionais1 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[0].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais1);
                var contratosopcionais2 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[1].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais2);
                var contratosopcionais3 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[2].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais3);
            }
            if (qndopcionais == 4)
            {
                var contratosopcionais1 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[0].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais1);
                var contratosopcionais2 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[1].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais2);
                var contratosopcionais3 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[2].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais3);
                var contratosopcionais4 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[3].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais4);
            }
            if (qndopcionais == 5)
            {
                var contratosopcionais1 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[0].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais1);
                var contratosopcionais2 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[1].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais2);
                var contratosopcionais3 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[2].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais3);
                var contratosopcionais4 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[3].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais4);
                var contratosopcionais5 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[4].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais5);
            }
            if (qndopcionais == 6)
            {
                var contratosopcionais1 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[0].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais1);
                var contratosopcionais2 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[1].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais2);
                var contratosopcionais3 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[2].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais3);
                var contratosopcionais4 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[3].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais4);
                var contratosopcionais5 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[4].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais5);
                var contratosopcionais6 = "C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAdicionais/" + opcionais[5].NOMEARQUIVO + "";
                email.anexos.Add(contratosopcionais6);
            }
            email.Enviar();

            return Json(true);
        }


        #endregion


        #region PDF


        [Autorizado]
        public ActionResult ContratoCAEPFPDF(int id)
        {
            var result = consulta.GetIdPlano(id);
            var consultastatus = consulta.GetIdQualificãcoes(id);

            if (consultastatus[0].STATUSASSINATURA == null)
            {
                ViewBag.STATUSASSINATURA = "";
            }
            if (consultastatus[0].STATUSASSINATURA != null)
            {
                ViewBag.STATUSASSINATURA = "Assinatura"+ id + ".pdf";
            }
            if (consultastatus[0].STATUSQUALIFICACAO == null)
            {
                ViewBag.QUALIFICACAO = ""; 
            }
            if (consultastatus[0].STATUSQUALIFICACAO != null)
            {
                ViewBag.QUALIFICACAO = "Qualificacao" + id + ".pdf";
            }
            if (consultastatus[0].STATUSESPELHO == null)
            {
                ViewBag.STATUSESPELHO = "";
            }
            if (consultastatus[0].STATUSESPELHO != null)
            {
                ViewBag.STATUSESPELHO = "EspelhoContrato" + id + ".pdf";
            }
            if (result[0].STATUSCONTRATO == null)
            {
                ViewBag.nomeArquivo = "";
            }
            if (result[0].STATUSCONTRATO != null)
            {
                ViewBag.nomeArquivo = "Contrato" + id + ".pdf"; ;
            }

            return View("~/Areas/Contratos/Views/CAEPF/Partial/_ArquivoPlanoPDF.cshtml");

        }

        [Autorizado]
        public ActionResult GerarOpcionaisPDF(long? id)
        {
            var result = consulta.GetallopcionaisAtivos(id);
            var termos = consulta.GetallTermosAtivos();

            PDFCAEPF teste2 = new PDFCAEPF();
            teste2.QNTDTERMOS = termos.Count;


            if (teste2.QNTDTERMOS == 1)
            {
                ViewBag.termo1 = termos[0].ARQUIVOTERMO;
            }

            if (teste2.QNTDTERMOS == 2)
            {
                ViewBag.termo1 = termos[0].ARQUIVOTERMO;
                ViewBag.termo2 = termos[1].ARQUIVOTERMO;
            }

            if (teste2.QNTDTERMOS == 3)
            {
                ViewBag.termo1 = termos[0].ARQUIVOTERMO;
                ViewBag.termo2 = termos[1].ARQUIVOTERMO;
                ViewBag.termo3 = termos[2].ARQUIVOTERMO;
            }

            if (teste2.QNTDTERMOS == 4)
            {
                ViewBag.termo1 = termos[0].ARQUIVOTERMO;
                ViewBag.termo2 = termos[1].ARQUIVOTERMO;
                ViewBag.termo3 = termos[2].ARQUIVOTERMO;
                ViewBag.termo4 = termos[3].ARQUIVOTERMO;
            }

            if (teste2.QNTDTERMOS == 5)
            {
                ViewBag.termo1 = termos[0].ARQUIVOTERMO;
                ViewBag.termo2 = termos[1].ARQUIVOTERMO;
                ViewBag.termo3 = termos[2].ARQUIVOTERMO;
                ViewBag.termo4 = termos[3].ARQUIVOTERMO;
                ViewBag.termo5 = termos[4].ARQUIVOTERMO;
            }

            if (result.Count == 1)
            {
                teste2.QNTOPCIONAIS = result[0].QNTOPCIONAIS;
                ViewBag.arquivo1 = result[0].NOMEARQUIVO;
            }

            if (result.Count == 2)
            {
                teste2.QNTOPCIONAIS = result[0].QNTOPCIONAIS;
                ViewBag.arquivo1 = result[0].NOMEARQUIVO;
                ViewBag.arquivo2 = result[1].NOMEARQUIVO;
            }

            if (result.Count == 3)
            {
                teste2.QNTOPCIONAIS = result[0].QNTOPCIONAIS;
                ViewBag.arquivo1 = result[0].NOMEARQUIVO;
                ViewBag.arquivo2 = result[1].NOMEARQUIVO;
                ViewBag.arquivo3 = result[2].NOMEARQUIVO;
            }

            if (result.Count == 4)
            {
                teste2.QNTOPCIONAIS = result[0].QNTOPCIONAIS;
                ViewBag.arquivo1 = result[0].NOMEARQUIVO;
                ViewBag.arquivo2 = result[1].NOMEARQUIVO;
                ViewBag.arquivo3 = result[2].NOMEARQUIVO;
                ViewBag.arquivo4 = result[3].NOMEARQUIVO;
            }

            if (result.Count == 5)
            {
                teste2.QNTOPCIONAIS = result[0].QNTOPCIONAIS;
                ViewBag.arquivo1 = result[0].NOMEARQUIVO;
                ViewBag.arquivo2 = result[1].NOMEARQUIVO;
                ViewBag.arquivo3 = result[2].NOMEARQUIVO;
                ViewBag.arquivo4 = result[3].NOMEARQUIVO;
                ViewBag.arquivo5 = result[4].NOMEARQUIVO;
            }

            if (result.Count == 6)
            {
                teste2.QNTOPCIONAIS = result[0].QNTOPCIONAIS;
                ViewBag.arquivo1 = result[0].NOMEARQUIVO;
                ViewBag.arquivo2 = result[1].NOMEARQUIVO;
                ViewBag.arquivo3 = result[2].NOMEARQUIVO;
                ViewBag.arquivo4 = result[3].NOMEARQUIVO;
                ViewBag.arquivo5 = result[4].NOMEARQUIVO;
                ViewBag.arquivo6 = result[5].NOMEARQUIVO;
            }

            if (result.Count == 7)
            {
                teste2.QNTOPCIONAIS = result[0].QNTOPCIONAIS;
                ViewBag.arquivo1 = result[0].NOMEARQUIVO;
                ViewBag.arquivo2 = result[1].NOMEARQUIVO;
                ViewBag.arquivo3 = result[2].NOMEARQUIVO;
                ViewBag.arquivo4 = result[3].NOMEARQUIVO;
                ViewBag.arquivo5 = result[4].NOMEARQUIVO;
                ViewBag.arquivo6 = result[5].NOMEARQUIVO;
                ViewBag.arquivo7 = result[6].NOMEARQUIVO;
            }
            if (result.Count > 7 || result.Count == 0)
            {
                teste2.QNTOPCIONAIS = 0;
            }

            return View("~/Areas/Contratos/Views/CAEPF/Partial/_GeraOpcionaisPDF.cshtml", teste2);
        }


        [Autorizado]
        public ActionResult PartialOpcionaisPDF(long? idplano)
        {
            var lista = consulta.GetOpcionalByPlano(idplano);

            return View("~/Areas/Contratos/Views/CAEPF/Partial/_OpcionalPDF.cshtml", lista);
        }

        [Autorizado]
        [ValidateInput(false)]
        [Obsolete]
        public ActionResult GerarContratoPDF(long? id)
        {
            var result = consulta.GetIdPlano(id);

            ViewBag.idcontrato = id;
            var fileName = "EspelhoContrato" + id + ".pdf";
            var filePath = Path.Combine(Server.MapPath("/Publico/EspelhoContrato/CAEPF/"), fileName);

            var relatorioPDF = new ViewAsPdf
            {
                Model = result[0],
                ViewName = "~/Areas/Contratos/Views/CAEPF/Partial/_GerarContratoPDF.cshtml",
                FileName = fileName,
                SaveOnServerPath = filePath,
                PageMargins = new Margins(5, 2, 2, 6)
            };

            consulta.UpdateStatusCAEPF(id);

            return relatorioPDF;

        }

        [Autorizado]
        [ValidateInput(false)]
        [Obsolete]
        public ActionResult GerarQualificaçãoPDF(long? id)
        {
            var result = consulta.GetIdQualificacao(id);
            var fileName = "Qualificacao" + id + ".pdf";
            var filePath = Path.Combine(Server.MapPath("/Publico/QualificacaoContrato/CAEPF/"), fileName);

            var relatorioPDF = new ViewAsPdf
            {
                Model = result[0],
                ViewName = "~/Areas/Contratos/Views/CAEPF/Partial/_GerarQualificacaoPDF.cshtml",
                FileName = fileName,
                SaveOnServerPath = filePath,
                PageMargins = new Margins(5, 2, 2, 6)
            };

            consulta.UpdateQualificacaoCAEPF(id);

            return relatorioPDF;

        }

        [ValidateInput(false)]
        [Obsolete]
        [Autorizado]
        public async Task<JsonResult> MergePDF(long? id)
        {
            var contrato = consulta.GetIdPlano(id);
            var assinatura = "Assinatura" + id + ".pdf";

            using (PdfDocument one = PdfReader.Open(Path.Combine(Server.MapPath("/Publico/contratos/"), contrato[0].NOMEARQUIVO), PdfDocumentOpenMode.Import))
            using (PdfDocument two = PdfReader.Open(Path.Combine(Server.MapPath("/Publico/Assinatura/CAEPF/" + assinatura + "")), PdfDocumentOpenMode.Import))

            using (PdfDocument outPdf = new PdfDocument())
            {
                CopyPages(one, outPdf);
                CopyPages(two, outPdf);


                outPdf.Save(Path.Combine(Server.MapPath("/Publico/ContratosAssinados/CAEPF/Contrato" + id + ".pdf")));

            }

            void CopyPages(PdfDocument from, PdfDocument to)
            {
                for (int i = 0; i < from.PageCount; i++)
                {
                    to.AddPage(from.Pages[i]);
                }
            }
            
            consulta.UpdateStatusContratoCAEPF(id);

            return Json(true);
        }


        [ValidateInput(false)]
        [Obsolete]
        [Autorizado]
        public ActionResult GerarAssinaturaPDF(long? id)
        {
            var result = consulta.GetIdAssinatura(id);
            var fileName = "Assinatura" + id + ".pdf";
            var filePath = Path.Combine(Server.MapPath("/Publico/Assinatura/CAEPF/"), fileName);

            result[0].DATACADASTRO = DateTime.Now;

            var relatorioPDF = new ViewAsPdf
            {
                Model = result[0],
                ViewName = "~/Areas/Contratos/Views/CAEPF/Partial/_GerarAssinaturaPDF.cshtml",
                FileName = fileName,
                SaveOnServerPath = filePath,
                PageMargins = new Margins(18, 15, 20, 20)

            };

            consulta.UpdateAssinaturaCAEPF(id);
            return relatorioPDF;
        }

        #endregion
    }
}