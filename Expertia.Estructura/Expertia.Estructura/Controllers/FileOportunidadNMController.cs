using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Journeyou;
using Expertia.Estructura.Repository.NuevoMundo;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.FileOportunidadNM)]
    public class FileOportunidadNMController : BaseController
    {
        private FileOportunidadNMRepository _fileOportunidadNMRepository;
               
        protected override ControllerName _controllerName => ControllerName.FileOportunidadNM;

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(FileOportunidadNM fileOportunidadNM)
        {
            UnidadNegocioKeys? _unidadNegocio = null;
            string exceptionMsg = string.Empty;
            try
            {
                RepositoryByBusiness(_unidadNegocio);
                //if ((_unidadNegocio = RepositoryByBusiness(cotizacion.Region.ToUnidadNegocioByCountry())) != null)
                //{
                var operation = _fileOportunidadNMRepository.AsociarFileOportunidad(fileOportunidadNM);
                fileOportunidadNM.codigo = operation[OutParameter.SF_Codigo].ToString();
                fileOportunidadNM.mensaje = operation[OutParameter.SF_Mensaje].ToString();
                    
                return Ok(fileOportunidadNM);
                //}
                //return NotFound();
            }
            catch (Exception ex)
            {
                exceptionMsg = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    UnidadNegocio = _unidadNegocio.ToLongName(),
                    Body = fileOportunidadNM,
                    Exception = exceptionMsg
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliar
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _fileOportunidadNMRepository = new FileOportunidadNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
        #endregion
    }
}
