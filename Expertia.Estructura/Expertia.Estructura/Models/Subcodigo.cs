using Expertia.Estructura.Models.Behavior;
using System.Collections.Generic;

namespace Expertia.Estructura.Models
{
    public class Subcodigo : ICrmApiResponse
    {
        public string UnidadNegocio { get; set; }
        public string Accion { get; set; }
        public int? DkAgencia_IA { get; set; }
        public int? DkAgencia_DM { get; set; }
        public int? CorrelativoSubcodigo_IA { get; set; }
        public int? CorrelativoSubcodigo_DM { get; set; }
        public string NombreSucursal { get; set; }
        public string DireccionSucursal { get; set; }
        public string EstadoSucursal { get; set; }
        public string Promotor_IA { get; set; }
        public string Promotor_DM { get; set; }
        public string CondicionPago_DM { get; set; }
        public string CondicionPago_IA { get; set; }
        public string Usuario { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public int Actualizados { get; set; } = -1;
        public int Actualizados_IA { get; set; } = -1;
    }
}