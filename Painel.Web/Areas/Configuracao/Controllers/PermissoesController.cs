using Painel.Core;
using Painel.Models;
using Painel.Repositories;
using Painel.Web.Areas.Configuracao.Models;
using System;
using System.Web.Mvc;


namespace Painel.Web.Areas.Configuracao.Controllers
{
    public class PermissoesController : Controller
    {
        PermissaoRepository permissaoRepo = new PermissaoRepository();

        [Autorizado("admin")]
        public ActionResult Index()
        {
            var permissoes = permissaoRepo.GetAll();

            return View(permissoes);
        }

        [Autorizado("admin")]
        public ActionResult Novo()
        {
            var dto = new PermissaoViewModel();
            return View("~/Areas/Configuracao/Views/Permissoes/Partial/_Cadastro.cshtml", dto);
        }

        [Autorizado("admin")]
        public ActionResult Editar(int id)
        {
            var permissao = permissaoRepo.GetById(id);

            var dto = new PermissaoViewModel();
            dto.Descricao = permissao.Descricao;
            dto.Regras = permissao.Regras;
            dto.Id = permissao.Id;

            return View("~/Areas/Configuracao/Views/Permissoes/Partial/_Cadastro.cshtml", dto);
        }

        [Autorizado("admin")]
        public ActionResult Apagar(int id)
        {
            bool apagado = permissaoRepo.Apagar(id);
            return Json(apagado);
        }

        [Autorizado("admin")]
        [HttpPost]
        public ActionResult Cadastro(PermissaoViewModel dto)
        {
            if (ModelState.IsValid)
            {
                var permissao = new Permissao();

                if (dto.Id == null)
                {
                    permissao.Descricao = dto.Descricao;
                    permissao.Regras = dto.Regras;
                    permissao.DataCadastro = DateTime.Now;
                    permissao.Status = 1;

                    permissaoRepo.Insert(ref permissao);
                }
                else
                {
                    permissao = permissaoRepo.GetById(dto.Id.Value);
                    permissao.Descricao = dto.Descricao;
                    permissao.Regras = dto.Regras;
                    permissao.DataAtualizado = DateTime.Now;
                    permissaoRepo.Update(permissao);
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
                return View("~/Areas/Configuracao/Views/Permissoes/Partial/_Cadastro.cshtml", dto);
            }
        }
    }
}