using Expertia.Estructura.Filters;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    /// <summary>
    /// Mantenimiento de entidad
    /// </summary>
    /// <typeparam name="T">Entidad Genérica</typeparam>
    [BasicAuthentication]
    public abstract class BaseController<T> : ApiController
    {
        /// <summary>
        /// Agrega una entidad
        /// </summary>
        /// <param name="entity">Nueva entidad</param>
        /// <returns>Status de transacción</returns>
        [HttpPost]
        public abstract IHttpActionResult Create(T entity);

        /// <summary>
        /// Actualiza una entidad
        /// </summary>
        /// <param name="entity">Entidad a actualizar</param>
        /// <returns>Status de transacción</returns>
        [HttpPost]
        public abstract IHttpActionResult Update(T entity);
    }
}
