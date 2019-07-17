using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

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
                entity.IdiomasComunicCliente.Add(new IdiomaComunicCliente() { ID = "Inglés" });
                entity.IdiomasComunicCliente.Add(new IdiomaComunicCliente() { ID = "Español" });
                entity.ID = (new Random()).Next(0, 1000);
                _logFileManager.WriteLine(LogType.Info, entity.IdSalesForce);
            }
            catch (Exception ex)
            {
                _logFileManager.WriteLine(LogType.Fail, ex.Message);
                throw;
            }
            
            //var json 
            return Ok(new { ID = entity.ID });
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(CuentaB2B entity)
        {
            return Ok();
        }

        #region Log
        protected override void WriteAllFieldsLog(CuentaB2B entity)
        {
            #region Cuenta
            WriteFieldLog("ID", entity.ID);
            WriteFieldLog("IdSalesForce", entity.IdSalesForce);
            WriteFieldLog("TipoPersona", entity.TipoPersona);
            WriteFieldLog("FechaNacimOrAniv", entity.FechaNacimOrAniv);
            WriteFieldLog("LogoFoto", entity.LogoFoto);
            WriteFieldLog("Documentos", entity.Documentos);
            WriteFieldLog("Direcciones", entity.Direcciones);
            WriteFieldLog("Pais", entity.Pais);
            WriteFieldLog("Departamento", entity.Departamento);
            WriteFieldLog("Ciudad", entity.Ciudad);
            WriteFieldLog("Distrito", entity.Distrito);
            WriteFieldLog("Telefonos", entity.Telefonos);
            WriteFieldLog("Sitios", entity.Sitios);
            WriteFieldLog("Correos", entity.Correos);
            WriteFieldLog("EmpleadoOrEjecutivoResponsable", entity.EmpleadoOrEjecutivoResponsable);
            WriteFieldLog("SupervisorKam", entity.SupervisorKam);
            WriteFieldLog("Gerente", entity.Gerente);
            WriteFieldLog("UnidadNegocio", entity.UnidadNegocio);
            WriteFieldLog("GrupoColabEjecRegionBranch", entity.GrupoColabEjecRegionBranch);
            WriteFieldLog("FlagPrincipal", entity.FlagPrincipal);
            WriteFieldLog("InteresesProdActiv", entity.InteresesProdActiv);
            WriteFieldLog("TipoArea", entity.TipoArea);
            WriteFieldLog("OrigenCuenta", entity.OrigenCuenta);
            WriteFieldLog("RecibirInformacion", entity.RecibirInformacion);
            WriteFieldLog("CanalRecibirInfo", entity.CanalRecibirInfo);
            WriteFieldLog("RegionMercadoBranch", entity.RegionMercadoBranch);
            WriteFieldLog("IdiomasComunicCliente", entity.IdiomasComunicCliente);
            WriteFieldLog("NivelImportancia", entity.NivelImportancia);
            WriteFieldLog("FechaIniRelacionComercial", entity.FechaIniRelacionComercial);
            WriteFieldLog("Comentarios", entity.Comentarios);
            WriteFieldLog("TipoCuenta", entity.TipoCuenta);
            WriteFieldLog("Estado", entity.Estado);
            WriteFieldLog("PresupEstimadoVenta", entity.PresupEstimadoVenta);
            WriteFieldLog("EsPotencial", entity.EsPotencial);
            WriteFieldLog("EsVIP", entity.EsVIP);
            #endregion

            #region CuentaB2B
            WriteFieldLog("RazonSocial", entity.RazonSocial);
            WriteFieldLog("Alias", entity.Alias);
            WriteFieldLog("CondicionPago", entity.CondicionPago);
            WriteFieldLog("TipoMonedaDeLineaCredito", entity.TipoMonedaDeLineaCredito);
            WriteFieldLog("MontoLineaCredito", entity.MontoLineaCredito);
            WriteFieldLog("FechaMaximaPagoLDC", entity.FechaMaximaPagoLDC);
            #endregion
        }
        #endregion
    }
}
