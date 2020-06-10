using Expertia.Estructura.Models.Behavior;
using System;
using System.Collections.Generic;

namespace Expertia.Estructura.Models
{
    public class OportunidadNM
    {
        public string idCuenta_SF { get; set; }
        public string Identificador_NM { get; set; }
        public string idOportunidad_SF { get; set; }
        public string fechaRegistro { get; set; }
        public string Servicio { get; set; }
        public string IdCanalVenta { get; set; }
        public string metabuscador { get; set; }
        public bool CajaVuelos { get; set; }
        public bool CajaHotel { get; set; }
        public bool CajaPaquetes { get; set; }
        public bool CajaServicios { get; set; }
        public bool CajaSeguro { get; set; }
        public string modoIngreso { get; set; }
        public string ordenAtencion { get; set; }
        public string evento { get; set; }
        public string Estado { get; set; }
        public int IdCotSRV { get; set; }
        public int? IdUsuarioSrv { get; set; }        
        public string requiereFirmaCliente { get; set; }
        public string counterAsignado { get; set; }
        public List<ReservasOportunidad_NM> ListReservas { get; set; }
        public string EmpresaCliente { get; set; }
        public string nombreCliente { get; set; }
        public string apellidosCliente { get; set; }
        public string IdLoginWeb { get; set; }
        public string telefonoCliente { get; set; }
        public string esEmitido { get; set; }
        public string fechaPlazoEmision { get; set; }
        public string cot_IataPrincipal { get; set; }
        public string tipoCotizacion { get; set; }
        public string emailUserLogin { get; set; }
        public int idUserLogin { get; set; }
        public string accion_SF { get; set; }                
    }


    public class RptaOportunidadSF : ICrmApiResponse
    {
        public string idOportunidad_SF { get; set; }
        public string Identificador_NM { get; set; }      
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }           
    }

    public class ReservasOportunidad_NM
    {
        public string IdReserva { get; set; }
        public string codReserva { get; set; }
        public string fechaCreación { get; set; }
        public string estadoVenta { get; set; }
        public string codigoAerolinea { get; set; }
        public string Tipo { get; set; }
        public string PCCOfficeID { get; set; }
        public string IATA { get; set; }
        public string RUCEmpresa { get; set; }
        public string razonSocial { get; set; }
        public bool aceptarPoliticas { get; set; }
        public float? ruc { get; set; }
        public string descripPaquete { get; set; }
        public string destinoPaquetes { get; set; }
        public string fechasPaquetes { get; set; }
        public string Proveedor { get; set; }
        public PlanReservaSeguro_NM PlanSeguro { get; set; }
        public EmergenciaReservaSeguro_NM EmergenciaSeguro { get; set; }
    }

    public class PlanReservaSeguro_NM
    {
        public string Plan { get; set; }
        public int? CantPasajeros { get; set; }
        public string Destino { get; set; }
        public string FechaSalida { get; set; }
        public string FechaRetorno { get; set; }
        public string Edades { get; set; }
    }
    public class EmergenciaReservaSeguro_NM
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
    }
}