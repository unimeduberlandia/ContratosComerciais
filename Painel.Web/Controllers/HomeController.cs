using Painel.Core;
using Painel.Repositories;
using System;
using System.Text;
using System.Web.Mvc;

namespace Painel.Web.Controllers
{
    public class HomeController : Controller
    {


        [Autorizado]
        public ActionResult Index()
        {


            return View();
        }

        [Autorizado]
        public ActionResult Erro403() => View();



    }
}