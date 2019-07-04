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
    public class ContactoB2CController : ApiController
    {
        /// <summary>
        /// Ingresa un contacto B2C
        /// </summary>
        /// <param name="contacto">Datos del nuevo contacto</param>
        /// <returns>Status de transacción</returns>
        [HttpPost]
        public IHttpActionResult Create(ContactoB2C contacto)
        {
            return Ok(contacto);
        }

        /// <summary>
        /// Actualiza un contacto
        /// </summary>
        /// <param name="contacto">Datos del contacto afectado</param>
        /// <returns>Status de transacción</returns>
        [HttpPut]
        public IHttpActionResult Update(ContactoB2C contacto)
        {
            return Ok(contacto);
        }
    }
}
