using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class Oportunidad : ICrmApiResponse, IActualizado
    {
        public string IdOportunidad { get; set; }
        public int? IdFile { get; set; }
        public string Accion { get; set; }
        public string Etapa { get; set; }
        public string DkCuenta { get; set; }
        public string UnidadNegocio { get; set; }
        public string Sucursal { get; set; }
        public string NombreSubcodigo { get; set; }
        public string PuntoVenta { get; set; }
        public string Subcodigo { get; set; }
        public DateTime FechaOportunidad { get; set; }
        public string NombreOportunidad { get; set; }
        public string OrigenOportunidad { get; set; }
        public string MedioOportunidad { get; set; }
        public string GDS { get; set; }
        public string TipoProducto { get; set; }
        public string RutaViaje { get; set; }
        public string CiudadOrigen { get; set; }
        public string CiudadDestino { get; set; }
        public string TipoRuta { get; set; }
        public int NumPasajeros { get; set; }
        public DateTime FechaInicioViaje1 { get; set; }
        public DateTime FechaFinViaje1 { get; set; }
        public DateTime FechaInicioViaje2 { get; set; }
        public DateTime FechaFinViaje2 { get; set; }
        public float MontoEstimado { get; set; }
        public float MontoReal { get; set; }
        public string Pnr1 { get; set; }
        public string Pnr2 { get; set; }
        public string MotivoPerdida { get; set; }
        public string Contacto { get; set; }
        public string CounterVentas { get; set; }
        public string CounterAdmin { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public int Actualizados { get; set; } = -1;
    }
}