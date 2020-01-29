using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.Condor;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.Cotizacion)]
    public class CotizacionController : BaseController<Cotizacion>
    {
        #region Properties
        private ICotizacion_CT _cotizacionRepositoryDM;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Read)]
        public IHttpActionResult Read(CotizacionRequest cotizacionRequest)
        {
            var error = string.Empty;
            try
            {
                if (RepositoryByBusiness(cotizacionRequest.Region.ToUnidadNegocioByCountry()) != null)
                {
                    var cotizacionResponse = _cotizacionRepositoryDM.GetCotizacionCT(cotizacionRequest);
                    var codigoRetorno = cotizacionResponse[OutParameter.CodigoError].ToString();
                    var mensajeRetorno = cotizacionResponse[OutParameter.MensajeError].ToString();
                    var data = (IEnumerable<CotizacionResponse>)cotizacionResponse[OutParameter.CursorCotizacion];
                    return Ok(new { codigoRetorno, mensajeRetorno, data });
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    Error = error,
                    Body = cotizacionRequest
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        public IHttpActionResult Send()
        {
            IEnumerable<CotizacionDM> cotizaciones;
            var error = string.Empty;
            try
            {
                //cotizaciones = (IEnumerable<CotizacionDM>)(_cotizacionRepository_DM.)
                return null;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    Error = error
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliar
        private void LoadResults(UnidadNegocioKeys? unidadNegocio, out object logResult, out object result)
        {
            switch (unidadNegocio)
            {
                case UnidadNegocioKeys.DestinosMundiales:
                    #region Log
                    logResult = new
                    {
                        Result = new
                        {
                            DestinosMundiales = new
                            {
                                Codes = GetErrorResult(UnidadNegocioKeys.DestinosMundiales),
                                IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString(),
                                IdCotizacion = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCotizacion].ToString()
                            }
                        }
                    };
                    #endregion
                    #region Client
                    result = new
                    {
                        Result = new
                        {
                            DestinosMundiales = new
                            {
                                CodigoError = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.CodigoError].ToString(),
                                MensajeError = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.MensajeError].ToString(),
                                IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString(),
                                IdCotizacion = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCotizacion].ToString()
                            }
                        }
                    };
                    #endregion
                    break;
                default:
                    logResult = null; result = null;
                    break;
            }
        }

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            _cotizacionRepositoryDM = new Cotizacion_CT_Repository(unidadNegocioKey);
            return unidadNegocioKey;
        }
        #endregion

        #region SalesforceEntities
        private object ToCotizacionEntity(Cotizacion cotizacion)
        {
            try
            {
                return new
                {
                    info = new
                    {
                        CodigoRetorno = "OK",
                        MensajeRetorno = "Mensaje Retorno",
                        Grupo = cotizacion.Grupo
                    }
                };
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
