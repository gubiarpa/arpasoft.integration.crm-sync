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
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Expertia.Estructura.Controllers
{
    /// <summary>
    /// Entidad Exclusiva para Condor Travel e Interagencias
    /// </summary>
    [RoutePrefix(RoutePrefix.File)]
    public class FileController : BaseController<object>
    {
        #region Properties
        private IFileRepository _fileRepository;
        #endregion

        #region Constructor
        public FileController()
        {
        }
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            /// ♫ Listas de Respuesta
            IEnumerable<AgenciaPnr> agenciasPnrs = null;
            IEnumerable<File> files; IEnumerable<Boleto> boletos;
            string exceptionMsg = string.Empty;

            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// I. Consulta de PNRs a PTA
                agenciasPnrs = (IEnumerable<AgenciaPnr>)_fileRepository.GetNewAgenciaPnr()[OutParameter.CursorAgenciaPnr];
                if (agenciasPnrs == null || agenciasPnrs.ToList().Count.Equals(0)) return Ok(agenciasPnrs);

                /// Obtiene Token para envío a Salesforce
                var token = RestBase.GetTokenByKey(SalesforceKeys.AuthServer, SalesforceKeys.AuthMethod);

                /// Procesamos los Files y Boletos de cada Agencia PNR
                foreach (var agenciaPnr in agenciasPnrs)
                {
                    /// II. Completar PNR en Salesforce
                    agenciaPnr.UnidadNegocio = unidadNegocio.Descripcion;

                    agenciaPnr.CodigoError = agenciaPnr.MensajeError = string.Empty;
                    if (string.IsNullOrEmpty(agenciaPnr.IdOportunidad))
                        SendToSalesforce(agenciaPnr, token);

                    /// III. Envío de Files, Boletos a Salesforce
                    if (!string.IsNullOrEmpty(agenciaPnr.IdOportunidad))
                    {
                        /// 1. Consulta Lista de Files, Lista de Boletos
                        var _operFileAndBoleto = _fileRepository.GetFileAndBoleto(agenciaPnr);

                        /// 2. Envío de Files
                        agenciaPnr.Files = files = (IEnumerable<File>)_operFileAndBoleto[OutParameter.CursorFile];
                        foreach (var file in files)
                        {
                            try
                            {
                                /// a. Envío a Salesforce
                                SendToSalesforce(file, token);
                                if (EvaluateRetry(file)) SendToSalesforce(file, token);

                                /// b. Resultado a BD
                                var operFileUpdate = _fileRepository.UpdateFile(file);
                                file.Actualizados = int.Parse(operFileUpdate[OutParameter.IdActualizados].ToString());
                            }
                            catch
                            {
                            }
                        }

                        /// 3. Envío de Boletos
                        agenciaPnr.Boletos = boletos = (IEnumerable<Boleto>)_operFileAndBoleto[OutParameter.CursorBoleto];
                        foreach (var boleto in boletos)
                        {
                            try
                            {
                                /// a. Envío a Salesforce
                                SendToSalesforce(boleto, token);
                                if (EvaluateRetry(boleto)) SendToSalesforce(boleto, token);

                                /// b. Resultado a BD
                                var operBoletoUpdate = _fileRepository.UpdateBoleto(boleto);
                                boleto.Actualizados = int.Parse(operBoletoUpdate[OutParameter.IdActualizados].ToString());
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                return Ok(new { AgenciasPnr = agenciasPnrs });
            }
            catch (Exception ex)
            {
                exceptionMsg = ex.Message;
                agenciasPnrs = null;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    UnidadNegocio = unidadNegocio.Descripcion,
                    Exception = exceptionMsg,
                    LegacySystems = agenciasPnrs
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Send
        private void SendToSalesforce(AgenciaPnr agenciaPnr, string token)
        {
            try
            {
                var agenciaPnrSf = ToSalesforceEntity(agenciaPnr);
                var responsePnr = RestBase.ExecuteByKey(SalesforceKeys.CrmServer, SalesforceKeys.PnrMethod, Method.POST, agenciaPnrSf, true, token);
                if (responsePnr.StatusCode.Equals(HttpStatusCode.OK))
                {
                    dynamic jsonResponse = new JavaScriptSerializer().DeserializeObject(responsePnr.Content);
                    try
                    {
                        agenciaPnr.CodigoError = jsonResponse[OutParameter.SF_CodigoError];
                        agenciaPnr.MensajeError = jsonResponse[OutParameter.SF_MensajeError];
                        agenciaPnr.IdOportunidad = jsonResponse[OutParameter.SF_IdOportunidad];
                        agenciaPnr.LastMethod = ConfigAccess.GetValueInAppSettings(SalesforceKeys.PnrMethod);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        private void SendToSalesforce(File file, string token)
        {
            try
            {
                var fileSf = ToSalesforceEntity(file);
                var responseFile = RestBase.ExecuteByKey(SalesforceKeys.CrmServer, SalesforceKeys.FileMethod, Method.POST, fileSf, true, token);
                if (responseFile.StatusCode.Equals(HttpStatusCode.OK))
                {
                    dynamic jsonResponse = new JavaScriptSerializer().DeserializeObject(responseFile.Content);
                    file.CodigoError = jsonResponse[OutParameter.SF_CodigoError];
                    file.MensajeError = jsonResponse[OutParameter.SF_MensajeError];
                    file.LastMethod = ConfigAccess.GetValueInAppSettings(SalesforceKeys.FileMethod);
                }
            }
            catch
            {
            }
        }

        private void SendToSalesforce(Boleto boleto, string token)
        {
            var boletoSf = ToSalesforceEntity(boleto);
            var responseBoleto = RestBase.ExecuteByKey(SalesforceKeys.CrmServer, SalesforceKeys.BoletoMethod, Method.POST, boletoSf, true, token);
            if (responseBoleto.StatusCode.Equals(HttpStatusCode.OK))
            {
                JsonManager.LoadText(responseBoleto.Content);
                boleto.CodigoError = JsonManager.GetSetting(OutParameter.SF_CodigoError);
                boleto.MensajeError = JsonManager.GetSetting(OutParameter.SF_MensajeError);
                boleto.LastMethod = ConfigAccess.GetValueInAppSettings(SalesforceKeys.BoletoMethod);
            }
        }
        #endregion

        #region Retry
        private bool EvaluateRetry(File file)
        {
            var retry = true;

            if (file.Accion.Equals(SalesforceKeys.CreateAction) && file.CodigoError.Equals(SfResponseCode.FileYaExiste))
                file.Accion = SalesforceKeys.UpdateAction;
            else if (file.Accion.Equals(SalesforceKeys.UpdateAction) && file.CodigoError.Equals(SfResponseCode.FileNoExiste))
                file.Accion = SalesforceKeys.CreateAction;
            else
                retry = false;

            return retry;
        }

        private bool EvaluateRetry(Boleto boleto)
        {
            var retry = true;

            if (boleto.Accion.Equals(SalesforceKeys.CreateAction) && boleto.CodigoError.Equals(SfResponseCode.BoletoYaExiste))
                boleto.Accion = SalesforceKeys.UpdateAction;
            else if (boleto.Accion.Equals(SalesforceKeys.UpdateAction) && boleto.CodigoError.Equals(SfResponseCode.BoletoNoExiste))
                boleto.Accion = SalesforceKeys.CreateAction;
            else
                retry = false;

            return retry;
        }
        #endregion

        #region Auxiliar
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.Interagencias:
                    _fileRepository = new File_IA_Repository();
                    break;
            }
            return unidadNegocioKey;
        }
        #endregion

        #region SalesforceEntities
        private object ToSalesforceEntity(AgenciaPnr agenciaPnr)
        {
            try
            {
                return new
                {
                    info = new
                    {
                        DkAgencia = agenciaPnr.DkAgencia.ToString(),
                        Pnr = agenciaPnr.PNR
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private object ToSalesforceEntity(File file)
        {
            try
            {
                return new
                {
                    info = new
                    {
                        idOportunidad = file.IdOportunidad,
                        accion = file.Accion,
                        file = file.NumeroFile.ToString(),
                        estadoFile = file.EstadoFile,
                        unidadNegocio = file.UnidadNegocio,
                        sucursal = file.Sucursal,
                        nombreGrupo = file.NombreGrupo,
                        counter = file.Counter,
                        fechaApertura = file.FechaApertura.ToString("dd/MM/yyyy"),
                        fechaInicio = file.FechaInicio.ToString("dd/MM/yyyy"),
                        fechaFin = file.FechaFin.ToString("dd/MM/yyyy"),
                        cliente = file.Cliente,
                        subcodigo = file.Subcodigo,
                        contacto = file.Contacto,
                        condicionPago = file.CondicionPago,
                        numPasajeros = file.NumPasajeros,
                        costo = file.Costo,
                        venta = file.Venta,
                        comisionAgencia = file.ComisionAgencia
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private object ToSalesforceEntity(Boleto boleto)
        {
            try
            {
                return new
                {
                    info = new
                    {
                        idOportunidad = boleto.IdOportunidad,
                        accion = boleto.Accion,
                        file = boleto.NumeroFile,
                        sucursal = boleto.Sucursal,
                        boleto = boleto.NumeroBoleto,
                        estadoBoleto = boleto.EstadoBoleto,
                        pnr = boleto.Pnr,
                        tipoBoleto = boleto.TipoBoleto,
                        lineaAerea = boleto.LineaAerea,
                        ruta = boleto.Ruta,
                        tipoRuta = boleto.TipoRuta,
                        ciudadOrigen = boleto.CiudadOrigen,
                        ciudadDestino = boleto.CiudadDestino,
                        puntoDeEmision = boleto.PuntoEmision,
                        nombrePasajero = boleto.NombrePasajero,
                        infanteConAdulto = boleto.InfanteAdulto.Equals("true"),
                        fechaEmision = boleto.FechaEmision.ToString("dd/MM/yyyy"),
                        emitidoCanje = boleto.EmitidoCanje,
                        agenteQuienEmite = boleto.AgenteQuienEmite,
                        montoTarifa = boleto.MontoTarifa,
                        montoComision = boleto.MontoComision,
                        montoTotal = boleto.MontoTotal,
                        formaPago = boleto.FormaPago,
                        reembolsado = boleto.Reembolsado.Equals("true"),
                        pagoConTarjeta = boleto.PagoConTarjeta.Equals("true"),
                        tieneWaiver = boleto.TieneWaiver == null ? false : boleto.TieneWaiver.Equals("true"),
                        tipoWaiver = boleto.TipoWaiver,
                        montoWaiver = boleto.MontoWaiver,
                        pagado = boleto.Pagado,
                        comprobante = boleto.Comprobante
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}