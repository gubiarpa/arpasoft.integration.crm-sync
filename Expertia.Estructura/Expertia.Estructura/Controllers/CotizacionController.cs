using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.DestinosMundiales;
using Expertia.Estructura.Repository.Condor;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Expertia.Estructura.RestManager.Base;
using RestSharp;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.Cotizacion)]
    public class CotizacionController : BaseController<Cotizacion>
    {
        #region Properties
        private ICotizacionCT _cotizacionCTRepository;
        #endregion

        #region Constructor
        public CotizacionController()
        {
            _cotizacionCTRepository = new Cotizacion_CT_Repository(UnidadNegocioKeys.CondorTravel);
        }
        #endregion

        #region PublicMethods
        [Route(RouteAction.Generate)]
        /*public*/ IHttpActionResult Generate(Cotizacion entity)
        {
            object error = null, logResult = null;
            try
            {
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                object result;
                _instants[InstantKey.Salesforce] = DateTime.Now;
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        _operCollection[UnidadNegocioKeys.CondorTravel] = _crmCollection[UnidadNegocioKeys.CondorTravel].Generate(entity);
                        LoadResults(UnidadNegocioKeys.CondorTravel, out logResult, out result);
                        break;
                    default:
                        return NotFound();
                }
                _instants[InstantKey.Oracle] = DateTime.Now;
                return Ok(result);
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
                    BusinessUnity = entity.UnidadNegocio.Descripcion,
                    LegacySystems = logResult,
                    Instants = GetInstants(),
                    Error = error,
                    Body = entity
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        [Route(RouteAction.Asociate)]
        /*public*/ IHttpActionResult Asociate(Cotizacion entity)
        {
            object error = null, logResult = null;
            try
            {
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                object result;
                _instants[InstantKey.Salesforce] = DateTime.Now;
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.DestinosMundiales:
                        _operCollection[UnidadNegocioKeys.DestinosMundiales] = _crmCollection[UnidadNegocioKeys.DestinosMundiales].Asociate(entity);
                        LoadResults(UnidadNegocioKeys.DestinosMundiales, out logResult, out result);
                        break;
                    default:
                        return NotFound();
                }
                _instants[InstantKey.Oracle] = DateTime.Now;
                return Ok(result);
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
                    BusinessUnity = entity.UnidadNegocio.Descripcion,
                    LegacySystems = logResult,
                    Instants = GetInstants(),
                    Error = error,
                    Body = entity
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        [Route(RouteAction.Read)]
        public IHttpActionResult Read(CotizacionRequest cotizacionRequest)
        {
            try
            {
                #region UnidadNegocio
                UnidadNegocioKeys? unidadNegocioKeys = null;
                switch (cotizacionRequest.Region)
                {
                    case "PE":
                        unidadNegocioKeys = UnidadNegocioKeys.CondorTravel;
                        break;
                    case "CL":
                        unidadNegocioKeys = UnidadNegocioKeys.CondorTravel_CL;
                        break;
                    default:
                        break;
                }
                RepositoryByBusiness(unidadNegocioKeys);
                #endregion

                var cotizaciones = (IEnumerable<Cotizacion>)_cotizacionCTRepository.GetCotizacionCT(cotizacionRequest)[OutParameter.CursorCotizacion];
                return Ok(cotizaciones);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
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
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.CondorTravel:
                    _cotizacionCTRepository = new Cotizacion_CT_Repository(UnidadNegocioKeys.CondorTravel);
                    break;
                case UnidadNegocioKeys.CondorTravel_CL:
                    _cotizacionCTRepository = new Cotizacion_CT_Repository(UnidadNegocioKeys.CondorTravel_CL);
                    break;
                /*
                case UnidadNegocioKeys.DestinosMundiales:
                    _crmCollection.Add(UnidadNegocioKeys.DestinosMundiales, new Cotizacion_DM_Repository());
                    break;
                case UnidadNegocioKeys.CondorTravel:
                    _crmCollection.Add(UnidadNegocioKeys.CondorTravel, new Cotizacion_CT_Repository());
                    break;
                */
                default:
                    break;
            }
            return unidadNegocioKey; // Devuelve el mismo parámetro
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
