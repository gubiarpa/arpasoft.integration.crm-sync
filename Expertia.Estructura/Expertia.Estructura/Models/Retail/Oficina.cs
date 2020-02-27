using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models.Retail
{
    public class Oficina
    {
        public int intIdOfi { get; set; }
        public string strNomOfi { get; set; }
        public string strDirecOfi { get; set; }
        public int intEstadoOfi { get; set; }
        public string strCodPostalOfi { get; set; }
        public string strTelfOfi { get; set; }
        public string strFaxOfi { get; set; }
        public string strNumDocOfi { get; set; }
        public int intIdTipDocOfi { get; set; }
        public int intIdWeb { get; set; }
        public int intIdEmpresa { get; set; }
        public string strIdPais { get; set; }
        public string strIdCiudad { get; set; }
        public int intIdDistrito { get; set; }
        public int intIdOfiSybase { get; set; }
        public bool bolEsRipley { get; set; }
        public string bolEsCAAdministrativo { get; set; }
    }
}