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
    public class ContactoB2BController : ApiController
    {
        /// <summary>
        /// Ingresa un contacto
        /// </summary>
        /// <param name="contacto">Datos del nuevo contacto</param>
        /// <returns>Status de transacción</returns>
        [HttpPost]
        public IHttpActionResult Create(ContactoB2B contacto)
        {
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacto"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult Update(ContactoB2B contacto)
        {
            return Ok();
        }
    }
}
