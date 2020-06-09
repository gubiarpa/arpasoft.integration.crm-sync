using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class DetalleHotelNM : ICrmApiResponse
    {
        public string idOportunidad_SF { get; set; }
        public string idDetalleHotel_SF { get; set; }
        public string Identificador_NM { get; set; }
        public string hotel { get; set; }
        public string direccion { get; set; }
        public string destino { get; set; }
        public string categoria { get; set; }
        public string fechaIngreso { get; set; }
        public string fechaSalida { get; set; }
        public string fechaCancelacion { get; set; }
        public string codigoReservaNemo { get; set; }
        public string Proveedor { get; set; }
        public string accion_SF { get; set; }        
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }        
    }

    public class RptaHotelSF : ICrmApiResponse
    {
        public string idOportunidad_SF { get; set; }
        public string idDetalleHotel_SF { get; set; }
        public string Identificador_NM { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
    }
}