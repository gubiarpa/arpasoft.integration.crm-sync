using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using System;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.CotizacionJourneyou)]
    public class CotizacionJYController : BaseController<object>
    {
        #region PublicMethods
        public IHttpActionResult Read(Cotizacion_JY cotizacion)
        {
            return null;
        }
        #endregion

        #region Auxiliar
        #endregion
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            throw new NotImplementedException();
        }
    }
}
