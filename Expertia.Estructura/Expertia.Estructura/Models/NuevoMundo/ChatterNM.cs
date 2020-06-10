using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class ChatterNM : ICrmApiResponse
    {
        public string idOportunidad_SF { get; set; }
        public string IdRegPostCotSrv_SF { get; set; }
        public int idPostCotSrv { get; set; }
        public string Identificador_NM { get; set; }
        public string cabecera { get; set; }
        public string texto { get; set; }
        public string fecha { get; set; }
        public string accion_SF { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }        
    }

    public class RptaChatterSF : ICrmApiResponse
    {
        public string idOportunidad_SF { get; set; }
        public string IdRegPostCotSrv_SF { get; set; }
        public string Identificador_NM { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
    }
}