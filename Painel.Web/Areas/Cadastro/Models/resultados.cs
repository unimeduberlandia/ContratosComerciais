using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;


namespace Painel.Web.Areas.Cadastro.Models
{
    public class resultados
    {
        public string abrangencia { get; set; }
        public string status { get; set; }
        public string segmentacaoassistencial { get; set; }
        public string acomodacao { get; set; }
        public string coparticipacao { get; set; }
        public string tipocontratacao { get; set; }
        public string formacaopreco { get; set; }
        public string nome { get; set; }
        public List<resultados> resultado { get; set; }

    }
}