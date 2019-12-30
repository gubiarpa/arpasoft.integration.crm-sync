using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.AppWebs;
using Expertia.Estructura.Repository.Condor;
using Expertia.Estructura.Repository.DestinosMundiales;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.Contacto)]
    public class ContactoController : BaseController<Contacto>
    {
        #region Properties
        protected IDictionary<UnidadNegocioKeys?, bool> _operNotAssociated;
        #endregion

        #region Constructor
        public ContactoController()
        {
            _operNotAssociated = new Dictionary<UnidadNegocioKeys?, bool>();
        }
        #endregion

        #region PublicMethods
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(Contacto entity)
        {
            object error = null, logResult = null;
            try
            {
                var codigoError = DbResponseCode.ContactoYaExiste;
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                object result;
                _instants[InstantKey.Salesforce] = DateTime.Now;
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        foreach (var unidadNegocio in new List<UnidadNegocioKeys?> {
                                UnidadNegocioKeys.CondorTravel,
                                UnidadNegocioKeys.CondorTravel_CL,
                                UnidadNegocioKeys.CondorTravel_EC,
                                UnidadNegocioKeys.CondorTravel_BR
                            })
                        {
                            CreateOrUpdate(unidadNegocio, entity, codigoError);
                            if (CuentaAsociadaNoExiste(unidadNegocio)) CreateOrUpdate(unidadNegocio, entity, codigoError, _delayTimeRetry);
                        }

                        LoadResults(UnidadNegocioKeys.CondorTravel, out logResult, out result);
                        break;
                    /// case UnidadNegocioKeys.DestinosMundiales: // Inhabilitado
                    case UnidadNegocioKeys.Interagencias:
                    case UnidadNegocioKeys.AppWebs:

                        foreach (var unidadNegocio in new List<UnidadNegocioKeys?> {
                                /*UnidadNegocioKeys.DestinosMundiales,*/
                                UnidadNegocioKeys.Interagencias,
                                UnidadNegocioKeys.AppWebs
                            })
                        {
                            CreateOrUpdate(unidadNegocio, entity, codigoError);
                            if (CuentaAsociadaNoExiste(unidadNegocio)) CreateOrUpdate(unidadNegocio, entity, codigoError, _delayTimeRetry);
                        }
                        LoadResults(UnidadNegocioKeys.DestinosMundiales, out logResult, out result);
                        break;

                    default:
                        return BadRequest(LogLineMessage.UnidadNegocioNotFound);
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
        public IHttpActionResult Update(Contacto entity)
        {
            object error = null, logResult = null;
            try
            {
                var codigoError = DbResponseCode.ContactoNoExiste;
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                object result;
                _instants[InstantKey.Salesforce] = DateTime.Now;
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:

                        foreach (var unidadNegocio in new List<UnidadNegocioKeys?> {
                                UnidadNegocioKeys.CondorTravel,
                                UnidadNegocioKeys.CondorTravel_CL,
                                UnidadNegocioKeys.CondorTravel_EC,
                                UnidadNegocioKeys.CondorTravel_BR
                            })
                        {
                            UpdateOrCreate(unidadNegocio, entity, codigoError);
                            if (CuentaAsociadaNoExiste(unidadNegocio)) UpdateOrCreate(unidadNegocio, entity, codigoError);
                        }

                        LoadResults(UnidadNegocioKeys.CondorTravel, out logResult, out result);
                        break;

                    /// case UnidadNegocioKeys.DestinosMundiales: // Inhabilitado
                    case UnidadNegocioKeys.Interagencias:
                    case UnidadNegocioKeys.AppWebs:

                        foreach (var unidadNegocio in new List<UnidadNegocioKeys?> {
                                /*UnidadNegocioKeys.DestinosMundiales,*/
                                UnidadNegocioKeys.Interagencias,
                                UnidadNegocioKeys.AppWebs
                            })
                        {
                            UpdateOrCreate(unidadNegocio, entity, codigoError);
                            if (CuentaAsociadaNoExiste(unidadNegocio)) UpdateOrCreate(unidadNegocio, entity, codigoError);
                        }

                        LoadResults(UnidadNegocioKeys.AppWebs, out logResult, out result);
                        break;

                    default:
                        return BadRequest(LogLineMessage.UnidadNegocioNotFound);
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
        private bool CuentaAsociadaNoExiste(UnidadNegocioKeys? unidadNegocio)
        {
            return (_operNotAssociated[unidadNegocio] =
                _operCollection[unidadNegocio][OutParameter.CodigoError].ToString().Equals(DbResponseCode.CuentaNoExiste));
        }

        private void LoadResults(UnidadNegocioKeys? unidadNegocio, out object logResult, out object result)
        {
            switch (unidadNegocio)
            {
                #region CondorTravel
                case UnidadNegocioKeys.CondorTravel:
                    #region Log
                    logResult = new
                    {
                        Codes = GetErrorResult(UnidadNegocioKeys.CondorTravel),
                        Retry = _operRetry[UnidadNegocioKeys.CondorTravel],
                        NotAssociated = _operNotAssociated[UnidadNegocioKeys.CondorTravel],
                        IdCuenta = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdCuenta].ToString(),
                        IdContacto = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdContacto].ToString()
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
                                IdCuenta = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdCuenta].ToString(),
                                IdContacto = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdContacto].ToString()
                            }
                        }
                    };
                    #endregion
                    break;
                #endregion

                #region CondorTravelCL
                case UnidadNegocioKeys.CondorTravel_CL:
                    #region Log
                    logResult = new
                    {
                        Codes = GetErrorResult(UnidadNegocioKeys.CondorTravel_CL),
                        Retry = _operRetry[UnidadNegocioKeys.CondorTravel_CL],
                        NotAssociated = _operNotAssociated[UnidadNegocioKeys.CondorTravel_CL],
                        IdCuenta = _operCollection[UnidadNegocioKeys.CondorTravel_CL][OutParameter.IdCuenta].ToString(),
                        IdContacto = _operCollection[UnidadNegocioKeys.CondorTravel_CL][OutParameter.IdContacto].ToString()
                    };
                    #endregion
                    #region Client
                    result = new
                    {
                        Result = new
                        {
                            CondorTravelCL = new
                            {
                                CodigoError = _operCollection[UnidadNegocioKeys.CondorTravel_CL][OutParameter.CodigoError].ToString(),
                                MensajeError = _operCollection[UnidadNegocioKeys.CondorTravel_CL][OutParameter.MensajeError].ToString(),
                                IdCuenta = _operCollection[UnidadNegocioKeys.CondorTravel_CL][OutParameter.IdCuenta].ToString(),
                                IdContacto = _operCollection[UnidadNegocioKeys.CondorTravel_CL][OutParameter.IdContacto].ToString()
                            }
                        }
                    };
                    #endregion
                    break;
                #endregion

                #region DestinosMundiales, Interagencias, AppWebs o TrustWeb
                case UnidadNegocioKeys.DestinosMundiales:
                case UnidadNegocioKeys.Interagencias:
                case UnidadNegocioKeys.AppWebs:
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
                                NotAssociated = _operNotAssociated[UnidadNegocioKeys.DestinosMundiales],
                                IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString(),
                                IdContacto = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdContacto].ToString()
                            },
                            */
                            InterAgencias = new
                            {
                                Codes = GetErrorResult(UnidadNegocioKeys.Interagencias),
                                Retry = _operRetry[UnidadNegocioKeys.Interagencias],
                                NotAssociated = _operNotAssociated[UnidadNegocioKeys.Interagencias],
                                IdCuenta = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.IdCuenta].ToString(),
                                IdContacto = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.IdContacto].ToString()
                            },
                            AppWebs = new
                            {
                                Codes = GetErrorResult(UnidadNegocioKeys.AppWebs),
                                Retry = _operRetry[UnidadNegocioKeys.AppWebs],
                                NotAssociated = _operNotAssociated[UnidadNegocioKeys.AppWebs],
                                IdCuenta = _operCollection[UnidadNegocioKeys.AppWebs][OutParameter.IdCuenta].ToString(),
                                IdContacto = _operCollection[UnidadNegocioKeys.AppWebs][OutParameter.IdContacto].ToString()
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
                                IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString(),
                                IdContacto = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdContacto].ToString()
                            },
                            */
                            InterAgencias = new
                            {
                                CodigoError = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.CodigoError].ToString(),
                                MensajeError = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.MensajeError].ToString(),
                                IdCuenta = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.IdCuenta].ToString(),
                                IdContacto = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.IdContacto].ToString()
                            },
                            AppWebs = new
                            {
                                CodigoError = _operCollection[UnidadNegocioKeys.AppWebs][OutParameter.CodigoError].ToString(),
                                MensajeError = _operCollection[UnidadNegocioKeys.AppWebs][OutParameter.MensajeError].ToString(),
                                IdCuenta = _operCollection[UnidadNegocioKeys.AppWebs][OutParameter.IdCuenta].ToString(),
                                IdContacto = _operCollection[UnidadNegocioKeys.AppWebs][OutParameter.IdContacto].ToString()
                            }
                        }
                    };
                    #endregion
                    break;
                #endregion

                #region Default
                default:
                    logResult = null; result = null;
                    break;
                #endregion
            }
        }

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.CondorTravel:
                    foreach (var unidadNegocio in new List<UnidadNegocioKeys?> {
                                UnidadNegocioKeys.CondorTravel,
                                UnidadNegocioKeys.CondorTravel_CL,
                                UnidadNegocioKeys.CondorTravel_EC,
                                UnidadNegocioKeys.CondorTravel_BR
                            })
                    {
                        _crmCollection.Add(unidadNegocio, new Contacto_CT_Repository());
                    }
                    break;
                /// case UnidadNegocioKeys.DestinosMundiales: // Inhabilitado
                case UnidadNegocioKeys.Interagencias:
                case UnidadNegocioKeys.AppWebs:
                    /*
                    _crmCollection.Add(UnidadNegocioKeys.DestinosMundiales, new Contacto_DM_Repository());
                    */
                    _crmCollection.Add(UnidadNegocioKeys.Interagencias, new Contacto_IA_Repository());
                    _crmCollection.Add(UnidadNegocioKeys.AppWebs, new Contacto_AW_Repository());
                    break;
                default:
                    break;
            }
            return unidadNegocioKey; // Devuelve el mismo parámetro
        }
        #endregion
    }
}
