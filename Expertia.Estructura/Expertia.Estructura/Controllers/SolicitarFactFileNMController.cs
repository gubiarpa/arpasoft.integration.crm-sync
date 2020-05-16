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
    /// CRMAFO06 : Envio de datos de File y actualización
    [RoutePrefix(RoutePrefix.SolicitarFactFileNM)]
    public class SolicitarFactFileNMController : BaseController
    {
        private SolicitarFactFileNMRepository _solicitarFactFileNMRepository;
               
        protected override ControllerName _controllerName => ControllerName.FileOportunidadNM;

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            return Ok();
        }
        #endregion

        #region Auxiliar
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _solicitarFactFileNMRepository = new SolicitarFactFileNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
        #endregion
    }
}
