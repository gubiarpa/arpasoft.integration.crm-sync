using Expertia.Estructura.Utils;
using System;


namespace Expertia.Estructura.Models.Auxiliar
{
    public class Cotizacion_CT
    {
        public UnidadNegocioKeys? ID { get; set; }

        public string Grupo { get; set; }

        public string Cliente { get; set; }

        public string Cliente_Cliente { get; set; }

        public string Ejecutivo { get; set; }

        public string Unidad_Negocio { get; set; }

        public string Branch { get; set; }

        public string Fecha_Apertura { get; set; }

        public DateTime Fecha_Inicio { get; set; }

        public DateTime Fecha_Fin { get; set; }

        public string Estado { get; set; }

        public bool Aceptada { get; set; }

    }
}