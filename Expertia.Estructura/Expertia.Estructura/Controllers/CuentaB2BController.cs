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
                WriteEntityLog(entity);
                /// Aquí va la interacción con BD
                return Ok();
            }
            catch (Exception ex)
            {
                _logFileManager.WriteLine(LogType.Fail, ex.Message);
                throw;
            }
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(CuentaB2B entity)
        {
            try
            {
                WriteEntityLog(entity);
                /// Aquí va la interacción con BD
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Log
        protected override void WriteAllFieldsLog(CuentaB2B entity)
        {
            int i = 0;
            #region Cuenta
            WriteFieldLog("IdSalesForce", entity.IdSalesForce);
            WriteFieldLog("TipoPersona", entity.TipoPersona);
            WriteFieldLog("FechaNacimOrAniv", entity.FechaNacimOrAniv);
            WriteFieldLog("LogoFoto", entity.LogoFoto);
            #region WriteFieldLog("Documentos", entity.Documentos);
            i = 0;
            if (entity.Documentos != null)
                foreach (var documento in entity.Documentos)
                {
                    WriteFieldLog("Documento[{0}].Tipo", i, documento.Tipo);
                    WriteFieldLog("Documento[{0}].Numero", i, documento.Numero);
                    i++;
                }
            else
                WriteFieldLog("Documentos");
            #endregion
            #region WriteFieldLog("Direcciones", entity.Direcciones);
            i = 0;
            if (entity.Direcciones != null)
                foreach (var direccion in entity.Direcciones)
                {
                    WriteFieldLog("Direccion[{0}].Tipo", i, direccion.Tipo);
                    WriteFieldLog("Direccion[{0}].Descripción", i, direccion.Descripcion);
                    WriteFieldLog("Direccion[{0}].Pais", i, direccion.Pais);
                    WriteFieldLog("Direccion[{0}].Departamento", i, direccion.Departamento);
                    WriteFieldLog("Direccion[{0}].Ciudad", i, direccion.Ciudad);
                    WriteFieldLog("Direccion[{0}].Distrito", i, direccion.Distrito);
                    i++;
                }
            else
                WriteFieldLog("Direcciones");
            #endregion
            #region WriteFieldLog("Telefonos", entity.Telefonos);
            i = 0;
            if (entity.Telefonos != null)
                foreach (var telefono in entity.Telefonos)
                {
                    WriteFieldLog("Telefono[{0}].Tipo", i, telefono.Tipo);
                    WriteFieldLog("Telefono[{0}].Numero", i, telefono.Numero);
                    i++;
                }
            else
                WriteFieldLog("Telefonos");
            #endregion
            #region WriteFieldLog("Sitios", entity.Sitios);
            i = 0;
            if (entity.Sitios != null)
                foreach (var sitio in entity.Sitios)
                {
                    WriteFieldLog("Sitio[{0}].Tipo", i, sitio.Tipo);
                    WriteFieldLog("Sitio[{0}].Descripcion", i, sitio.Descripcion);
                    i++;
                }
            else
                WriteFieldLog("Sitios");
            #endregion
            #region WriteFieldLog("Correos", entity.Correos);
            i = 0;
            if (entity.Correos != null)
                foreach (var correo in entity.Correos)
                {
                    WriteFieldLog("Correo[{0}].Tipo", i, correo.Tipo);
                    WriteFieldLog("Correo[{0}].Descripcion", i, correo.Descripcion);
                    i++;
                }
            else
                WriteFieldLog("Correos");
            #endregion
            #region WriteFieldLog("Participantes", entity.Participantes);
            i = 0;
            if (entity.Participantes != null)
                foreach (var participante in entity.Participantes)
                {
                    WriteFieldLog("Participante[{0}].EmpleadoOrEjecutivoResponsable", i, participante.EmpleadoOrEjecutivoResponsable);
                    WriteFieldLog("Participante[{0}].SupervisorKam", i, participante.SupervisorKam);
                    WriteFieldLog("Participante[{0}].Gerente", i, participante.Gerente);
                    WriteFieldLog("Participante[{0}].UnidadNegocio", i, participante.UnidadNegocio);
                    WriteFieldLog("Participante[{0}].GrupoColabEjecRegionBranch", i, participante.GrupoColabEjecRegionBranch);
                    WriteFieldLog("Participante[{0}].FlagPrincipal", i, participante.FlagPrincipal);
                    i++;
                }
            else
                WriteFieldLog("Participantes");
            #endregion
            #region WriteFieldLog("InteresesProdActiv", entity.InteresesProdActiv);
            i = 0;
            if (entity.InteresesProdActiv != null)
                foreach (var interes in entity.InteresesProdActiv)
                {
                    WriteFieldLog("Interes[{0}].Tipo", i, interes.Tipo);
                    i++;
                }
            else
                WriteFieldLog("Intereses");
            #endregion
            WriteFieldLog("OrigenCuenta", entity.PuntoContacto);
            WriteFieldLog("RecibirInformacion", entity.RecibirInformacion);
            #region WriteFieldLog("CanalRecibirInfo", entity.CanalesRecibirInfo);
            i = 0;
            if (entity.CanalesRecibirInfo != null)
                foreach (var canalInfo in entity.CanalesRecibirInfo)
                {
                    WriteFieldLog("CanalInfo[{0}].Descriocion", i, canalInfo.Descripcion);
                    i++;
                }
            else
                WriteFieldLog("CanalesInfo");
            #endregion
            #region WriteFieldLog("RegionMercadoBranch", entity.Branches);
            i = 0;
            if (entity.Branches != null)
                foreach (var branch in entity.Branches)
                {
                    WriteFieldLog("RegionMercadoBranch[{0}].RegionMercadoBranch", i, branch.RegionMercadoBranch);
                    i++;
                }
            else
                WriteFieldLog("RegionMercadoBranch");
            #endregion
            #region WriteFieldLog("IdiomasComunicCliente", entity.IdiomasComunicCliente);
            i = 0;
            if (entity.IdiomasComunicCliente != null)
                foreach (var idioma in entity.IdiomasComunicCliente)
                {
                    WriteFieldLog("Idioma[{0}].ID", i, idioma.ID);
                    i++;
                }
            else
                WriteFieldLog("Idiomas");
            #endregion 
            WriteFieldLog("NivelImportancia", entity.NivelImportancia);
            WriteFieldLog("FechaIniRelacionComercial", entity.FechaIniRelacionComercial);
            WriteFieldLog("Comentarios", entity.Comentarios);
            WriteFieldLog("TipoCuenta", entity.TipoCuenta);
            WriteFieldLog("Estado", entity.Estado);
            WriteFieldLog("PaisProcedencia", entity.PaisProcedencia);
            #endregion

            #region CuentaB2B
            WriteFieldLog("RazonSocial", entity.RazonSocial);
            WriteFieldLog("Alias", entity.Alias);
            WriteFieldLog("CondicionPago", entity.CondicionPago);
            WriteFieldLog("TipoMonedaDeLineaCredito", entity.TipoMonedaDeLineaCredito);
            WriteFieldLog("MontoLineaCredito", entity.MontoLineaCredito);
            #endregion
        }
        #endregion
    }
}
