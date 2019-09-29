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
        public override IHttpActionResult Create(CuentaB2B entity)
        {
            object result = null, error = null;
            try
            {
                var codigoError = DbResponseCode.CuentaYaExiste;
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        CreateOrUpdate(UnidadNegocioKeys.CondorTravel, entity, codigoError);
                        result = new
                        {
                            Result = new
                            {
                                CondorTravel = new
                                {
                                    Codes = GetErrorResult(UnidadNegocioKeys.CondorTravel),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdCuenta].ToString()
                                }
                            }
                        };
                        break;
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.InterAgencias:
                        CreateOrUpdate(UnidadNegocioKeys.DestinosMundiales, entity, codigoError);
                        CreateOrUpdate(UnidadNegocioKeys.InterAgencias, entity, codigoError);
                        result = new
                        {
                            Result = new
                            {
                                DestinosMundiales = new
                                {
                                    Codes = GetErrorResult(UnidadNegocioKeys.DestinosMundiales),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString()
                                },
                                InterAgencias = new
                                {
                                    Codes = GetErrorResult(UnidadNegocioKeys.InterAgencias),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.IdCuenta].ToString()
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
        public override IHttpActionResult Update(CuentaB2B entity)
        {
            object result = null, error = null;
            try
            {
                var codigoError = DbResponseCode.CuentaNoExiste;
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        UpdateOrCreate(UnidadNegocioKeys.CondorTravel, entity, codigoError);
                        result = new
                        {
                            Result = new
                            {
                                CondorTravel = new
                                {
                                    Codes = GetErrorResult(UnidadNegocioKeys.CondorTravel),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdCuenta].ToString()
                                }
                            }
                        };
                        break;
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.InterAgencias:
                        UpdateOrCreate(UnidadNegocioKeys.DestinosMundiales, entity, codigoError);
                        UpdateOrCreate(UnidadNegocioKeys.InterAgencias, entity, codigoError);
                        result = new
                        {
                            Result = new
                            {
                                DestinosMundiales = new
                                {
                                    Codes = GetErrorResult(UnidadNegocioKeys.DestinosMundiales),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString()
                                },
                                InterAgencias = new
                                {
                                    Codes = GetErrorResult(UnidadNegocioKeys.InterAgencias),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.IdCuenta].ToString()
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
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.CondorTravel:
                    _crmCollection.Add(UnidadNegocioKeys.CondorTravel, new CuentaB2B_CT_Repository());
                    break;
                case UnidadNegocioKeys.DestinosMundiales:
                case UnidadNegocioKeys.InterAgencias:
                    _crmCollection.Add(UnidadNegocioKeys.DestinosMundiales, new CuentaB2B_DM_Repository());
                    _crmCollection.Add(UnidadNegocioKeys.InterAgencias, new CuentaB2B_IA_Repository());
                    break;
                default:
                    break;
            }
            return unidadNegocioKey;
        }
        #endregion
    }
}
