using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class DetalleItinerarioNM : ICrmApiResponse
    {
        public string idOportunidad_SF { get; set; }
        public string Identificador_NM { get; set; }
        public string id_Itinerario_SF { get; set; }
        public int id_reserva { get; set; }
        public string id_itinerario { get; set; }
        public string LAerea { get; set; }
        public string Origen { get; set; }
        public string Salida { get; set; }
        public string Destino { get; set; }
        public string llegada { get; set; }
        public string numeroVuelo { get; set; }
        public string Clase { get; set; }
        public string fareBasis { get; set; }
        public string OperadoPor { get; set; }
        public string esRetornoItinerario { get; set; }
        public string accion_SF { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }        
    }

    public class RptaItinerarioSF : ICrmApiResponse
    {
        public string idOportunidad_SF { get; set; }
        public string idItinerario_SF { get; set; }
        public string Identificador_NM { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }        
    }
}