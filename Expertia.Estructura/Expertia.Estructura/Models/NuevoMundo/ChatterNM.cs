using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class ChatterNM : ICrmApiResponse, IActualizado
    {
        public string idOportunidad_SF { get; set; }
        public string idCotSrv_SF { get; set; }
        public string texto { get; set; }
        public string accion_SF { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public int Actualizados { get; set; } = -1;
    }
}