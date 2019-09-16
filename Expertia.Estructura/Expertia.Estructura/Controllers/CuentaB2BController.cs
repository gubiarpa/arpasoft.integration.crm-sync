using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.Condor;
using Expertia.Estructura.Repository.NuevoMundo;
using Expertia.Estructura.Utils;
using System;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    /// <summary>
    /// Mantenimiento para Contactos B2B
    /// </summary>
    [RoutePrefix(RoutePrefix.CuentaB2B)]
    public class CuentaB2BController : BaseController<CuentaB2B>
    {
        [Route(RouteAction.Create)]
        public override IHttpActionResult Create(CuentaB2B entity)
        {
            try
            {
                #region UnidadNegocio
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                _crmRepository = GetRepository(entity.UnidadNegocio.ID);
                #endregion

                var operationResult = _crmRepository.Create(entity);

                entity.WriteLogObject(_logFileManager, _clientFeatures);

                return Ok(new
                {
                    Result = new
                    {
                        Type = ResultType.Success,
                        CodError = operationResult["P_CODIGO_ERROR"],
                        MensajeError = operationResult["P_MENSAJE_ERROR"]
                    },
                    Entity = entity
                });
            }
            catch (Exception ex)
            {
                ex.WriteLogObject(_logFileManager, _clientFeatures, LogType.Fail);
                return InternalServerError(ex);
            }
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(CuentaB2B entity)
        {
            try
            {
                var operationResult = _crmRepository.Update(entity);
                entity.WriteLogObject(_logFileManager, _clientFeatures);
                return Ok(new
                {
                    Result = new
                    {
                        Type = ResultType.Success,
                        CodError = operationResult["P_CODIGO_ERROR"],
                        MensajeError = operationResult["P_MENSAJE_ERROR"]
                    },
                    Entity = entity
                });
            }
            catch (Exception ex)
            {
                ex.WriteLogObject(_logFileManager, _clientFeatures, LogType.Fail);
                return InternalServerError(ex);
            }
        }

        protected override ICrud<CuentaB2B> GetRepository(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.CondorTravel:
                    return new CuentaB2B_CT_Repository();
                case UnidadNegocioKeys.DestinosMundiales:
                    break;
                case UnidadNegocioKeys.NuevoMundo:
                    return new CuentaB2B_NM_Repository();
                case UnidadNegocioKeys.InterAgencias:
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}
