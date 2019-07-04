using Expertia.Estructura.Models;
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
    [AllowAnonymous]
    [RoutePrefix("api/CuentaB2B")]
    public class CuentaB2BController : ApiController
    {
        /// <summary>
        /// Ingresa un contacto B2B
        /// </summary>
        /// <param name="contacto">Datos del nuevo contacto</param>
        /// <returns>Status de transacción</returns>
        [HttpPost]
        [Route("Create")]
        public IHttpActionResult Create(CuentaB2B contacto)
        {
            contacto.IdiomasComunicCliente.Add(new IdiomaComunicCliente() { ID = "Inglés" });
            contacto.IdiomasComunicCliente.Add(new IdiomaComunicCliente() { ID = "Español" });
            contacto.ID = (new Random()).Next(0, 1000);
            return Ok(new { ID = contacto.ID });
        }

        /// <summary>
        /// Actualiza un contacto
        /// </summary>
        /// <param name="contacto">Datos del contacto afectado</param>
        /// <returns>Status de transacción</returns>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update(CuentaB2B contacto)
        {
            return Ok();
        }
    }
}
