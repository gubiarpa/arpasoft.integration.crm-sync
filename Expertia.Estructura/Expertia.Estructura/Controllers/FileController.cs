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
using System.Threading.Tasks;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    /// <summary>
    /// Entidad Exclusiva para Condor Travel e Interagencias
    /// </summary>
    [RoutePrefix(RoutePrefix.File)]
    public class FileController : BaseController<AgenciaPnr>
    {
        #region Properties
        private IDictionary<UnidadNegocioKeys?, IFileRepository> _fileCollection;
        #endregion

        #region Constructor
        public FileController()
        {
            _fileCollection = new Dictionary<UnidadNegocioKeys?, IFileRepository>();
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

                /// I. Consulta de PNRs a PTA
                _operCollection[_unidadNegocio] = _fileCollection[_unidadNegocio].GetNewAgenciaPnr();
                var agenciasPnrs = (IEnumerable<AgenciaPnr>)_operCollection[_unidadNegocio][OutParameter.CursorAgenciaPnr];
                if (agenciasPnrs == null || agenciasPnrs.ToList().Count.Equals(0)) return Ok();

                /// Configuraciones
                var authServer = ConfigAccess.GetValueInAppSettings("AUTH_SERVER");
                var authMethodName = ConfigAccess.GetValueInAppSettings("AUTH_METHODNAME");
                var crmServer = ConfigAccess.GetValueInAppSettings("CRM_SERVER");
                var crmPnrMethod = ConfigAccess.GetValueInAppSettings("PNR_METHODNAME");

                /// Obtiene Token para envío a Salesforce
                var token = RestBase.GetToken(authServer, authMethodName, Method.POST);

                /// Intentando enviar en paralelo
                var tasks = new List<Task>();
                foreach (var agenciaPnr in agenciasPnrs)
                {
                    /*var task = new Task(() =>
                    {*/
                    /// II. Completar PNR en Salesforce
                    try
                    {
                        agenciaPnr.UnidadNegocio = unidadNegocio.Descripcion;
                        agenciaPnr.CodigoError = agenciaPnr.MensajeError = string.Empty;
                        var responsePnr = RestBase.Execute(crmServer, crmPnrMethod, Method.POST, ToSalesforceEntity(agenciaPnr), true, token);
                        JsonManager.LoadText(responsePnr.Content);
                        agenciaPnr.CodigoError = JsonManager.GetSetting("CODIGO_ERROR");
                        agenciaPnr.MensajeError = JsonManager.GetSetting("MENSAJE_ERROR");
                        agenciaPnr.DkAgencia = int.Parse(JsonManager.GetSetting("ID_CUENTA_SF"));
                        agenciaPnr.IdOportunidad = JsonManager.GetSetting("ID_OPORTUNIDAD_SF");
                    }
                    catch
                    {
                    }

                    /// III. Envío de Files, Boletos a Salesforce
                    try
                    {
                        if (!string.IsNullOrEmpty(agenciaPnr.IdOportunidad))
                        {
                            _operCollection[_unidadNegocio] = _fileCollection[_unidadNegocio].GetFileAndBoleto(agenciaPnr);

                            /// a. Envío de Files
                            var files = (IEnumerable<File>)_operCollection[_unidadNegocio][OutParameter.CursorFile];
                            var crmFileMethod = ConfigAccess.GetValueInAppSettings("FILE_METHODNAME");

                            foreach (var file in files)
                            {
                                try
                                {
                                    var responseFile = RestBase.Execute(crmServer, crmFileMethod, Method.POST, ToSalesforceEntity(file), true, token);
                                    JsonManager.LoadText(responseFile.Content);
                                    file.CodigoError = JsonManager.GetSetting("CODIGO_ERROR");
                                    file.MensajeError = JsonManager.GetSetting("MENSAJE_ERROR");
                                    _fileCollection[_unidadNegocio].UpdateFile(file);
                                }
                                catch
                                {
                                }
                            }

                            /// b. Envío de Boletos
                            var boletos = (IEnumerable<Boleto>)_operCollection[_unidadNegocio][OutParameter.CursorBoleto];
                            var crmBoletoMethod = ConfigAccess.GetValueInAppSettings("BOLETO_METHODNAME");
                            foreach (var boleto in boletos)
                            {
                                try
                                {
                                    var responseBoleto = RestBase.Execute(crmServer, crmBoletoMethod, Method.POST, ToSalesforceEntity(boleto), true, token);
                                    JsonManager.LoadText(responseBoleto.Content);
                                    boleto.CodigoError = JsonManager.GetSetting("CODIGO_ERROR");
                                    boleto.MensajeError = JsonManager.GetSetting("MENSAJE_ERROR");
                                    _fileCollection[_unidadNegocio].UpdateBoleto(boleto);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    /*});*/
                    /*task.Start();*/
                }
                //Task.WaitAll(tasks.ToArray());
                return Ok();
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
                case UnidadNegocioKeys.CondorTravel:
                    break;
                case UnidadNegocioKeys.Interagencias:
                    _fileCollection.Add(UnidadNegocioKeys.Interagencias, new File_IA_Repository());
                    break;
                default:
                    break;
            }
            return unidadNegocioKey;
        }

        private object ToSalesforceEntity(AgenciaPnr agenciaPnr)
        {
            try
            {
                return new
                {
                    Dk_Agencia = agenciaPnr.DkAgencia,
                    Pnr = agenciaPnr.PNR
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
                    Id_Oportunidad = file.IdOportunidad,
                    Accion = file.Accion,
                    File = file.NumeroFile,
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