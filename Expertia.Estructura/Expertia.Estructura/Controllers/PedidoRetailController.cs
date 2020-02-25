using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.AppWebs;
using Expertia.Estructura.Repository.General;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Utils;
using System;
using System.Web.Http;
using System.Collections.Generic;
using Expertia.Estructura.ws_compra;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.PedidoRetail)]
    public class PedidoRetailController : BaseController<object>
    {
        #region Properties       
        private IPedidoRepository _pedidoRepository;
        private IDatosUsuario _datosUsuario;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(Pedido pedido)
        {
            Models.PedidoRS _resultpedido = new Models.PedidoRS();
            UsuarioLogin DtsUsuarioLogin = null;
            var errorPedido = string.Empty;
            bool _return = false;
            try
            {
                /*Validaciones*/
                validacionPedido(ref pedido, ref _resultpedido, ref _return, ref DtsUsuarioLogin);
                if(_return == true) return Ok(_resultpedido);
                
                RepositoryByBusiness(pedido.UnidadNegocio.ID);

                /*Generamos el Pedido*/               
                var operation = _pedidoRepository.Create(pedido);
                if(operation[Operation.Result].ToString() == ResultType.Success.ToString() && Convert.ToInt32(operation[OutParameter.IdPedido].ToString()) > 0)
                {
                    _resultpedido.IdPedido = Convert.ToInt32(operation[OutParameter.IdPedido].ToString());
                }
                else
                {
                    _resultpedido.CodigoError = "GP"; _resultpedido.MensajeError = "Error al intentar generar el pedido";
                    return Ok(_resultpedido);
                }

                /*Generamos el codigo en SafetyPay*/
                ws_compra.ws_compra Ws_Compra = new ws_compra.ws_compra();
                CustomOnlinePaymentRequestType request = new CustomOnlinePaymentRequestType();
                ws_compra.AmountType objAmountType = new ws_compra.AmountType();

                objAmountType.CurrencyID = "150"; /*Valor del dolar para SafetyPay*/
                objAmountType.Value = Convert.ToDecimal(pedido.Monto);
                
                request.BankID = String.Empty;
                request.TransactionIdentifier = String.Empty;
                request.MerchantAccount = String.Empty;
                request.MerchantSalesID = _resultpedido.IdPedido.ToString();
                request.TrackingCode = "0";
                request.ExpirationTime = (Convert.ToInt16(pedido.TiempoExpiracionCIP) * 60);
                request.ExpirationTimeSpecified = true;                        
                request.Language = (pedido.IdWeb == Webs_Cid.DM_WEB_ID ? "PE" : "ES");
                request.CountryID = "PER";
                request.Amount = objAmountType;
                request.TransactionOkURL = (pedido.IdWeb == Webs_Cid.DM_WEB_ID ? "http://www.destinosmundialesperu.com/" : "http://www.nmviajes.com/");
                request.TransactionErrorURL = (pedido.IdWeb == Webs_Cid.DM_WEB_ID ? "http://www.destinosmundialesperu.com/" : "http://www.nmviajes.com/seguros#");
                                
                request.SendEmailToShopper = true;
                if (request.SendEmailToShopper) {
                    request.CustomerInformation_Value = DtsUsuarioLogin.EmailUsuario;
                }

                request.IdDepartamento = DtsUsuarioLogin.IdDep;
                request.IdOficina = DtsUsuarioLogin.IdOfi;
                
                request.ApplicationID = Convert.ToInt16(pedido.IdWeb);

                ws_compra.RptaPagoSafetyPay response = new ws_compra.RptaPagoSafetyPay();
                if(pedido.IdWeb == Webs_Cid.DM_WEB_ID || (pedido.IdCanalVta != null && pedido.IdCanalVta == Constantes_Pedido.ID_CANAL_VENTA_CONTACT_CENTER))
                {
                    request.WebId = Convert.ToInt32(pedido.IdWeb);
                    response = Ws_Compra.GenerarPago_SafetyPay_OnlineDM(request);
                }
                else
                {
                    response = Ws_Compra.GenerarPago_SafetyPay_Online(request);
                }
                       
                if(response.OperationId != null && response.TransaccionIdentifier != null)
                {
                    _resultpedido.CodigoOperacion = response.OperationId;
                    _resultpedido.CodigoTransaction = response.TransaccionIdentifier;
                }
                else
                {
                    _resultpedido.CodigoError = "GP"; _resultpedido.MensajeError = "Error al intentar generar el CIP";
                    return Ok(_resultpedido);
                }

                Models.RptaPagoSafetyPay RptaPagoSafetyPayBD = _pedidoRepository.Get_Rpta_SagetyPay(_resultpedido.IdPedido);

                List<Models.PaymentLocationType> lstPaymentType = new List<Models.PaymentLocationType>();                
                    
                if (response.lst_PaymentLocationType != null)
                {
                    List<Models.PaymentStepType> lstPaymentStep = new List<Models.PaymentStepType>();
                    List<Models.PaymentInstructionType> lstPaymentInstructions = new List<Models.PaymentInstructionType>();
                    foreach (ws_compra.PaymentLocationType objPaymentLocationTypeRSTmp in response.lst_PaymentLocationType)
                    {                        
                        Models.PaymentLocationType objPaymentLocationTypeRS = new Models.PaymentLocationType();
                        objPaymentLocationTypeRS.Id = objPaymentLocationTypeRSTmp.ID;

                        foreach(ws_compra.PaymentStepType objPaymentStepRSTmp in objPaymentLocationTypeRSTmp.lst_PaymentStepType)
                        {
                            Models.PaymentStepType objPaymentStepRS = new Models.PaymentStepType();
                            objPaymentStepRS.Step = objPaymentStepRSTmp.Step;
                            objPaymentStepRS.StepSpecified = objPaymentStepRSTmp.StepSpecified;
                            objPaymentStepRS.Value = objPaymentStepRSTmp.Value;
                            lstPaymentStep.Add(objPaymentStepRS);
                        }

                        if(objPaymentLocationTypeRSTmp.PaymentInstructions != null)
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

                    _resultpedido.CorreoEnviado = objEnviarCorreo.Enviar_SolicitudPagoServicioSafetyPay(pedido.IdUsuario.ToString(),Convert.ToInt32(pedido.IdWeb), Convert.ToInt32(pedido.IdLang),pedido.IdCotVta, pedido.Email, null,pedido.NombreClienteCot, pedido.ApellidoClienteCot, null, DtsUsuarioLogin.NomCompletoUsuario, DtsUsuarioLogin.EmailUsuario, (pedido.CodePasarelaPago == Constantes_SafetyPay.CodeSafetyPayOnline ? Constantes_Pedido.ID_FORMA_PAGO_SAFETYPAY_ONLINE : Convert.ToInt16(0)), RptaPagoSafetyPayBD.TransaccionIdentifier, _resultpedido.IdPedido, Convert.ToDouble(pedido.Monto), RptaPagoSafetyPayBD.ExpirationDateTime, RptaPagoSafetyPayBD.lstAmountType, lstPaymentType);
                    objEnviarCorreo = null;
                }
                catch (Exception ex)
                {
                    errorPedido = "Error al enviar el correo |" + ex.Message;
                    /*return InternalServerError(ex);*/
                }

                /*Inserta Forma de Pedido*/
                _pedidoRepository.InsertFormaPagoPedido(pedido, _resultpedido);
                _pedidoRepository.Update_FechaExpira_Pedido(pedido, _resultpedido);

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

                return Ok(_resultpedido);

            }            
            catch (Exception ex)
            {
                errorPedido = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    Request = pedido,
                    Response = _resultpedido,
                    Exception = errorPedido
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
              
        private void validacionPedido(ref Pedido _pedido, ref Models.PedidoRS _resultPedido, ref bool _return, ref UsuarioLogin UserLogin)
        {
            string mensajeError = string.Empty;
            _pedido.IdLang = 1; _pedido.IdWeb = 0;

            if (_pedido.IdCotVta <= 0)
            {
                mensajeError += "Envie el codigo de SRV|";
            }
            if (string.IsNullOrEmpty(_pedido.NombreClienteCot) || string.IsNullOrEmpty(_pedido.ApellidoClienteCot))
            {
                mensajeError += "Envie los datos del SRV|";
            }
            if (string.IsNullOrEmpty(_pedido.CodePasarelaPago)){
                mensajeError += "Envie el tipo de pasarela|";
            }
            else if (!_pedido.CodePasarelaPago.Trim().Contains(Constantes_Pedido.CODE_FPAGO_GENERAL)) {
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
            if (_pedido.TiempoExpiracionCIP == null || _pedido.TiempoExpiracionCIP <= 0)
            {
                mensajeError += "Envie el tiempo de expiracion del CIP|";
            }            
            if (_pedido.UnidadNegocio == null || string.IsNullOrEmpty(_pedido.UnidadNegocio.Descripcion)) /*Duda*/
            {
                mensajeError += "Envie los datos de su unidad de negocio|";
            }
            if (string.IsNullOrEmpty(_pedido.IdUsuario) == false && isInt32(_pedido.IdUsuario) == true)
            {
                /*Cargamos Datos del Usuario*/
                _datosUsuario = new DatosUsuario(_pedido.UnidadNegocio.ID);
                UserLogin = _datosUsuario.Get_Dts_Usuario_Personal(Convert.ToInt32(_pedido.IdUsuario));
                if(UserLogin == null){mensajeError += "ID del Usuario no registrado|";}
            }
            else
            {
                mensajeError += "Envie el ID del Usuario correctamente|";
            }

            if (string.IsNullOrEmpty(mensajeError) == false)
            {
                _return = true;
                _resultPedido.CodigoError = "VP";
                _resultPedido.MensajeError = mensajeError;
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

        public bool isInt32(String num)
        {
            bool isNum;
            double retNum;

            try
            {
                isNum = Double.TryParse(Convert.ToString(num), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
                return isNum;            
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Auxiliar
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            _pedidoRepository = new Pedido_AW_Repository(unidadNegocioKey);            
            return unidadNegocioKey;
        }
        #endregion

    }
}
