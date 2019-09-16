using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using System;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    /// <summary>
    /// Mantenimiento para Contactos B2C
    /// </summary>
    [RoutePrefix(RoutePrefix.CuentaB2C)]
    public class CuentaB2CController : BaseController<CuentaB2C>
    {
        /// <summary>
        /// Crea un
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
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

        protected override ICrud<CuentaB2C> GetRepository(UnidadNegocioKeys? unidadNegocioKey)
        {
            throw new NotImplementedException();
        }
    }
}
