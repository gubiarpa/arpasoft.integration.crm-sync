using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Condor;
using Expertia.Estructura.Repository.DestinosMundiales;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.Utils;
using System;
using System.Threading;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.Contacto)]
    public class ContactoController : BaseController<Contacto>
    {
        #region PublicMethods
        [Route(RouteAction.Create)]
        public override IHttpActionResult Create(Contacto entity)
        {
            object result = null, error = null;
            try
            {
                var codigoError = DbResponseCode.ContactoYaExiste;
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        CreateOrUpdate(UnidadNegocioKeys.CondorTravel, entity, codigoError);
                        if (CuentaAsociadaNoExiste(UnidadNegocioKeys.CondorTravel))
                        {
                            Thread.Sleep(_delayTimeRetry);
                            CreateOrUpdate(UnidadNegocioKeys.CondorTravel, entity, codigoError);
                        }
                        result = new
                        {
                            Result = new
                            {
                                CondorTravel = new
                                {
                                    Codes = GetErrorResult(UnidadNegocioKeys.CondorTravel),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdCuenta].ToString(),
                                    IdContacto = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdContacto].ToString()
                                }
                            }
                        };
                        break;
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.InterAgencias:
                        CreateOrUpdate(UnidadNegocioKeys.DestinosMundiales, entity, codigoError);
                        if (CuentaAsociadaNoExiste(UnidadNegocioKeys.DestinosMundiales))
                        {
                            Thread.Sleep(_delayTimeRetry);
                            CreateOrUpdate(UnidadNegocioKeys.DestinosMundiales, entity, codigoError);
                        }
                        CreateOrUpdate(UnidadNegocioKeys.InterAgencias, entity, codigoError);
                        if (CuentaAsociadaNoExiste(UnidadNegocioKeys.InterAgencias))
                        {
                            Thread.Sleep(_delayTimeRetry);
                            CreateOrUpdate(UnidadNegocioKeys.InterAgencias, entity, codigoError);
                        }
                        result = new
                        {
                            Result = new
                            {
                                DestinosMundiales = new
                                {
                                    Codes = GetErrorResult(UnidadNegocioKeys.DestinosMundiales),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString(),
                                    IdContacto = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdContacto].ToString()
                                },
                                InterAgencias = new
                                {
                                    Codes = GetErrorResult(UnidadNegocioKeys.InterAgencias),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.IdCuenta].ToString(),
                                    IdContacto = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.IdContacto].ToString()
                                }
                            }
                        };
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
                    LegacySystems = result,
                    Error = error,
                    Body = entity
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(Contacto entity)
        {
            object result = null, error = null;
            try
            {
                var codigoError = DbResponseCode.ContactoNoExiste;
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        UpdateOrCreate(UnidadNegocioKeys.CondorTravel, entity, codigoError);
                        if (CuentaAsociadaNoExiste(UnidadNegocioKeys.CondorTravel))
                        {
                            Thread.Sleep(_delayTimeRetry);
                            UpdateOrCreate(UnidadNegocioKeys.CondorTravel, entity, codigoError);
                        }
                        result = new
                        {
                            Result = new
                            {
                                CondorTravel = new
                                {
                                    Codes = GetErrorResult(UnidadNegocioKeys.CondorTravel),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdCuenta].ToString(),
                                    IdContacto = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdContacto].ToString()
                                }
                            }
                        };
                        break;
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.InterAgencias:
                        UpdateOrCreate(UnidadNegocioKeys.DestinosMundiales, entity, codigoError);
                        if (CuentaAsociadaNoExiste(UnidadNegocioKeys.DestinosMundiales))
                        {
                            Thread.Sleep(_delayTimeRetry);
                            UpdateOrCreate(UnidadNegocioKeys.DestinosMundiales, entity, codigoError);
                        }
                        UpdateOrCreate(UnidadNegocioKeys.InterAgencias, entity, codigoError);
                        if (CuentaAsociadaNoExiste(UnidadNegocioKeys.InterAgencias))
                        {
                            Thread.Sleep(_delayTimeRetry);
                            UpdateOrCreate(UnidadNegocioKeys.InterAgencias, entity, codigoError);
                        }
                        result = new
                        {
                            Result = new
                            {
                                DestinosMundiales = new
                                {
                                    Codes = GetErrorResult(UnidadNegocioKeys.DestinosMundiales),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString(),
                                    IdContacto = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdContacto].ToString()
                                },
                                InterAgencias = new
                                {
                                    Codes = GetErrorResult(UnidadNegocioKeys.InterAgencias),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.IdCuenta].ToString(),
                                    IdContacto = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.IdContacto].ToString()
                                }
                            }
                        };
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
                    LegacySystems = result,
                    Error = error,
                    Body = entity
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliar
        private bool CuentaAsociadaNoExiste(UnidadNegocioKeys? unidadNegocio)
        {
            return (_operCollection[unidadNegocio][OutParameter.CodigoError].ToString().Equals(DbResponseCode.CuentaNoExiste));
        }

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.CondorTravel:
                    _crmCollection.Add(UnidadNegocioKeys.CondorTravel, new Contacto_CT_Repository());
                    break;
                case UnidadNegocioKeys.DestinosMundiales:
                case UnidadNegocioKeys.InterAgencias:
                    _crmCollection.Add(UnidadNegocioKeys.DestinosMundiales, new Contacto_DM_Repository());
                    _crmCollection.Add(UnidadNegocioKeys.InterAgencias, new Contacto_IA_Repository());
                    break;
                default:
                    break;
            }
            return unidadNegocioKey;
        }
        #endregion
    }
}
