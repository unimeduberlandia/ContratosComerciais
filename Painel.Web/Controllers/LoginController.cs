using HumanAPIClient.Model;
using HumanAPIClient.Service;
using Painel.Core;
using Painel.Models;
using System.Security.Cryptography;
using Painel.Repositories;
using Painel.Web.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;
using System.Web.Mvc;

namespace Painel.Web.Controllers
{
    public class LoginController : Controller
    {
        SessionManager session;

        UsuarioRepository usuarioRepo = new UsuarioRepository();
        PermissaoRepository permissaoRepo = new PermissaoRepository();



        public ActionResult Index()
        {
            //SimpleSending sms = new SimpleSending("uberlandia.api", "YoIVpBSCHA");
            //SimpleMessage message = new SimpleMessage();
            //message.To = "5562998212841";
            //message.Message = "#6666#gprs#1#";            
            //message.Id = DateTime.Now.ToUniversalTime().ToString();
            //List<String> response = sms.send(message);


            return View();
        }

        public ActionResult Sair()
        {
            session = SessionManager.GetInstance();
            session.Usuario = null;

            return RedirectToAction("", "Login");
        }

        [HttpGet]
        public ActionResult PartialLogin()
        {
            return PartialView("~/Views/Login/Partial/_Login.cshtml");
        }

        [HttpGet]
        public ActionResult EsqueciSenha()
        {
            return PartialView("~/Views/Login/Partial/_EsqueciSenha.cshtml");
        }

        [Autorizado]
        public ActionResult TrocarSenha()
        {
            var dto = new TrocarSenhaViewModel();
            session = SessionManager.GetInstance();
            dto.idusuario = session.Usuario.Id;
            dto.Login = session.Usuario.Login;


            return PartialView("~/Views/Login/Partial/_TrocarSenha.cshtml", dto);
        }

        public string GerarMD5(string senha)

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

        [HttpPost]
        [Autorizado]
        [ValidateAntiForgeryToken]
        public ActionResult InserirNovaSenha(TrocarSenhaViewModel dto)
        {
            if (dto.senha != null && dto.novasenha != null && dto.novasenha2 != null)
            {
                if (dto.novasenha != dto.novasenha2)
                {
                    ModelState.AddModelError("novasenha", "Senhas diferentes!");
                    ModelState.AddModelError("novasenha2", "Senhas diferentes!");

                    return PartialView("~/Views/Login/Partial/_TrocarSenha.cshtml", dto);
                }
                var senha = GerarMD5(dto.senha);
                var usuario = usuarioRepo.GetByLogin(dto.Login, senha);

                if (usuario != null)
                {
                    var novasenha = GerarMD5(dto.novasenha);

                    usuario = new Usuario();
                    usuario.Login = dto.Login;
                    usuario.senha = novasenha;
                    usuario.idusuario = dto.idusuario;

                    usuarioRepo.UpdateSenha(usuario);

                    session = SessionManager.GetInstance();
                    session.Usuario = null;

                    return RedirectToAction("", "Login");
                }
                else
                {
                    ModelState.AddModelError("senha", "Senha inválida");
                    return PartialView("~/Views/Login/Partial/_TrocarSenha.cshtml", dto);
                }
            }

            return PartialView("~/Views/Login/Partial/_TrocarSenha.cshtml", dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostEsqueciSenha(LoginViewModel dto)
        {
            if (dto.Usuario != null)
            {
                var usuario = usuarioRepo.getemailCpf(dto.Usuario);

                if (usuario != null)
                {

                    //   var nome = usuario.Nome;
                    //  var cpf = usuario.CPF;
                    var emailcadastro = usuario.email;

                    string chars = "abcdefghjkmnpqrstuvwxyz0123456789";
                    string pass = "";
                    Random random = new Random();
                    for (int f = 0; f < 7; f++)
                    {
                        pass = pass + chars.Substring(random.Next(0, chars.Length - 1), 1);
                    }

                    var senha = GerarMD5(pass);

                    usuario.senha = senha;
                    usuario.idusuario = usuario.id;
                    usuario.DataAtualizado = DateTime.Now;

                    var resetsenha = usuarioRepo.Resetsenha(usuario);

                    if (resetsenha == true)
                    {

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

                        ViewData["Message"] = "Uma nova senha foi enviada para o e-mail cadastrado";

                        return PartialView("~/Views/Login/Partial/_EsqueciSenha.cshtml");
                    }

                    else
                    {
                        ModelState.AddModelError("usuario", "Não foi possível enviar senha");
                        return PartialView("~/Views/Login/Partial/_EsqueciSenha.cshtml");

                    }
                }
                else
                {
                    ModelState.AddModelError("usuario", "CPF inválido");
                    return PartialView("~/Views/Login/Partial/_EsqueciSenha.cshtml");
                }
            }

            return PartialView("~/Views/Login/Partial/_EsqueciSenha.cshtml", dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostLogin(LoginViewModel dto)
        {
            if (ModelState.IsValid)
            {
                var senha = GerarMD5(dto.Senha);
                var usuario = usuarioRepo.GetByLogin(dto.Usuario, senha.ToString());

                if (usuario != null)
                {
                    var nome = usuario.Nome;
                    var cpf = usuario.CPF;
                    var id = usuario.Id.Value;

                    usuario = new Usuario();
                    usuario.Login = dto.Usuario;
                    usuario.Nome = nome;
                    usuario.CPF = cpf;
                    usuario.Id = id;
                    usuario.Permissoes = permissaoRepo.GetByUsuarioid(id);
                    usuario.DataUltLogin = DateTime.Now;
                    usuarioRepo.Update(usuario);

                    session = SessionManager.GetInstance();
                    session.Usuario = usuario;


                    return Json(new
                    {
                        success = true,
                        url = Url.Action("", "home")
                    });
                }
                else
                {
                    ModelState.AddModelError("usuario", "Usuário ou senha inválidos");
                }
            }

            return PartialView("~/Views/Login/Partial/_Login.cshtml", dto);
        }
    }
}