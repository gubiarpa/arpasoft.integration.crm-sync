using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.Oportunidad)]
    public class OportunidadController : BaseController<object>
    {
        #region Properties
        private IDictionary<UnidadNegocioKeys?, IOportunidadRepository> _oportunidadCollection;
        #endregion

        #region Constructor
        public OportunidadController()
        {
            _oportunidadCollection = new Dictionary<UnidadNegocioKeys?, IOportunidadRepository>();
        }
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// I. Consulta de Oportunidades
                var oportunidades = (IEnumerable<Oportunidad>)_oportunidadCollection[_unidadNegocio].GetOportunidades()[OutParameter.CursorOportunidad];
                if (oportunidades == null || oportunidades.ToList().Count.Equals(0)) return Ok();

                /// Configuraciones
                var authServer = ConfigAccess.GetValueInAppSettings(SalesforceKeys.AuthServer);
                var authMethodName = ConfigAccess.GetValueInAppSettings(SalesforceKeys.AuthMethod);
                var crmServer = ConfigAccess.GetValueInAppSettings(SalesforceKeys.CrmServer);
                var crmOportunidadMethod = ConfigAccess.GetValueInAppSettings(SalesforceKeys.OportunidadMethod);

                /// Obtiene Token para envío a Salesforce
                var token = RestBase.GetToken(authServer, authMethodName, Method.POST);

                foreach (var oportunidad in oportunidades)
                {
                    /// II. Enviar Oportunidad a Salesforce
                    try
                    {
                        oportunidad.UnidadNegocio = unidadNegocio.Descripcion;
                        oportunidad.CodigoError = oportunidad.MensajeError = string.Empty;
                        var oportunidadSf = ToSalesfoceEntity(oportunidad);
                        var responseOportunidad = RestBase.Execute(crmServer, crmOportunidadMethod, Method.POST, oportunidadSf, true, token);
                        JsonManager.LoadText(responseOportunidad.Content);
                        oportunidad.CodigoError = JsonManager.GetSetting(OutParameter.SF_CodigoError);
                        oportunidad.MensajeError = JsonManager.GetSetting(OutParameter.SF_MensajeError);
                    }
                    catch
                    {
                    }

                    /// III. Actualización en PTA
                    try
                    {
                        if (oportunidad.CodigoError.Equals(DbResponseCode.Success))
                        {
                            _oportunidadCollection[_unidadNegocio].Update(oportunidad);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }
        }
        #endregion

        #region Auxiliar
        #endregion

        #region SalesforceEntities
        private object ToSalesfoceEntity(Oportunidad oportunidad)
        {
            try
            {
                return new
                {
                    info = new
                    { 
                        IdOportunidad = oportunidad.IdOportunidad,
                        Accion = oportunidad.Accion,
                        Etapa = oportunidad.Etapa,
                        DkCuenta = oportunidad.DkCuenta,
                        UnidadNegocio = oportunidad.UnidadNegocio,
                        Sucursal = oportunidad.Sucursal,
                        PuntoVenta = oportunidad.PuntoVenta,
                        Subcodigo = oportunidad.Subcodigo,
                        FechaOportunidad = oportunidad.FechaOportunidad,
                        NombreOportunidad = oportunidad.NombreOportunidad,
                        OrigenOportunidad = oportunidad.OrigenOportunidad,
                        TipoProducto = oportunidad.TipoProducto,
                        RutaViaje = oportunidad.RutaViaje,
                        CiudadOrigen = oportunidad.CiudadOrigen,
                        CiudadDestino = oportunidad.CiudadDestino,
                        TipoRuta = oportunidad.TipoRuta,
                        NumPasajeros = oportunidad.NumPasajeros,
                        FechaInicioViaje1 = oportunidad.FechaInicioViaje1,
                        FechaFinViaje1 = oportunidad.FechaFinViaje1,
                        FechaInicioViaje2 = oportunidad.FechaInicioViaje2,
                        FechaFinViaje2 = oportunidad.FechaFinViaje2,
                        MontoEstimado = oportunidad.MontoEstimado,
                        MontoReal = oportunidad.MontoReal,
                        Pnr1 = oportunidad.Pnr1,
                        Pnr2 = oportunidad.Pnr2,
                        MotivoPerdida = oportunidad.MotivoPerdida
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.Interagencias:
                    _oportunidadCollection.Add(UnidadNegocioKeys.Interagencias, new Oportunidad_IA_Repository());
                    break;
            }
            return unidadNegocioKey;
        }
    }
}
