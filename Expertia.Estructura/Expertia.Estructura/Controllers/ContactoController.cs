using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
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
            var startReq = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            entity.ID = (new Random()).Next(0, 1000);
            return Ok(new
                {
                    ID = entity.ID,
                    TimeRequest = new TimeRequest()
                    {
                        Start = startReq,
                        End = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                    }
                });
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(Contacto entity)
        {
            var startReq = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            return Ok(new
                {
                    timeRequest = new
                    {
                        StartRequest = startReq,
                        EndRequest = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                    }
                });
        }
    }
}
