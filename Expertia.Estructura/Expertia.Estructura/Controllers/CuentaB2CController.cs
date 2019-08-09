using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
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
                WriteEntityInLog(entity);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteObjectInLog(ex, LogType.Fail);
                return InternalServerError(ex);
            }            
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(CuentaB2C entity)
        {
            try
            {
                WriteEntityInLog(entity);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteObjectInLog(ex, LogType.Fail);
                return InternalServerError(ex);
            }
        }
    }
}
