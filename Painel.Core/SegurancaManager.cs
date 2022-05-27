using Painel.Models;
using System;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Painel.Core
{
    public static class SegurancaManager
    {
        private static SessionManager session;

        static SegurancaManager()
        {
            session = SessionManager.GetInstance();
            usuario = session.Usuario;
        }

        public static bool Autorizado()
        {
            session = SessionManager.GetInstance();

            if ((session.Usuario != null) && (session.Usuario.Id != 0))
            {
                usuario = session.Usuario;
                return true;
            }
            else
                return false;
        }

        public static UsuarioLDAP LoginLdap(string usuario, string senha)
        {
            try
            {
                string dominio = "unimed014.com.br";
                string ldapAddress = "LDAP://10.14.0.250:389";
                string email = usuario + "@" + dominio;

                DirectoryEntry de = new DirectoryEntry(ldapAddress, email, senha);
                DirectorySearcher ds = new DirectorySearcher(de);

                ds.Filter = "(&(objectCategory=Person)(objectClass=user)(sAMAccountName=" + usuario + "))";
                ds.SearchScope = System.DirectoryServices.SearchScope.Subtree;

                SearchResult rs = ds.FindOne();

                if (rs == null)
                    return null;

                var usuarioLdap = new UsuarioLDAP();

                usuarioLdap.Login = usuario;

                if (rs.GetDirectoryEntry().Properties["givenname"].Value != null)
                    usuarioLdap.Nome = rs.GetDirectoryEntry().Properties["givenname"].Value.ToString();

                if (rs.GetDirectoryEntry().Properties["description"].Value != null)
                    usuarioLdap.Departamento = rs.GetDirectoryEntry().Properties["description"].Value.ToString();

                return usuarioLdap;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool Autorizado(params string[] regras)
        {
            session = SessionManager.GetInstance();

            if ((session.Usuario != null) && (session.Usuario.Id != null))
            {
                var regras_usuario = session.Usuario.Permissoes.Select(q => q.Regras).ToArray();
                usuario = session.Usuario;

                if ((regras.Intersect(regras_usuario).Count() == 0) && (!regras_usuario.Contains("admin")))
                    return false;

                return true;
            }
            else
                return false;
        }

        public static Usuario usuario { get; set; }
    }

    internal class Http403Result : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = 403;
        }
    }

    public class Autorizado : ActionFilterAttribute
    {
        SessionManager session;

        private string[] regras;

        public Autorizado()
        {
        }

        public Autorizado(params string[] regras)
        {
            this.regras = regras;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            session = SessionManager.GetInstance();

            if (session.Usuario.Id!=null)
            {
                var tmp = session.Usuario.GetType().FullName;

                filterContext.Controller.ViewBag.usuario = session.Usuario;

                var regras_usuario = session.Usuario.Permissoes.Select(q => q.Regras).ToArray();

                if ((regras != null) && (regras.Count() > 0) && (regras.Intersect(regras_usuario).Count() == 0) && (!regras_usuario.Contains("admin")))
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                        filterContext.Result = new Http403Result();
                    else
                        filterContext.Result = new RedirectResult("/erro403", false);
                }                    
            }
            else
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                    filterContext.Result = new Http403Result();
                else
                    filterContext.Result = new RedirectResult("/login", false);

                //if (!String.IsNullOrEmpty(filterContext.HttpContext.Request.Url.AbsolutePath) && (filterContext.HttpContext.Request.Url.AbsolutePath != "/painel") && (filterContext.HttpContext.Request.Url.AbsolutePath != "/"))
                //filterContext.Result = new RedirectResult("/erro401", false);
            }
        }
        

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();
        }
    }
}
