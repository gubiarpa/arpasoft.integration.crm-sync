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
    /// Mantenimiento de Contactos
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/Contacto")]
    public class ContactoController : ApiController
    {
        /// <summary>
        /// Ingresa un contacto
        /// </summary>
        /// <param name="contacto">Datos del nuevo contacto</param>
        /// <returns>Status de transacción</returns>
        [HttpPost]
        [Route("Create")]
        public IHttpActionResult Create(Contacto contacto)
        {
            contacto.ID = (new Random()).Next(0, 1000);
            return Ok(contacto.ID);
        }

        /// <summary>
        /// Actualiza un contacto
        /// </summary>
        /// <param name="contacto">Datos del contacto afectado</param>
        /// <returns>Status de transacción</returns>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update(Contacto contacto)
        {
            return Ok();
        }
    }
}
