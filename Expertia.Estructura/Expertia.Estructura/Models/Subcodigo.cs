using Expertia.Estructura.Models.Behavior;
using System.Collections.Generic;

namespace Expertia.Estructura.Models
{
    public class Subcodigo : ICrmApiResponse
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
    }

    public class SubcodigoCrm
    {
        public string Accion { get; set; }
        public IEnumerable<SimpleNegocioDesc> Dk_Agencias { get; set; }
        public IEnumerable<SimpleNegocioDesc> Correlativos { get; set; }
        public string Nombre_Sucursal { get; set; }
        public string Direccion_Sucursal { get; set; }
        public string Estado_Sucursal { get; set; }
        public IEnumerable<SimpleNegocioDesc> Promotores { get; set; }
        public IEnumerable<SimpleNegocioDesc> CondicionesPago { get; set; }
    }
}