using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.NuevoMundo
{
    public class OportunidadVentaNMRepository : OracleBase, IOportunidadVentaNMRepository
    {
        #region Constructor
        public OportunidadVentaNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetOportunidadVentas()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Parse
        #endregion
    }
}