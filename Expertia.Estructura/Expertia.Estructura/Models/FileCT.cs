using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using System;


namespace Expertia.Estructura.Models
{
    public class FileCTRequest
    {
        public string IdOportunidadSf { get; set; }
        public string IdCotizacionSf { get; set; }
        public string Region { get; set; }
        public string File { get; set; }
        public int subfile { get; set; }
    }

    public class FileCT : IUnidadNegocio
    {
        #region ForeignKey
        public UnidadNegocio UnidadNegocio { get; set; }

        public string Grupo { get; set; }

        public string File { get; set; }

        public int Subfile { get; set; }

        public string Cliente { get; set; }

        public string Cliente_Cliente { get; set; }

        public string Ejecutivo { get; set; }

        public string Unidad_Negocio { get; set; }

        public string Branch { get; set; }

        public DateTime Fecha_Apertura { get; set; }

        public DateTime Fecha_Inicio { get; set; }

        public DateTime Fecha_Fin { get; set; }

        public string Estado { get; set; }
        public string Num_Pasajero { get; set; }

        public bool Aceptada { get; set; }

        public SimpleDesc CondicionPago { get; set; }
        #endregion

    }


}