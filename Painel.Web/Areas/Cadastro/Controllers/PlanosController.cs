using Newtonsoft.Json;
using Painel.Core;
using Painel.Models.Cadastro;
using Painel.Repositories.Cadastro;
using Newtonsoft.Json;
using Painel.Web.Areas.Cadastro.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Globalization;

namespace Painel.Web.Areas.Cadastro.Controllers
{
    public class PlanosController : Controller
    {
        PlanosRepository consulta = new PlanosRepository();

        private string url_hrp_consulta_plano = "http://192.168.215.4/services/ServicesUberlandia/ServiceDepartamentos.ijs";


        private void FormularioViewBags()
        {
            ViewBag.abrangencia = new[] {
                new { value = "Local", text = "Local"  },
                new { value = "Estadual", text = "Estadual"  },
                new { value = "Nacional", text = "Nacional"  },
            };

            ViewBag.acomodacao = new[] {
                new { value = "Enfermaria", text = "Enfermaria"  },
                new { value = "Apartamento", text = "Apartamento"  },
            };

            ViewBag.modalidade = new[] {
                new { value = "Comcoparticipacao", text = "Com Coparticipação"  },
                new { value = "Semcoparticipacao", text = "Sem Coparticipação"  },
            };

        }

        [Autorizado("admin")]
        public ActionResult Index()
        {
            var dto = consulta.GetAllPlanos();

            return View(dto);
        }

        [Autorizado("admin")]
        public ActionResult NovoPlano()
        {
            return View("~/Areas/Cadastro/Views/Planos/Partial/_Consulta.cshtml");
        }


        [Autorizado("admin")]
        public ActionResult BuscarPlanos(string codigoconsulta)
        {
            if (codigoconsulta != "")
            {
                var consultaporcodigo = consulta.GetPlanosByCodigo(codigoconsulta);
                if (consultaporcodigo.Count > 0)
                {
                    ModelState.AddModelError("codigoconsulta", "Plano já foi cadastrado!");
                    FormularioViewBags();
                    return View("~/Areas/Cadastro/Views/Planos/Partial/_Consulta.cshtml");
                }
                var sb = new StringBuilder();
                sb.Append("{\"tipo\" : \"22\",");
                sb.Append("\"codigo\" : \"" + codigoconsulta + "\"}");

                string enviojson = Model.Util.Requisicao.POST(Encoding.UTF8, url_hrp_consulta_plano, sb.ToString());

                // var teste = parse(enviojson);

                UTF8Encoding utf8 = new UTF8Encoding();

                var data = utf8.GetBytes(enviojson);

                string utf8String = String.Empty;

                for (int i = 0; i < data.Length; i++)
                {
                    byte[] utf8Container = new byte[2] { data[i], 0 };
                    utf8String += BitConverter.ToChar(utf8Container, 0);
                }

                var jsonconvertido = JsonConvert.DeserializeObject<resultados>(utf8String);

                PlanosViewModel dto = new PlanosViewModel();

                if (jsonconvertido.resultado.Count == 0)
                {
                    ModelState.AddModelError("codigoconsulta", "Código inválido!");
                    return View("~/Areas/Cadastro/Views/Planos/Partial/_Consulta.cshtml");
                }

                dto.PLANO = jsonconvertido.resultado[0].nome.Replace("REFERï¿½", "REFERÊ").Replace("ADESï¿½O", "ADESÃO").Replace("PRï¿½", "PRÉ").Replace("Bï¿½SICO", "BÁSICO").Replace("OBSTETRï¿½", "OBSTETRI").Replace("SOLUï¿½ï¿½ES", "SOLUÇÕES").Replace("PADRï¿½O", "PADRÃO").Replace("OBSTETETRï¿½CIA", "OBSTETRICIA").Replace("EQUILï¿½BRIO", "EQUILIBRIO");
                dto.CODIGO = codigoconsulta;

                if (jsonconvertido.resultado[0].tipocontratacao != null)
                {
                    dto.TIPOCONTRATACAO = jsonconvertido.resultado[0].tipocontratacao.Replace("esï¿½o", "esão").Replace("rï¿½", "ré");
                }
                if (jsonconvertido.resultado[0].tipocontratacao == null)
                {
                    dto.TIPOCONTRATACAO = "";
                }

                if (jsonconvertido.resultado[0].formacaopreco != null)
                {
                    dto.FORMACAOPRECO = jsonconvertido.resultado[0].formacaopreco.Replace("esï¿½o", "esão").Replace("rï¿½", "ré");
                }
                if (jsonconvertido.resultado[0].formacaopreco == null)
                {
                    dto.FORMACAOPRECO = "";
                }

                if (jsonconvertido.resultado[0].abrangencia != null)
                {
                    dto.ABRANGENCIA = jsonconvertido.resultado[0].abrangencia.Replace("eferï¿½", "eferê").Replace("bstetrï¿½", "bstetri").Replace("adrï¿½", "adrã").Replace("equilï¿½brio", "equilibrio");
                }
                if (jsonconvertido.resultado[0].abrangencia == null)
                {
                    dto.ABRANGENCIA = "";
                }

                if (jsonconvertido.resultado[0].segmentacaoassistencial != null)
                {
                    dto.DESCRICAO = jsonconvertido.resultado[0].segmentacaoassistencial.Replace("eferï¿½", "eferê").Replace("bstetrï¿½", "bstetri");
                }
                if (jsonconvertido.resultado[0].segmentacaoassistencial == null)
                {
                    dto.DESCRICAO = "";
                }

                if (jsonconvertido.resultado[0].acomodacao != null)
                {
                    dto.ACOMODACAO = jsonconvertido.resultado[0].acomodacao.Replace("eferï¿½", "eferê").Replace("bstetrï¿½", "bstetri");
                }
                if (jsonconvertido.resultado[0].acomodacao == null)
                {
                    dto.ACOMODACAO = "";
                }

                if (jsonconvertido.resultado[0].coparticipacao != null)
                {
                    dto.MODALIDADE = jsonconvertido.resultado[0].coparticipacao.Replace("eferï¿½", "eferê").Replace("bstetrï¿½", "bstetri");
                }
                if (jsonconvertido.resultado[0].coparticipacao == null)
                {
                    dto.MODALIDADE = "";
                }


                return View("~/Areas/Cadastro/Views/Planos/Partial/_NovoPlano.cshtml", dto);
            }
            else
            {
                ModelState.AddModelError("codigoconsulta", "Informe um código!");
                return View("~/Areas/Cadastro/Views/Planos/Partial/_Consulta.cshtml");
            }
        }


