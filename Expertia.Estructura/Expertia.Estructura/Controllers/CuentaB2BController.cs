using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Condor;
using Expertia.Estructura.Repository.DestinosMundiales;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.CuentaB2B)]
    public class CuentaB2BController : BaseController<CuentaB2B>
    {
        protected override ControllerName _controllerName => ControllerName.CuentaB2B;
        #region PublicMethods
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(CuentaB2B entity)
        {
            object error = null, logResult = null;
            string error_PE = null, error_CL = null, error_EC = null, error_BR = null; /// RB
            string error_DM = null, error_IA = null; /// PTA
            try
            {
                var codigoError = DbResponseCode.CuentaYaExiste;
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                object result;
                var tasks = new List<Task>();
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        tasks.Add(new Task(() => { /// Perú
                            try { CreateOrUpdate(UnidadNegocioKeys.CondorTravel, entity, codigoError, _delayTimeRetry); } catch (Exception ex) { error_PE = ex.Message; } }));
                        tasks.Add(new Task(() => { /// Chile
                            try { CreateOrUpdate(UnidadNegocioKeys.CondorTravel_CL, entity, codigoError, _delayTimeRetry); } catch (Exception ex) { error_CL = ex.Message; } }));
                        tasks.Add(new Task(() => { /// Ecuador
                            try { CreateOrUpdate(UnidadNegocioKeys.CondorTravel_EC, entity, codigoError, _delayTimeRetry); } catch (Exception ex) { error_EC = ex.Message; } }));
                        tasks.Add(new Task(() => { /// Brasil
                            try { CreateOrUpdate(UnidadNegocioKeys.CondorTravel_BR, entity, codigoError, _delayTimeRetry); } catch (Exception ex) { error_BR = ex.Message; } }));
                        tasks.ForEach(t => t.Start());
                        Task.WaitAll(tasks.ToArray());
                        LoadResults(UnidadNegocioKeys.CondorTravel, out logResult, out result);
                        break;
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.Interagencias:
                        tasks.Add(new Task(() => { /// Destinos Mundiales
                            if (!string.IsNullOrEmpty(entity.DkAgencia_DM) & (!entity.DkAgencia_DM.Equals("null"))) try { CreateOrUpdate(UnidadNegocioKeys.DestinosMundiales, entity, codigoError); } catch (Exception ex) { error_DM = ex.Message; } }));
                        tasks.Add(new Task(() => { /// Interagencias
                            if (!string.IsNullOrEmpty(entity.DkAgencia_IA) & (!entity.DkAgencia_IA.Equals("null"))) try { CreateOrUpdate(UnidadNegocioKeys.Interagencias, entity, codigoError); } catch (Exception ex) { error_IA = ex.Message; } }));
                        tasks.ForEach(t => t.Start());
                        Task.WaitAll(tasks.ToArray());
                        LoadResults(UnidadNegocioKeys.DestinosMundiales, out logResult, out result); // Se escoge DM o IA (es indistinto)
                        break;
                    default:
                        return NotFound();
                }
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
                    Errors = new
                    {
                        InApi = error,
                        OnDB = new
                        {
                            RB = new
                            {
                                error_PE,
                                error_CL,
                                error_EC,
                                error_BR
                            },
                            PTA = new
                            {
                                error_DM,
                                error_IA
                            }
                        }
                    },
                    Body = entity
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        [Route(RouteAction.Update)]
        public IHttpActionResult Update(CuentaB2B entity)
        {
            object error = null, logResult = null;
            string error_PE = null, error_CL = null, error_EC = null, error_BR = null; /// RB
            string error_DM = null, error_IA = null; /// PTA
            try
            {
                var codigoError = DbResponseCode.CuentaNoExiste;
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                object result;
                var tasks = new List<Task>();
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        tasks.Add(new Task(() => { /// Perú
                            try { UpdateOrCreate(UnidadNegocioKeys.CondorTravel, entity, codigoError, _delayTimeRetry); } catch (Exception ex) { error_PE = ex.Message; }
                        }));
                        tasks.Add(new Task(() => { /// Chile
                            try { UpdateOrCreate(UnidadNegocioKeys.CondorTravel_CL, entity, codigoError, _delayTimeRetry); } catch (Exception ex) { error_CL = ex.Message; }
                        }));
                        tasks.Add(new Task(() => { /// Ecuador
                            try { UpdateOrCreate(UnidadNegocioKeys.CondorTravel_EC, entity, codigoError, _delayTimeRetry); } catch (Exception ex) { error_EC = ex.Message; }
                        }));
                        tasks.Add(new Task(() => { /// Brasil
                            try { UpdateOrCreate(UnidadNegocioKeys.CondorTravel_BR, entity, codigoError, _delayTimeRetry); } catch (Exception ex) { error_BR = ex.Message; }
                        }));
                        tasks.ForEach(t => t.Start());
                        Task.WaitAll(tasks.ToArray());
                        LoadResults(UnidadNegocioKeys.CondorTravel, out logResult, out result);
                        break;
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.Interagencias:
                        tasks.Add(new Task(() => { /// Destinos Mundiales
                            if (!string.IsNullOrEmpty(entity.DkAgencia_DM) & (!entity.DkAgencia_DM.Equals("null"))) try { UpdateOrCreate(UnidadNegocioKeys.DestinosMundiales, entity, codigoError); } catch (Exception ex) { error_DM = ex.Message; }
                        }));
                        tasks.Add(new Task(() => { /// Interagencias
                            if (!string.IsNullOrEmpty(entity.DkAgencia_IA) & (!entity.DkAgencia_IA.Equals("null"))) try { UpdateOrCreate(UnidadNegocioKeys.Interagencias, entity, codigoError); } catch (Exception ex) { error_IA = ex.Message; }
                        }));
                        tasks.ForEach(t => t.Start());
                        Task.WaitAll(tasks.ToArray());
                        LoadResults(UnidadNegocioKeys.DestinosMundiales, out logResult, out result); // Se escoge DM o IA (es indistinto)
                        break;
                    default:
                        return NotFound();
                }
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
                    Errors = new
                    {
                        OnApi = error,
                        OnDB = new
                        {
                            RB = new
                            {
                                Err_Peru = error_PE,
                                Err_Chile = error_CL,
                                Err_Ecuador = error_EC,
                                Err_Brasil = error_BR
                            },
                            PTA = new
                            {
                                Err_DestinosMundiales = error_DM,
                                Err_Interagencias = error_IA
                            }
                        }
                    },
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

                    #region logResult_DM
                    object logResult_DM;
                    try
                    {
                        logResult_DM = new
                        {
                            Codes = GetErrorResult(UnidadNegocioKeys.DestinosMundiales),
                            Retry = _operRetry[UnidadNegocioKeys.DestinosMundiales],
                            IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString()
                        };
                    }
                    catch (Exception ex)
                    {
                        logResult_DM = new
                        {
                            Codes = ex.Message,
                            Retry = string.Empty,
                            IdCuenta = string.Empty
                        };
                    }
                    #endregion

                    #region logResult_IA
                    object logResult_IA;
                    try
                    {
                        logResult_IA = new
                        {
                            Codes = GetErrorResult(UnidadNegocioKeys.Interagencias),
                            Retry = _operRetry[UnidadNegocioKeys.Interagencias],
                            IdCuenta = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.IdCuenta].ToString()
                        };
                    }
                    catch (Exception ex)
                    {
                        logResult_IA = new
                        {
                            Codes = ex.Message,
                            Retry = string.Empty,
                            IdCuenta = string.Empty
                        };
                    }
                    #endregion
                    
                    logResult = new
                    {
                        Result = new
                        {
                            DestinosMundiales = logResult_DM,
                            InterAgencias = logResult_IA
                        }
                    };
                    #endregion
                    #region Client
                    
                    #region log_DM
                    object log_DM;
                    try
                    {
                        log_DM = new
                        {
                            CodigoError = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.CodigoError].ToString(),
                            MensajeError = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.MensajeError].ToString(),
                            IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString()
                        };
                    }
                    catch (Exception ex)
                    {
                        log_DM = new
                        {
                            CodigoError = ex.Message,
                            MensajeError = string.Empty,
                            IdCuenta = string.Empty
                        };
                    }
                    #endregion

                    #region log_IA
                    object log_IA;
                    try
                    {
                        log_IA = new
                        {
                            CodigoError = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.CodigoError].ToString(),
                            MensajeError = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.MensajeError].ToString(),
                            IdCuenta = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.IdCuenta].ToString()
                        };
                    }
                    catch (Exception ex)
                    {
                        log_IA = new
                        {
                            CodigoError = ex.Message,
                            MensajeError = string.Empty,
                            IdCuenta = string.Empty
                        };
                    }
                    #endregion

                    result = new
                    {
                        Result = new
                        {
                            DestinosMundiales = log_DM,
                            InterAgencias = log_IA
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
                    _crmCollection.Add(UnidadNegocioKeys.CondorTravel_CL, new CuentaB2B_CT_Repository(UnidadNegocioKeys.CondorTravel_CL));
                    _crmCollection.Add(UnidadNegocioKeys.CondorTravel_EC, new CuentaB2B_CT_Repository(UnidadNegocioKeys.CondorTravel_EC));
                    _crmCollection.Add(UnidadNegocioKeys.CondorTravel_BR, new CuentaB2B_CT_Repository(UnidadNegocioKeys.CondorTravel_BR));
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
