using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class InformacionPagoNM : ICrmApiResponse, IActualizado
    {
        public string idOportunidad_SF { get; set; }
        public string Identificador_NM { get; set; }
        public string reservaID { get; set; }
        public string tipoServicio { get; set; }
        public string tipoPasajero { get; set; }
        public float totalBoleto { get; set; }
        public float tarifaNeto { get; set; }
        public float impuestos { get; set; }
        public float cargos { get; set; }
        public string nombreHotel { get; set; }
        public float totalPagar { get; set; }
        public string descripcion { get; set; }
        public float montodescuento { get; set; }
        public string textodescuento { get; set; }
        public string promowebcode { get; set; }
        public float totalfacturar { get; set; }
        public float MontoSeg { get; set; }
        public string paq_reserva_tipo { get; set; }
        public float DescuentoSeg { get; set; }
        public float feeAsumidoGeneralBoletos { get; set; }
        public float numHabitacionPaquete { get; set; }
        public string tipoPasajeroPaq { get; set; }
        public float cantidadPasajeroPaq { get; set; }
        public string monedaPaq { get; set; }
        public float precioUnitarioPaq { get; set; }
        public float totalUnitarioPaq { get; set; }
        public float precioTotalPorHabitacionPaq { get; set; }
        public float precioTotalHabitacionesPaq { get; set; }
        public float gastosAdministrativosPaq { get; set; }
        public float tarjetaDeTurismo { get; set; }
        public float tarjetaDeAsistencia { get; set; }
        public string paq_reserva_id { get; set; }//Nuevo
        public float precioTotalActividadesPaq { get; set; }//falta en SP
        public float precioTotalPagarPaq { get; set; }
        public string textoDescuentoPaq { get; set; }
        public float montoDescuentoPaq { get; set; }
        public float totalFacturarPaq { get; set; }
        public int cantDiasSeg { get; set; }
        public float precioUnitarioSeg { get; set; }
        public float MontoReservaSeg { get; set; }
        public string accion_SF { get; set; }
        public string IdInformacionPago_SF { get; set; }
        public string Id_Sucursal { get; set; }
        public string Codigoweb { get; set; }
        public string PaqueteId { get; set; }
        public string SeguroId { get; set; }
        public string IdCotizacion { get; set; }
        public string OrdenServicio { get; set; }
        public string OrdenDatos { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public int Actualizados { get; set; } = -1;
    }
}