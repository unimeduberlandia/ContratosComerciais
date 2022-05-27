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
    public class TabelaPrecosController : Controller
    {
        TabelaPrecosRepository consulta = new TabelaPrecosRepository();

        private void FormularioViewBags()
        {
            var planos = consulta.GetAllPlanosAtivos();
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

        }

        [Autorizado("admin")]
        public ActionResult Index(long? idplano)
        {
            var dto = consulta.GetByIdPlano(idplano);
            ViewBag.idplano = idplano;
            return View(dto);
        }

        [Autorizado("admin")]
        public ActionResult NovaTabela(long? id)      
        {           
            TabelaPrecosViewModel dto = new TabelaPrecosViewModel();
            dto.IDPLANO = id;
            dto.novo = "sim";
            return View("~/Areas/Cadastro/Views/TabelaPrecos/Partial/_NovoPreco.cshtml", dto);
        }

        [Autorizado("admin")]
        public ActionResult EditarTabela(long? ID)
        {
            var permissao = consulta.GetByIdPreco(ID);

            var dto = new TabelaPrecosViewModel();
            dto.id = ID;
            dto.IDPLANO = permissao.IDPLANO;
            dto.INICIO = permissao.INICIO;
            var datafim = "";
            if (permissao.FIM != 999)
            {
                datafim = permissao.FIM.ToString();
            }
            
            dto.FIM = datafim;
            dto.VALOR = permissao.VALOR;
            dto.STATUS = permissao.STATUS;
            ViewBag.ativo = permissao.STATUS;
            ViewBag.datafim = permissao.FIM;
            FormularioViewBags();
            return View("~/Areas/Cadastro/Views/TabelaPrecos/Partial/_NovoPreco.cshtml", dto);
        }

        [Autorizado("admin")]
        [HttpPost]
        public ActionResult InserirPreco(TabelaPrecosViewModel dto)
        {
            if (dto.VALOR != null)
            {               
                var permissao = new TabelaPrecos();
                SessionManager session = SessionManager.GetInstance();

                if (dto.novo == "sim")
                {
                    var datafim = 0;

                    if (dto.FIM == null)
                    {
                        datafim = 999;
                    }
                    else
                    {
                        datafim = Convert.ToInt32(dto.FIM);
                    }

                    if (dto.INICIO > datafim)
                    {
                        ModelState.AddModelError("INICIO", "Faixa inicial não pode ser maior que a final!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/TabelaPrecos/Partial/_NovoPreco.cshtml", dto);
                    }

                    var consultatabela = consulta.GetTabelaBynomevalor(dto.IDPLANO, dto.INICIO, datafim);
                    if (consultatabela.Count > 0)
                    {
                        ModelState.AddModelError("INICIO", "Existe um cadastro para essa faixa e plano!");
                        ModelState.AddModelError("FIM", "Existe um cadastro para essa faixa e plano!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/TabelaPrecos/Partial/_NovoPreco.cshtml", dto);
                    }

                    var consultatabela2 = consulta.GetTabelaByfaixa1(dto.IDPLANO, dto.INICIO, datafim);
                    if (consultatabela2.Count > 0)
                    {
                        ModelState.AddModelError("INICIO", "Existe um cadastro para essa faixa e plano!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/TabelaPrecos/Partial/_NovoPreco.cshtml", dto);
                    }

                    var consultatabela3 = consulta.GetTabelaByfaixa2(dto.IDPLANO, dto.INICIO, datafim);
                    if (consultatabela3.Count > 0)
                    {
                        ModelState.AddModelError("FIM", "Existe um cadastro para essa faixa e plano!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/TabelaPrecos/Partial/_NovoPreco.cshtml", dto);
                    }
                    var fim = "";

                    if (datafim == 999)
                    {
                        fim = "-ou mais";
                    }
                    else
                    {
                        fim = "-"+datafim.ToString();
                    }

                    permissao.NOME = dto.INICIO.ToString() + fim;
                    permissao.IDPLANO = dto.IDPLANO;
                    permissao.INICIO = dto.INICIO;
                    permissao.FIM = datafim;
                    permissao.VALOR = dto.VALOR;
                    permissao.STATUS = dto.STATUS;
                    permissao.DATACADASTRO = DateTime.Now;
                    permissao.USUARIOCADASTRO = session.Usuario.Login;
                    consulta.InserirPreco(ref permissao);
                }
                else
                {
                    var datafim = 0;

                    if (dto.FIM == null)
                    {
                        datafim = 999;
                    }
                    else
                    {
                        datafim = Convert.ToInt32(dto.FIM);
                    }

                    if (dto.INICIO > datafim)
                    {
                        ModelState.AddModelError("INICIO", "Faixa inicial não pode ser maior que a final!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/TabelaPrecos/Partial/_NovoPreco.cshtml", dto);
                    }

                    var consultatabela = consulta.GetTabelaBynomevalorid(dto.id, dto.IDPLANO, dto.INICIO, datafim);
                    if (consultatabela.Count > 0)
                    {
                        ModelState.AddModelError("INICIO", "Existe um cadastro para essa faixa e plano!");
                        ModelState.AddModelError("FIM", "Existe um cadastro para essa faixa e plano!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/TabelaPrecos/Partial/_NovoPreco.cshtml", dto);
                    }

                    var consultatabela2 = consulta.GetTabelaByfaixa3(dto.id, dto.IDPLANO, dto.INICIO, datafim);
                    if (consultatabela2.Count > 0)
                    {
                        ModelState.AddModelError("INICIO", "Existe um cadastro para essa faixa e plano!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/TabelaPrecos/Partial/_NovoPreco.cshtml", dto);
                    }

                    var consultatabela3 = consulta.GetTabelaByfaixa4(dto.id, dto.IDPLANO, dto.INICIO, datafim);
                    if (consultatabela3.Count > 0)
                    {
                        ModelState.AddModelError("FIM", "Existe um cadastro para essa faixa e plano!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/TabelaPrecos/Partial/_NovoPreco.cshtml", dto);
                    }

                    var fim = "";

                    if (datafim == 999)
                    {
                        fim = "-ou mais";
                    }
                    else
                    {
                        fim = "-" + datafim.ToString();
                    }

                    permissao.NOME = dto.INICIO.ToString() + fim;
                    permissao.ID = dto.id;
                    permissao.IDPLANO = dto.IDPLANO;
                    permissao.INICIO = dto.INICIO;
                    permissao.FIM = datafim;
                    permissao.VALOR = dto.VALOR;
                    permissao.STATUS = dto.STATUS;
                    permissao.DATAATUALIZACAO = DateTime.Now;
                    permissao.USUARIOATUALIZACAO = session.Usuario.Login;
                    consulta.UpdatePreco(permissao);
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
                FormularioViewBags();
                return View("~/Areas/Cadastro/Views/TabelaPrecos/Partial/_NovoPreco.cshtml");
            }

        }
    }
}