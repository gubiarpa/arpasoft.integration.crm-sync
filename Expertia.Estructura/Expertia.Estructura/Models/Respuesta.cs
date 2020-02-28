using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class Respuesta
    {
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public string Numero_Afectados { get; set; }
    }
}