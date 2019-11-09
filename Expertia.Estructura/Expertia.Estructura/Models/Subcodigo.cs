using Expertia.Estructura.Models.Behavior;
using System.Collections.Generic;

namespace Expertia.Estructura.Models
{
    public class Subcodigo
    {
        #region Properties
        public string NombreSucursal { get; set; }
        public string DireccionSucursal { get; set; }
        #endregion

        #region ForeignKey
        public IEnumerable<SimpleNegocioDesc> IdCuentas { get; set; }
        public IEnumerable<SimpleNegocioDesc> CondicionesPago { get; set; }
        public SimpleDesc Accion { get; set; }
        public SimpleDesc EstadoSucursal { get; set; }
        public IEnumerable<SimpleNegocioDesc> Promotores { get; set; }
        public SimpleDesc Usuario { get; set; }
        #endregion
    }
}