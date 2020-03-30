using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class OportunidadNM : ICrmApiResponse, IActualizado
    {
        public string idCuenta_SF { get; set; }
        public string fechaRegistro { get; set; }
        public string IdCanalVenta { get; set; }
        public string metabuscador { get; set; }
        public bool CajaVuelos { get; set; }
        public bool? CajaHotel { get; set; }
        public bool? CajaPaquetes { get; set; }
        public bool? CajaServicios { get; set; }
        public string modoIngreso { get; set; }
        public string ordenAtencion { get; set; }
        public string evento { get; set; }
        public string Estado { get; set; }
        public float IdCotSRV { get; set; }
        public float IdUsuarioSrv { get; set; }
        public string codReserva { get; set; }
        public string fechaCreación { get; set; }
        public string estadoVenta { get; set; }
        public string codigoAerolinea { get; set; }
        public string Tipo { get; set; }
        public float RUCEmpresa { get; set; }
        public string PCCOfficeID { get; set; }
        public string counterAsignado { get; set; }
        public string IATA { get; set; }
        public string descripPaquete { get; set; }
        public string destinoPaquetes { get; set; }
        public string fechasPaquetes { get; set; }
        public string EmpresaCliente { get; set; }
        public string nombreCliente { get; set; }
        public string apeliidosCliente { get; set; }
        public string IdLoginWeb { get; set; }
        public float? telefonoCliente { get; set; }
        public string accion_SF { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public string idOportunidad_SF { get; set; }
        public int Actualizados { get; set; } = -1;
    }
}