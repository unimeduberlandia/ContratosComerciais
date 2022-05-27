using Painel.Core;
using Painel.Models.Cadastro;
using Painel.Repositories.Cadastro;
using Painel.Web.Areas.Cadastro.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Painel.Web.Areas.Cadastro.Controllers
{
    public class EmpresasController : Controller
    {
        EmpresasRepository consulta = new EmpresasRepository();

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

        public async Task<JsonResult> ValidaCPFRep(string cpf)
        {
            if (!Validacoes.ValidaCPF(cpf))
            {
                return Json(new
                {
                    cpf = "F"
                });
            }
            else
            {
                return Json(new
                {
                    cpf = "V"
                });
            }
        }

        public async Task<JsonResult> BuscaEmpresa(string cnpj)
        {
            try
            {
                var dto = consulta.GetEmpresaByCNPJ(cnpj);
                return Json(new
                {
                    busca = "F"

                });
            }
            catch (System.Exception)
            {
                return Json(false);
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


        [Autorizado("admin")]
        public ActionResult Index()
        {
            var dto = consulta.GetAllEmpresssas();

            return View(dto);
        }

        [Autorizado("admin")]
        public ActionResult NovaEmpresa()
        {
            FormularioViewBags();
            var dto = new EmpresasViewModel();
            return View("~/Areas/Cadastro/Views/Empresas/Partial/_NovaEmpresa.cshtml", dto);
        }

        [Autorizado("admin")]
        public ActionResult EditarEmpresa(long? ID)
        {
            var permissao = consulta.GetByIdEmpresa(ID);

            var dto = new EmpresasViewModel();
            dto.id = ID;
            dto.CNPJ = permissao.CNPJ;
            dto.RAZAOSOCIAL = permissao.RAZAOSOCIAL;
            dto.NOMEFANTASIA = permissao.NOMEFANTASIA;
            dto.INSCRICAOESTADUAL = permissao.INSCRICAOESTADUAL;
            dto.ISS = permissao.ISS;
            dto.EMAIL = permissao.EMAIL;
            dto.ENDERECO = permissao.ENDERECO;
            dto.NUMERO = permissao.NUMERO;
            dto.COMPLEMENTO = permissao.COMPLEMENTO;
            dto.BAIRRO = permissao.BAIRRO;
            dto.CIDADE = permissao.CIDADE;
            dto.UF = permissao.UF;
            dto.idUF = permissao.UF;
            dto.CEP = permissao.CEP;
            dto.STATUS = permissao.STATUS;
            dto.CONTATO = permissao.CONTATO;
            dto.TELEFONE = permissao.TELEFONE;
            dto.TELEFONE2 = permissao.TELEFONE2;
            dto.REPRESENTANTELEGAL = permissao.REPRESENTANTELEGAL;
            dto.CPFREPRESENTANTE = permissao.CPFREPRESENTANTE;
            dto.RGREPRESENTANTE = permissao.RGREPRESENTANTE;

            FormularioViewBags();
            return View("~/Areas/Cadastro/Views/Empresas/Partial/_NovaEmpresa.cshtml", dto);
        }

        [Autorizado("admin")]
        [HttpPost]
        public ActionResult InserirEmpresa(EmpresasViewModel dto)
        {
            if (dto.CNPJ != null && dto.RAZAOSOCIAL != null && dto.NOMEFANTASIA != null && dto.EMAIL != null && dto.ENDERECO != null && dto.NUMERO != null && dto.BAIRRO != null && dto.CEP != null && dto.idUF != null && dto.CIDADE != null && dto.REPRESENTANTELEGAL != null && dto.CPFREPRESENTANTE != null && dto.RGREPRESENTANTE != null)
            {
                var permissao = new Empresas();
                SessionManager session = SessionManager.GetInstance();

                if (dto.id == null)
                {
                    var consultaempresa = consulta.GetByCNPJ(dto.CNPJ);
                    if (consultaempresa.Count > 0)
                    {
                        ModelState.AddModelError("CNPJ", "CNPJ já cadastrado!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/Empresas/Partial/_NovaEmpresa.cshtml", dto);
                    }

                    permissao.CNPJ = dto.CNPJ;
                    permissao.RAZAOSOCIAL = dto.RAZAOSOCIAL;
                    permissao.NOMEFANTASIA = dto.NOMEFANTASIA;
                    permissao.INSCRICAOESTADUAL = dto.INSCRICAOESTADUAL;
                    permissao.ISS = dto.ISS;
                    permissao.CPFREPRESENTANTE = dto.CPFREPRESENTANTE;
                    permissao.REPRESENTANTELEGAL = dto.REPRESENTANTELEGAL;
                    permissao.RGREPRESENTANTE = dto.RGREPRESENTANTE;
                    permissao.EMAIL = dto.EMAIL;
                    permissao.ENDERECO = dto.ENDERECO;
                    permissao.NUMERO = dto.NUMERO;
                    permissao.COMPLEMENTO = dto.COMPLEMENTO;
                    permissao.BAIRRO = dto.BAIRRO;
                    permissao.CIDADE = dto.CIDADE;
                    permissao.UF = dto.idUF;
                    permissao.CEP = dto.CEP;
                    permissao.STATUS = dto.STATUS;
                    permissao.CONTATO = dto.CONTATO;
                    permissao.TELEFONE = dto.TELEFONE;
                    permissao.TELEFONE2 = dto.TELEFONE2;
                    permissao.DATACADASTRO = DateTime.Now;
                    permissao.USUARIOCADASTRO = session.Usuario.Login;
                    consulta.InserirEmpresa(ref permissao);
                }
                else
                {
                    var consultaempresa = consulta.GetByCnpjID(dto.CNPJ, dto.id);
                    if (consultaempresa.Count > 0)
                    {
                        ModelState.AddModelError("CNPJ", "CNPJ já cadastrado!");
                        FormularioViewBags();
                        return View("~/Areas/Cadastro/Views/Empresas/Partial/_NovaEmpresa.cshtml", dto);
                    }

                    permissao.ID = dto.id;
                    permissao.CNPJ = dto.CNPJ;
                    permissao.RAZAOSOCIAL = dto.RAZAOSOCIAL;
                    permissao.NOMEFANTASIA = dto.NOMEFANTASIA;
                    permissao.INSCRICAOESTADUAL = dto.INSCRICAOESTADUAL;
                    permissao.ISS = dto.ISS;
                    permissao.EMAIL = dto.EMAIL;
                    permissao.ENDERECO = dto.ENDERECO;
                    permissao.NUMERO = dto.NUMERO;
                    permissao.COMPLEMENTO = dto.COMPLEMENTO;
                    permissao.BAIRRO = dto.BAIRRO;
                    permissao.CIDADE = dto.CIDADE;
                    permissao.UF = dto.idUF;
                    permissao.CEP = dto.CEP;
                    permissao.STATUS = dto.STATUS;
                    permissao.CONTATO = dto.CONTATO;
                    permissao.CPFREPRESENTANTE = dto.CPFREPRESENTANTE;
                    permissao.REPRESENTANTELEGAL = dto.REPRESENTANTELEGAL;
                    permissao.RGREPRESENTANTE = dto.RGREPRESENTANTE;
                    permissao.TELEFONE = dto.TELEFONE;
                    permissao.TELEFONE2 = dto.TELEFONE2;
                    permissao.DATAATUALIZACAO = DateTime.Now;
                    permissao.USUARIOATUALIZACAO = session.Usuario.Login;
                    consulta.UpdateEmpresa(permissao);
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
                return View("~/Areas/Cadastro/Views/Empresas/Partial/_NovaEmpresa.cshtml", dto);
            }
        }
    }
}