        [Autorizado("admin")]
        public ActionResult EditarPlano(long? id)
        {
            var permissao = consulta.GetByIdPlano(id);

            var dto = new PlanosViewModel();
            dto.PLANO = permissao.PLANO;
            dto.id = id;
            dto.CODIGO = permissao.CODIGO;
            dto.DESCRICAO = permissao.DESCRICAO;
            dto.ABRANGENCIA = permissao.ABRANGENCIA;
            dto.ACOMODACAO = permissao.ACOMODACAO;
            dto.MODALIDADE = permissao.MODALIDADE;
            dto.TIPOCONTRATACAO = permissao.TIPOCONTRATACAO;
            dto.FORMACAOPRECO = permissao.FORMACAOPRECO;
            dto.STATUS = permissao.STATUS;
            dto.NOMEARQUIVO = permissao.NOMEARQUIVO;
            dto.NUMEROCONTRATO = permissao.NUMEROCONTRATO;
            ViewBag.ativo = permissao.STATUS;
            ViewBag.tipocontrato = permissao.TIPOCONTRATO;
            FormularioViewBags();
            return View("~/Areas/Cadastro/Views/Planos/Partial/_NovoPlano.cshtml", dto);
        }

        [Autorizado("admin")]
        public ActionResult VisualizarPDF(int id)
        {
            var result = consulta.GetByIdPlano(id);
            if (result.NOMEARQUIVO == null)
            {
                ViewBag.nomeArquivo = "";
            }
            else
            {
                ViewBag.nomeArquivo = result.NOMEARQUIVO;
            }
            return View("~/Areas/Cadastro/Views/Planos/Partial/_VisualizarPDF.cshtml");

        }

        [Autorizado("admin")]
        public ActionResult PartialTabela()
        {
            var lista = new List<TabelaPrecos>();

            if (Session["valorplano"] != null)
                lista = (List<TabelaPrecos>)Session["valorplano"];

            return View("~/Areas/Cadastro/Views/Planos/Partial/_TabelaPreco.cshtml", lista);
        }

        [Autorizado("admin")]
        public ActionResult NovaTabela()
        {
            var dto = new TabelaPrecosViewModel();
            return View("~/Areas/Cadastro/Views/Planos/Partial/_NovoTabelaPreco.cshtml", dto);
        }

