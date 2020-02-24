using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.FileRetail)]
    public class FileRetailController : BaseController<object>
    {
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            throw new NotImplementedException();
        }
        #region PublicMethod
        [Route(RouteAction.Asociate)]
        public IHttpActionResult Asociate()
        {
            return null;
        }
        #endregion
    }
}
