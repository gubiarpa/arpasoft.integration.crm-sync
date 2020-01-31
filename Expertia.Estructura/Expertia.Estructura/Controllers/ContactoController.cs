using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.AppWebs;
using Expertia.Estructura.Repository.Condor;
using Expertia.Estructura.Repository.DestinosMundiales;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.Contacto)]
    public class ContactoController : BaseController<Contacto>
    {
        #region Properties
        protected IDictionary<UnidadNegocioKeys?, bool?> _operNotAssociated;
        private IDictionary<UnidadNegocioKeys?, string> _errorsValuesPairs;
        #endregion

        #region Constructor
        public ContactoController()
        {
            _operNotAssociated = new Dictionary<UnidadNegocioKeys?, bool?>();
            _errorsValuesPairs = new Dictionary<UnidadNegocioKeys?, string>();
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
                var unidadNegocioList = new List<UnidadNegocioKeys?>();
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        unidadNegocioList.Add(UnidadNegocioKeys.CondorTravel);
                        unidadNegocioList.Add(UnidadNegocioKeys.CondorTravel_CL);
                        unidadNegocioList.Add(UnidadNegocioKeys.CondorTravel_EC);
                        unidadNegocioList.Add(UnidadNegocioKeys.CondorTravel_BR);
                        CreateOrUpdate(unidadNegocioList, entity, codigoError);
                        LoadResults(UnidadNegocioKeys.CondorTravel, out logResult, out result);
                        break;
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.Interagencias:
                        // ▼ Adiciona U.Neg si está lleno el campo DkAgencia_XX
                        if (entity.DkAgencia_DM != null & entity.DkAgencia_DM != 0) unidadNegocioList.Add(UnidadNegocioKeys.DestinosMundiales);
                        if (entity.DkAgencia_IA != null & entity.DkAgencia_IA != 0) unidadNegocioList.Add(UnidadNegocioKeys.Interagencias);
                        CreateOrUpdate(unidadNegocioList, entity, codigoError);
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
                string error_PE = null, error_CL = null, error_EC = null, error_BR = null, error_DM = null, error_IA = null;
                try { error_PE = _errorsValuesPairs[UnidadNegocioKeys.CondorTravel]; } catch { error_PE = null; }
                try { error_CL = _errorsValuesPairs[UnidadNegocioKeys.CondorTravel]; } catch { error_CL = null; }
                try { error_EC = _errorsValuesPairs[UnidadNegocioKeys.CondorTravel]; } catch { error_EC = null; }
                try { error_BR = _errorsValuesPairs[UnidadNegocioKeys.CondorTravel]; } catch { error_BR = null; }
                try { error_DM = _errorsValuesPairs[UnidadNegocioKeys.CondorTravel]; } catch { error_DM = null; }
                try { error_IA = _errorsValuesPairs[UnidadNegocioKeys.CondorTravel]; } catch { error_IA = null; }
                (new
                {
                    BusinessUnity = entity.UnidadNegocio.Descripcion,
                    LegacySystems = logResult,
                    Instants = GetInstants(),
                    Error = new { RB = new { error_PE, error_CL, error_EC, error_BR }, PTA = new { error_DM, error_IA } },
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
                var codigoError = DbResponseCode.ContactoYaExiste;
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                object result;
                _instants[InstantKey.Salesforce] = DateTime.Now;
                var unidadNegocioList = new List<UnidadNegocioKeys?>();
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        unidadNegocioList.Add(UnidadNegocioKeys.CondorTravel);
                        unidadNegocioList.Add(UnidadNegocioKeys.CondorTravel_CL);
                        unidadNegocioList.Add(UnidadNegocioKeys.CondorTravel_EC);
                        unidadNegocioList.Add(UnidadNegocioKeys.CondorTravel_BR);
                        UpdateOrCreate(unidadNegocioList, entity, codigoError);
                        LoadResults(UnidadNegocioKeys.CondorTravel, out logResult, out result);
                        break;
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.Interagencias:
                        unidadNegocioList = new List<UnidadNegocioKeys?>(); // ▼ Adiciona U.Neg si está lleno el campo DkAgencia_XX
                        if (entity.DkAgencia_DM != null & entity.DkAgencia_DM != 0) unidadNegocioList.ToList().Add(UnidadNegocioKeys.DestinosMundiales);
                        if (entity.DkAgencia_IA != null & entity.DkAgencia_IA != 0) unidadNegocioList.ToList().Add(UnidadNegocioKeys.Interagencias);
                        UpdateOrCreate(unidadNegocioList, entity, codigoError);
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
                string error_PE = null, error_CL = null, error_EC = null, error_BR = null, error_DM = null, error_IA = null;
                try { error_PE = _errorsValuesPairs[UnidadNegocioKeys.CondorTravel]; } catch { error_PE = null; }
                try { error_CL = _errorsValuesPairs[UnidadNegocioKeys.CondorTravel]; } catch { error_CL = null; }
                try { error_EC = _errorsValuesPairs[UnidadNegocioKeys.CondorTravel]; } catch { error_EC = null; }
                try { error_BR = _errorsValuesPairs[UnidadNegocioKeys.CondorTravel]; } catch { error_BR = null; }
                try { error_DM = _errorsValuesPairs[UnidadNegocioKeys.CondorTravel]; } catch { error_DM = null; }
                try { error_IA = _errorsValuesPairs[UnidadNegocioKeys.CondorTravel]; } catch { error_IA = null; }
                (new
                {
                    BusinessUnity = entity.UnidadNegocio.Descripcion,
                    LegacySystems = logResult,
                    Instants = GetInstants(),
                    Error = new { RB = new { error_PE, error_CL, error_EC, error_BR }, PTA = new { error_DM, error_IA } },
                    Body = entity
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliar
        private void CreateOrUpdate(IEnumerable<UnidadNegocioKeys?> unidadNegocioList, Contacto entity, string codigoError)
        {
            foreach (var unidadNegocio in unidadNegocioList)
            {
                try
                {
                    CreateOrUpdate(unidadNegocio, entity, codigoError);
                    if (CuentaAsociadaNoExiste(unidadNegocio) ?? false) CreateOrUpdate(unidadNegocio, entity, codigoError, _delayTimeRetry);
                    _errorsValuesPairs[unidadNegocio] = null;
                }
                catch (Exception ex)
                {
                    _errorsValuesPairs[unidadNegocio] = ex.Message;
                }
            }
        }

        private void UpdateOrCreate(IEnumerable<UnidadNegocioKeys?> unidadNegocioList, Contacto entity, string codigoError)
        {
            foreach (var unidadNegocio in unidadNegocioList)
            {
                try
                {
                    UpdateOrCreate(unidadNegocio, entity, codigoError);
                    if (CuentaAsociadaNoExiste(unidadNegocio) ?? false) UpdateOrCreate(unidadNegocio, entity, codigoError, _delayTimeRetry);
                    _errorsValuesPairs[unidadNegocio] = null;
                }
                catch (Exception ex)
                {
                    _errorsValuesPairs[unidadNegocio] = ex.Message;
                }
            }
        }

        private bool? CuentaAsociadaNoExiste(UnidadNegocioKeys? unidadNegocio)
        {
            return (_operNotAssociated[unidadNegocio] =
                _operCollection[unidadNegocio][OutParameter.CodigoError].ToString().Equals(DbResponseCode.CuentaNoExiste));
        }

        private void LoadResults(UnidadNegocioKeys? unidadNegocio, out object logResult, out object result)
        {
            switch (unidadNegocio)
            {
                #region RB
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

                #region PTA
                case UnidadNegocioKeys.DestinosMundiales:
                case UnidadNegocioKeys.Interagencias:
                    #region Log
                    logResult = new
                    {
                        Result = new
                        {
                            DestinosMundiales = new
                            {
                                CodigoError = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.CodigoError].ToString() ,
                                MensajeError = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.MensajeError].ToString() ,
                                IdCuenta = _operCollection.ContainsKey(UnidadNegocioKeys.DestinosMundiales) ?
                                    _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString() : null,
                                IdContacto = _operCollection.ContainsKey(UnidadNegocioKeys.DestinosMundiales) ?
                                    _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdContacto].ToString() : null
                            },
                            InterAgencias = new
                            {
                                CodigoError = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.CodigoError].ToString() ,
                                MensajeError = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.MensajeError].ToString() ,
                                IdCuenta = _operCollection.ContainsKey(UnidadNegocioKeys.Interagencias) ?
                                    _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.IdCuenta].ToString() : null,
                                IdContacto = _operCollection.ContainsKey(UnidadNegocioKeys.Interagencias) ?
                                    _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.IdContacto].ToString() : null
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
                                IdCuenta = _operCollection.ContainsKey(UnidadNegocioKeys.DestinosMundiales) ?
                                    _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString() : null,
                                IdContacto = _operCollection.ContainsKey(UnidadNegocioKeys.DestinosMundiales) ?
                                    _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdContacto].ToString() : null
                            },
                            InterAgencias = new
                            {
                                CodigoError = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.CodigoError].ToString(),
                                MensajeError = _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.MensajeError].ToString(),
                                IdCuenta = _operCollection.ContainsKey(UnidadNegocioKeys.Interagencias) ?
                                    _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.IdCuenta].ToString() : null,
                                IdContacto = _operCollection.ContainsKey(UnidadNegocioKeys.Interagencias) ?
                                    _operCollection[UnidadNegocioKeys.Interagencias][OutParameter.IdContacto].ToString() : null
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
            IEnumerable<UnidadNegocioKeys?> unidadNegocioList;

            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.CondorTravel:
                    unidadNegocioList = new List<UnidadNegocioKeys?>()
                    {
                        UnidadNegocioKeys.CondorTravel,
                        UnidadNegocioKeys.CondorTravel_CL,
                        UnidadNegocioKeys.CondorTravel_EC,
                        UnidadNegocioKeys.CondorTravel_BR
                    };
                    foreach (var unidadNegocio in unidadNegocioList)
                        _crmCollection.Add(unidadNegocio, new Contacto_CT_Repository(unidadNegocio));
                    break;
                case UnidadNegocioKeys.DestinosMundiales:
                case UnidadNegocioKeys.Interagencias:
                    _crmCollection.Add(UnidadNegocioKeys.DestinosMundiales, new Contacto_DM_Repository(UnidadNegocioKeys.DestinosMundiales));
                    _crmCollection.Add(UnidadNegocioKeys.Interagencias, new Contacto_DM_Repository(UnidadNegocioKeys.Interagencias));
                    break;
                default:
                    unidadNegocioList = null;
                    break;
            }

            return unidadNegocioKey; // Devuelve el mismo parámetro
        }
        #endregion
    }
}
