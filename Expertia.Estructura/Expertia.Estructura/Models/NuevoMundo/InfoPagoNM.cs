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

        public List<ListPagoBoletoServicios> ListPago_Boleto_Servicios { get; set; }

        public float totalPagar { get; set; }
        public float montoDescuento { get; set; }
        public string textodescuento { get; set; }
        public string promoWebCode { get; set; }
        public float totalFacturar { get; set; }
        public float feeAsumidoGeneralBoletos { get; set; }
        public List<ListPagosDesglosePaquete> ListPagosDesglose_Paquete { get; set; }


        public float precioTotalHabitacionesPaq { get; set; }
        public float gastosAdministrativosPaq { get; set; }
        public string tarjetaDeTurismo { get; set; }
        public string tarjetaDeAsistencia { get; set; }
        public List<ListPagosServicioPaquete> ListPagosServicio_Paquete { get;set;}
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

        //public string Id_Sucursal { get; set; }
        //public string Codigoweb { get; set; }
        //public string PaqueteId { get; set; }
        //public string SeguroId { get; set; }
        //public string IdCotizacion { get; set; }
        //public string OrdenServicio { get; set; }
        //public string OrdenDatos { get; set; }
        //public string CodigoError { get; set; }
        //public string MensajeError { get; set; }
        //public int Actualizados { get; set; } = -1;
    }

    public partial class ListPagoBoletoServicios
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
    public partial class ListPagosDesglosePaquete
    {       
        public List<ListPorHabitacion_Paq> ListPorHabitacionPaq { get; set; }
        public float precioTotalPorHabitacionPaq { get; set; }
    }

    public partial class ListPorHabitacion_Paq
    {
        public float numHabitacionPaquete { get; set; }
        public string tipoPasajeroPaq { get; set; }
        public float cantidadPasajeroPaq { get; set; }
        public string monedaPaq { get; set; }
        public float precioUnitarioPaq { get; set; }
        public float totalUnitarioPaq { get; set; }
    }

    public partial class ListPagosServicioPaquete
    { 
        public string descripcionServ { get; set; }
        public float pasajerosServ { get; set; }
        public float precioServ { get; set; }
    }


}
