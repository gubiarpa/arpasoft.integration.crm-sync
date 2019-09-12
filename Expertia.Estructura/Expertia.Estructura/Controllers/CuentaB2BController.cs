using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.Condor;
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
        private ICrud<CuentaB2B> _crmRepository;

        [Route(RouteAction.Create)]
        public override IHttpActionResult Create(CuentaB2B entity)
        {
            try
            {
                #region UnidadNegocio
                // 1. Conversión
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);

                // 2. Selección
                switch (entity.UnidadNegocio.ID)
                {
                    case UnidadNegocioKeys.CondorTravel:
                        _crmRepository = new CuentaB2B_RbRepository();
                        break;
                    case UnidadNegocioKeys.DestinosMundiales:
                        break;
                    case UnidadNegocioKeys.NuevoMundo:
                        break;
                    case UnidadNegocioKeys.InterAgencias:
                        break;
                    default:
                        break;
                }
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
    }
}
