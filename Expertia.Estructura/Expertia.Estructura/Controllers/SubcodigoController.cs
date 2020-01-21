using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.DestinosMundiales;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.RestManager.RestParse;
using Expertia.Estructura.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Expertia.Estructura.Controllers
{
    /// <summary>
    /// Entidad exclusiva para Destinos Mundiales e Interagencias
    /// </summary>
    [RoutePrefix(RoutePrefix.Subcodigo)]
    public class SubcodigoController : BaseController<object>
    {
        #region Properties
        private ISubcodigoRepository _subcodigoRepository;
        private ICrmApiResponse _response;
        #endregion

        #region Constructor
        public SubcodigoController()
        {
        }
        #endregion

        #region PublicMethods
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(Subcodigo subcodigo)
        {
            object result = null;
            try
            {
                var unidadNegocio = RepositoryByBusiness(subcodigo.UnidadNegocio.ToUnidadNegocio());
                _instants[InstantKey.Salesforce] = DateTime.Now;

                var operResult = _subcodigoRepository.Create(subcodigo);
                //_response = new CrmApiResponse(operResult[OutParameter.CodigoError].ToString(), operResult[OutParameter.MensajeError].ToString());
                _response = new CrmApiResponse(subcodigo.CodigoError, subcodigo.MensajeError);
                if (!int.TryParse(operResult[OutParameter.IdSubcodigo].ToString(), out int idSubcodigo)) idSubcodigo = -1;
                result = new
                {
                    Result = _response,
                    Response = new
                    {
                        IdSubcodigo = idSubcodigo
                    }
                };

                _instants[InstantKey.Oracle] = DateTime.Now;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    Instants = GetInstants(),
                    Result = result,
                    Body = subcodigo
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<Subcodigo> subcodigos = null;
            string exceptionMsg = string.Empty;
            try
            {
                var unidadNegocioType = RepositoryByBusiness(unidadNegocio.Descripcion.ToUnidadNegocio());

                /// Consulta de Subcodigos a PTA
                subcodigos = (IEnumerable<Subcodigo>)(_subcodigoRepository.Read())[OutParameter.CursorSubcodigo];

                if (subcodigos == null || subcodigos.ToList().Count.Equals(0)) return Ok();

                /// Obtiene Token para envío a Salesforce
                var token = RestBase.GetTokenByKey(SalesforceKeys.AuthServer, SalesforceKeys.AuthMethod);

                foreach (var subcodigo in subcodigos)
                {
                    try
                    {
                        /// Envío de subcodigo a Salesforce
                        var responseSubcodigo = RestBase.ExecuteByKey(SalesforceKeys.CrmServer, SalesforceKeys.SubcodigoMethod, Method.POST, subcodigo.ToSalesforceEntity(), true, token);
                        if (responseSubcodigo.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            dynamic jsonResponse = new JavaScriptSerializer().DeserializeObject(responseSubcodigo.Content);
                            subcodigo.CodigoError = jsonResponse[OutParameter.SF_CodigoError];
                            subcodigo.MensajeError = jsonResponse[OutParameter.SF_MensajeError];

                            /// Actualización de estado de subcodigo a PTA
                            var updateResponse = _subcodigoRepository.Update(subcodigo);
                            subcodigo.Actualizados = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        subcodigo.CodigoError = ApiResponseCode.ErrorCode;
                        subcodigo.MensajeError = ex.Message;
                    }
                }

                return Ok(subcodigos);
            }
            catch (Exception ex)
            {
                subcodigos = null;
                exceptionMsg = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    UnidadNegocio = unidadNegocio.Descripcion,
                    Exception = exceptionMsg,
                    LegacySystems = subcodigos
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliar
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.CondorTravel:
                    break;
                case UnidadNegocioKeys.DestinosMundiales:
                    _subcodigoRepository = new Subcodigo_DM_Repository();
                    break;
                case UnidadNegocioKeys.Interagencias:
                    _subcodigoRepository = new Subcodigo_IA_Repository();
                    break;
                default:
                    break;
            }
            return unidadNegocioKey;
        }
        #endregion
    }
}
