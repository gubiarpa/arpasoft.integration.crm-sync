using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.AppWebs;
using Expertia.Estructura.Repository.General;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Journeyou;
using Expertia.Estructura.Repository.NuevoMundo;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.RestManager.RestParse;
using Expertia.Estructura.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using Expertia.Estructura.ws_compra;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Script.Serialization;
//using PaymentLocationType = Expertia.Estructura.ws_compra.PaymentLocationType;
//using PaymentStepType = Expertia.Estructura.ws_compra.PaymentStepType;
//using PaymentInstructionType = Expertia.Estructura.ws_compra.PaymentInstructionType;

namespace Expertia.Estructura.Controllers
{
    /// Expertia_4 : Generar una nueva solicitud de pago Tarjeta C/D o actualizar la solicitud de pago
    [RoutePrefix(RoutePrefix.GenCodigoPagoNM)]
    public class GenCodigoPagoNMController : BaseController
    {
        #region Properties       
        private IPedidoRepository _pedidoRepository;
        private IDatosUsuario _datosUsuario;
        private GenCodigoPagoNMRepository _genCodigoPagoNMRepository;         
        protected override ControllerName _controllerName => ControllerName.GenCodigoPagoNM;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(DatosPedido_NM pedido_NM)
        {
            var pedido = pedido_NM.ToRetail(); // Conversión a Retail
            Pedido_AW_Repository _pedidoRepository = new Pedido_AW_Repository();
            Models.PedidoRS _resultpedido = new Models.PedidoRS();
            UsuarioLogin DtsUsuarioLogin = null;
            var errorPedido = string.Empty;
            int intIdFormaPago = 0;
            bool _return = false;
            try
            {
                /*Validaciones*/
                validacionPedido(ref pedido, ref _resultpedido, ref _return, ref DtsUsuarioLogin);
                //if (_return == true) return Ok(_resultpedido);

                RepositoryByBusiness(pedido.UnidadNegocio.ID);                

                /*Generamos el Pedido*/
                var operation = _pedidoRepository.CreateNM(pedido);
                if (operation[Operation.Result].ToString() == ResultType.Success.ToString() && Convert.ToInt32(operation[OutParameter.IdPedido].ToString()) > 0)
                {
                    _resultpedido.IdPedido = Convert.ToInt32(operation[OutParameter.IdPedido].ToString());

                    /*Solo debe quedar la validacion del pedido(Modificar al agregar un par de validaciones en el lugar correcto)*/
                    if (_resultpedido.IdPedido > 0 && string.IsNullOrEmpty(pedido_NM.IdOportunidad_SF) == false && string.IsNullOrEmpty(pedido_NM.IdSolicitudpago_SF) == false  && string.IsNullOrEmpty(pedido_NM.Accion_SF) == false && pedido_NM.Accion_SF.ToUpper().Trim() == "INSERT")
                    {                        
                        _genCodigoPagoNMRepository.RegistraSolicitudPagoSF(pedido_NM.IdSolicitudpago_SF, pedido_NM.IdOportunidad_SF, _resultpedido.IdPedido);
                    }
                    else if (_resultpedido.IdPedido > 0 && string.IsNullOrEmpty(pedido_NM.IdOportunidad_SF) == false && string.IsNullOrEmpty(pedido_NM.IdSolicitudpago_SF) == false && string.IsNullOrEmpty(pedido_NM.Accion_SF) == false && pedido_NM.Accion_SF.ToUpper().Trim() == "UPDATE")
                    {
                        _genCodigoPagoNMRepository.UpdateSolicitudPagoSF(pedido_NM.IdSolicitudpago_SF, pedido_NM.IdOportunidad_SF, _resultpedido.IdPedido, Estados_Oportunidad.ID_ST_REGI_NO_ENVIO);
                    }

                    ///Actualizamos el campo solicitado por CRM SalesForce asociado al numero de pedido
                    _pedidoRepository.Update_Pedido_SolicitudPago_SF(_resultpedido.IdPedido, pedido.IdCotVta, pedido.IdOportunidad_SF, pedido.IdSolicitudpago_SF);
                }
                else
                {
                    _resultpedido.CodigoError = "ER"; _resultpedido.MensajeError = "GP - Error al intentar generar el pedido";
                    return Ok(_resultpedido);
                }

                /// SEGUN SEA EL METODO DE PAGO REALIZA DIFERENTE PROCESO
                if (pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_TARJETA_VISA)
                {
                    intIdFormaPago = Constantes_FileRetail.INT_ID_FORMA_PAGO_SOLO_TARJETA;
                    pedido.CodePasarelaPago = Constantes_MetodoDePago.CODE_FPAGO_TARJETA_VISA;
                }
                else if (pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_TARJETA_MASTERCARD)
                {
                    intIdFormaPago = Constantes_FileRetail.INT_ID_FORMA_PAGO_SOLO_TARJETA;
                    pedido.CodePasarelaPago = Constantes_MetodoDePago.CODE_FPAGO_TARJETA_MASTERCARD_CA;
                }
                else if (pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_TARJETA_DINERS)
                {
                    intIdFormaPago = Constantes_FileRetail.INT_ID_FORMA_PAGO_SOLO_TARJETA;
                    pedido.CodePasarelaPago = Constantes_MetodoDePago.CODE_FPAGO_TARJETA_DINERS;
                }
                else if (pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_TARJETA_AMERICANEX)
                {
                    intIdFormaPago = Constantes_FileRetail.INT_ID_FORMA_PAGO_SOLO_TARJETA;
                    pedido.CodePasarelaPago = Constantes_MetodoDePago.CODE_FPAGO_TARJETA_AMERICANEX;
                }
                else if (pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_PAGOEFECTIVO)
                {
                    GenerarPedido_Pago_Efectivo(pedido, _resultpedido, DtsUsuarioLogin);
                }
                else if (pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_SAFETYPAY_ONLINE)
                {
                    GenerarPedido_Safetypay_Online(pedido, _resultpedido, DtsUsuarioLogin);
                }
                else if (pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_SAFETYPAY_CASH)
                {
                    intIdFormaPago = Constantes_Pedido.ID_FORMA_PAGO_SAFETYPAY_CASH;
                    GenerarPedido_Safetypay_Cash(pedido, _resultpedido, DtsUsuarioLogin);
                    pedido.CodePasarelaPago = "";// Para este caso se setea a vacio ya que el procedimiento siguiente solo acepta valores de tarjetas VI, MC, DN, AX
                }
                else if (pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_SAFETYPAY_INTERN)
                {
                    GenerarPedido_Safetypay_Internacional(pedido, _resultpedido, DtsUsuarioLogin);
                }
                else if (pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_TARJETA_UATP)
                {

                }
                else if (pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_INDEPENDENCIA)
                {
                    GenerarPedido_Independencia(pedido, _resultpedido, DtsUsuarioLogin);
                }

                #region LinkPago
                if ((new List<string>() {
                    Constantes_MetodoDePago.CODE_FPAGO_TARJETA_VISA, 
                    Constantes_MetodoDePago.CODE_FPAGO_TARJETA_MASTERCARD,
                    Constantes_MetodoDePago.CODE_FPAGO_TARJETA_AMERICANEX,
                    Constantes_MetodoDePago.CODE_FPAGO_TARJETA_DINERS}).Contains(pedido.CodePasarelaPago))
                {
                    _resultpedido.LinkPago = ObtieneLinkPago(pedido.IdWeb ?? 0, _resultpedido.IdPedido, pedido.IdCotVta);
                }
                else
                {
                    _resultpedido.LinkPago = string.Empty;
                }
                #endregion
                
                /*Inserta Forma de Pedido  General para todos las metodos de pago*/
                _pedidoRepository.InsertFormaPagoPedidoNM(pedido, _resultpedido, intIdFormaPago);

                if (pedido.TiempoExpiracionCIP != null || pedido.TiempoExpiracionCIP > 0)
                {
                    _pedidoRepository.Update_FechaExpira_PedidoNM(pedido, _resultpedido);
                }


                ///Aca deberia estar una validacion de Fee implementar si es el caso
                
                /*Insertamos el POST en el SRV*/
                string strTextoPost = "<span class='texto_cambio_estado'>Cambio de estado a <strong>Pendiente de Pago</strong></span><br><br>" + "La pasarela de pago ha actualizado el estado de su cotización.";
                ICotizacionSRV_Repository _CotizacionSRV = new CotizacionSRV_AW_Repository(pedido.UnidadNegocio.ID);
                Post_SRV _PostSRV_RQ = new Post_SRV()
                {
                    IdCot = pedido.IdCotVta,
                    TipoPost = Convert.ToInt16(Constantes_SRV.ID_TIPO_POST_SRV_USUARIO),
                    TextoPost = strTextoPost,
                    IPUsuCrea = (string.IsNullOrEmpty(pedido.IPUsuario) ? "127.0.0.0" : pedido.IPUsuario),
                    LoginUsuCrea = DtsUsuarioLogin.LoginUsuario,
                    IdUsuWeb = DtsUsuarioLogin.IdUsuario,
                    IdDep = DtsUsuarioLogin.IdDep,
                    IdOfi = DtsUsuarioLogin.IdOfi,
                    Archivos = null,
                    LstFilesPTA = null,
                    IdEstado = Constantes_SRV.ID_ESTADO_COT_PENDIENTE_PAGO,
                    CambioEstado = true,
                    LstFechasCotVta = null,
                    EsAutomatico = true,
                    ArchivoMail = null,
                    EsCounterAdmin = false,
                    IdUsuWebCounterCrea = null,
                    IdOfiCounterCrea = null,
                    IdDepCounterCrea = null,
                    EsUrgenteEmision = null,
                    FecPlazoEmision = null,
                    IdMotivoNoCompro = null,
                    OtroMotivoNoCompro = null,
                    MontoEstimadoFile = null
                };
                _CotizacionSRV.ProcesosPostCotizacion(_PostSRV_RQ);

                _resultpedido.CodigoError = "OK";
                _resultpedido.MensajeError = "El proceso se realizó exitosamente";

                return Ok(_resultpedido);

            }
            catch (Exception ex)
            {
                errorPedido = ex.Message;
                _resultpedido.CodigoError = "ER";
                _resultpedido.MensajeError = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    Request = pedido_NM,
                    Response = _resultpedido,
                    Exception = errorPedido
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        [Route(RouteAction.Send)]
        public IHttpActionResult Send(GenCodigoPagoNM genCodigoPagoNM)
        {           
            UnidadNegocioKeys? _unidadNegocio = null;
            string exceptionMsg = string.Empty;
            try
            {
                RepositoryByBusiness(_unidadNegocio);
                //if ((_unidadNegocio = RepositoryByBusiness(cotizacion.Region.ToUnidadNegocioByCountry())) != null)
                //{
                var operation = _genCodigoPagoNMRepository.GenerarCodigoPago(genCodigoPagoNM);
                var RptaCodigoPagoNM = new
                {
                    respuesta =
                   new
                   {
                       codigo = operation[OutParameter.SF_Codigo].ToString(),
                       mensaje = operation[OutParameter.SF_Mensaje].ToString(),
                       codigoTransaccion = operation[OutParameter.CodigoTransaccion].ToString()
            }
                };
                
                return Ok(RptaCodigoPagoNM);
                //}
                //return NotFound();
            }
            catch (Exception ex)
            {
                exceptionMsg = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    UnidadNegocio = _unidadNegocio.ToLongName(),
                    Body = genCodigoPagoNM,
                    Exception = exceptionMsg
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region GenerarPedido
        // Método de Pago: SC
        private void GenerarPedido_Safetypay_Cash(DatosPedido pedido, 
                                      Models.PedidoRS resultPedido,
                                      UsuarioLogin DtsUsuarioLogin)
        {
            Pedido_AW_Repository _pedidoRepository = new Pedido_AW_Repository();
            var errorPedido = string.Empty;

            /*Generamos el codigo en SafetyPay*/
            ws_compra.ws_compra Ws_Compra = new ws_compra.ws_compra();
            CustomCashPaymentRequestType requestCash = new CustomCashPaymentRequestType();
            ws_compra.AmountType objAmountType = new ws_compra.AmountType();

            objAmountType.CurrencyID = "150"; /*Valor del dolar para SafetyPay*/
            objAmountType.Value = Convert.ToDecimal(pedido.Monto);

            requestCash.BankID = String.Empty;
            requestCash.IncludeAllBanks = true;
            requestCash.TransactionIdentifier = String.Empty;
            requestCash.MerchantAccount = String.Empty;
            requestCash.MerchantSalesID = resultPedido.IdPedido.ToString();
            requestCash.TrackingCode = "";
            requestCash.ExpirationTime = (Convert.ToInt16(pedido.TiempoExpiracionCIP) * 60);
            requestCash.ExpirationTimeSpecified = true;
            requestCash.Language = (pedido.IdWeb == Webs_Cid.DM_WEB_ID ? "PE" : "ES");
            requestCash.CountryID = "PER";
            requestCash.Amount = objAmountType;

            requestCash.SendEmailToShopper = true;
            if (requestCash.SendEmailToShopper)
            {
                requestCash.CustomerInformation_Value = DtsUsuarioLogin.EmailUsuario;
            }

            requestCash.CustomMerchantName = (pedido.IdWeb == Webs_Cid.DM_WEB_ID ? "Destinos Mundiales Perú" : "NMVIAJES");
            requestCash.ApplicationID = Convert.ToInt16(pedido.IdWeb);

            ws_compra.RptaPagoSafetyPay response = new ws_compra.RptaPagoSafetyPay();
            if (pedido.IdWeb == Webs_Cid.DM_WEB_ID)
            {
                requestCash.WebId = Convert.ToString(pedido.IdWeb);
                response = Ws_Compra.GenerarPago_SafetyPay_Cash_DM(requestCash);
            }
            else
            {
                requestCash.IdDepartamento = DtsUsuarioLogin.IdDep;
                requestCash.IdOficina = DtsUsuarioLogin.IdOfi;
                response = Ws_Compra.GenerarPago_SafetyPay_Cash(requestCash);
            }

            if (response.OperationId != null && response.TransaccionIdentifier != null)
            {
                resultPedido.CodigoOperacion = response.OperationId;
                resultPedido.CodigoTransaction = response.TransaccionIdentifier;

                resultPedido.CodigoError = "OK";
                resultPedido.MensajeError = "Se generó el código de pedido.";
            }
            else
            {
                resultPedido.CodigoError = "ER"; resultPedido.MensajeError = "GP - Error al intentar generar el CIP";
                //return Ok(resultPedido);
            }

            Models.RptaPagoSafetyPay RptaPagoSafetyPayBD = _pedidoRepository.Get_Rpta_SagetyPay(resultPedido.IdPedido);

            List<Models.PaymentLocationType> lstPaymentType = new List<Models.PaymentLocationType>();

            if (response.lst_PaymentLocationType != null)
            {
                List<Models.PaymentStepType> lstPaymentStep = new List<Models.PaymentStepType>();
                List<Models.PaymentInstructionType> lstPaymentInstructions = new List<Models.PaymentInstructionType>();
                foreach (ws_compra.PaymentLocationType objPaymentLocationTypeRSTmp in response.lst_PaymentLocationType)
                {
                    Models.PaymentLocationType objPaymentLocationTypeRS = new Models.PaymentLocationType();
                    objPaymentLocationTypeRS.Id = objPaymentLocationTypeRSTmp.ID;

                    foreach (ws_compra.PaymentStepType objPaymentStepRSTmp in objPaymentLocationTypeRSTmp.lst_PaymentStepType)
                    {
                        Models.PaymentStepType objPaymentStepRS = new Models.PaymentStepType();
                        objPaymentStepRS.Step = objPaymentStepRSTmp.Step;
                        objPaymentStepRS.StepSpecified = objPaymentStepRSTmp.StepSpecified;
                        objPaymentStepRS.Value = objPaymentStepRSTmp.Value;
                        lstPaymentStep.Add(objPaymentStepRS);
                    }

                    if (objPaymentLocationTypeRSTmp.PaymentInstructions != null)
                    {
                        foreach (ws_compra.PaymentInstructionType objPaymentInstructionTypeRSTmp in objPaymentLocationTypeRSTmp.PaymentInstructions)
                        {
                            Models.PaymentInstructionType objPaymentInstructionsRS = new Models.PaymentInstructionType();
                            objPaymentInstructionsRS.Name = objPaymentInstructionTypeRSTmp.Name;
                            objPaymentInstructionsRS.Value = objPaymentInstructionTypeRSTmp.Value;
                            lstPaymentInstructions.Add(objPaymentInstructionsRS);
                        }
                    }

                    objPaymentLocationTypeRS.lstPaymentStepType = lstPaymentStep;
                    objPaymentLocationTypeRS.Name = objPaymentLocationTypeRSTmp.Name;
                    objPaymentLocationTypeRS.PaymentInstructions = lstPaymentInstructions.ToArray();
                    objPaymentLocationTypeRS.PaymentSteps = lstPaymentStep.ToArray();
                    lstPaymentType.Add(objPaymentLocationTypeRS);
                }
            }

            /*Realizamos el envio del correo*/
            try
            {
                IEnviarCorreo objEnviarCorreo = new EnviarCorreo(pedido.UnidadNegocio.ID);
                DateTime datFechaActual = DateTime.Now;
                DateTime datFechaExpiraPago;
                string strExpirationDateTime = response.ExpirationDateTime;
                string[] arrExp = strExpirationDateTime.Split('(');
                datFechaExpiraPago = datFechaActual.AddHours(Convert.ToInt16(pedido.TiempoExpiracionCIP));

                resultPedido.CorreoEnviado = objEnviarCorreo.Enviar_SolicitudPagoServicioSafetyPay(
                    pedido.IdUsuario.ToString(), Convert.ToInt32(pedido.IdWeb), Convert.ToInt32(pedido.IdLang),
                    pedido.IdCotVta, pedido.Email, null, pedido.NombreClienteCot, pedido.ApellidoClienteCot, null,
                    DtsUsuarioLogin.NomCompletoUsuario, DtsUsuarioLogin.EmailUsuario,
                    (pedido.CodePasarelaPago == Constantes_SafetyPay.CodeSafetyPayCash ? Constantes_Pedido.ID_FORMA_PAGO_SAFETYPAY_CASH : Convert.ToInt16(0)),
                    RptaPagoSafetyPayBD.TransaccionIdentifier, resultPedido.IdPedido, Convert.ToDouble(pedido.Monto),
                    RptaPagoSafetyPayBD.ExpirationDateTime, RptaPagoSafetyPayBD.lstAmountType, lstPaymentType);
                objEnviarCorreo = null;
            }
            catch (Exception ex)
            {
                errorPedido = "Error al enviar el correo |" + ex.Message;
                /*return InternalServerError(ex);*/
            }
        }

        // Método de Pago: SO
        private void GenerarPedido_Safetypay_Online(DatosPedido pedido,
                                      Models.PedidoRS resultPedido,
                                      UsuarioLogin DtsUsuarioLogin)
        {
            var strEmailsCli = pedido.Email;
            var ws_SafetyPay = new ws_compra.ws_compra();
            var request = new CustomOnlinePaymentRequestType();
            var objAmountType = new ws_compra.AmountType();
            objAmountType.CurrencyID = CurrencyEnumType.USD.ToString();
            objAmountType.Value = decimal.TryParse(pedido.MontoPagar.ToString(), out decimal dblMontoPagar) ? dblMontoPagar : 0;
            request.BankID = string.Empty;
            request.TransactionIdentifier = string.Empty;
            request.MerchantAccount = string.Empty;
            request.MerchantSalesID = resultPedido.IdPedido.ToString();
            request.TrackingCode = "0";
            request.ExpirationTime = pedido.TiempoExpiracionCIP ?? 0 * 60;
            request.ExpirationTimeSpecified = true;

            request.Language = pedido.IdWeb == Webs_Cid.DM_WEB_ID ? "PE": "ES";
            request.CountryID = "PER";
            request.Amount = objAmountType;
            request.TransactionOkURL = pedido.IdWeb == Webs_Cid.DM_WEB_ID ?
                "http://www.destinosmundialesperu.com/" :
                "http://www.nmviajes.com/";
            request.TransactionErrorURL = pedido.IdWeb == Webs_Cid.DM_WEB_ID ?
                "http://www.destinosmundialesperu.com/" :
                "http://www.nmviajes.com/seguros#";

            request.SendEmailToShopper = true;
            if (request.SendEmailToShopper) request.CustomerInformation_Value = DtsUsuarioLogin.EmailUsuario;
            request.IdDepartamento = DtsUsuarioLogin.IdDep;
            request.IdOficina = DtsUsuarioLogin.IdOfi;
            request.ApplicationID = (short)(pedido.IdWeb ?? 0);


            //var respond As ws_srv_gnm.ws_compra.RptaPagoSafetyPay
            ws_compra.RptaPagoSafetyPay respond;
            if (pedido.IdWeb == Webs_Cid.DM_WEB_ID)
            {
                request.WebId = pedido.IdWeb ?? 0;
                respond = ws_SafetyPay.GenerarPago_SafetyPay_OnlineDM(request);
            }
            else
            {
                request.IdDepartamento = DtsUsuarioLogin.IdDep;
                request.IdOficina = DtsUsuarioLogin.IdOfi;
                respond = ws_SafetyPay.GenerarPago_SafetyPay_Online(request);
            }

            
            var objRptaPagoSafetyPay = (new Pedido_AW_Repository()).Get_Rpta_SagetyPay(resultPedido.IdPedido);

            var lstPaymentType = new List<Models.PaymentLocationType>();

            if (respond.lst_PaymentLocationType != null)
            {
                var lstPaymentStep = new List<Models.PaymentStepType>();
                var lstPaymentInstructions = new List<Models.PaymentInstructionType>();

                foreach (var objPaymentLocationTypeRSTmp in respond.lst_PaymentLocationType)
                {
                    var objPaymentLocationTypeRS = new Models.PaymentLocationType();
                    objPaymentLocationTypeRS.Id = objPaymentLocationTypeRSTmp.ID;

                    foreach (var objPaymentStepRSTmp in objPaymentLocationTypeRSTmp.lst_PaymentStepType)
                    {
                        var objPaymentStepRS = new Models.PaymentStepType();
                        objPaymentStepRS.Step = objPaymentStepRSTmp.Step;
                        objPaymentStepRS.StepSpecified = objPaymentStepRSTmp.StepSpecified;
                        objPaymentStepRS.Value = objPaymentStepRSTmp.Value;
                        lstPaymentStep.Add(objPaymentStepRS);
                    }

                    if (objPaymentLocationTypeRSTmp.PaymentInstructions != null)
                    {
                        foreach (var objPaymentInstructionTypeRSTmp in objPaymentLocationTypeRSTmp.PaymentInstructions)
                        {
                            var objPaymentInstructionsRS = new Models.PaymentInstructionType();
                            objPaymentInstructionsRS.Name = objPaymentInstructionTypeRSTmp.Name;
                            objPaymentInstructionsRS.Value = objPaymentInstructionTypeRSTmp.Value;
                            lstPaymentInstructions.Add(objPaymentInstructionsRS);
                        }
                    }

                    objPaymentLocationTypeRS.lstPaymentStepType = lstPaymentStep;
                    objPaymentLocationTypeRS.Name = objPaymentLocationTypeRSTmp.Name;
                    objPaymentLocationTypeRS.PaymentInstructions = lstPaymentInstructions.ToArray();
                    objPaymentLocationTypeRS.PaymentSteps = lstPaymentStep.ToArray();
                    lstPaymentType.Add(objPaymentLocationTypeRS);
                }
            }
            var intIdFormaPago = Constantes_Pedido.ID_FORMA_PAGO_SAFETYPAY_ONLINE;

            try
            {
                var objEnviarCorreo = new EnviarCorreo();
                var datFechaActual = DateTime.Now;
                //var datFechaExpiraPago As Date
                var strExpirationDateTime = respond.ExpirationDateTime;
                var arrExp = strExpirationDateTime.Split('(');
                var datFechaExpiraPago = resultPedido.FechaExpiracion = datFechaActual.AddHours(pedido.TiempoExpiracionCIP ?? 0);

                resultPedido.CorreoEnviado = objEnviarCorreo.Enviar_SolicitudPagoServicioSafetyPay(
                    pedido.IdUsuario.ToString(),
                    Convert.ToInt32(pedido.IdWeb),
                    Convert.ToInt32(pedido.IdLang),
                    pedido.IdCotVta,
                    pedido.Email,
                    null,
                    pedido.NombreClienteCot,
                    pedido.ApellidoClienteCot,
                    null,
                    DtsUsuarioLogin.NomCompletoUsuario,
                    DtsUsuarioLogin.EmailUsuario,
                    (pedido.CodePasarelaPago == Constantes_SafetyPay.CodeSafetyPayOnline ? Constantes_Pedido.ID_FORMA_PAGO_SAFETYPAY_ONLINE : Convert.ToInt16(0)),
                    objRptaPagoSafetyPay.TransaccionIdentifier,
                    resultPedido.IdPedido,
                    Convert.ToDouble(pedido.Monto),
                    objRptaPagoSafetyPay.ExpirationDateTime,
                    objRptaPagoSafetyPay.lstAmountType,
                    lstPaymentType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Método de Pago: SI
        private void GenerarPedido_Safetypay_Internacional(DatosPedido pedido,
                                      Models.PedidoRS resultPedido,
                                      UsuarioLogin DtsUsuarioLogin)
        {
        }

        // Método de Pago: IN { Descartado en el proyecto }
        private void GenerarPedido_Independencia(DatosPedido pedido,
                                      Models.PedidoRS resultPedido,
                                      UsuarioLogin DtsUsuarioLogin)
        {
        }

        // Método de Pago: PE
        private void GenerarPedido_Pago_Efectivo(DatosPedido pedido,
                                      Models.PedidoRS resultPedido,
                                      UsuarioLogin DtsUsuarioLogin)
        {
            DateTime datFechaActual = DateTime.Now;
            var ddlHoraExpiraCIP = pedido.TiempoExpiracionCIP ?? 0;
            var datFechaExpiraPago = datFechaActual.AddHours(ddlHoraExpiraCIP);
            resultPedido.FechaExpiracion = datFechaExpiraPago;

            var intIdFormaPago = Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC;

            try
            {
                var strEmailsCliPE = pedido.Email;
                var dblMontoPagar = pedido.MontoPagar;
                var intIdPedido = resultPedido.IdPedido;

                var intIdWeb = pedido.IdWeb;
                var intIdLang = pedido.IdLang;
                var intIdCotVta = pedido.IdCotVta;

                // Dim paymentRequest As New BEGenRequest
                var paymentRequest = new ws_pagoefectivo.BEGenRequest();

                paymentRequest.IdMoneda = ConfigAccess.GetValueInAppSettings("PE_MONEDA");
                paymentRequest.Total = dblMontoPagar.ToString("0.00");
                paymentRequest.MetodosPago = ConfigAccess.GetValueInAppSettings("PE_MEDIO_PAGO");
                paymentRequest.Codtransaccion = intIdPedido.ToString("");
                paymentRequest.ConceptoPago = string.Format("{0}: Orden {1}",
                    ConfigAccess.GetValueInAppSettings("PE_COMERCIO_CONCEPTO_PAGO"),
                    paymentRequest.Codtransaccion);
                paymentRequest.EmailComercio = strEmailsCliPE;
                paymentRequest.FechaAExpirar = datFechaExpiraPago.ToString("yyyy-MM-dd");
                paymentRequest.UsuarioNombre = DtsUsuarioLogin.LoginUsuario;
                paymentRequest.UsuarioApellidos = DtsUsuarioLogin.ApePatUsuario;
                paymentRequest.UsuarioEmail = DtsUsuarioLogin.EmailUsuario;
                paymentRequest.IdDepartamento = DtsUsuarioLogin.IdDep;
                paymentRequest.IdOficina = DtsUsuarioLogin.IdOfi;

                var paymentResponse = (new ws_pagoefectivo.ws_PagoEfectivo()).GenerarCIP(paymentRequest);

                // Reintento de GENERACIÓN DE CIP
                if (paymentResponse == null || paymentResponse.Estado != "1" || paymentResponse.Token == null)
                {
                    paymentResponse = (new ws_pagoefectivo.ws_PagoEfectivo()).GenerarCIP(paymentRequest);
                }

                var strCIP = resultPedido.CodigoCIP = paymentResponse.NumeroCip;

                //  Enviar correo
                var objEnviarCorreo = new EnviarCorreo();
                var strEmailsCli = pedido.Email;

                if (Enviar_SolicitudPagoServicioPagoEfectivo(
                    intIdWeb ?? 0,
                    intIdLang ?? 0,
                    intIdCotVta,
                    strEmailsCli,
                    null,
                    pedido.NombreClienteCot,
                    pedido.ApellidoClienteCot,
                    null,
                    DtsUsuarioLogin.NomCompletoUsuario,
                    DtsUsuarioLogin.EmailUsuario,
                    short.Parse(pedido.FormaPago),
                    strCIP,
                    string.Empty,
                    intIdPedido,
                    dblMontoPagar,
                    datFechaExpiraPago))
                {
                    resultPedido.CorreoEnviado = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Auxiliar
        private bool Enviar_SolicitudPagoServicioPagoEfectivo(
            int pIntIdWeb,
            int pIntIdLang,
            int? pIntIdCotVta,
            string pStrEmailTO,
            string pStrEMailCC,
            string pStrNomCli,
            string pStrApeCli,
            string pStrURLPago,
            string pStrNomCompletoUsuWeb,
            string pStrEmailUsuWeb,
            short pIntIdFormaPago,
            string pStrCIP,
            string pStrCodigoBarras,
            int pIntIdPedido,
            double pDblMontoPagar,
            DateTime pDatFechaExpiraPago)
        {
            //var objCorreoWebBO = New CorreoWebBO
            //var objCorreo As Correo = Nothing
            //var objNMMail As New NMMail
            //var objEncriptaCadena As New NuevoMundoSecurity.EncriptaCadena
            return true;
        }

        private string ObtieneLinkPago(int pIntIdWeb, int pIntIdPedido, int pIntIdCotSRV)
        {
            EncriptaCadena objNMEncriptaCadena = new EncriptaCadena();
            try
            {
                string strURLPago = "";
                if (pIntIdWeb == Webs_Cid.ID_WEB_NM_PERUTRIP)
                    strURLPago = ConfigAccess.GetValueInAppSettings("URL_PAGO_SERVICIO_ONLINE_PERUTRIP");
                else if (pIntIdWeb == Webs_Cid.DM_WEB_ID)
                    strURLPago = ConfigAccess.GetValueInAppSettings("URL_PAGO_SERVICIO_ONLINE_DM");
                else
                    strURLPago = ConfigAccess.GetValueInAppSettings("URL_PAGO_SERVICIO_ONLINE");

                string strIdEncrypt = objNMEncriptaCadena.DES_Encrypt(pIntIdPedido + ";" + pIntIdCotSRV, objNMEncriptaCadena.GetKEY(EncriptaCadena.TIPO_KEY.KEY_ENCRIPTA_NRO_PEDIDO_PAGO_ONLINE));

                return strURLPago + "?id=" + strIdEncrypt;
            }
            catch (Exception ex)
            {
                return "Error";
            }
            finally
            {
                objNMEncriptaCadena = null;
            }
        }

        private void validacionPedido(ref DatosPedido _pedido, ref Models.PedidoRS _resultPedido, ref bool _return, ref UsuarioLogin UserLogin)
        {
            string mensajeError = string.Empty;
            _pedido.IdLang = 1; _pedido.IdWeb = 0;
            string metodoDePago = string.Empty;

            if (_pedido.IdCotVta <= 0)
            {
                mensajeError += "Envie el codigo de SRV|";
            }
            if (string.IsNullOrEmpty(_pedido.NombreClienteCot) || string.IsNullOrEmpty(_pedido.ApellidoClienteCot))
            {
                mensajeError += "Envie los datos del SRV|";
            }
            if (_pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_PAGOEFECTIVO)
                metodoDePago = _pedido.CodePasarelaPago;
            else if(_pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_SAFETYPAY_CASH)
                metodoDePago = _pedido.CodePasarelaPago;
            else if(_pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_SAFETYPAY_INTERN)
                metodoDePago = _pedido.CodePasarelaPago;
            else if (_pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_SAFETYPAY_ONLINE)
                metodoDePago = _pedido.CodePasarelaPago;
            else if (_pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_TARJETA_AMERICANEX)
                metodoDePago = _pedido.CodePasarelaPago;
            else if (_pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_TARJETA_DINERS)
                metodoDePago = _pedido.CodePasarelaPago;
            else if (_pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_TARJETA_MASTERCARD)
                metodoDePago = _pedido.CodePasarelaPago;
            else if (_pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_TARJETA_MASTERCARD_CA)
                metodoDePago = _pedido.CodePasarelaPago;
            else if (_pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_TARJETA_VISA)
                metodoDePago = _pedido.CodePasarelaPago;

            if (string.IsNullOrEmpty(_pedido.CodePasarelaPago))
            {
                mensajeError += "Envie el tipo de pasarela|";
            }
            else if (!_pedido.CodePasarelaPago.Trim().Contains(metodoDePago))
            {
                mensajeError += "No se tiene registrado esta pasarela de pago|";
            }
            if (string.IsNullOrEmpty(_pedido.DetalleServicio))
            {
                mensajeError += "Envie el detalle del servicio|";
            }
            if (string.IsNullOrEmpty(_pedido.Email))
            {
                mensajeError += "Envie su Email|";
            }
            if (string.IsNullOrEmpty(_pedido.Monto))
            {
                mensajeError += "Envie el monto|";
            }
            if (!string.IsNullOrEmpty(_pedido.Monto) && _pedido.Monto.Contains(","))
            {
                mensajeError += "El uso de la coma (,) no es válido como separador decimal.";
            }
            if (_pedido.TiempoExpiracionCIP == null || _pedido.TiempoExpiracionCIP <= 0)
            {
                mensajeError += "Envie el tiempo de expiracion del CIP|";
            }
            if (_pedido.UnidadNegocio == null || string.IsNullOrEmpty(_pedido.UnidadNegocio.Descripcion)) /*Duda*/
            {
                mensajeError += "Envie los datos de su unidad de negocio|";
            }
            if (string.IsNullOrEmpty(_pedido.IdUsuario) == false && ValidateProcess.isInt32(_pedido.IdUsuario) == true)
            {
                /*Cargamos Datos del Usuario*/
                _datosUsuario = new DatosUsuario(_pedido.UnidadNegocio.ID);
                UserLogin = _datosUsuario.Get_Dts_Usuario_Personal_NM(Convert.ToInt32(_pedido.IdUsuario));
                if (UserLogin == null) { mensajeError += "ID del Usuario no registrado|"; }
                else if (UserLogin.IdUsuario != Convert.ToInt32(_pedido.IdUsuario)) { _pedido.IdUsuario = UserLogin.IdUsuario.ToString(); }
            }
            else
            {
                mensajeError += "Envie el ID del Usuario correctamente|";
            }
            if (string.IsNullOrEmpty(_pedido.IdOportunidad_SF))
            {
                mensajeError += "Envie el IdOportunidad_SF|";
            }
            if (string.IsNullOrEmpty(_pedido.IdSolicitudpago_SF))
            {
                mensajeError += "Envie el IdSolicitudpago_SF";
            }

            if (string.IsNullOrEmpty(mensajeError) == false)
            {
                _return = true;
                _resultPedido.CodigoError = "ER";
                _resultPedido.MensajeError = "VP - " + mensajeError;
            }
            else
            {
                if (_pedido.IdCanalVta != null && _pedido.IdCanalVta == Constantes_Pedido.ID_CANAL_VENTA_CONTACT_CENTER)
                {
                    _pedido.IdWeb = Webs_Cid.ID_WEB_IA;
                }

                if (_pedido.IdWeb == 0)
                {
                    if ((UserLogin.IdOfi == Oficina.ID_OFI_NMV & UserLogin.IdDep == Departamento.ID_DEP_INTERNO) | (UserLogin.IdOfi == Oficina.ID_OFI_NMV & UserLogin.IdDep == Departamento.ID_DEP_RECEPTIVO) | (UserLogin.IdOfi == Oficina.ID_OFI_NMV & UserLogin.IdDep == Departamento.ID_DEP_OPERACIONES))
                        _pedido.IdWeb = Webs_Cid.ID_WEB_NMV_RECEPTIVO;
                    else if ((UserLogin.IdOfi == Oficina.ID_OFI_NMVCOM & UserLogin.IdDep == Departamento.ID_DEP_COUNTER) | (UserLogin.IdOfi == Oficina.ID_OFI_NMV & UserLogin.IdDep == Departamento.ID_DEP_SISTEMAS))
                        _pedido.IdWeb = Webs_Cid.NM_WEB_ID;
                    else
                        _pedido.IdWeb = Webs_Cid.ID_WEB_WEBFAREFINDER;
                }
            }
        }

        private Boolean ValidarMontoMaxPasarela(double monto)
        {
            if (monto > Constantes_Pedido.DBL_VI_MONTO_MAX_TX)
                return false;
            else
                return true;
        }

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _genCodigoPagoNMRepository = new GenCodigoPagoNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
        #endregion
    }
}
