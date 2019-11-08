using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class Subcodigo : IUnidadNegocio
    {
        #region Properties
        public string DkAgencia { get; set; }
        public string NombreSucursal { get; set; }
        public string DireccionSucursal { get; set; }
        #endregion

        #region ForeignKey
        public UnidadNegocio UnidadNegocio { get; set; }
        public SimpleDesc Accion { get; set; }
        public SimpleDesc EstadoSucursal { get; set; }
        public IEnumerable<Promotor> Promotores { get; set; }
        #endregion
    }

    public class Promotor : SimpleDesc, IUnidadNegocio
    {
        public UnidadNegocio UnidadNegocio { get; set; }
    }
}