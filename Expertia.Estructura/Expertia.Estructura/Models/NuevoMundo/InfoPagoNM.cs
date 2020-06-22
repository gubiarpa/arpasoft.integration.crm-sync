using Expertia.Estructura.Models.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models.NuevoMundo
{
    public class InfoPagoNM
    {
        public string idOportunidad_SF { get; set; }
        public string Identificador_NM { get; set; }
        public string IdInformacionPago_SF { get; set; }
        //public List<PagoBoletoServicios> ListPago_Boleto_Servicios { get; set; }
        public List<PagoBoletoServicios> ListPago_Boleto_Servicios { get; set; }
        public float totalPagar { get; set; }
        public float montoDescuento { get; set; }
        public string textoDescuento { get; set; }
        public string promoWebCode { get; set; }
        public float totalFacturar { get; set; }
        public float feeAsumidoGeneralBoletos { get; set; }
        public List<PagosDesglosePaquete> ListPagosDesglose_Paquete { get; set; }
        public float precioTotalHabitacionesPaq { get; set; }
        public float gastosAdministrativosPaq { get; set; }
        public float tarjetaDeTurismo { get; set; }
        public float tarjetaDeAsistencia { get; set; }
        public string PaqueteId { get; set; }
        public List<PagosServicioPaquete> ListPagosServicio_Paquete { get;set;}
        public float precioTotalActividadesPaq { get; set; }//falta en SP
        public string textoDescuentoPaq { get; set; }
        public float montoDescuentoPaq { get; set; }
        public float totalFacturarPaq { get; set; }
        public float precioTotalPagarPaq { get; set; }
        public int cantDiasSeg { get; set; }
        public float precioUnitarioSeg { get; set; }
        public float MontoSeg { get; set; }
        public float DescuentoSeg { get; set; }
        public float MontoReservaSeg { get; set; }
        public string accion_SF { get; set; }
    }

    public partial class PagoBoletoServicios
    {
        public string reservaID { get; set; }
        public string tipoServicio { get; set; }
        public string tipoPasajero { get; set; }
        public float totalBoleto { get; set; }
        public float tarifaNeto { get; set; }
        public float impuestos { get; set; }
        public float cargos { get; set; }
        public string descripcion { get; set; }
    }
    public partial class PagosDesglosePaquete
    {       
        public List<PorHabitacion_Paq> ListPorHabitacionPaq { get; set; }
        public float precioTotalPorHabitacionPaq { get; set; }
    }

    public partial class PorHabitacion_Paq
    {
        public float numHabitacionPaquete { get; set; }
        public string tipoPasajeroPaq { get; set; }
        public float cantidadPasajeroPaq { get; set; }
        public string monedaPaq { get; set; }
        public float precioUnitarioPaq { get; set; }
        public float totalUnitarioPaq { get; set; }
    }

    public partial class PagosServicioPaquete
    { 
        public string descripcionServ { get; set; }
        public float pasajerosServ { get; set; }
        public float precioServ { get; set; }
    }
    //P_CODIGO_ERROR IN VARCHAR2,
    //P_MENSAJE_ERROR IN VARCHAR2,
    //P_IDOPORTUNIDAD_SF IN VARCHAR2,
    //P_IDINFOPAGO_SF IN VARCHAR2,
    //P_CODE_SERV_NM IN VARCHAR2, 
    //P_IDENTIFY_NM_CAMB IN NUMBER, 
    //P_ACTUALIZADOS OUT NUMBER
    public class RptaInformacionPagoSF : ICrmApiResponse
    {
        public string idOportunidad_SF { get; set; }
        public string IdInfoPago_SF { get; set; } //Viene de Salesforce
        public string Identificador_NM { get; set; } //P_IDENTIFY_NM_CAMB id _reserva
        public string CodigoServicio_NM { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
    }
}
