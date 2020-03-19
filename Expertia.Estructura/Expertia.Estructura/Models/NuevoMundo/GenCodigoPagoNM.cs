using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class GenCodigoPagoNM
    {
        public string detalledeServicio { get; set; }
        public string totalAPagar { get; set; }
        public float tiempoExpiracion { get; set; }
        public string email { get; set; }
        public string idOportunidad_SF { get; set; }
        public string idCotSRV { get; set; }
        public string idSolicitudPago_SF { get; set; }
        public float idUsuarioSrv_SF { get; set; }
        public string idCanalVenta { get; set; }
        public string nombreCli { get; set; }
        public string apePatCli { get; set; }
        public string unidadNegocio { get; set; }
        public string id { get; set; }
        public string descripcion { get; set; }
        public float idLang { get; set; }
        public float idWeb { get; set; }
        public string ipUsuario { get; set; }
        public string browser { get; set; }
        public string codePasarelaPago { get; set; }
        public string codigoTransaccion { get; set; }
        public string codigo { get; set; }
        public string mensaje { get; set; }
    }
}