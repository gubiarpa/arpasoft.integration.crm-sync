using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class AgenciaPnr : IUnidadNegocio
    {
        public UnidadNegocio UnidadNegocio { get; set; }
        public int DkAgencia { get; set; }
        public string PNR { get; set; }
        public int IdFile { get; set; }
        public string IdSucursal { get; set; }
        public string IdOportunidadCrm { get; set; }
    }

    public class File
    {
        public string IdOportunidad { get; set; }
        public string Accion { get; set; }
        public int IdFile { get; set; }
        public string EstadoFile { get; set; }
        public string UnidadNegocio { get; set; }
        public string Sucursal { get; set; }
        public string NombreGrupo { get; set; }
        public string Counter { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Cliente { get; set; }
        public string Subcodigo { get; set; }
        public string Contacto { get; set; }
        public string CondicionPago { get; set; }
        public int NumPasajeros { get; set; }
        public float Costo { get; set; }
        public float Venta { get; set; }
        public float ComisionAgencia { get; set; }
    }
}