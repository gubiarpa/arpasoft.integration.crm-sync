using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Expertia.Estructura.Controllers
{
    /// <summary>
    /// Mantenimiento para Contactos B2B
    /// </summary>
    [RoutePrefix(RoutePrefix.CuentaB2B)]
    public class CuentaB2BController : BaseController<CuentaB2B>
    {
        [Route(RouteAction.Create)]
        public override IHttpActionResult Create(CuentaB2B entity)
        {
            entity.IdiomasComunicCliente.Add(new IdiomaComunicCliente() { ID = "Inglés" });
            entity.IdiomasComunicCliente.Add(new IdiomaComunicCliente() { ID = "Español" });
            entity.ID = (new Random()).Next(0, 1000);

            //var json 
            return Ok(new { ID = entity.ID });
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(CuentaB2B entity)
        {
            return Ok();
        }
    }
}
