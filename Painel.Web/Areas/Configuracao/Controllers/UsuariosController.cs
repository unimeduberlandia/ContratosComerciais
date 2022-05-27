using Painel.Core;
using Painel.Models;
using Painel.Repositories;
using Painel.Web.Areas.Configuracao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace Painel.Web.Areas.Configuracao.Controllers
{
    public class UsuariosController : Controller
    {
        UsuarioRepository usuarioRepo = new UsuarioRepository();

        PermissaoRepository permissaoRepo = new PermissaoRepository();

        private void FormularioViewBags()
        {
            var Empresa = usuarioRepo.GetAllEmpresasAtivas();
            var EmpresaSelecionavel = new List<SelectListItem>();
            foreach (var item in Empresa)
            {
                EmpresaSelecionavel.Add(new SelectListItem
                {
                    Value = item.id.ToString(),
                    Text = item.NOMEFANTASIA
                });
            }
            ViewBag.empresas = EmpresaSelecionavel;

        }


        [Autorizado("admin")]
        public ActionResult Index()
        {
            var usuarios = usuarioRepo.GetAll();

            return View(usuarios);
        }

        [Autorizado("admin")]
        public ActionResult GerarNovaSenha(int id)
        {
            var session = SessionManager.GetInstance();
            var usuario = new NovoUsuario();
            var cadastro = usuarioRepo.getemail(id);
            string chars = "abcdefghjkmnpqrstuvwxyz0123456789";
            string pass = "";
            Random random = new Random();
            for (int f = 0; f < 6; f++)
            {
                pass = pass + chars.Substring(random.Next(0, chars.Length - 1), 1);
            }

            var senha = CriarMD5(pass);

            usuario.senha = senha;
            usuario.idusuario = id;
            usuario.usuariocadastro = session.Usuario.Login;
            usuario.DataAtualizado = DateTime.Now;

            var emailcadastro = cadastro.email;

            var resetsenha = usuarioRepo.Resetsenha(usuario);

            StringBuilder emailText = new StringBuilder();
            emailText.Append("Sua nova senha foi gerada com sucesso!");
            emailText.Append("<br/><br/><br/>");
            emailText.Append("<b>Senha</b>: " + pass);
            emailText.Append("<br/><br/>");
            emailText.Append("Faça o login novamente no sistema de Contratos e troque a sua senha");
            emailText.Append("<br/><br/><br/><br/>");

            Email email = new Email();
            email.destinatarios.Add(emailcadastro);
            email.assunto = "Nova senha gerada";
            email.templateText = emailText;
            email.Enviar();

            return Json(resetsenha);
        }

        [Autorizado("admin")]
        public ActionResult Editar(int id)
        {
            var usuario = usuarioRepo.GetById(id);
            usuario.Permissoes = permissaoRepo.GetByUsuarioid(usuario.Id.Value);

            var dto = new UsuarioViewModel();
            dto.Login = usuario.Login;

            dto.Id = usuario.Id.Value;
            dto.PermissoesIds = usuario.Permissoes.Select(x => x.Id.ToString()).ToArray();
            dto.PermissoesSelecionadas = usuario.Permissoes;
            dto.TodasPermissoes = permissaoRepo.GetAll();

            return View("~/Areas/Configuracao/Views/Usuarios/Partial/_EditarPermissao.cshtml", dto);
        }

        [Autorizado("admin")]
        public ActionResult Novo()
        {
            var dto = new CadastroUsuarioViewModel();
            FormularioViewBags();
            return View("~/Areas/Configuracao/Views/Usuarios/Partial/_NovoUsuario.cshtml", dto);
        }


        [Autorizado("admin")]
        public ActionResult EditarUsuario(int id)
        {
            var usuario = usuarioRepo.GetById(id);

            var dto = new CadastroUsuarioViewModel();
            dto.Login = usuario.Login;
            dto.Id = usuario.Id;
            dto.Email = usuario.Email;
            dto.CPF = usuario.CPF;
            dto.Nome = usuario.Nome;
            dto.Telefone1 = usuario.Telefone1;
            dto.Telefone2 = usuario.Telefone2;
            dto.IdEmpresa = usuario.IdEmpresa;
            dto.userstatus = usuario.userstatus;
            ViewBag.status = usuario.userstatus;
            FormularioViewBags();
            return View("~/Areas/Configuracao/Views/Usuarios/Partial/_NovoUsuario.cshtml", dto);
        }

        [Autorizado("admin")]
        [HttpPost]
        public ActionResult Editar(UsuarioViewModel dto)
        {
            var permissoes = permissaoRepo.GetAll();
            dto.TodasPermissoes = permissoes;

            if (ModelState.IsValid)
            {
                if (dto.PermissoesIds is null)
                {
                    ModelState.AddModelError("Login", "Pelo menos uma permissão deve estar marcada!");
                    return View("~/Areas/Configuracao/Views/Usuarios/Partial/_EditarPermissao.cshtml", dto);
                }

                var usuario = usuarioRepo.GetByusuario(dto.Login.Trim());

                usuario = usuarioRepo.GetById(usuario.Id.Value);
                usuario.Login = dto.Login;
                usuario.DataAtualizado = DateTime.Now;

                usuarioRepo.Update(usuario);
                permissaoRepo.DeleteByUsuarioid(usuario.Id.Value);

                if (!usuario.Id.Equals(0))
                {

                    if (dto.PermissoesIds.Contains("1"))
                    {
                        dto.PermissoesIds = null;
                        dto.PermissoesIds = new[] { "1" };
                    }

                    if (dto.PermissoesIds.Count() > 0)
                    {
                        foreach (var item in dto.PermissoesIds)
                            permissaoRepo.InsertToUser(usuario.Id.Value, Convert.ToInt32(item));
                    }
                    return Json(new { success = true });
                }
                else
                    return Json(new { success = false });
            }
            else
            {
                return View("~/Areas/Configuracao/Views/Usuarios/Partial/_EditarPermissao.cshtml", dto);
            }
        }

        public string CriarMD5(string senha)
        {

            MD5 md5Hasher = MD5.Create();

            byte[] valorCriptografado = md5Hasher.ComputeHash(Encoding.Default.GetBytes(senha));

            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < valorCriptografado.Length; i++)

            {

                strBuilder.Append(valorCriptografado[i].ToString("x2"));

            }

            return strBuilder.ToString();

        }

        [Autorizado("admin")]
        [HttpPost]
        public ActionResult InserirUsuario(CadastroUsuarioViewModel dto)
        {
            if (dto.Nome != null && dto.CPF != null && dto.IdEmpresa != null && dto.Telefone2 != null && dto.Email != null)
            {

                if (dto.Id == null)
                {
                    var consulta = usuarioRepo.getbycpf(dto.CPF);

                    if (consulta.Count > 0)
                    {
                        ModelState.AddModelError("CPF", "CPF já cadastrado!");
                        FormularioViewBags();
                        return View("~/Areas/Configuracao/Views/Usuarios/Partial/_NovoUsuario.cshtml");
                    }

                    if (!Validacoes.ValidaCPF(dto.CPF))
                    {
                        ModelState.AddModelError("CPF", "CPF Inválido!");
                        FormularioViewBags();
                        return View("~/Areas/Configuracao/Views/Usuarios/Partial/_NovoUsuario.cshtml");
                    }

                    var session = SessionManager.GetInstance();
                    var usuario = new NovoUsuario();

                    string chars = "abcdefghjkmnpqrstuvwxyz0123456789";
                    string pass = "";
                    Random random = new Random();
                    for (int f = 0; f < 6; f++)
                    {
                        pass = pass + chars.Substring(random.Next(0, chars.Length - 1), 1);
                    }

                    var senha = CriarMD5(pass);

                    usuario.Login = dto.CPF;
                    usuario.Nome = dto.Nome;
                    usuario.senha = senha;
                    usuario.IdEmpresa = dto.IdEmpresa;
                    usuario.CPF = dto.CPF;
                    usuario.userstatus = dto.userstatus;
                    usuario.Telefone1 = dto.Telefone1;
                    usuario.Telefone2 = dto.Telefone2;
                    usuario.Email = dto.Email;
                    usuario.datacadastro = DateTime.Now;
                    usuario.usuariocadastro = session.Usuario.Login;

                    usuarioRepo.InsertUsuario(ref usuario);

                    StringBuilder emailText = new StringBuilder();
                    emailText.Append("Seu cadastro foi realizado com sucesso!");
                    emailText.Append("<br/><br/><br/>");
                    emailText.Append("Acesse o link abaixo para fazer o login");
                    emailText.Append("<br/><br/>");
                    emailText.Append("contratoscomerciais.unimeduberlandia.coop.br");
                    emailText.Append("<br/><br/>");
                    emailText.Append("<b>Login</b>: " + dto.CPF);
                    emailText.Append("<br/><br/>");
                    emailText.Append("<b>Senha</b>: " + pass);
                    emailText.Append("<br/><br/>");
                    emailText.Append("<br/><br/>");
                    emailText.Append("<br/><br/><br/><br/>");

                    Email email = new Email();
                    email.destinatarios.Add(dto.Email);
                    email.assunto = "Cadastro Sistema de Contratos Unimed";
                    email.templateText = emailText;
                    email.Enviar();

                    if (!usuario.idusuario.Equals(0))
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                }
                else
                {
                    var session = SessionManager.GetInstance();
                    var usuarioupdate = new NovoUsuario();

                    usuarioupdate.id = dto.Id;
                    usuarioupdate.Login = dto.CPF;
                    usuarioupdate.Nome = dto.Nome;
                    usuarioupdate.IdEmpresa = dto.IdEmpresa;
                    usuarioupdate.CPF = dto.CPF;
                    usuarioupdate.userstatus = dto.userstatus;
                    usuarioupdate.Telefone1 = dto.Telefone1;
                    usuarioupdate.Telefone2 = dto.Telefone2;
                    usuarioupdate.Email = dto.Email;
                    usuarioupdate.usuariocadastro = session.Usuario.Login;
                    usuarioupdate.dataatualizado = DateTime.Now;
                    usuarioRepo.Updateusuario(usuarioupdate);

                    if (!usuarioupdate.idusuario.Equals(0))
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                }
            }
            else
            {
                FormularioViewBags();
                return View("~/Areas/Configuracao/Views/Usuarios/Partial/_NovoUsuario.cshtml");
            }
        }
    }
}