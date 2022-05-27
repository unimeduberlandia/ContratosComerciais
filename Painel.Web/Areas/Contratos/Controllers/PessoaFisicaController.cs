using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Painel.Core;
using Painel.Models.Contratos;
using Painel.Repositories.Contratos;
using Painel.Web.Areas.Contratos.Models;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace Painel.Web.Areas.Contratos.Controllers
{
    public class PessoaFisicaController : Controller
    {
        PessoaFisicaRepository consulta = new PessoaFisicaRepository();

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

            ViewBag.sexo = new[] {

                new { value = "M", text = "Masculino"  },
                new { value = "F", text = "Feminino"  }
            };

            ViewBag.parentesco = new[] {

                new { value = "Filho(a)", text = "Filho(a)"  },
                new { value = "Conjuge", text = "Cônjuge"  },
                new { value = "Companheiro(a)", text = "Companheiro(a)"  },
                new { value = "Filho(a) Deficiente", text = "Filho(a) Deficiente"  },
                new { value = "Filho(a) Universitário", text = "Filho(a) Universitário"  }
            };

            ViewBag.estadocivil = new[] {

                new { value = "Solteiro", text = "Solteiro"  },
                new { value = "Casado", text = "Casado"  },
                new { value = "Viuvo", text = "Viúvo"  },
                new { value = "Divorciado", text = "Divorciado"  },
                new { value = "Separado", text = "Separado"  },
                new { value = "Uniao Estavel", text = "União Estável"  }
            };

            //ViewBag.segundoendereco = new[] {

            //    new { value = "Sim", text = "Sim"  },
            //    new { value = "Nao", text = "Não"  }
            //};

            var planos = consulta.GetAllPlanosPFAtivos();
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

            var emailcadastro = cadastro[0].EMAIL;

            int qndtermos = termos.Count;

            var fileName = "EspelhoContrato" + id + ".pdf";
            var qualificacao = "Qualificacao" + id + ".pdf";
            //       var assinatura = "Assinatura" + id + ".pdf";
            var contratogerado = "Contrato" + id + ".pdf";
            var arquivoespelho = Path.Combine("C:/inetpub/wwwroot/contratoscomerciais/Publico/EspelhoContrato/PF/", fileName);

            //var arquivoespelho = Path.Combine(Server.MapPath("/Publico/EspelhoContrato/PF/"), fileName);

            var contratos = Path.Combine("C:/inetpub/wwwroot/contratoscomerciais/Publico/ContratosAssinados/PF/", fileName);

            //var contratos = Path.Combine(Server.MapPath("/Publico/ContratosAssinados/PF/"), contratogerado);
            //var qualificacoes = Path.Combine(Server.MapPath("/Publico/QualificacaoContrato/PF/"), qualificacao);
            var qualificacoes = Path.Combine("C:/inetpub/wwwroot/contratoscomerciais/Publico/QualificacaoContrato/PF/", fileName);

            // var assinaturas = Path.Combine(Server.MapPath("/Publico/Assinatura/PF/"), assinatura);
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

            email.assunto = "Contrato Assistencial Unimed Uberlândia";
            email.templateText = emailText;
            email.anexos.Add(arquivoespelho);
            email.anexos.Add(contratos);
            email.anexos.Add(qualificacoes);
            //      email.anexos.Add(assinaturas);

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


        #region contrato    

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
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Deseja realmente sair?", "Rei dos Pisos", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (Convert.ToString(result) == "Cancel") { e.Cancel = true; }
        }
        public async Task<JsonResult> BuscaValorPF(long? idplano, int idade)
        {
            var consultavalor = consulta.GetValorPlano(idplano, idade);
            {
                if (consultavalor.Count > 0)
                {
                    return Json(new
                    {
                        abrangencia = consultavalor[0].ABRANGENCIA,
                        acomodacao = consultavalor[0].ACOMODACAO,
                        modalidade = consultavalor[0].MODALIDADE,
                        valor = consultavalor[0].VALORINDIVIDUAL,
                        contrato = consultavalor[0].NUMEROCONTRATO
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false);
                }
            }
        }

        public async Task<JsonResult> CalcularValor(string valortitular, string valortaxa, string datavigencia)
        {
            var taxateste2 = valortaxa.Replace("R$", "");
            var taxateste = Convert.ToDouble(taxateste2);

            var taxa = taxateste;

            if (valortitular != "0")
            {
                double OutVal;
                double.TryParse(valortitular, out OutVal);

                var primeirovalor = OutVal;

                var list = new List<Dependente>();
                if (Session["dependentes"] != null)
                    list = (List<Dependente>)Session["dependentes"];

                double totaldendente;
                double valordependente = 0;

                int qntdependentes = list.Count;

                for (int i = 0; i <= list.Count - 1; i++)
                {
                    var dep = list[i];
                    double OutValor;

                    double.TryParse(dep.VALORDep, out OutValor);
                    totaldendente = valordependente + OutValor;
                    valordependente = totaldendente;
                }

                var lista = new List<Opcional>();
                if (Session["opcionais"] != null)
                    lista = (List<Opcional>)Session["opcionais"];

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
                var totalpessoas = qntdependentes + 1;

                var totalopcionalgeral = valoropcionalpessoal * totalpessoas;
                var taxas = totalpessoas * taxa;

                var totalprimeirafatura = valordependente + primeirovalor + valoropcionalfamiliar + totalopcionalgeral + taxas;
                var total = valordependente + primeirovalor + valoropcionalfamiliar + totalopcionalgeral;
                var totalplanos = valordependente + primeirovalor;
                var opcionaltotal = valoropcionalfamiliar + totalopcionalgeral;

                var totalprimeirafatura1 = "";
                var diasprorata = 0;
                double totalprorata = 0;

                if (datavigencia != "")
                {
                    DateTime datecon = Convert.ToDateTime(datavigencia);
                    var ultimodia = DateTime.DaysInMonth(datecon.Year, datecon.Month);
                    var diaescolhido = Convert.ToInt32(datecon.Day);

                    diasprorata = ultimodia - diaescolhido;

                    var diasproratatotal = diasprorata + 1;

                    var custodia = totalplanos / ultimodia;

                    totalprorata = custodia * diasproratatotal;

                    totalprorata = totalprorata + opcionaltotal + taxas;

                }

                var somaopcional = Convert.ToDouble(opcionaltotal).ToString("C");
                var somaplano = Convert.ToDouble(totalplanos).ToString("C");
                var taxa1 = Convert.ToDouble(taxa).ToString("C");

                if (diasprorata != 0)
                {
                    totalprimeirafatura1 = Convert.ToDouble(totalprorata).ToString("C");
                }
                if (diasprorata == 0)
                {
                    totalprimeirafatura1 = Convert.ToDouble(totalprimeirafatura).ToString("C");

                }

                var total1 = Convert.ToDouble(total).ToString("C");
                var primeirovalor1 = Convert.ToDouble(primeirovalor).ToString("C");
                var valordependente1 = Convert.ToDouble(valordependente).ToString("C");

                return Json(new
                {
                    somaopcionais = somaopcional,
                    somaplanos = somaplano,
                    taxainscricao2 = taxa1,
                    taxainscricao = taxa1,
                    valortotal = total1,
                    valorprimeirafatura = totalprimeirafatura1,
                    valortitular = primeirovalor1,
                    qntdependente = totalpessoas,
                    valordepenten = valordependente1
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    taxainscricao = taxa,
                    valortotal = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> BuscaIdade(DateTime nascimento)
        {
            var calculaidade = DateTime.Now.Year - nascimento.Year;

            if (DateTime.Now.DayOfYear < nascimento.DayOfYear)
            {
                calculaidade = calculaidade - 1;
            }

            try
            {
                return Json(new
                {
                    idade = calculaidade
                }, JsonRequestBehavior.AllowGet); ; ;
            }
            catch (System.Exception)
            {
                return Json(false);
            }
        }

        public async Task<JsonResult> ValidaCPFTitular(string cpftitular)
        {
            if (!Validacoes.ValidaCPF(cpftitular))
            {
                return Json(new
                {
                    cpftitular = "F"
                });
            }
            else
            {
                return Json(new
                {
                    cpftitular = "V"
                });
            }
        }

        public async Task<JsonResult> Buscatitular(string cpftitular)
        {
            try
            {
                var dto = consulta.GetDadosTitular(cpftitular);
                return Json(new
                {
                    titular = dto[0].TITULAR,
                    responsavelfinanceiro = dto[0].RESPONSAVELFINANCEIRO,
                    cpfresponsavel = dto[0].CPFRESPONSAVEL,
                    datanascimento = dto[0].DATANASCIMENTO,
                    cartaosaude = dto[0].CARTAOSAUDE,
                    rg = dto[0].RG,
                    orgaoexpedidor = dto[0].ORGAOEXPEDIDOR,
                    dataexpedicao = dto[0].DATAEXPEDICAO,
                    nomedamae = dto[0].NOMEDAMAE,
                    email = dto[0].EMAIL,
                    endereco = dto[0].ENDERECO,
                    numero = dto[0].NUMERO,
                    bairro = dto[0].BAIRRO,
                    sexo = dto[0].SEXO,
                    cidade = dto[0].CIDADE,
                    uf = dto[0].UF,
                    cep = dto[0].CEP,
                    complemento = dto[0].COMPLEMENTO,
                    telefone2 = dto[0].TELEFONE2,
                    telefone1 = dto[0].TELEFONE1

                }, JsonRequestBehavior.AllowGet); ; ;
            }
            catch (System.Exception)
            {
                return Json(false);
            }
        }

        [Autorizado]
        public ActionResult Index()
        {
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

            var dto = new List<PessoaFisica>();
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
                Session["opcionais"] = null;
                Session["dependentes"] = null;

                dto = consulta.GetContratoslogin(usuario, dataini, datafim);
            }
            if (tipo1 == "Administrador")
            {
                Session["opcionais"] = null;
                Session["dependentes"] = null;

                dto = consulta.GetAllContratos(dataini, datafim);
            }


            return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_PartialConsulta.cshtml", dto);

        }


        [Autorizado]
        public ActionResult NovoContrato()
        {
            Session["opcionais"] = null;
            Session["dependentes"] = null;
            SessionManager secao = SessionManager.GetInstance();

            var taxainscricao = consulta.GetTaxaAtiva();
            double OutTaxa;
            double.TryParse(taxainscricao[0].VALOR, out OutTaxa);

            var taxas = Convert.ToDouble(OutTaxa).ToString("C");

            FormularioViewBags();
            var dto = new PessoaFisicaViewModel();
            dto.TAXADEINSCRICAO = taxas.ToString();
            dto.DATAVENCIMENTOMENSAL = "13";
            dto.DATAFIMVIGENCIA = DateTime.Now;
            dto.DATAEXPEDICAO = DateTime.Now;
            dto.DATANASCIMENTO = DateTime.Now;
            dto.USUARIOCADASTRO = secao.Usuario.Nome;
            dto.DATACADASTRO = DateTime.Now;
            var datavencimento = DateTime.Now;
            var mesfatura = datavencimento.AddMonths(1);
            dto.statuscadastro = "nao";

            dto.DATAVENCIMENTOPRIMEIRAFATURA = new DateTime(mesfatura.Year, mesfatura.Month, 13);

            return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_novoContratoPF.cshtml", dto);
        }

        #endregion

        #region Opcionais    
        public async Task<JsonResult> BuscaValorAdicionalPF(long? idadicional)
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

            if (Session["opcionais"] != null)
                lista = (List<Opcional>)Session["opcionais"];

            return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_OpcionalPF.cshtml", lista);
        }

        [Autorizado]
        public ActionResult AdicionarOpcional(string guid)
        {
            if (guid != null)
            {
                var list = new List<Opcional>();


                if (Session["opcionais"] != null)
                {
                    list = (List<Opcional>)Session["opcionais"];
                    var dep = list.FirstOrDefault(q => q.guid == guid);
                    var dto = new OpcionalViewModel();
                    FormularioViewBags();

                    return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_OpcionalPF.cshtml", dto);

                }
                else
                {
                    var dto = new OpcionalViewModel();
                    dto.guid = Guid.NewGuid().ToString().Replace("-", "");
                    FormularioViewBags();

                    return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_OpcionalPF.cshtml", dto);

                }
            }
            else
            {
                var dto = new OpcionalViewModel();
                FormularioViewBags();

                return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_NovoOpcionalPF.cshtml", dto);
            }
        }


        [Autorizado]
        public ActionResult Postopcionais(OpcionalViewModel dto)
        {
            if (dto.IDADICIONAIS != null)
            {
                var sel = new Opcional();

                var list = new List<Opcional>();

                if (Session["opcionais"] != null)
                    list = (List<Opcional>)Session["opcionais"];

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

                Session["opcionais"] = list;

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
                return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_NovoOpcionalPF.cshtml", dto);
            }
        }



        [Autorizado]
        public ActionResult ExcluirOpcional(string guid)
        {
            if (!String.IsNullOrEmpty(guid))
            {
                var list = new List<Opcional>();

                if (Session["opcionais"] != null)
                    list = (List<Opcional>)Session["opcionais"];

                var dep = list.FirstOrDefault(q => q.guid == guid);

                list.Remove(dep);

                Session["opcionais"] = list;

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

        #region Dependentes

        [Autorizado]
        public ActionResult PartialDependente()
        {
            var lista = new List<Dependente>();

            if (Session["dependentes"] != null)
                lista = (List<Dependente>)Session["dependentes"];

            return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_Dependente.cshtml", lista);
        }

        [Autorizado]
        public ActionResult InserirDependente(DependenteViewModel dto)
        {
            if (dto.idplano != null && dto.NOMEDep != null && dto.CPFDep != null && dto.IDPARENTESCODep != null && dto.IDSEXODep != null && dto.IDESTADOCIVILDep != null && dto.NACIONALIDADEDep != null && dto.DATANASCIMENTODep.ToString() != "01/01/0001 00:00:00" && dto.NOMEDAMAEDep != null && dto.CARTAODESAUDEDep != null)
            {
                if (!Validacoes.ValidaCPF(dto.CPFDep))
                {
                    ModelState.AddModelError("CPFDep", "Este CPF não é válido");
                    FormularioViewBags();
                    return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_NovoDependente.cshtml", dto);
                }
                var depen = new Dependente();

                var list = new List<Dependente>();

                if (Session["dependentes"] != null)
                    list = (List<Dependente>)Session["dependentes"];

                if (!string.IsNullOrEmpty(dto.guidDependente))
                {
                    depen = list.FirstOrDefault(d => d.guidDependente == dto.guidDependente);
                    list.Remove(depen);
                }

                depen.NOMEDep = dto.NOMEDep;
                depen.CPFDep = dto.CPFDep;
                depen.PARENTESCODep = dto.IDPARENTESCODep;
                depen.SEXODep = dto.IDSEXODep;
                depen.ESTADOCIVILDep = dto.IDESTADOCIVILDep;
                depen.NACIONALIDADEDep = dto.NACIONALIDADEDep;
                depen.DATANASCIMENTODep = dto.DATANASCIMENTODep;
                depen.IDADEDep = dto.IDADEDep;
                depen.VALORDep = dto.VALORDep;
                depen.NOMEDAMAEDep = dto.NOMEDAMAEDep;
                depen.CARTAODESAUDEDep = dto.CARTAODESAUDEDep;
                depen.guidDependente = Guid.NewGuid().ToString().Replace("-", "");

                list.Add(depen);

                Session["dependentes"] = list;


                return Json(new
                {
                    success = true,
                    dep = true,
                    msg = "Adicionado com sucesso"
                });
            }

            else
            {
                FormularioViewBags();
                return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_NovoDependente.cshtml", dto);
            }
        }


        [Autorizado]
        public ActionResult AdicionarDependente(long? ID)
        {
            var dto = new DependenteViewModel();
            dto.idplano = ID;
            FormularioViewBags();

            return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_NovoDependente.cshtml", dto);
        }


        [Autorizado]
        public ActionResult ExcluirDepentende(string guidDependente)
        {
            if (!String.IsNullOrEmpty(guidDependente))
            {
                var list = new List<Dependente>();

                if (Session["dependentes"] != null)
                    list = (List<Dependente>)Session["dependentes"];

                var dep = list.FirstOrDefault(q => q.guidDependente == guidDependente);

                list.Remove(dep);

                Session["dependentes"] = list;

                return Json(new
                {
                    success = true,
                    msg = "Dependente excluído"
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

        public async Task<JsonResult> BuscaIdadeDependente(DateTime datanascimento, long? idplanos)
        {
            var calculaidade = DateTime.Now.Year - datanascimento.Year;

            if (DateTime.Now.DayOfYear < datanascimento.DayOfYear)
            {
                calculaidade = calculaidade - 1;
            }

            var consultavalor = consulta.GetValorPlano(idplanos, calculaidade);
            {
                if (consultavalor.Count > 0)
                {
                    return Json(new
                    {
                        idade = calculaidade,
                        valor = consultavalor[0].VALORINDIVIDUAL
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false);
                }
            }
        }

        #endregion

        #region EditarInserir

        [Autorizado]
        public ActionResult InserirContratoPF(long? id, string cpf, string nometitular, DateTime nascimento, string responsavelfin, string cpfresp, string idsexo, string nomedamae, string cartaosaude, string rg, string orgaoexpedidor, DateTime dataexpedicao, string email, string telefone1, string telefone2, long? idade, long? idplano, string numerdodaproposta, string numerodocontrato, string valorindividual, string abrangencia, string acomodacao, string modalidade, string cep, string endereco, string numero, string bairro, string complemento, string cidade, string iduf, string idsegundoendereco, string cepcobranca, string enderecocobranca, string numerocobranca, string bairrocobranca, string complementocobranca, string cidadecobranca, string idufcobranca, string taxadeinscricao, string totaldevidas, string datavencimentomensal, DateTime datavencimentoprimeirafatura, string valortotal, string valorprimeirafatura, string observacoes, DateTime datafimvigencia)
        {
            var taxainscricao = consulta.GetTaxaAtiva();
            SessionManager secao = SessionManager.GetInstance();
            PessoaFisica inserir = new PessoaFisica();

            if (nometitular == "Ciro")
            {
                ModelState.AddModelError("TITULAR", "TITULAR já cadastrado!");
                FormularioViewBags();
                return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_novoContratoPF.cshtml");
            }

            var list = new List<Dependente>();
            if (Session["dependentes"] != null)
                list = (List<Dependente>)Session["dependentes"];

            var lista = new List<Opcional>();
            if (Session["opcionais"] != null)
                lista = (List<Opcional>)Session["opcionais"];

            int qntdependentes = list.Count;
            int qntopcionais = lista.Count;

            if (id == null)
            {
                inserir.IDTAXAINSCRICAO = taxainscricao[0].ID;
                inserir.CPFTITULAR = cpf;
                inserir.TITULAR = nometitular;
                inserir.DATANASCIMENTO = nascimento;
                inserir.SEXO = idsexo;
                inserir.NOMEDAMAE = nomedamae;
                inserir.CARTAOSAUDE = cartaosaude;
                inserir.RG = rg;
                inserir.ORGAOEXPEDIDOR = orgaoexpedidor;
                inserir.DATAEXPEDICAO = dataexpedicao;
                inserir.EMAIL = email;
                inserir.TELEFONE1 = telefone1;
                inserir.TELEFONE2 = telefone2;
                inserir.RESPONSAVELFINANCEIRO = responsavelfin;
                inserir.CPFRESPONSAVEL = cpfresp;
                inserir.IDADE = Convert.ToInt32(idade);
                inserir.IDPLANO = idplano;
                inserir.NUMEROCONTRATO = numerodocontrato;
                inserir.NUMEROPROPOSTA = numerdodaproposta;
                inserir.VALORINDIVIDUAL = valorindividual;
                inserir.ABRANGENCIA = abrangencia;
                inserir.ACOMODACAO = acomodacao;
                inserir.MODALIDADE = modalidade;
                inserir.CEP = cep;
                inserir.ENDERECO = endereco;
                inserir.NUMERO = numero;
                inserir.BAIRRO = bairro;
                inserir.COMPLEMENTO = complemento;
                inserir.CIDADE = cidade;
                inserir.UF = iduf;
                inserir.CEPCOBRANCA = cepcobranca;
                inserir.ENDERECOCOBRANCA = enderecocobranca;
                inserir.NUMEROCOBRANCA = numerocobranca;
                inserir.BAIRROCOBRANCA = bairrocobranca;
                inserir.COMPLEMENTOCOBRANCA = complementocobranca;
                inserir.CIDADECOBRANCA = cidadecobranca;
                inserir.UFCOBRANCA = idufcobranca;
                inserir.TAXAINSCRICAO = taxadeinscricao;
                inserir.TOTALVIDAS = totaldevidas;
                inserir.DIAVENCIMENTO = datavencimentomensal;
                inserir.DATAPRIMEIRAFATURA = datavencimentoprimeirafatura;
                inserir.VALORTOTAL = valortotal;
                inserir.VALORPRIMEIRAFATURA = valorprimeirafatura;
                inserir.OBSERVACAO = observacoes;
                inserir.DATAVIGENCIACONTRATO = datafimvigencia;
                inserir.QNTDEPENDETES = qntdependentes;
                inserir.QNTOPCIONAIS = qntopcionais;
                inserir.USUARIOCADASTRO = secao.Usuario.Login;
                inserir.DATACADASTRO = DateTime.Now;
                consulta.InsertContratoPF(ref inserir);

                var idcontratoPF = inserir.ID;
                Dependente inserirdep = new Dependente();

                for (int i = 0; i <= list.Count - 1; i++)
                {
                    var dep = list[i];

                    inserirdep.IDCONTRATO = idcontratoPF;
                    inserirdep.NOMEDep = dep.NOMEDep;
                    inserirdep.guidDependente = dep.guidDependente;
                    inserirdep.IDADEDep = dep.IDADEDep;
                    inserirdep.CPFDep = dep.CPFDep;

                    if (dep.CARTAODESAUDEDep == null)
                    {
                        inserirdep.CARTAODESAUDEDep = "N/A";
                    }
                    else
                    {
                        inserirdep.CARTAODESAUDEDep = dep.CARTAODESAUDEDep;
                    }

                    inserirdep.DATANASCIMENTODep = dep.DATANASCIMENTODep;
                    inserirdep.SEXODep = dep.SEXODep;
                    inserirdep.VALORDep = dep.VALORDep;
                    inserirdep.PARENTESCODep = dep.PARENTESCODep;
                    inserirdep.NACIONALIDADEDep = dep.NACIONALIDADEDep;
                    inserirdep.NOMEDAMAEDep = dep.NOMEDAMAEDep;
                    inserirdep.ESTADOCIVILDep = dep.ESTADOCIVILDep;
                    inserirdep.DATACADASTRO = DateTime.Now;
                    inserirdep.USUARIOCADASTRO = secao.Usuario.Login;

                    consulta.InsertDependetesPF(ref inserirdep);
                }

                Opcional inseriropc = new Opcional();

                for (int i = 0; i <= lista.Count - 1; i++)
                {
                    var opc = lista[i];
                    inseriropc.IDCONTRATO = idcontratoPF;
                    inseriropc.guid = opc.guid;
                    inseriropc.IDADICIONAIS = opc.IDADICIONAIS;
                    inseriropc.NOME = opc.NOME;
                    inseriropc.GRUPO = opc.GRUPO;
                    inseriropc.VALOR = opc.VALOR;
                    inseriropc.DATACADASTRO = DateTime.Now;
                    inseriropc.USUARIOCADASTRO = secao.Usuario.Login;

                    consulta.InsertOpcionaisPF(ref inseriropc);
                }


                if (idcontratoPF != null)
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
                inserir.ID = id;
                inserir.CPFTITULAR = cpf;
                inserir.TITULAR = nometitular;
                inserir.DATANASCIMENTO = nascimento;
                inserir.SEXO = idsexo;
                inserir.NOMEDAMAE = nomedamae;
                inserir.CARTAOSAUDE = cartaosaude;
                inserir.RG = rg;
                inserir.ORGAOEXPEDIDOR = orgaoexpedidor;
                inserir.DATAEXPEDICAO = dataexpedicao;
                inserir.EMAIL = email;
                inserir.TELEFONE1 = telefone1;
                inserir.TELEFONE2 = telefone2;
                inserir.RESPONSAVELFINANCEIRO = responsavelfin;
                inserir.CPFRESPONSAVEL = cpfresp;
                inserir.IDADE = Convert.ToInt32(idade);
                inserir.IDPLANO = idplano;
                inserir.NUMEROCONTRATO = numerodocontrato;
                inserir.NUMEROPROPOSTA = numerdodaproposta;
                inserir.VALORINDIVIDUAL = valorindividual;
                inserir.ABRANGENCIA = abrangencia;
                inserir.ACOMODACAO = acomodacao;
                inserir.MODALIDADE = modalidade;
                inserir.CEP = cep;
                inserir.ENDERECO = endereco;
                inserir.NUMERO = numero;
                inserir.BAIRRO = bairro;
                inserir.COMPLEMENTO = complemento;
                inserir.CIDADE = cidade;
                inserir.UF = iduf;
                inserir.CEPCOBRANCA = cepcobranca;
                inserir.ENDERECOCOBRANCA = enderecocobranca;
                inserir.NUMEROCOBRANCA = numerocobranca;
                inserir.BAIRROCOBRANCA = bairrocobranca;
                inserir.COMPLEMENTOCOBRANCA = complementocobranca;
                inserir.CIDADECOBRANCA = cidadecobranca;
                inserir.UFCOBRANCA = idufcobranca;
                inserir.TAXAINSCRICAO = taxadeinscricao;
                inserir.TOTALVIDAS = totaldevidas;
                inserir.DIAVENCIMENTO = datavencimentomensal;
                inserir.DATAPRIMEIRAFATURA = datavencimentoprimeirafatura;
                inserir.VALORTOTAL = valortotal;
                inserir.VALORPRIMEIRAFATURA = valorprimeirafatura;
                inserir.OBSERVACAO = observacoes;
                inserir.DATAVIGENCIACONTRATO = datafimvigencia;
                inserir.QNTDEPENDETES = qntdependentes;
                inserir.QNTOPCIONAIS = qntopcionais;
                inserir.USUARIOATUALIZACAO = secao.Usuario.Login;
                inserir.DATAATUALIZACAO = DateTime.Now;
                consulta.UpdateContratoPF(inserir);
                consulta.ExcluirDepente(id);
                consulta.ExcluirOpcionais(id);

                var idcontratoPF = inserir.ID;
                Dependente inserirdep = new Dependente();

                for (int i = 0; i <= list.Count - 1; i++)
                {
                    var dep = list[i];

                    inserirdep.IDCONTRATO = idcontratoPF;
                    inserirdep.NOMEDep = dep.NOMEDep;
                    inserirdep.guidDependente = dep.guidDependente;
                    inserirdep.IDADEDep = dep.IDADEDep;
                    inserirdep.CPFDep = dep.CPFDep;

                    if (dep.CARTAODESAUDEDep == null)
                    {
                        inserirdep.CARTAODESAUDEDep = "N/A";
                    }
                    else
                    {
                        inserirdep.CARTAODESAUDEDep = dep.CARTAODESAUDEDep;
                    }
                    //inserirdep.CARTAODESAUDEDep = dep.CARTAODESAUDEDep;
                    inserirdep.DATANASCIMENTODep = dep.DATANASCIMENTODep;
                    inserirdep.SEXODep = dep.SEXODep;
                    inserirdep.VALORDep = dep.VALORDep;
                    inserirdep.PARENTESCODep = dep.PARENTESCODep;
                    inserirdep.NACIONALIDADEDep = dep.NACIONALIDADEDep;
                    inserirdep.NOMEDAMAEDep = dep.NOMEDAMAEDep;
                    inserirdep.ESTADOCIVILDep = dep.ESTADOCIVILDep;
                    inserirdep.DATACADASTRO = DateTime.Now;
                    inserirdep.USUARIOCADASTRO = secao.Usuario.Login;

                    consulta.InsertDependetesPF(ref inserirdep);
                }

                Opcional inseriropc = new Opcional();

                for (int i = 0; i <= lista.Count - 1; i++)
                {
                    var opc = lista[i];
                    inseriropc.IDCONTRATO = idcontratoPF;
                    inseriropc.guid = opc.guid;
                    inseriropc.IDADICIONAIS = opc.IDADICIONAIS;
                    inseriropc.NOME = opc.NOME;
                    inseriropc.GRUPO = opc.GRUPO;
                    inseriropc.VALOR = opc.VALOR;
                    inseriropc.DATACADASTRO = DateTime.Now;
                    inseriropc.USUARIOCADASTRO = secao.Usuario.Login;

                    consulta.InsertOpcionaisPF(ref inseriropc);
                }


                if (idcontratoPF != null)
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

        [Autorizado]
        public ActionResult EditarContrato(long? ID)
        {
            var dto = new PessoaFisicaViewModel();
            var editar = consulta.GetContratoByID(ID);

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


            dto.ID = editar[0].ID;
            dto.IDPLANO = editar[0].IDPLANO;
            dto.IDTAXAINSCRICAO = editar[0].IDTAXAINSCRICAO;
            dto.DATAFIMVIGENCIA = editar[0].DATAVIGENCIACONTRATO;
            dto.DATAVENCIMENTOMENSAL = editar[0].DIAVENCIMENTO;
            dto.VALORINDIVIDUAL = editar[0].VALORINDIVIDUAL;
            dto.DATAVENCIMENTOPRIMEIRAFATURA = editar[0].DATAPRIMEIRAFATURA;
            dto.VALORPRIMEIRAFATURA = editar[0].VALORPRIMEIRAFATURA;
            dto.TITULAR = editar[0].TITULAR;
            dto.ABRANGENCIA = editar[0].ABRANGENCIA;
            dto.ACOMODACAO = editar[0].ACOMODACAO;
            dto.MODALIDADE = editar[0].MODALIDADE;
            dto.TAXADEINSCRICAO = editar[0].TAXAINSCRICAO;
            dto.TOTALDEVIDAS = editar[0].TOTALVIDAS;
            dto.NUMEROCONTRATO = editar[0].NUMEROCONTRATO;
            dto.NUMEROPROPOSTA = editar[0].NUMEROPROPOSTA;
            dto.DATAVIGENCIACONTRATO = editar[0].DATAVIGENCIACONTRATO;
            dto.CPFTITULAR = editar[0].CPFTITULAR;
            dto.DATANASCIMENTO = editar[0].DATANASCIMENTO;
            dto.IDADE = editar[0].IDADE;
            dto.CARTAOSAUDE = editar[0].CARTAOSAUDE;
            dto.IDSEXO = editar[0].SEXO;
            dto.RG = editar[0].RG;
            dto.ORGAOEXPEDIDOR = editar[0].ORGAOEXPEDIDOR;
            dto.DATAEXPEDICAO = editar[0].DATAEXPEDICAO;
            dto.NOMEDAMAE = editar[0].NOMEDAMAE;
            dto.EMAIL = editar[0].EMAIL;
            dto.ENDERECO = editar[0].ENDERECO;
            dto.NUMERO = editar[0].NUMERO;
            dto.COMPLEMENTO = editar[0].COMPLEMENTO;
            dto.BAIRRO = editar[0].BAIRRO;
            dto.CIDADE = editar[0].CIDADE;
            dto.idUF = editar[0].UF;
            dto.CEP = editar[0].CEP;
            dto.TELEFONE1 = editar[0].TELEFONE1;
            dto.TELEFONE2 = editar[0].TELEFONE2;
            dto.RESPONSAVELFINANCEIRO = editar[0].RESPONSAVELFINANCEIRO;
            dto.CPFRESPONSAVEL = editar[0].CPFRESPONSAVEL;
            dto.OBSERVACAO = editar[0].OBSERVACAO;
            dto.ENDERECOCOBRANCA = editar[0].ENDERECOCOBRANCA;
            dto.NUMEROCOBRANCA = editar[0].NUMEROCOBRANCA;
            dto.COMPLEMENTOCOBRANCA = editar[0].COMPLEMENTOCOBRANCA;
            dto.BAIRROCOBRANCA = editar[0].BAIRROCOBRANCA;
            dto.CIDADECOBRANCA = editar[0].CIDADECOBRANCA;
            dto.IdUFCOBRANCA = editar[0].UFCOBRANCA;
            dto.CEPCOBRANCA = editar[0].CEPCOBRANCA;


            var listaopcionais = consulta.GetOpcionaisByIDContrato(editar[0].ID);

            if (listaopcionais.Count > 0)
                Session["opcionais"] = listaopcionais;

            var listadependentes = consulta.GetDependentesByIDContrato(editar[0].ID);

            if (listadependentes.Count > 0)
                Session["dependentes"] = listadependentes;

            FormularioViewBags();

            return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_novoContratoPF.cshtml", dto);
        }


        #endregion

        #region GerarPDF

        [Autorizado]
        public ActionResult ContratoPDF(int id)
        {
            var result = consulta.GetIdPlano(id);
            var consultastatus = consulta.GetIdQualificãcoes(id);

            if (consultastatus[0].STATUSASSINATURA == null)
            {
                ViewBag.STATUSASSINATURA = "";
            }
            if (consultastatus[0].STATUSASSINATURA != null)
            {
                ViewBag.STATUSASSINATURA = "Assinatura" + id + ".pdf";
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

            return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_ArquivoPlanoPDF.cshtml");

        }


        private string StripHtml(string source)
        {
            string output;

            output = Regex.Replace(source, "</p>", "\r\n", RegexOptions.Multiline);

            output = Regex.Replace(source, "<[^>]*>", string.Empty, RegexOptions.Multiline).Replace("&Aacute;", "Á").Replace("&Eacute;", "É").Replace("&Iacute;", "Í").Replace("&Oacute;", "Ó").Replace("&Uacute;", "Ú").Replace("&aacute;", "á").Replace("&eacute;", "é").Replace("&iacute;", "í").Replace("&oacute;", "ó").Replace("&uacute;", "ú").Replace("&Acirc;", "Â").Replace("&Ecirc;", "Ê").Replace("&Ocirc;", "Ô").Replace("&acirc;", "â").Replace("&ecirc;", "ê").Replace("&ocirc;", "ô").Replace("&Agrave;", "À").Replace("&agrave;", "à").Replace("&Uuml;", "Ü").Replace("&uuml;", "ü").Replace("&Ccedil;", "Ç").Replace("&ccedil;", "ç").Replace("&Atilde;", "Ã").Replace("&Otilde;", "Õ").Replace("&atilde;", "ã").Replace("&otilde;", "õ").Replace("&Ntilde;", "Ñ").Replace("&ntilde;", "ñ").Replace("&amp;", "&").Replace("&ordm;", "º");

            return output;
        }


        [Autorizado]
        public ActionResult GerarOpcionaisPDF(long? id)
        {
            var result = consulta.GetallopcionaisAtivos(id);
            var termos = consulta.GetallTermosAtivos();

            PDFPessoaFisica teste2 = new PDFPessoaFisica();
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

            if (teste2.QNTDTERMOS == 6)
            {
                ViewBag.termo1 = termos[0].ARQUIVOTERMO;
                ViewBag.termo2 = termos[1].ARQUIVOTERMO;
                ViewBag.termo3 = termos[2].ARQUIVOTERMO;
                ViewBag.termo4 = termos[3].ARQUIVOTERMO;
                ViewBag.termo5 = termos[4].ARQUIVOTERMO;
                ViewBag.termo6 = termos[5].ARQUIVOTERMO;

            }

            if (teste2.QNTDTERMOS == 7)
            {
                ViewBag.termo1 = termos[0].ARQUIVOTERMO;
                ViewBag.termo2 = termos[1].ARQUIVOTERMO;
                ViewBag.termo3 = termos[2].ARQUIVOTERMO;
                ViewBag.termo4 = termos[3].ARQUIVOTERMO;
                ViewBag.termo5 = termos[4].ARQUIVOTERMO;
                ViewBag.termo6 = termos[5].ARQUIVOTERMO;
                ViewBag.termo7 = termos[6].ARQUIVOTERMO;

            }

            if (teste2.QNTDTERMOS == 8)
            {
                ViewBag.termo1 = termos[0].ARQUIVOTERMO;
                ViewBag.termo2 = termos[1].ARQUIVOTERMO;
                ViewBag.termo3 = termos[2].ARQUIVOTERMO;
                ViewBag.termo4 = termos[3].ARQUIVOTERMO;
                ViewBag.termo5 = termos[4].ARQUIVOTERMO;
                ViewBag.termo6 = termos[5].ARQUIVOTERMO;
                ViewBag.termo7 = termos[6].ARQUIVOTERMO;
                ViewBag.termo8 = termos[7].ARQUIVOTERMO;

            }

            if (teste2.QNTDTERMOS == 9)
            {
                ViewBag.termo1 = termos[0].ARQUIVOTERMO;
                ViewBag.termo2 = termos[1].ARQUIVOTERMO;
                ViewBag.termo3 = termos[2].ARQUIVOTERMO;
                ViewBag.termo4 = termos[3].ARQUIVOTERMO;
                ViewBag.termo5 = termos[4].ARQUIVOTERMO;
                ViewBag.termo6 = termos[5].ARQUIVOTERMO;
                ViewBag.termo7 = termos[6].ARQUIVOTERMO;
                ViewBag.termo8 = termos[7].ARQUIVOTERMO;
                ViewBag.termo9 = termos[8].ARQUIVOTERMO;

            }

            if (teste2.QNTDTERMOS == 10)
            {
                ViewBag.termo1 = termos[0].ARQUIVOTERMO;
                ViewBag.termo2 = termos[1].ARQUIVOTERMO;
                ViewBag.termo3 = termos[2].ARQUIVOTERMO;
                ViewBag.termo4 = termos[3].ARQUIVOTERMO;
                ViewBag.termo5 = termos[4].ARQUIVOTERMO;
                ViewBag.termo6 = termos[5].ARQUIVOTERMO;
                ViewBag.termo7 = termos[6].ARQUIVOTERMO;
                ViewBag.termo8 = termos[7].ARQUIVOTERMO;
                ViewBag.termo9 = termos[8].ARQUIVOTERMO;
                ViewBag.termo10 = termos[9].ARQUIVOTERMO;

            }

            if (teste2.QNTDTERMOS == 11)
            {
                ViewBag.termo1 = termos[0].ARQUIVOTERMO;
                ViewBag.termo2 = termos[1].ARQUIVOTERMO;
                ViewBag.termo3 = termos[2].ARQUIVOTERMO;
                ViewBag.termo4 = termos[3].ARQUIVOTERMO;
                ViewBag.termo5 = termos[4].ARQUIVOTERMO;
                ViewBag.termo6 = termos[5].ARQUIVOTERMO;
                ViewBag.termo7 = termos[6].ARQUIVOTERMO;
                ViewBag.termo8 = termos[7].ARQUIVOTERMO;
                ViewBag.termo9 = termos[8].ARQUIVOTERMO;
                ViewBag.termo10 = termos[9].ARQUIVOTERMO;
                ViewBag.termo11 = termos[10].ARQUIVOTERMO;

            }

            if (teste2.QNTDTERMOS == 12)
            {
                ViewBag.termo1 = termos[0].ARQUIVOTERMO;
                ViewBag.termo2 = termos[1].ARQUIVOTERMO;
                ViewBag.termo3 = termos[2].ARQUIVOTERMO;
                ViewBag.termo4 = termos[3].ARQUIVOTERMO;
                ViewBag.termo5 = termos[4].ARQUIVOTERMO;
                ViewBag.termo6 = termos[5].ARQUIVOTERMO;
                ViewBag.termo7 = termos[6].ARQUIVOTERMO;
                ViewBag.termo8 = termos[7].ARQUIVOTERMO;
                ViewBag.termo9 = termos[8].ARQUIVOTERMO;
                ViewBag.termo10 = termos[9].ARQUIVOTERMO;
                ViewBag.termo11 = termos[10].ARQUIVOTERMO;
                ViewBag.termo12 = termos[11].ARQUIVOTERMO;

            }

            if (teste2.QNTDTERMOS == 13)
            {
                ViewBag.termo1 = termos[0].ARQUIVOTERMO;
                ViewBag.termo2 = termos[1].ARQUIVOTERMO;
                ViewBag.termo3 = termos[2].ARQUIVOTERMO;
                ViewBag.termo4 = termos[3].ARQUIVOTERMO;
                ViewBag.termo5 = termos[4].ARQUIVOTERMO;
                ViewBag.termo6 = termos[5].ARQUIVOTERMO;
                ViewBag.termo7 = termos[6].ARQUIVOTERMO;
                ViewBag.termo8 = termos[7].ARQUIVOTERMO;
                ViewBag.termo9 = termos[8].ARQUIVOTERMO;
                ViewBag.termo10 = termos[9].ARQUIVOTERMO;
                ViewBag.termo11 = termos[10].ARQUIVOTERMO;
                ViewBag.termo12 = termos[11].ARQUIVOTERMO;
                ViewBag.termo13 = termos[12].ARQUIVOTERMO;

            }

            if (teste2.QNTDTERMOS == 14)
            {
                ViewBag.termo1 = termos[0].ARQUIVOTERMO;
                ViewBag.termo2 = termos[1].ARQUIVOTERMO;
                ViewBag.termo3 = termos[2].ARQUIVOTERMO;
                ViewBag.termo4 = termos[3].ARQUIVOTERMO;
                ViewBag.termo5 = termos[4].ARQUIVOTERMO;
                ViewBag.termo6 = termos[5].ARQUIVOTERMO;
                ViewBag.termo7 = termos[6].ARQUIVOTERMO;
                ViewBag.termo8 = termos[7].ARQUIVOTERMO;
                ViewBag.termo9 = termos[8].ARQUIVOTERMO;
                ViewBag.termo10 = termos[9].ARQUIVOTERMO;
                ViewBag.termo11 = termos[10].ARQUIVOTERMO;
                ViewBag.termo12 = termos[11].ARQUIVOTERMO;
                ViewBag.termo13 = termos[12].ARQUIVOTERMO;
                ViewBag.termo14 = termos[13].ARQUIVOTERMO;

            }

            if (teste2.QNTDTERMOS == 15)
            {
                ViewBag.termo1 = termos[0].ARQUIVOTERMO;
                ViewBag.termo2 = termos[1].ARQUIVOTERMO;
                ViewBag.termo3 = termos[2].ARQUIVOTERMO;
                ViewBag.termo4 = termos[3].ARQUIVOTERMO;
                ViewBag.termo5 = termos[4].ARQUIVOTERMO;
                ViewBag.termo6 = termos[5].ARQUIVOTERMO;
                ViewBag.termo7 = termos[6].ARQUIVOTERMO;
                ViewBag.termo8 = termos[7].ARQUIVOTERMO;
                ViewBag.termo9 = termos[8].ARQUIVOTERMO;
                ViewBag.termo10 = termos[9].ARQUIVOTERMO;
                ViewBag.termo11 = termos[10].ARQUIVOTERMO;
                ViewBag.termo12 = termos[11].ARQUIVOTERMO;
                ViewBag.termo13 = termos[12].ARQUIVOTERMO;
                ViewBag.termo14 = termos[13].ARQUIVOTERMO;
                ViewBag.termo15 = termos[14].ARQUIVOTERMO;

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

            return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_GeraOpcionaisPDF.cshtml", teste2);
        }

        private string gerarcodigo(string texto)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("" + texto + "");
            return sb.ToString();
        }


        [ValidateInput(false)]
        [Obsolete]
        [Autorizado]
        public ActionResult GerarContratoPDF(long? id)
        {
            var result = consulta.GetIdPlano(id);
            ViewBag.idcontrato = id;
            var fileName = "EspelhoContrato" + id + ".pdf";
            //var filePath = Path.Combine("C:/inetpub/wwwroot/contratoscomerciais/Publico/EspelhoContrato/PF/", fileName);
            var filePath = Path.Combine(Server.MapPath("/Publico/EspelhoContrato/PF/"), fileName);

            var relatorioPDF = new ViewAsPdf
            {
                Model = result[0],
                ViewName = "~/Areas/Contratos/Views/PessoaFisica/Partial/_GerarContratoPDF.cshtml",
                FileName = fileName,
                SaveOnServerPath = filePath,
                PageMargins = new Margins(5, 2, 2, 6)

            };


            consulta.UpdateStatusPF(id);

            return relatorioPDF;


        }

        [ValidateInput(false)]
        [Obsolete]
        [Autorizado]
        public ActionResult GerarQualificacaoPDF(long? id)
        {
            var result = consulta.GetIdQualificacao(id);
            ViewBag.idcontrato = id;
            var fileName = "Qualificacao" + id + ".pdf";
            //var filePath = Path.Combine("C:/inetpub/wwwroot/contratoscomerciais/Publico/QualificacaoContrato/PF/", fileName);

            var filePath = Path.Combine(Server.MapPath("/Publico/QualificacaoContrato/PF/"), fileName);

            var relatorioPDF = new ViewAsPdf
            {
                Model = result[0],
                ViewName = "~/Areas/Contratos/Views/PessoaFisica/Partial/_GerarQualificacaoPDF.cshtml",
                FileName = fileName,
                SaveOnServerPath = filePath,
                PageMargins = new Margins(18, 15, 15, 15)

            };

            consulta.UpdateQualificacaoPF(id);
            return relatorioPDF;
        }

        [ValidateInput(false)]
        [Obsolete]
        [Autorizado]
        public async Task<JsonResult> MergePDF(long? id)
        {
            //var contrato = consulta.GetIdPlano(id);
            //var assinatura = "Assinatura" + id + ".pdf";

            //using (PdfDocument one = PdfReader.Open(Path.Combine(Server.MapPath("/Publico/contratos/"), contrato[0].NOMEARQUIVO), PdfDocumentOpenMode.Import))
            //using (PdfDocument two = PdfReader.Open(Path.Combine(Server.MapPath("/Publico/Assinatura/PF/" + assinatura + "")), PdfDocumentOpenMode.Import))

            //using (PdfDocument outPdf = new PdfDocument())
            //{
            //    CopyPages(one, outPdf);
            //    CopyPages(two, outPdf);


            //    outPdf.Save(Path.Combine(Server.MapPath("/Publico/ContratosAssinados/PF/Contrato" + id + ".pdf")));

            //}

            //void CopyPages(PdfDocument from, PdfDocument to)
            //{
            //    for (int i = 0; i < from.PageCount; i++)
            //    {
            //        to.AddPage(from.Pages[i]);
            //    }
            //}
            //consulta.UpdateStatusContratoPF(id);

            return Json(true);
        }

        [ValidateInput(false)]
        [Obsolete]
        [Autorizado]
        public ActionResult GerarAssinaturaPDF(long? id)
        {
            var result = consulta.GetIdAssinatura(id);
            var fileName = "Assinatura" + id + ".pdf";
            //var filePath = Path.Combine("C:/inetpub/wwwroot/contratoscomerciais/Publico/Assinatura/PF/", fileName);

            var filePath = Path.Combine(Server.MapPath("/Publico/Assinatura/PF/"), fileName);

            result[0].DATACADASTRO = DateTime.Now;

            var relatorioPDF = new ViewAsPdf
            {
                Model = result[0],
                ViewName = "~/Areas/Contratos/Views/PessoaFisica/Partial/_GerarAssinaturaPDF.cshtml",
                FileName = fileName,
                SaveOnServerPath = filePath,
                PageMargins = new Margins(18, 15, 20, 20)

            };

            consulta.UpdateAssinaturaPF(id);
            return relatorioPDF;
        }


        [Autorizado]
        public ActionResult PartialOpcionaisPDF(long? idplano)
        {
            var lista = consulta.GetOpcionalByPlano(idplano);

            return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_OpcionalPDF.cshtml", lista);
        }

        [Autorizado]
        public ActionResult PartialDependentesPDF(long? idplano)
        {
            var lista = consulta.GetDependenteByPlano(idplano);

            return View("~/Areas/Contratos/Views/PessoaFisica/Partial/_DependentePdf.cshtml", lista);
        }


        #endregion
    }
}