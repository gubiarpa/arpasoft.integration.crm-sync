using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Condor;
using Expertia.Estructura.Repository.DestinosMundiales;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.Utils;
using System;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.CuentaB2B)]
    public class CuentaB2BController : BaseController<CuentaB2B>
    {
        #region PublicMethods
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(CuentaB2B entity)
        {
            object error = null, logResult = null;
            try
            {
                var codigoError = DbResponseCode.CuentaYaExiste;
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                object result;
                _instants[InstantKey.Salesforce] = DateTime.Now;
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        CreateOrUpdate(UnidadNegocioKeys.CondorTravel, entity, codigoError);
                        LoadResults(UnidadNegocioKeys.CondorTravel, out logResult, out result);
                        break;
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.Interagencias:
                        /*
                        CreateOrUpdate(UnidadNegocioKeys.DestinosMundiales, entity, codigoError);
                        */
                        CreateOrUpdate(UnidadNegocioKeys.Interagencias, entity, codigoError);
                        LoadResults(UnidadNegocioKeys.DestinosMundiales, out logResult, out result); // Se escoge DM o IA (es indistinto)
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

        [Route(RouteAction.Update)]
        public IHttpActionResult Update(CuentaB2B entity)
        {
            object error = null, logResult = null;
            try
            {
                var codigoError = DbResponseCode.CuentaNoExiste;
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                object result;
                _instants[InstantKey.Salesforce] = DateTime.Now;
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        UpdateOrCreate(UnidadNegocioKeys.CondorTravel, entity, codigoError);
                        LoadResults(UnidadNegocioKeys.CondorTravel, out logResult, out result);
                        break;
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.Interagencias:
                        /*
                        UpdateOrCreate(UnidadNegocioKeys.DestinosMundiales, entity, codigoError);
                        */
                        UpdateOrCreate(UnidadNegocioKeys.Interagencias, entity, codigoError);
                        LoadResults(UnidadNegocioKeys.DestinosMundiales, out logResult, out result); // Se escoge DM o IA (es indistinto)
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
        #endregion

        #region Auxiliar
        private void LoadResults(UnidadNegocioKeys? unidadNegocio, out object logResult, out object result)
        {
            switch (unidadNegocio)
            {
                case UnidadNegocioKeys.CondorTravel:
                    #region Log
                    logResult = new
                    {
                        Codes = GetErrorResult(UnidadNegocioKeys.CondorTravel),
                        Retry = _operRetry[UnidadNegocioKeys.CondorTravel],
                        IdCuenta = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdCuenta].ToString()
                    };
                    #endregion
                    #region Client
                    result = new
                    {
                        Result = new
                        {
                            CondorTravel = new
                            {
                                CodigoError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.CodigoError].ToString(),
                                MensajeError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.MensajeError].ToString(),
                                IdCuenta = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdCuenta].ToString()
                            }
                        }
                    };
                    #endregion
                    break;
                case UnidadNegocioKeys.DestinosMundiales:
                case UnidadNegocioKeys.Interagencias:
                    #region Log
                    logResult = new
                    {
                        Result = new
                        {
                            /*
                            DestinosMundiales = new
                            {
                                Codes = GetErrorResult(UnidadNegocioKeys.DestinosMundiales),
                                Retry = _operRetry[UnidadNegocioKeys.DestinosMundiales],
                                IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString()
                            },
                            */
                            InterAgencias = new
                            {
                                Codes = GetErrorResult(UnidadNegocioKeys.Interagencias),
                                Retry = _operRetry[UnidadNegocioKeys.Interagencias],
                                IdCuenta = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.IdCuenta].ToString()
                            }
                        }
                    };
                    #endregion
                    #region Client
                    result = new
                    {
                        Result = new
                        {
                            /*
                            DestinosMundiales = new
                            {
                                CodigoError = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.CodigoError].ToString(),
                                MensajeError = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.MensajeError].ToString(),
                                IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString()
                            },
                            */
                            InterAgencias = new
                            {
                                CodigoError = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.CodigoError].ToString(),
                                MensajeError = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.MensajeError].ToString(),
                                IdCuenta = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.IdCuenta].ToString()
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
                    _crmCollection.Add(UnidadNegocioKeys.CondorTravel, new CuentaB2B_CT_Repository());
                    break;
                case UnidadNegocioKeys.DestinosMundiales:
                case UnidadNegocioKeys.Interagencias:
                    _crmCollection.Add(UnidadNegocioKeys.DestinosMundiales, new CuentaB2B_DM_Repository());
                    _crmCollection.Add(UnidadNegocioKeys.Interagencias, new CuentaB2B_IA_Repository());
                    break;
                default:
                    break;
            }
            return unidadNegocioKey;
        }
        #endregion
    }
}
