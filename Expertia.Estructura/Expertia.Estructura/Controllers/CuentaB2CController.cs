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
    /// Mantenimiento para Contactos B2C
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/CuentaB2C")]
    public class CuentaB2CController : ApiController
    {
        /// <summary>
        /// Ingresa un contacto B2C
        /// </summary>
        /// <param name="contacto">Datos del nuevo contacto</param>
        /// <returns>Status de transacción</returns>
        [HttpPost]
        [Route("Create")]
        public IHttpActionResult Create(CuentaB2C contacto)
        {
            contacto.IdiomasComunicCliente.Add(new IdiomaComunicCliente() { ID = "Inglés" });
            contacto.IdiomasComunicCliente.Add(new IdiomaComunicCliente() { ID = "Español" });
            contacto.ID = (new Random()).Next(0, 1000);
            return Ok(new { contacto.ID });
        }

        /// <summary>
        /// Actualiza un contacto
        /// </summary>
        /// <param name="contacto">Datos del contacto afectado</param>
        /// <returns>Status de transacción</returns>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update(CuentaB2C contacto)
        {
            return Ok();
        }
    }
}
