using Expertia.Estructura.Models.Behavior;
using System.Collections.Generic;

namespace Expertia.Estructura.Models
{
    public class Subcodigo : ICrmApiResponse, IActualizado
    {
        public string UnidadNegocio { get; set; }
        public string Accion { get; set; }
        public int DkAgencia { get; set; }
        public int CorrelativoSubcodigo { get; set; }
        public string NombreSucursal { get; set; }
        public string DireccionSucursal { get; set; }
        public string EstadoSucursal { get; set; }
        public string Promotor { get; set; }
        public string CondicionPago { get; set; }
        public string Usuario { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public int Actualizados { get; set; } = -1;
    }
}