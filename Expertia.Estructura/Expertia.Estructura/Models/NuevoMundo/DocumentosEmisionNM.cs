using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models.NuevoMundo
{
    public class DocumentosEmisionNM
    {        
        public string idOportunidad_SF { get; set; }
        public int idCotizacionSRV { get; set; }
        public int IdUsuarioSrv_SF { get; set; }
        public string correo { get; set; }
    }

    public class DocEmisionRS
    {
        public string codigo { get; set; }
        public string mensaje { get; set; }
    }
}