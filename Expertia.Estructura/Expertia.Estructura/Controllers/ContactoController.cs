using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using System;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.Contacto)]
    public class ContactoController : BaseController<Contacto>
    {
        [Route(RouteAction.Create)]
        public override IHttpActionResult Create(Contacto entity)
        {
            try
            {
                WriteEntityLog(entity);
                // Aquí va la interacción con BD
                return Ok();
            }
            catch (Exception ex)
            {
                _logFileManager.WriteLine(LogType.Fail, ex.Message);
                return InternalServerError(ex);
            }            
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(Contacto entity)
        {
            try
            {
                WriteEntityLog(entity);
                // Aquí va la interacción con BD
                return Ok();
            }
            catch (Exception ex)
            {
                _logFileManager.WriteLine(LogType.Fail, ex.Message);
                return InternalServerError(ex);
            }
        }
    }
}
