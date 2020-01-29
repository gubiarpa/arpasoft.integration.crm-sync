using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class CotizacionRequest
    {
        public string IdOportunidadSf { get; set; }
        public string IdCotizacionSf { get; set; }
        public string IdCuentaSf { get; set; }
        public string Cotizacion { get; set; }
        public string Region { get; set; }
        public string Accion { get; set; }
        public string Usuario { get; set; }
    }

    public class CotizacionResponse
    {
        public string Grupo { get; set; }
        public string Estado { get; set; }
        public float VentaEstimada { get; set; }
        public bool Elegida { get; set; }
        public string File { get; set; }
        public float VentaFile { get; set; }
        public float MargenFile { get; set; }
        public int PaxsFile { get; set; }
        public string EstadoFile { get; set; }
    }

    public class CotizacionJYResponse
    {
        public string IdOportunidadSf { get; set; }
        public string IdCotizacionSf { get; set; }
        public string IdCuentaSf { get; set; }
        public string Cotizacion { get; set; }
        public string Grupo { get; set; }
        public float VentaEstimada { get; set; }
        public bool Elegida { get; set; }
        public string FileSubfile { get; set; }
        public float VentaFile { get; set; }
        public float MargenFile { get; set; }
        public int PaxsFile { get; set; }
        public string EstadoFile { get; set; }
    }

    public class Cotizacion : IUnidadNegocio
    {
        #region ForeignKey
        public UnidadNegocio UnidadNegocio { get; set; }

        public string Grupo { get; set; }

        public string Cliente { get; set; }

        public string Cliente_Cliente { get; set; }

        public string Ejecutivo { get; set; }

        public string Unidad_Negocio { get; set; }

        public string Branch { get; set; }

        public DateTime Fecha_Apertura { get; set; }

        public DateTime Fecha_Inicio { get; set; }

        public DateTime Fecha_Fin { get; set; }

        public string Estado { get; set; }

        public bool Aceptada { get; set; }

        public SimpleDesc CondicionPago { get; set; }
        #endregion

        #region ResponseSalesforce
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        #endregion
    }

    public class CotizacionDM
    {
        public string IdOportunidadSf { get; set; }
        public string IdCotizacionSf { get; set; }
        public int? IdCotizacion { get; set; }
        public float? MontoCotizacion { get; set; }
        public float? MontoComision { get; set; }
        public string EstadoCotizacion { get; set; }
        public string NombreCotizacion { get; set; }
        public int? NumPasajerosAdult { get; set; }
        public int? NumPasajerosChild { get; set; }
        public int? NumPasajerosTotal { get; set; }
    }
}