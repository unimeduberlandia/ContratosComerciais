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
    public class TabelaAdicionaisController : Controller
    {
        TabelaAdicionaisRepository consulta = new TabelaAdicionaisRepository();

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
            var dto = consulta.GetAllAdicionais();

            return View(dto);
        }

        [Autorizado("admin")]
        public ActionResult NovoAdicional()
        {
            FormularioViewBags();
            var dto = new TabelaAdicionaisViewModel();
            return View("~/Areas/Cadastro/Views/TabelaAdicionais/Partial/_NovoAdicional.cshtml", dto);
        }

        [Autorizado("admin")]
        public ActionResult EditarAdicional(long? ID)
        {
            var permissao = consulta.GetByIdAdicional(ID);

            var dto = new TabelaAdicionaisViewModel();
            dto.id = ID;
            dto.NOME = permissao.NOME;
            dto.GRUPO = permissao.GRUPO;
            dto.VALOR = permissao.VALOR;
            dto.mensagem = permissao.TEXTOCONTRATO;
            dto.STATUS = permissao.STATUS;
            ViewBag.ativo = permissao.STATUS;
            FormularioViewBags();
            return View("~/Areas/Cadastro/Views/TabelaAdicionais/Partial/_NovoAdicional.cshtml", dto);
        }

        [Autorizado("admin")]
        public ActionResult VisualizarPDF(int id)
        {
            var result = consulta.GetByIdAdicional(id);
            if (result.NOMEARQUIVO == null)
            {
                ViewBag.nomeArquivo = "";
            }
            else
            {
                ViewBag.nomeArquivo = result.NOMEARQUIVO;
            }
            return View("~/Areas/Cadastro/Views/TabelaAdicionais/Partial/_VisualizarPDF.cshtml");

        }

        [Autorizado("admin")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Inseriradicional(TabelaAdicionaisViewModel dto)
        {
            if (dto.NOME != null && dto.VALOR != null && dto.IDGRUPO != null)
            {
                var permissao = new TabelaAdicionais();
                SessionManager session = SessionManager.GetInstance();

                if (dto.id == null)
                {
                    dto.GRUPO = dto.IDGRUPO;
                    var consultanome = consulta.GetAdicionalByNome(dto.NOME);

                    if (consultanome.Count > 0)
                    {
                        ModelState.AddModelError("NOME", "Existe um cadastro com o mesmo nome!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/TabelaAdicionais/Partial/_NovoAdicional.cshtml", dto);
                    }
                    int arquivosSalvos = 0;

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase arquivo = Request.Files[i];

                        if (arquivo.ContentLength > 0)
                        {
                            var uploadPath = Server.MapPath("~/Publico/ContratosAdicionais/");
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
                        permissao.GRUPO = dto.IDGRUPO;
                        permissao.VALOR = dto.VALOR;
                        permissao.STATUS = dto.STATUS;
                        permissao.TEXTOCONTRATO = dto.mensagem;
                        permissao.DATACADASTRO = DateTime.Now;
                        permissao.USUARIOCADASTRO = session.Usuario.Login;
                    
                        consulta.InserirAdicional(ref permissao);
                                               
                        var ALL = consulta.GetAllAdicionais();
                        ViewData["Message"] = "Cadastro efetudado com sucesso";
                        return View("~/Areas/Cadastro/Views/TabelaAdicionais/index.cshtml", ALL);
                    }
                    else
                    {
                        ModelState.AddModelError("NOMEARQUIVO", "Arquivo do adicional é obrigatório");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/TabelaAdicionais/Partial/_NovoAdicional.cshtml", dto);
                    }

                    
                }
                else
                {
                    var consultanome = consulta.GetAdicionalByNomeId(dto.id, dto.NOME);

                    if (consultanome.Count > 0)
                    {
                        ModelState.AddModelError("NOME", "Existe um cadastro com o mesmo nome!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/TabelaAdicionais/Partial/_NovoAdicional.cshtml", dto);
                    }

                    permissao.ID = dto.id;
                    permissao.NOME = dto.NOME;
                    permissao.GRUPO = dto.IDGRUPO;
                    permissao.VALOR = dto.VALOR;
                    permissao.STATUS = dto.STATUS;
                    permissao.TEXTOCONTRATO = dto.mensagem;
                    permissao.DATAATUALIZACAO = DateTime.Now;
                    permissao.USUARIOATUALIZACAO = session.Usuario.Login;                 

                    int arquivosSalvos = 0;

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase arquivo = Request.Files[i];

                        if (arquivo.ContentLength > 0)
                        {
                            var uploadPath = Server.MapPath("~/Publico/ContratosAdicionais/");
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
                        consulta.UpdateAdicional(permissao);

                        var ALL = consulta.GetAllAdicionais();
                        ViewData["Message"] = "Cadastro efetudado com sucesso";
                        return View("~/Areas/Cadastro/Views/TabelaAdicionais/index.cshtml", ALL);
                    }
                    else
                    {
                        consulta.UpdateAdicional2(permissao);
                        var ALL = consulta.GetAllAdicionais();
                        ViewData["Message"] = "Cadastro atualizado com sucesso";
                        return View("~/Areas/Cadastro/Views/TabelaAdicionais/index.cshtml", ALL);
                    }
                                       
                }
                              
            }
            else
            {
                FormularioViewBags();
                return View("~/Areas/Cadastro/Views/TabelaAdicionais/Partial/_NovoAdicional.cshtml", dto);
            }

        }
    }
}