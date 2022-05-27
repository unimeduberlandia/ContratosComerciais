using Painel.Core;
using Painel.Models.Cadastro;
using Painel.Repositories.Cadastro;
using Painel.Web.Areas.Cadastro.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Painel.Web.Areas.Cadastro.Controllers
{
    public class TermosContratuaisController : Controller
    {
        TermosContratualRepository consulta = new TermosContratualRepository();

        private void FormularioViewBags()
        {
            ViewBag.grupos = new[] {
                new { value = "Pessoal", text = "Cobrar Por Pessoa"  },
                new { value = "Familiar", text = "Cobrar única vez"  },
            };

        }

        [Autorizado("admin")]
        public ActionResult Index()
        {
            var dto = consulta.GetAllTermos();

            return View(dto);
        }

        [Autorizado("admin")]
        public ActionResult NovoTermo()
        {
            FormularioViewBags();
            var dto = new TermosContratuaisViewModel();
            return View("~/Areas/Cadastro/Views/TermosContratuais/Partial/_NovoTermo.cshtml", dto);
        }

        [Autorizado("admin")]
        public ActionResult EditarTermo(long? ID)
        {
            var permissao = consulta.GetByIdTermo(ID);

            var dto = new TermosContratuaisViewModel();
            dto.id = ID;
            dto.NOME = permissao.NOME;
            dto.STATUS = permissao.STATUS;
            ViewBag.ativo = permissao.STATUS;
            FormularioViewBags();
            return View("~/Areas/Cadastro/Views/TermosContratuais/Partial/_NovoTermo.cshtml", dto);
        }

        [Autorizado("admin")]
        public ActionResult VisualizarPDF(int id)
        {
            var result = consulta.GetByIdTermo(id);
            if (result.NOMEARQUIVO == null)
            {
                ViewBag.nomeArquivo = "";
            }
            else
            {
                ViewBag.nomeArquivo = result.NOMEARQUIVO;
            }
            return View("~/Areas/Cadastro/Views/TermosContratuais/Partial/_VisualizarPDF.cshtml");

        }

        [Autorizado("admin")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult InserirTermo(TermosContratuaisViewModel dto)
        {
            if (dto.NOME != null)
            {
                var permissao = new TermosContratual();
                SessionManager session = SessionManager.GetInstance();

                if (dto.id == null)
                {
                    var consultanome = consulta.GetTermoByNome(dto.NOME);

                    if (consultanome.Count > 0)
                    {
                        ModelState.AddModelError("NOME", "Existe um termo com o mesmo nome!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/TermosContratuais/Partial/_NovoTermo.cshtml", dto);
                    }
                    int arquivosSalvos = 0;

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase arquivo = Request.Files[i];

                        if (arquivo.ContentLength > 0)
                        {
                            var uploadPath = Server.MapPath("~/Publico/TermosContratuais/");
                            string caminhoArquivo = Path.Combine(@uploadPath, Path.GetFileName(dto.NOME + '.' + arquivo.FileName));

                            if (i == 0)
                            {
                                permissao.NOMEARQUIVO = dto.NOME + '.' + arquivo.FileName;
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
                        permissao.NOME = dto.NOME;
                        permissao.STATUS = dto.STATUS;
                        permissao.DATACADASTRO = DateTime.Now;
                        permissao.USUARIOCADASTRO = session.Usuario.Login;

                        consulta.InserirTermo(ref permissao);

                        var ALL = consulta.GetAllTermos();
                        ViewData["Message"] = "Cadastro efetudado com sucesso";
                        return View("~/Areas/Cadastro/Views/TermosContratuais/index.cshtml", ALL);
                    }
                    else
                    {
                        ModelState.AddModelError("NOMEARQUIVO", "Arquivo do adicional é obrigatório");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/TermosContratuais/Partial/_NovoTermo.cshtml", dto);
                    }


                }
                else
                {
                    var consultanome = consulta.GetTermoByNomeId(dto.id, dto.NOME);

                    if (consultanome.Count > 0)
                    {
                        ModelState.AddModelError("NOME", "Existe um cadastro com o mesmo nome!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/TermosContratuais/Partial/_NovoTermo.cshtml", dto);
                    }

                    permissao.ID = dto.id;
                    permissao.NOME = dto.NOME;
                    permissao.STATUS = dto.STATUS;
                    permissao.DATAATUALIZACAO = DateTime.Now;
                    permissao.USUARIOATUALIZACAO = session.Usuario.Login;

                    int arquivosSalvos = 0;

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase arquivo = Request.Files[i];

                        if (arquivo.ContentLength > 0)
                        {
                            var uploadPath = Server.MapPath("~/Publico/TermosContratuais/");
                            string caminhoArquivo = Path.Combine(@uploadPath, Path.GetFileName(dto.NOME + '.' + arquivo.FileName));

                            if (i == 0)
                            {
                                permissao.NOMEARQUIVO = dto.NOME + '.' + arquivo.FileName;
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
                        consulta.UpdateTermo(permissao);

                        var ALL = consulta.GetAllTermos();
                        ViewData["Message"] = "Cadastro efetudado com sucesso";
                        return View("~/Areas/Cadastro/Views/TermosContratuais/index.cshtml", ALL);
                    }
                    else
                    {
                        consulta.UpdateTermo2(permissao);
                        var ALL = consulta.GetAllTermos();
                        ViewData["Message"] = "Cadastro efetudado com sucesso";
                        return View("~/Areas/Cadastro/Views/TermosContratuais/index.cshtml", ALL);
                    }

                }

            }
            else
            {
                FormularioViewBags();
                return View("~/Areas/Cadastro/Views/TermosContratuais/Partial/_NovoTermo.cshtml", dto);
            }

        }
    }
}