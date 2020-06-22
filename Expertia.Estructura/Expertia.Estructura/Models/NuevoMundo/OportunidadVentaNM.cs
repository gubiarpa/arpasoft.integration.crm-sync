using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models.NuevoMundo
{
    public class OportunidadVentaNM
    {
        public string idCuenta_SF { get; set; }
        public string NumDoc { get; set; }
        public string IdTipoDoc { get; set; }
        public short IdCanalVenta { get; set; } // * cambio a int
        public string NombreCli { get; set; }
        public string ApePatCli { get; set; }
        public string ApeMatCli { get; set; }
        public string EmailCli { get; set; }
        public string CiudadIata { get; set; }
        public string IdDestino { get; set; }
        public string EnviarPromociones { get; set; }
        /*public int UsuarioCrea { get; set; }*//*Desestimado*/
        public string Comentario { get; set; }
        public int? IdCotSRV { get; set; }
        public string IdOportunidad_SF { get; set; }
        public int IdUsuarioSrv_SF { get; set; } // * cambio a int
        public string Accion_SF { get; set; }
        public short idEstado { get; set; }
        public string Estado { get; set; }
        public bool? Emitido { get; set; }
        public bool Asignarse { get; set; }
        public string counterAsignado { get; set; }        
        public string IdMotivoNoCompro { get; set; }
        public DateTime? fechaPlazoEmision { get; set; }
        public string CodReserva { get; set; }
        public float? MontoCompra { get; set; }
        public float? MontoEstimado { get; set; }        
        public short? ModalidadCompra { get; set; }
        public string tipoCotizacion { get; set; }        
        public bool? RequiereFirmaCliente { get; set; }        
        public string ServiciosAdicionales { get; set; }
        public short ModoIngreso { get; set; } // * cambio a short
        public short CantidadAdultos { get; set; }
        public short? CantidadNinos { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? Fecharegreso { get; set; }
    }

    public class RptaOportunidadVentaNM
    {
        public string codigo { get; set; }
        public string mensaje { get; set; }
        public int? IdCotSrv { get; set; }        
    }
}