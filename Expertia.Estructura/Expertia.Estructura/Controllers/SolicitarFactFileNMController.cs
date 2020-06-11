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
    /// Expertia 1 : Solicitud de facturacion del file / Desglose CA
    [RoutePrefix(RoutePrefix.SolicitarFactFileNM)]
    public class SolicitarFactFileNMController : BaseController
    {
        private SolicitarFactFileNMRepository _solicitarFactFileNMRepository;
               
        protected override ControllerName _controllerName => ControllerName.FileOportunidadNM;

        #region PublicMethods
        [Route(RouteAction.Read)]
        public IHttpActionResult Read(SolicitarFactFileNM solicitarFactFileNM)
        {
            var operation = new Operation();
                
            var result = _solicitarFactFileNMRepository.GuardarDesgloseCA(solicitarFactFileNM);

            if (solicitarFactFileNM.existeArchivoList)
            {
                _solicitarFactFileNMRepository.GuardarArchivo(solicitarFactFileNM, result, int.Parse(solicitarFactFileNM.idusuario));
            }

            if (solicitarFactFileNM.existeIdDatosFacturacion)
            {
                var archivoList = _solicitarFactFileNMRepository.ObtenerArchivos(solicitarFactFileNM.iddatosfacturacion);
            }

            return Ok(new
            {
                Codigo = DbResponseCode.Success,
                Mensaje = string.Empty
            });
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
