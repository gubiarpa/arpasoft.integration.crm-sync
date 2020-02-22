using Expertia.Estructura.Models.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class OportunidadRetailReq
    {
        public string IdOportunidadSf { get; set; }
        public string IdCotizacionSf { get; set; }
        public string IdCuentaSf { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public string TipoDocumento { get; set; }
        public string Documento { get; set; }
        public string Correo { get; set; }
        public string Region { get; set; }
        public string Cotizacion { get; set; }
        public int? NumeroPaxs { get; set; }
    }

    public class OportunidadRetailRes : ICrmApiResponse
    {
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public string Grupo { get; set; }
        public string Estado { get; set; }
        public float? VentaEstimada { get; set; }
        public string FileSubfile { get; set; }
        public float? VentaFile { get; set; }
        public float? MargenFile { get; set; }
        public int? PaxsFile { get; set; }
        public string EstadoFile { get; set; }
    }
}