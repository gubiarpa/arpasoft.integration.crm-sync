using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.NuevoMundo;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.NuevoMundo;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.RestManager.RestParse;
using Expertia.Estructura.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Expertia.Estructura.Controllers
{
    /// CRMEC003_5 : Registro de Información de pago
    [RoutePrefix(RoutePrefix.InformacionPagoNM)]
    public class InformacionPagoNMController : BaseController
    {
        #region Properties
        private IInformacionPagoNMRepository _informacionPagoNMRepository;
        protected override ControllerName _controllerName => ControllerName.InformacionPagoNM;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<InformacionPagoNM> informacionPagoNMs = null;
            List<RptaInformacionPagoSF> ListRptaInformacionPagoSF = null;
            List<InfoPagoNM> lInfoPagoNM = null;
            RptaInformacionPagoSF _rptaInformacionPagoSF = null;
            string error = string.Empty;
            object objEnvio = null;
            dynamic jsonResponse = null;
            try
            {
                var _unidadNegocio = RepositoryByBusiness(unidadNegocio.Descripcion.ToUnidadNegocio());
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// I. Consulta de Informacion Pago NM
                informacionPagoNMs = (IEnumerable<InformacionPagoNM>)(_informacionPagoNMRepository.GetInformacionPago(_unidadNegocio))[OutParameter.CursorInformacionPagoNM];
                if (informacionPagoNMs == null || informacionPagoNMs.ToList().Count.Equals(0)) return Ok(informacionPagoNMs);

                /// Obtiene Token para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                //Armando estructura de objetos.
                InfoPagoNM oInfoPagoNM;



                var results = (from p in informacionPagoNMs
                               group p.idOportunidad_SF by p.idOportunidad_SF into g
                               select new { idOportunidad_SF = g.Key }).ToList();


                string idOportunidad_SF = string.Empty;
                int index = -1;
                if (results != null)
                {
                    int id_sucursal = 0;
                    int codigoweb = 0;
                    int PaqueteId = 0;
                    lInfoPagoNM = new List<InfoPagoNM>();
                    foreach (var item in results)
                    {
                        oInfoPagoNM = new InfoPagoNM();

                        index = informacionPagoNMs.ToList().FindIndex(x => x.idOportunidad_SF == item.idOportunidad_SF);
                        oInfoPagoNM.idOportunidad_SF = item.idOportunidad_SF;

                        oInfoPagoNM.Identificador_NM = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).Identificador_NM;
                        oInfoPagoNM.IdInformacionPago_SF = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).IdInformacionPago_SF;

                        //Aqui Lista ListPago_Boleto_Servicios

                        oInfoPagoNM.ListPago_Boleto_Servicios = informacionPagoNMs.Where(x => x.idOportunidad_SF == item.idOportunidad_SF && !string.IsNullOrWhiteSpace(x.reservaID))
                            .Select(x => new PagoBoletoServicios
                            {
                                reservaID = x.reservaID,
                                tipoServicio = x.tipoServicio,
                                tipoPasajero = x.tipoPasajero,
                                totalBoleto = x.totalBoleto,
                                tarifaNeto = x.tarifaNeto,
                                impuestos = x.impuestos,
                                cargos = x.cargos,
                                descripcion = x.descripcion
                            }).ToList();


                        oInfoPagoNM.totalPagar = oInfoPagoNM.ListPago_Boleto_Servicios.Sum(x => x.totalBoleto);
                        oInfoPagoNM.montoDescuento = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).montodescuento;
                        oInfoPagoNM.textoDescuento = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).textodescuento;
                        oInfoPagoNM.promoWebCode = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).promowebcode;
                        oInfoPagoNM.totalFacturar = oInfoPagoNM.totalPagar - oInfoPagoNM.montoDescuento; 
                        oInfoPagoNM.feeAsumidoGeneralBoletos = informacionPagoNMs.Where(x => x.idOportunidad_SF == item.idOportunidad_SF).Sum(x=>x.feeAsumidoGeneralBoletos);
                        ////Aqui Lista ListPagosDesglose_Paquete


                        if (!string.IsNullOrWhiteSpace(informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).PaqueteId))
                        {
                            oInfoPagoNM.ListPagosDesglose_Paquete = new List<PagosDesglosePaquete>();
                            oInfoPagoNM.ListPagosDesglose_Paquete.Add(new PagosDesglosePaquete
                            {
                                precioTotalPorHabitacionPaq = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF && !string.IsNullOrWhiteSpace(x.PaqueteId)).precioTotalPorHabitacionPaq,
                                ListPorHabitacionPaq = informacionPagoNMs.Where(y => y.idOportunidad_SF == item.idOportunidad_SF && !string.IsNullOrWhiteSpace(y.PaqueteId))
                                                                                                 .Select(y => new PorHabitacion_Paq
                                                                                                 {
                                                                                                     numHabitacionPaquete = y.numHabitacionPaquete,
                                                                                                     tipoPasajeroPaq = y.tipoPasajeroPaq,
                                                                                                     cantidadPasajeroPaq = y.cantidadPasajeroPaq,
                                                                                                     monedaPaq = y.monedaPaq,
                                                                                                     precioUnitarioPaq = y.precioUnitarioPaq,
                                                                                                     totalUnitarioPaq = y.totalUnitarioPaq
                                                                                                 }).ToList()
                            });
                        }


                        oInfoPagoNM.precioTotalHabitacionesPaq = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).precioTotalHabitacionesPaq;
                        oInfoPagoNM.gastosAdministrativosPaq = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).gastosAdministrativosPaq;
                        oInfoPagoNM.tarjetaDeTurismo = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).tarjetaDeTurismo;
                        oInfoPagoNM.tarjetaDeAsistencia = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).tarjetaDeAsistencia;

                        ////Aqui Lista ListPagosServicio_Paquete =>GetListPagosServicio

                        if (!string.IsNullOrWhiteSpace(informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).PaqueteId))
                        {
                            int.TryParse(informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).PaqueteId, out PaqueteId);
                            int.TryParse(informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).Id_Sucursal, out id_sucursal);
                            int.TryParse(informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).Codigoweb, out codigoweb);
                            string paq_reserva_tipo = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).paq_reserva_tipo;
                            oInfoPagoNM.ListPagosServicio_Paquete = ((IEnumerable<PagosServicioPaquete>)(_informacionPagoNMRepository.GetListPagosServicio(codigoweb, PaqueteId, id_sucursal, paq_reserva_tipo))[OutParameter.CursorInformacionPagoNM]).ToList();

                        }

                        if (oInfoPagoNM.ListPagosServicio_Paquete != null && oInfoPagoNM.ListPagosServicio_Paquete.Count>0)
                        {
                            oInfoPagoNM.precioTotalActividadesPaq = oInfoPagoNM.ListPagosServicio_Paquete.Sum(x => x.precioServ);  
                        }
                        oInfoPagoNM.precioTotalPagarPaq = oInfoPagoNM.precioTotalHabitacionesPaq + oInfoPagoNM.gastosAdministrativosPaq + oInfoPagoNM.precioTotalActividadesPaq;
                        oInfoPagoNM.montoDescuentoPaq = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).montoDescuentoPaq;
                        oInfoPagoNM.totalFacturarPaq = oInfoPagoNM.precioTotalPagarPaq + oInfoPagoNM.montoDescuentoPaq;
                        oInfoPagoNM.textoDescuentoPaq = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).textoDescuentoPaq;
                     
                  
                        oInfoPagoNM.cantDiasSeg = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).cantDiasSeg;
                        oInfoPagoNM.precioUnitarioSeg = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).precioUnitarioSeg;
                        oInfoPagoNM.MontoSeg = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).MontoSeg;
                        oInfoPagoNM.DescuentoSeg = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).DescuentoSeg;
                        oInfoPagoNM.MontoReservaSeg = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).MontoReservaSeg;
                        oInfoPagoNM.accion_SF = informacionPagoNMs.First(x => x.idOportunidad_SF == item.idOportunidad_SF).accion_SF;
                        lInfoPagoNM.Add(oInfoPagoNM);
                    }
                }

                var informacionPagoNMSF = new List<object>();
                foreach (var itemlInfoPagoNM in lInfoPagoNM)
                    informacionPagoNMSF.Add(itemlInfoPagoNM.ToSalesforceEntity());

                try
                {
                    /// Envío de Informacion de Pago a Salesforce
                    ClearQuickLog("body_request.json", "InformacionPagoNM"); /// ♫ Trace
                    objEnvio = new { datos = informacionPagoNMSF };
                    QuickLog(objEnvio, "body_request.json", "InformacionPagoNM"); /// ♫ Trace


                    var responseInformacionPagoNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.InformacionPagoNMMethod, Method.POST, objEnvio, true, token);
                    if (responseInformacionPagoNM.StatusCode.Equals(HttpStatusCode.OK))
                    {

                        jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseInformacionPagoNM.Content);
                        QuickLog(jsonResponse, "body_response.json", "InformacionPagoNM"); /// ♫ Trace
                        string[] codigoServicio = null;
                        string oportunidad = string.Empty;
                        ListRptaInformacionPagoSF = new List<RptaInformacionPagoSF>();

                        foreach (var jsResponse in jsonResponse["respuestas"])
                        {
                            oportunidad = jsResponse[OutParameter.SF_IdOportunidad3];

                            codigoServicio = jsResponse[OutParameter.SF_IdentificadorNM].Split('-');
                            _rptaInformacionPagoSF = new RptaInformacionPagoSF();

                            _rptaInformacionPagoSF.CodigoError = jsResponse[OutParameter.SF_Codigo];
                            _rptaInformacionPagoSF.MensajeError = jsResponse[OutParameter.SF_Mensaje];
                            _rptaInformacionPagoSF.idOportunidad_SF = jsResponse[OutParameter.SF_IdOportunidad3];
                            _rptaInformacionPagoSF.IdInfoPago_SF = jsResponse[OutParameter.SF_IdInformacionPago2];
                            if (_rptaInformacionPagoSF.CodigoError != "ER" && codigoServicio.Count() > 0)
                            {
                                _rptaInformacionPagoSF.CodigoServicio_NM = codigoServicio[0].ToString();
                                _rptaInformacionPagoSF.Identificador_NM = codigoServicio[1].ToString();
                            }

                            var updOperation = _informacionPagoNMRepository.Update(_rptaInformacionPagoSF);

                            if (Convert.IsDBNull(updOperation[OutParameter.IdActualizados]) == true || updOperation[OutParameter.IdActualizados].ToString().ToLower().Contains("null") || Convert.ToInt32(updOperation[OutParameter.IdActualizados].ToString()) <= 0)
                            {
                                error = error + "Error en el Proceso de Actualizacion - No Actualizo Ningun Registro. Identificador NM : " + _rptaInformacionPagoSF.Identificador_NM.ToString() + "||||";
                                ListRptaInformacionPagoSF.Add(_rptaInformacionPagoSF);

                            }
                        }
                    }
                    else
                    {
                        jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseInformacionPagoNM.Content);
                        QuickLog(jsonResponse, "body_response.json", "InformacionPagoNM"); /// ♫ Trace
                        error = responseInformacionPagoNM.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    error = error + "Error en el Proceso de Actualizacion - Response SalesForce : " + ex.Message + "||||";
                    ListRptaInformacionPagoSF.Add(_rptaInformacionPagoSF);
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                error = error + " / " + ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                if (objEnvio != null || jsonResponse != null || ListRptaInformacionPagoSF != null || string.IsNullOrEmpty(error) == false)
                {
                    (new
                    {
                        Request = objEnvio,
                        Response = jsonResponse,
                        Rpta_NoUpdate_Fail = ListRptaInformacionPagoSF,
                        Exception = error
                        //LegacySystems = lInfoPagoNM
                    }).TryWriteLogObject(_logFileManager, _clientFeatures);
                }                    
            }
        }
        #endregion

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _informacionPagoNMRepository = new InformacionPagoNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }

    }
}
