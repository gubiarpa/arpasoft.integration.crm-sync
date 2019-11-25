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
using System.Threading.Tasks;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.Oportunidad)]
    public class OportunidadController : BaseController<object>
    {
        #region Properties
        private IOportunidadRepository _oportunidadRepository;
        #endregion

        #region Constructor
        public OportunidadController()
        {
        }
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<Oportunidad> oportunidades = null;
            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// I. Consulta de Oportunidades
                oportunidades = (IEnumerable<Oportunidad>)_oportunidadRepository.GetOportunidades()[OutParameter.CursorOportunidad];
                if (oportunidades == null || oportunidades.ToList().Count.Equals(0)) return Ok();

                /// Obtiene Token para envío a Salesforce
                var token = RestBase.GetTokenByKey(SalesforceKeys.AuthServer, SalesforceKeys.AuthMethod, Method.POST);

                var oportunidadTasks = new List<Task>();
                foreach (var oportunidad in oportunidades)
                {
                    var task = new Task(() =>
                    {
                        /// II. Enviar Oportunidad a Salesforce
                        try
                        {
                            oportunidad.UnidadNegocio = unidadNegocio.Descripcion;
                            oportunidad.CodigoError = oportunidad.MensajeError = string.Empty;
                            var oportunidadSf = ToSalesfoceEntity(oportunidad);
                            var responseOportunidad = RestBase.ExecuteByKey(SalesforceKeys.CrmServer, SalesforceKeys.OportunidadMethod, Method.POST, oportunidadSf, true, token);
                            if (responseOportunidad.StatusCode.Equals(HttpStatusCode.OK))
                            {
                                JsonManager.LoadText(responseOportunidad.Content);
                                oportunidad.CodigoError = JsonManager.GetSetting(OutParameter.SF_CodigoError);
                                oportunidad.MensajeError = JsonManager.GetSetting(OutParameter.SF_MensajeError);
                                _oportunidadRepository.Update(oportunidad);
                            }
                        }
                        catch
                        {
                        }
                    });
                    task.Start();
                    oportunidadTasks.Add(task);
                }

                Task.WaitAll(oportunidadTasks.ToArray());
                return Ok(oportunidades);
            }
            catch (Exception ex)
            {
                oportunidades = null;
                throw ex;
            }
            finally
            {
                (new
                {
                    LegacySystems = oportunidades
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
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
                    _oportunidadRepository = new Oportunidad_IA_Repository();
                    break;
            }
            return unidadNegocioKey;
        }
    }
}
