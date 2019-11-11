using Expertia.Estructura.Controllers.Base;
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
    /// Entidad Exclusiva para Destinos Mundiales e Interagencias
    /// </summary>
    [RoutePrefix(RoutePrefix.File)]
    public class FileController : BaseController<File>
    {
        #region Constructor
        public FileController()
        {
        }
        #endregion

        #region PublicMethods
        public IHttpActionResult Send()
        {
            object files = null;
            try
            {

                return Ok(files);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }
        #endregion

        #region NotImplemented
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
