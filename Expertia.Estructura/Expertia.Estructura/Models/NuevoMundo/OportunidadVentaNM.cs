using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models.NuevoMundo
{
    public class OportunidadVentaNM
    {
        public string NumDoc { get; set; }
        public string IdTipoDoc { get; set; }
        public short IdCanalVenta { get; set; } // * cambio a int
        public string NombreCli { get; set; }
        public string ApePatCli { get; set; }
        public string ApeMatCli { get; set; }
        public string EmailCli { get; set; }
        public string IdDestino { get; set; }
        public string EnviarPromociones { get; set; }
        public int UsuarioCrea { get; set; } // * cambio a int
        public string Comentario { get; set; }
        public string IdCotSRV { get; set; }
        public string IdOportunidad_SF { get; set; }
        public int IdUsuarioSrv_SF { get; set; } // * cambio a int
        public string Accion_SF { get; set; }
        public string Estado { get; set; }
        public string HoraEmision { get; set; }
        public string CodReserva { get; set; }
        public float MontoEstimado { get; set; }
        public float MontoCompra { get; set; }
        public string ModalidadCompra { get; set; }
        public string Tipo { get; set; }
        public string DireccionCliente { get; set; }
        public string NumTelefono { get; set; }
        public bool RequiereFirmaCliente { get; set; }
        public bool CajaVuelos { get; set; }
        public bool CajaHotel { get; set; }
        public bool CajaPaquetes { get; set; }
        public bool CajaServicios { get; set; }
        public bool CajaSeguros { get; set; }
        public short ModoIngreso { get; set; } // * cambio a short
        public int CantidadAdultos { get; set; }
        public int CantidadNinos { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaEgreso { get; set; }
    }
}