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
    /// <summary>
    /// Mantenimiento para Contactos B2B
    /// </summary>
    [RoutePrefix(ApiRoutePrefix.CuentaB2B)]
    public class CuentaB2BController : BaseController<CuentaB2B>
    {
        [Route(ApiAction.Create)]
        public override IHttpActionResult Create(CuentaB2B entity)
        {
            entity.IdiomasComunicCliente.Add(new IdiomaComunicCliente() { ID = "Inglés" });
            entity.IdiomasComunicCliente.Add(new IdiomaComunicCliente() { ID = "Español" });
            entity.ID = (new Random()).Next(0, 1000);
            return Ok(new { ID = entity.ID });
        }

        [Route(ApiAction.Update)]
        public override IHttpActionResult Update(CuentaB2B entity)
        {
            return Ok();
        }
    }
}
