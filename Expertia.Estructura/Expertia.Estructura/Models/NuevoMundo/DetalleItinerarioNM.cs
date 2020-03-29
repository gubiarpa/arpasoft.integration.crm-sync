using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class DetalleItinerarioNM : ICrmApiResponse, IActualizado
    {
        public string idOportunidad_SF { get; set; }
        public string lAerea { get; set; }
        public string origen { get; set; }
        public string salida { get; set; }
        public string destino { get; set; }
        public string llegada { get; set; }
        public float numeroVuelo { get; set; }
        public string clase { get; set; }
        public string fareBasis { get; set; }
        public string operadoPor { get; set; }
        public string idItinerario_SF { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public int Actualizados { get; set; } = -1;
    }
}