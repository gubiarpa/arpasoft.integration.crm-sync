using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using System;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    /// <summary>
    /// Mantenimiento para Contactos B2C
    /// </summary>
    [RoutePrefix(RoutePrefix.CuentaB2C)]
    public class CuentaB2CController : BaseController<CuentaB2C>
    {
        /// <summary>
        /// Crea un
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Route(RouteAction.Create)]
        public override IHttpActionResult Create(CuentaB2C entity)
        {
            try
            {
                WriteAllFieldsLog(entity);
                return Ok();
            }
            catch (Exception ex)
            {
                _logFileManager.WriteLine(LogType.Fail, ex.Message);
                return InternalServerError(ex);
            }            
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(CuentaB2C entity)
        {
            try
            {
                WriteAllFieldsLog(entity);
                return Ok();
            }
            catch (Exception ex)
            {
                _logFileManager.WriteLine(LogType.Fail, ex.Message);
                return InternalServerError(ex);
            }
        }

        protected override void WriteAllFieldsLog(CuentaB2C entity)
        {
            #region Cuenta
            WriteFieldLog("IdSalesForce", entity.IdSalesForce);
            WriteFieldLog("TipoPersona", entity.TipoPersona);
            WriteFieldLog("FechaNacimOrAniv", entity.FechaNacimOrAniv);
            WriteFieldLog("LogoFoto", entity.LogoFoto);
            WriteFieldLog("Documentos", entity.Documentos);
            //WriteFieldLog("Direcciones", entity.Direcciones);
            //WriteFieldLog("Pais", entity.Pais);
            //WriteFieldLog("Departamento", entity.Departamento);
            //WriteFieldLog("Ciudad", entity.Ciudad);
            //WriteFieldLog("Distrito", entity.Distrito);
            WriteFieldLog("Telefonos", entity.Telefonos);
            WriteFieldLog("Sitios", entity.Sitios);
            WriteFieldLog("Correos", entity.Correos);
            WriteFieldLog("Participantes", entity.Participantes);
            WriteFieldLog("InteresesProdActiv", entity.InteresesProdActiv);
            WriteFieldLog("OrigenCuenta", entity.PuntoContacto);
            WriteFieldLog("RecibirInformacion", entity.RecibirInformacion);
            WriteFieldLog("CanalRecibirInfo", entity.CanalesRecibirInfo);
            WriteFieldLog("RegionMercadoBranch", entity.Branches);
            WriteFieldLog("IdiomasComunicCliente", entity.IdiomasComunicCliente);
            WriteFieldLog("NivelImportancia", entity.NivelImportancia);
            WriteFieldLog("FechaIniRelacionComercial", entity.FechaIniRelacionComercial);
            WriteFieldLog("Comentarios", entity.Comentarios);
            WriteFieldLog("TipoCuenta", entity.TipoCuenta);
            WriteFieldLog("Estado", entity.Estado);
            WriteFieldLog("EsVIP", entity.PaisProcedencia);
            #endregion

            #region CuentaB2C
            WriteFieldLog("Nombre", entity.Nombre);
            WriteFieldLog("ApePaterno", entity.ApePaterno);
            WriteFieldLog("ApeMaterno", entity.ApeMaterno);
            WriteFieldLog("EstadoCivil", entity.EstadoCivil);
            WriteFieldLog("Genero", entity.Genero);
            WriteFieldLog("Nacionalidad", entity.Nacionalidad);
            WriteFieldLog("GradoEstudios", entity.GradoEstudios);
            WriteFieldLog("Profesion", entity.Profesion);
            WriteFieldLog("PreferenciasGenerales", entity.PreferenciasGenerales);
            WriteFieldLog("ConsideracionesSalud", entity.ConsideracionesSalud);
            WriteFieldLog("TipoViaje", entity.TipoViaje);
            WriteFieldLog("CategoriaViaje", entity.CategoriaViaje);
            WriteFieldLog("TipoAcompañante", entity.TipoAcompanante);
            #endregion
        }
    }
}
