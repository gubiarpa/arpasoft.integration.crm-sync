using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class GenCodigoPagoNM
    {
        public string detalleServicio { get; set; }
        public string monto { get; set; }
        public int tiempoExpiracionCIP { get; set; }
        public string email { get; set; }
        public string idOportunidad_SF { get; set; }
        public int idCotVta { get; set; }
        public string idSolicitudPago_SF { get; set; }
        public int idUsuario { get; set; }
        public string idCanalVta { get; set; }
        public string NombreClienteCot { get; set; }
        public string ApellidoClienteCot { get; set; }        
        public UnidadNegocio unidadDeNegocio { get; set; }        
        public int idLang { get; set; }
        public int idWeb { get; set; }
        public string ipUsuario { get; set; }
        public string browser { get; set; }
        public string codePasarelaPago { get; set; }        
    }
}