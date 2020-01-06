using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class VentasRequest
    {
        public string IdCuentaSf { get; set; }
        public string Usuario { get; set; }
        public string Region { get; set; }
    }

    public class VentasResponse
    {
        public string CodigoRetorno { get; set; }
        public string MensajeRetorno { get; set; }
        public IEnumerable<VentasRow> VentaRowList { get; set; }
    }

    public class VentasRow
    {
        public string Indicador { get; set; }
        public string Region { get; set; }
        public float? Monto { get; set; }
        public float? Comparativo { get; set; }
    }
}