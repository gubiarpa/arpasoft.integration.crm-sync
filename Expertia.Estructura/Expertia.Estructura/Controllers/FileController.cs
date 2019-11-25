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
    /// <summary>
    /// Entidad Exclusiva para Condor Travel e Interagencias
    /// </summary>
    [RoutePrefix(RoutePrefix.File)]
    public class FileController : BaseController<object>
    {
        #region Properties
        private IFileRepository _fileRepository;
        //private IDictionary<UnidadNegocioKeys?, IFileRepository> _fileCollection;
        #endregion

        #region Constructor
        public FileController()
        {
            //_fileCollection = new Dictionary<UnidadNegocioKeys?, IFileRepository>();
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

                /// ♫ Listas de Respuesta
                IEnumerable<AgenciaPnr> agenciasPnrs = null;
                IEnumerable<File> files = null;
                IEnumerable<Boleto> boletos = null;

                /// I. Consulta de PNRs a PTA
                agenciasPnrs = (IEnumerable<AgenciaPnr>)_fileRepository.GetNewAgenciaPnr()[OutParameter.CursorAgenciaPnr];
                if (agenciasPnrs == null || agenciasPnrs.ToList().Count.Equals(0)) return Ok();

                /// Obtiene Token para envío a Salesforce
                var token = RestBase.GetTokenByKey(SalesforceKeys.AuthServer, SalesforceKeys.AuthMethod);

                /// Intentando enviar en paralelo
                var agenciasPnrTasks = new List<Task>();
                foreach (var agenciaPnr in agenciasPnrs)
                {
                    var agenciaPnrTask = new Task(() =>
                    {
                        /// II. Completar PNR en Salesforce
                        try
                        {
                            agenciaPnr.UnidadNegocio = unidadNegocio.Descripcion;
                            agenciaPnr.CodigoError = agenciaPnr.MensajeError = string.Empty;
                            var agenciaPnrSf = ToSalesforceEntity(agenciaPnr);
                            var responsePnr = RestBase.ExecuteByKey(SalesforceKeys.CrmServer, SalesforceKeys.PnrMethod, Method.POST, agenciaPnrSf, true, token);
                            if (responsePnr.StatusCode.Equals(HttpStatusCode.OK))
                            {
                                JsonManager.LoadText(responsePnr.Content);
                                agenciaPnr.CodigoError = JsonManager.GetSetting(OutParameter.SF_CodigoError);
                                agenciaPnr.MensajeError = JsonManager.GetSetting(OutParameter.SF_MensajeError);
                                agenciaPnr.IdCuenta = JsonManager.GetSetting(OutParameter.SF_IdCuenta);
                                agenciaPnr.IdOportunidad = JsonManager.GetSetting(OutParameter.SF_IdOportunidad);
                            }
                        }
                        catch
                        {
                        }

                        /// III. Envío de Files, Boletos a Salesforce
                        try
                        {
                            if (!string.IsNullOrEmpty(agenciaPnr.IdOportunidad))
                            {
                                _operCollection[_unidadNegocio] = _fileRepository.GetFileAndBoleto(agenciaPnr);
                                var filesOrBoletosTasks = new List<Task>();

                                /// a. Envío de Files
                                files = (IEnumerable<File>)_operCollection[_unidadNegocio][OutParameter.CursorFile];
                                foreach (var file in files)
                                {
                                    var fileTask = new Task(() =>
                                    {
                                        try
                                        {
                                            var fileSf = ToSalesforceEntity(file);
                                            var responseFile = RestBase.ExecuteByKey(SalesforceKeys.CrmServer, SalesforceKeys.FileMethod, Method.POST, fileSf, true, token);
                                            if (responseFile.StatusCode.Equals(HttpStatusCode.OK))
                                            {
                                                JsonManager.LoadText(responseFile.Content);
                                                file.CodigoError = JsonManager.GetSetting(OutParameter.SF_CodigoError);
                                                file.MensajeError = JsonManager.GetSetting(OutParameter.SF_MensajeError);
                                                _fileRepository.UpdateFile(file);
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    });
                                    fileTask.Start();
                                    filesOrBoletosTasks.Add(fileTask);
                                }

                                /// b. Envío de Boletos
                                boletos = (IEnumerable<Boleto>)_operCollection[_unidadNegocio][OutParameter.CursorBoleto];
                                foreach (var boleto in boletos)
                                {
                                    var boletoTask = new Task(() =>
                                    {
                                        try
                                        {
                                            var boletoSf = ToSalesforceEntity(boleto);
                                            var responseBoleto = RestBase.ExecuteByKey(SalesforceKeys.CrmServer, SalesforceKeys.BoletoMethod, Method.POST, boletoSf, true, token);
                                            if (responseBoleto.StatusCode.Equals(HttpStatusCode.OK))
                                            {
                                                JsonManager.LoadText(responseBoleto.Content);
                                                boleto.CodigoError = JsonManager.GetSetting(OutParameter.SF_CodigoError);
                                                boleto.MensajeError = JsonManager.GetSetting(OutParameter.SF_MensajeError);
                                                _fileRepository.UpdateBoleto(boleto);
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    });
                                    boletoTask.Start();
                                    filesOrBoletosTasks.Add(boletoTask);
                                }

                                /// c. Espera los Files, Boletos
                                Task.WaitAll(filesOrBoletosTasks.ToArray());
                            }
                        }
                        catch
                        {
                        }
                    });
                    agenciaPnrTask.Start();
                    agenciasPnrTasks.Add(agenciaPnrTask);
                }
                Task.WaitAll(agenciasPnrTasks.ToArray());
                return Ok(new
                {
                    AgenciasPnr = agenciasPnrs
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                //files.TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliar
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.Interagencias:
                    _fileRepository = new File_IA_Repository();
                    //_fileCollection.Add(UnidadNegocioKeys.Interagencias, new File_IA_Repository());
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
                    Id_Oportunidad_Sf = file.IdOportunidad,
                    Resumen = new
                    {
                        Accion = file.Accion,
                        File = file.NumeroFile.ToString(),
                        Objeto = "FILE", // Hardcode
                        Estado_File = file.EstadoFile,
                        Unidad_Negocio = file.UnidadNegocio,
                        Sucursal = file.Sucursal,
                        Nombre_Grupo = file.NombreGrupo,
                        Counter = file.Counter,
                        Fecha_Apertura = file.FechaApertura,
                        Fecha_Inicio = file.FechaInicio,
                        Fecha_Fin = file.FechaFin,
                        Cliente = file.Cliente,
                        Subcodigo = file.Subcodigo,
                        Contacto = file.Contacto,
                        Condicion_Pago = file.CondicionPago,
                        Num_Pasajeros = file.NumPasajeros,
                        Costo = file.Costo,
                        Venta = file.Venta,
                        Comision_Agencia = file.ComisionAgencia
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
                    Id_Oportunidad = boleto.IdOportunidad,
                    Accion = boleto.Accion,
                    File = boleto.NumeroFile,
                    Sucursal = boleto.Sucursal,
                    Boleto = boleto.NumeroBoleto,
                    Estado_Boleto = boleto.EstadoBoleto,
                    Pnr = boleto.Pnr,
                    Tipo_Boleto = boleto.TipoBoleto,
                    Linea_Aerea = boleto.LineaAerea,
                    Ruta = boleto.Ruta,
                    Tipo_Ruta = boleto.TipoRuta,
                    Ciudad_Origen = boleto.CiudadOrigen,
                    Ciudad_Destino = boleto.CiudadDestino,
                    Punto_De_Emision = boleto.PuntoEmision,
                    Nombre_Pasajero = boleto.NombrePasajero,
                    Infante_Con_Adulto = boleto.InfanteAdulto,
                    Fecha_Emision = boleto.FechaEmision,
                    Emitido_Canje = boleto.EmitidoCanje,
                    Agente_Quien_Emite = boleto.AgenteQuienEmite,
                    Monto_Tarifa = boleto.MontoTarifa,
                    Monto_Comision = boleto.MontoComision,
                    Monto_Total = boleto.MontoTotal,
                    Forma_Pago = boleto.FormaPago,
                    Reembolsado = boleto.Reembolsado,
                    Pago_Con_Tarjeta = boleto.PagoConTarjeta,
                    Tiene_Waiver = boleto.TieneWaiver,
                    Tipo_Waiver = boleto.TipoWaiver,
                    Monto_Waiver = boleto.MontoWaiver,
                    Pagado = boleto.Pagado,
                    Comprobante = boleto.Comprobante
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