using Painel.Models;
using System;
using System.Collections.Generic;

namespace Painel.Models.Contratos
{
    public class Dependente : BaseEntity
    {
        public long? ID { get; set; }

        public long? IDPLANO { get; set; }

        public long? IDCONTRATO { get; set; }

        public string NOMEDep { get; set; }
        
        public string IDADEDep { get; set; }
        
        public string CPFDep { get; set; }
        
        public string SEXODep { get; set; }
        
        public string guidDependente { get; set; }
        
        public string VALORDep { get; set; }
        
        public string PARENTESCODep { get; set; }
        
        public string NACIONALIDADEDep { get; set; }
        
        public string NOMEDAMAEDep { get; set; }
        
        public string ESTADOCIVILDep { get; set; }
        
        public string CARTAODESAUDEDep { get; set; }
        
        public DateTime DATANASCIMENTODep { get; set; }
        
        public DateTime DATACADASTRO { get; set; }
        
        public DateTime DATAATUALIZACAO { get; set; }
        
        public string USUARIOCADASTRO { get; set; }
        
        public string USUARIOATUALIZACAO { get; set; }


    }
}
