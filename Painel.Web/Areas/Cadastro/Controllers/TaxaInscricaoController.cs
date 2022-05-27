using Painel.Core;
using Painel.Models.Cadastro;
using Painel.Repositories.Cadastro;
using Painel.Web.Areas.Cadastro.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Painel.Web.Areas.Cadastro.Controllers
{
    public class TaxaInscricaoController : Controller
    {
        TaxaInscricaoRepository consulta = new TaxaInscricaoRepository();


        [Autorizado("admin")]
        public ActionResult Index()
        {
            var dto = consulta.GetAllTaxas();

            return View(dto);
        }

        [Autorizado("admin")]
        public ActionResult NovaTaxa()
        {
            var dto = new TaxaInscricaoViewModel();
            return View("~/Areas/Cadastro/Views/TaxaInscricao/Partial/_NovaTaxa.cshtml", dto);
        }

        [Autorizado("admin")]
        public ActionResult EditarTaxa(long? ID)
        {
            var permissao = consulta.GetByTaxaId(ID);

            var dto = new TaxaInscricaoViewModel();
            dto.id = ID;
            dto.NOME = permissao.NOME;
            dto.VALOR = permissao.VALOR;
            dto.STATUS = permissao.STATUS;
            ViewBag.ativo = permissao.STATUS;
            return View("~/Areas/Cadastro/Views/TaxaInscricao/Partial/_NovaTaxa.cshtml", dto);
        }

        [Autorizado("admin")]
        [HttpPost]
        public ActionResult InserirTaxa(TaxaInscricaoViewModel dto)
        {
            if (dto.NOME != null && dto.VALOR != null)
            {
                var permissao = new TaxaInscricao();
                SessionManager session = SessionManager.GetInstance();

                if (dto.id == null)
                {
                    var consultanome = consulta.GetAllTaxasAtivas();

                    if (dto.STATUS == "Sim" && consultanome.Count > 0)
                    {
                        ModelState.AddModelError("NOME", "Existe um cadastro ativo!");
                        return View("~/Areas/Cadastro/Views/TaxaInscricao/Partial/_NovaTaxa.cshtml", dto);
                    }

                    permissao.NOME = dto.NOME;
                    permissao.VALOR = dto.VALOR;
                    permissao.STATUS = dto.STATUS;
                    permissao.DATACADASTRO = DateTime.Now;
                    permissao.USUARIOCADASTRO = session.Usuario.Login;
                    consulta.InserirTaxaInscricao(ref permissao);
                }
                else
                {
                    var consultanome = consulta.GetByTaxaDiferenteId(dto.id);

                    if (dto.STATUS == "Sim" && consultanome.Count > 0)
                    {
                        ModelState.AddModelError("NOME", "Existe um cadastro ativo!");
                        return View("~/Areas/Cadastro/Views/TaxaInscricao/Partial/_NovaTaxa.cshtml", dto);
                    }

                    permissao.ID = dto.id;
                    permissao.NOME = dto.NOME;
                    permissao.VALOR = dto.VALOR;
                    permissao.STATUS = dto.STATUS;
                    permissao.DATAATUALIZACAO = DateTime.Now;
                    permissao.USUARIOATUALIZACAO = session.Usuario.Login;
                    consulta.UpdateTaxaInscricao(permissao);
                }

                if (!permissao.Id.Equals(0))
                {
                    return Json(new { success = true });
                }
                else
                    return Json(new { success = false });
            }
            else
            {
                return View("~/Areas/Cadastro/Views/TaxaInscricao/Partial/_NovaTaxa.cshtml", dto);
            }

        }
    }
}