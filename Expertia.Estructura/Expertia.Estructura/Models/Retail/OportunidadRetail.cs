using Expertia.Estructura.Models.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models.Retail
{
    public class OportunidadRetailReq
    {
        public string Numdoc { get; set; }
        public string IdTipoDoc { get; set; }
        public short IdCanalVenta { get; set; }
        public string NombreCli { get; set; }
        public string ApePatCli { get; set; }
        public string ApeMatCli { get; set; }
        public string EmailCli { get; set; }
        public string IdDestino { get; set; }
        public string EnviarPromociones { get; set; }
        public short MotivoCrea { get; set; }
        public string Area { get; set; }
        public int UsuarioCrea { get; set; }
        public string Comentario { get; set; }
        public int? IdCotSRV { get; set; }
        public string IdOportunidad_SF { get; set; }
        public int IdUsuarioSrv_SF { get; set; }
        public string Accion_SF { get; set; }
    }

    public class OportunidadRetailRes : ICrmApiResponse
    {
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public string IdOportunidad_SF { get; set; }
        public int? IdCotSrv { get; set; }
        public string FechaCreacion { get; set; }

    }
}