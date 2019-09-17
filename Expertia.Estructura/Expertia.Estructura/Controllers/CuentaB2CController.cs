using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using System;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.CuentaB2C)]
    public class CuentaB2CController : BaseController<CuentaB2C>
    {
        [Route(RouteAction.Create)]
        public override IHttpActionResult Create(CuentaB2C entity)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                //WriteLogObject(ex, LogType.Fail);
                return InternalServerError(ex);
            }
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(CuentaB2C entity)
        {
            try
            {
                //WriteLogObject(entity);
                return Ok();
            }
            catch (Exception ex)
            {
                //WriteLogObject(ex, LogType.Fail);
                return InternalServerError(ex);
            }
        }

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            throw new NotImplementedException();
        }
    }
}
