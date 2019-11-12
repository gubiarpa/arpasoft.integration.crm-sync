using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Repository.InterAgencias;
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
    /// Entidad Exclusiva para Condor Travel e Interagencias
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
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            object files = null;
            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;
                _operCollection[_unidadNegocio] = _crmCollection[_unidadNegocio].Read(null);
                _instants[InstantKey.Oracle] = DateTime.Now;
                files = new { Interagencias = (List<File>)_operCollection[_unidadNegocio][OutParameter.CursorFile] };
                return Ok(files);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
            }
        }
        #endregion

        #region NotImplemented
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.CondorTravel:
                    break;
                case UnidadNegocioKeys.InterAgencias:
                    _crmCollection.Add(UnidadNegocioKeys.InterAgencias, new File_IA_Repository());
                    break;
                default:
                    break;
            }
            return unidadNegocioKey;
        }
        #endregion
    }
}