        [Autorizado("admin")]
        [HttpPost]
        public ActionResult InserirPlano(PlanosViewModel dto)
        {
            if (dto.PLANO != null && dto.DESCRICAO != null && dto.CODIGO != null)
            {
                var permissao = new Planos();
                SessionManager session = SessionManager.GetInstance();

                if (dto.id == null)
                {
                    var planos = consulta.GetPlanosBynome(dto.PLANO);
                    if (planos.Count > 0)
                    {
                        ModelState.AddModelError("PLANO", "Plano já foi cadastrado!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/Planos/Partial/_NovoPlano.cshtml", dto);
                    }

                    permissao.PLANO = dto.PLANO;
                    permissao.CODIGO = dto.CODIGO;
                    permissao.DESCRICAO = dto.DESCRICAO;
                    permissao.ABRANGENCIA = dto.ABRANGENCIA;
                    permissao.ACOMODACAO = dto.ACOMODACAO;
                    permissao.MODALIDADE = dto.MODALIDADE;
                    permissao.STATUS = dto.STATUS;
                    permissao.TIPOCONTRATACAO = dto.TIPOCONTRATACAO;
                    permissao.FORMACAOPRECO = dto.FORMACAOPRECO;
                    permissao.TIPOCONTRATO = dto.tipocontrato;
                    permissao.NUMEROCONTRATO = dto.NUMEROCONTRATO;
                    permissao.DATACADASTRO = DateTime.Now;
                    permissao.USUARIOCADASTRO = session.Usuario.Login;

                    int arquivosSalvos = 0;

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase arquivo = Request.Files[i];

                        if (arquivo.ContentLength > 0)
                        {
                            var uploadPath = Server.MapPath("~/Publico/Contratos/");
                            string caminhoArquivo = Path.Combine(@uploadPath, Path.GetFileName(dto.PLANO + '.' + arquivo.FileName));

                            if (i == 0)
                            {
                                permissao.NOMEARQUIVO = dto.PLANO + '.' + arquivo.FileName;
                            }
                            try
                            {
                                arquivo.SaveAs(caminhoArquivo);
                                arquivosSalvos++;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    if (permissao.NOMEARQUIVO != null)
                    {
                        consulta.InserirPlano(ref permissao);
                        var ALL = consulta.GetAllPlanos();
                        ViewData["Message"] = "Cadastro efetudado com sucesso";
                        return View("~/Areas/Cadastro/Views/Planos/index.cshtml", ALL);
                    }
                    else
                    {
                        ModelState.AddModelError("NOMEARQUIVO", "Arquivo do contrato é obrigatório");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/Planos/Partial/_NovoPlano.cshtml", dto);
                    }

                }
                else
                {
                    var planos = consulta.GetPlanosBynomeId(dto.PLANO, dto.id);

                    if (planos.Count > 0)
                    {
                        ModelState.AddModelError("PLANO", "Existe um plano com o mesmo nome!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/Planos/Partial/_NovoPlano.cshtml", dto);
                    }
                    permissao.PLANO = dto.PLANO;
                    permissao.CODIGO = dto.CODIGO;
                    permissao.DESCRICAO = dto.DESCRICAO;
                    permissao.ABRANGENCIA = dto.ABRANGENCIA;
                    permissao.ACOMODACAO = dto.ACOMODACAO;
                    permissao.MODALIDADE = dto.MODALIDADE;
                    permissao.id = dto.id;
                    permissao.TIPOCONTRATO = dto.tipocontrato;
                    permissao.STATUS = dto.STATUS;
                    permissao.TIPOCONTRATACAO = dto.TIPOCONTRATACAO;
                    permissao.FORMACAOPRECO = dto.FORMACAOPRECO;
                    permissao.NUMEROCONTRATO = dto.NUMEROCONTRATO;
                    permissao.DATAATUALIZACAO = DateTime.Now;
                    permissao.USUARIOATUALIZACAO = session.Usuario.Login;

                    int arquivosSalvos = 0;

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase arquivo = Request.Files[i];

                        if (arquivo.ContentLength > 0)
                        {
                            var uploadPath = Server.MapPath("~/Publico/Contratos/");
                            string caminhoArquivo = Path.Combine(@uploadPath, Path.GetFileName(dto.PLANO + '.' + arquivo.FileName));

                            if (i == 0)
                            {
                                permissao.NOMEARQUIVO = dto.PLANO + '.' + arquivo.FileName;
                            }
                            try
                            {
                                arquivo.SaveAs(caminhoArquivo);
                                arquivosSalvos++;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    if (permissao.NOMEARQUIVO != null)
                    {
                        consulta.UpdateEmpresa(permissao);
                        var ALL = consulta.GetAllPlanos();
                        ViewData["Message"] = "Cadastro atualizado com sucesso";
                        return View("~/Areas/Cadastro/Views/Planos/index.cshtml", ALL);
                    }
                    else
                    {
                        consulta.UpdateEmpresa2(permissao);
                        var ALL = consulta.GetAllPlanos();
                        ViewData["Message"] = "Cadastro atualizado com sucesso";
                        return View("~/Areas/Cadastro/Views/Planos/index.cshtml", ALL);
                    }

                }

            }
            else
            {
                FormularioViewBags();
                return View("~/Areas/Cadastro/Views/Empresas/Partial/_NovaEmpresa.cshtml", dto);
            }
        }
    }
}