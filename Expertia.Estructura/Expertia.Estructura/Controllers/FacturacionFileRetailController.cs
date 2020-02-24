using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.FacturacionFileRetail)]
    public class FacturacionFileRetailController : BaseController
    {
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            throw new NotImplementedException();
        }

        #region PublicMethod
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(FacturacionFileRetailReq models)
        {
            string result = "";

            return null;
        }
        #endregion
    }
}
