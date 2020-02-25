using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class Pedido : IUnidadNegocio
    {
        public string IdUsuario { get; set; }
        public int? IdLang { get; set; }
        public int? IdWeb { get; set; }       
        public string IPUsuario { get; set; }
        public string Browser { get; set; }
        public string DetalleServicio { get; set; }
        public string CodePasarelaPago { get; set; }
        public string Email { get; set; }
        public int? TiempoExpiracionCIP { get; set; }
        public string Monto { get; set; }
        public int IdCotVta { get; set; }
        public int? IdCanalVta { get; set; }
        public string NombreClienteCot { get; set; }
        public string ApellidoClienteCot { get; set; }       
        public UnidadNegocio UnidadNegocio { get; set; }
    }

    public class PedidoRS : ICrmApiResponse
    {       
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }        
        public int IdPedido { get; set; }
        public bool CorreoEnviado { get; set; }
        public string CodigoTransaction { get; set; }
        public string CodigoOperacion { get; set; }
    }
}