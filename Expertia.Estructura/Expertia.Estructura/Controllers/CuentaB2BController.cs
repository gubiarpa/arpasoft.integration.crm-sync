﻿using Expertia.Estructura.Controllers.Base;
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
        private ICrud<CuentaB2B> _rbRepository;

        public CuentaB2BController() : base()
        {
            _rbRepository = new CuentaB2B_RbRepository();
        }

        [Route(RouteAction.Create)]
        public override IHttpActionResult Create(CuentaB2B entity)
        {
            try
            {
                var operationResult = _rbRepository.Create(entity);
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
                var operationResult = _rbRepository.Update(entity);
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
