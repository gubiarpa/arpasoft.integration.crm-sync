using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class OportunidadNM : ICrmApiResponse, IActualizado
    {
        public string idCuenta_SF { get; set; }
        public string fechaRegistro { get; set; }
        public string idCanalVenta { get; set; }
        public string metaBuscador { get; set; }
        public bool cajaVuelos { get; set; }
        public bool? cajaHotel { get; set; }
        public bool? cajaPaquetes { get; set; }
        public bool? cajaServicios { get; set; }
        public string modoIngreso { get; set; }
        public string ordenAtencion { get; set; }
        public string evento { get; set; }
        public string estado { get; set; }
        public float idCotSRV { get; set; }
        public float idUsuarioSrv { get; set; }
        public string codReserva { get; set; }
        public string fechaCreación { get; set; }
        public string estadoVenta { get; set; }
        public string codigoAerolinea { get; set; }
        public string tipo { get; set; }
        public float ruc { get; set; }
        public string pcc_OfficeID { get; set; }
        public string counterAsignado { get; set; }
        public string iata { get; set; }
        public string descripPaquete { get; set; }
        public string destinoPaquetes { get; set; }
        public string fechasPaquetes { get; set; }
        public string empresaCliente { get; set; }
        public string nombreCliente { get; set; }
        public string apeliidosCliente { get; set; }
        public string idLoginWeb { get; set; }
        public float? telefonoCliente { get; set; }
        public string accion_SF { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public string idOportunidad_SF { get; set; }
        public int Actualizados { get; set; } = -1;
    }
}