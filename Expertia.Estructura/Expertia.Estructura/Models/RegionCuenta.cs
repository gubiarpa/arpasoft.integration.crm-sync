using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class RegionCuenta
    {
        public string IdCuentaSf { get; set; }
        public string Region { get; set; }
        public string Estado { get; set; }
        public float? MontoFacturacion { get; set; }
        public string NivelImportancia { get; set; }
        public string Supervisor { get; set; }
        public string OperadorActual1 { get; set; }
        public string NivelSatOperador1 { get; set; }
        public string OperadorActual2 { get; set; }
        public string NivelSatOperador2 { get; set; }
        public string OperadorActual3 { get; set; }
        public string NivelSatOperador3 { get; set; }
        public string Usuario { get; set; }
    }
}