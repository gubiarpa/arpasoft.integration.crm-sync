using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Collections.Generic;

namespace Expertia.Estructura.Repository.AppWebs
{
    public class Pedido_AW_Repository : OracleBase<Pedido>, IPedidoRepository
    {
        #region Constructor
        public Pedido_AW_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation Create(Pedido pedido)
        {
            try
            {
                var operation = new Operation();

                #region Parameter
                AddParameter("pChrMotorTipo_in", OracleDbType.Char, Constantes_Pedido.ID_TIPO_PEDIDO_OTROS, ParameterDirection.Input,3);                                
                AddParameter("pVarIdSesion_in", OracleDbType.Varchar2, null, ParameterDirection.Input, 50);                                
                AddParameter("pNumIdResVue_in", OracleDbType.Int32,null, ParameterDirection.Input);                
                AddParameter("pNumIdResPaq_in", OracleDbType.Int32, null, ParameterDirection.Input);                
                AddParameter("pChrTipoPaq_in", OracleDbType.Char, null, ParameterDirection.Input,1);                                
                AddParameter("pNumIdWeb_in", OracleDbType.Int32, pedido.IdWeb, ParameterDirection.Input);                
                AddParameter("pNumIdLang_in", OracleDbType.Int32, pedido.IdLang, ParameterDirection.Input);                
                AddParameter("pVarIP_in", OracleDbType.Varchar2,(string.IsNullOrEmpty(pedido.IPUsuario) ? "127.0.0.0" : pedido.IPUsuario), ParameterDirection.Input, 30);                
                AddParameter("pVarBrowser_in", OracleDbType.Varchar2, (string.IsNullOrEmpty(pedido.Browser) ? "Servicio Saleforce" : pedido.IPUsuario), ParameterDirection.Input, 200);                
                AddParameter("pNumIdCotSRV_in", OracleDbType.Int32,pedido.IdCotVta, ParameterDirection.Input);                
                AddParameter("pVarDetalleServ_in", OracleDbType.Varchar2, pedido.DetalleServicio, ParameterDirection.Input, 1000);                
                AddParameter("pNumMonto_in", OracleDbType.Double, Convert.ToDouble(pedido.Monto), ParameterDirection.Input);                
                
                AddParameter(OutParameter.IdPedido, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);                
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Create_Pedido);

                operation[OutParameter.IdPedido] = GetOutParameter(OutParameter.IdPedido);
                operation[Operation.Result] = ResultType.Success;
                #endregion

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation GetPedidosProcesados()
        {
            var operation = new Operation();
            try
            {
                #region Parameters                
                AddParameter(OutParameter.CursorPedidosProcesados, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                #endregion

                #region Invoke              
                ExecuteStoredProcedure(StoredProcedureName.AW_Get_Pedidos_Procesados);
                operation[OutParameter.CursorPedidosProcesados] = FillPedidosProcess(GetDtParameter(OutParameter.CursorPedidosProcesados));
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return operation;
        }

        public void InsertFormaPagoPedido(Pedido pedidoRQ,PedidoRS pedidoRS)
        {
            try
            {
                #region Parameter
                AddParameter("pNumIdPedido_in", OracleDbType.Int32, pedidoRS.IdPedido, ParameterDirection.Input);
                AddParameter("pNumIdFormaPago_in", OracleDbType.Int16, (pedidoRQ.CodePasarelaPago == Constantes_SafetyPay.CodeSafetyPayOnline ? Constantes_Pedido.ID_FORMA_PAGO_SAFETYPAY_ONLINE : 0), ParameterDirection.Input);
                AddParameter("pChrTipoTarj_in", OracleDbType.Char, "", ParameterDirection.Input,2);
                AddParameter("pIntTotalPtos_in", OracleDbType.Int32, 0, ParameterDirection.Input);
                AddParameter("pIntPtosPago_in", OracleDbType.Int32, 0, ParameterDirection.Input);
                AddParameter("pNumMontoPago_in", OracleDbType.Decimal, pedidoRQ.Monto, ParameterDirection.Input,10);
                AddParameter("pNumPuntoXDolar_in", OracleDbType.Decimal, 0, ParameterDirection.Input,10);
                AddParameter("pNumTipoCambio_in", OracleDbType.Decimal, 0, ParameterDirection.Input, 10);
                AddParameter("pVarCodAuthPtos_in", OracleDbType.Varchar2, "", ParameterDirection.Input, 100);                
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Insert_FormaPago_Pedido);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update_FechaExpira_Pedido(Pedido pedidoRQ, PedidoRS pedidoRS)
        {
            try
            {
                #region Parameter
                AddParameter("pNumIdPedido_in", OracleDbType.Int32, pedidoRS.IdPedido, ParameterDirection.Input);
                AddParameter("pdatefeChaExpira", OracleDbType.Date, DateTime.Now.AddHours(Convert.ToDouble(pedidoRQ.TiempoExpiracionCIP)), ParameterDirection.Input);               
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Update_FechaExpira_Pedido);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update_Pedido_Process(PedidosProcesados PedidosProccess)
        {
            try
            {
                #region Parameter
                AddParameter("pNumIdPedido_in", OracleDbType.Int32, PedidosProccess.idPedido, ParameterDirection.Input);
                AddParameter("pVarEstadoProcessPed_in", OracleDbType.Char, PedidosProccess.estadoProcess, ParameterDirection.Input,1);                
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Update_Pedido_Procesado);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RptaPagoSafetyPay Get_Rpta_SagetyPay(int IdPedido)
        {
            try
            {
                RptaPagoSafetyPay _RptaPagoSafetyPay = new RptaPagoSafetyPay();

                #region Parameter
                AddParameter("pNumIdPedido_in", OracleDbType.Int32, IdPedido, ParameterDirection.Input);
                AddParameter(OutParameter.CursorSafetyPay, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
                #endregion

                #region Invoke                
                ExecuteStoredProcedure(StoredProcedureName.AW_Rpta_SafetyPay);
                _RptaPagoSafetyPay = FillRptaSafetyPay(GetDtParameter(OutParameter.CursorSafetyPay));
                _RptaPagoSafetyPay.lstAmountType = Get_Monedas_PedidoSafetyPay(IdPedido);
                #endregion

                return _RptaPagoSafetyPay;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AmountType> Get_Monedas_PedidoSafetyPay(int IdPedido)
        {
            try
            {
                List<AmountType> lstMonedas = new List<AmountType>();

                #region Parameter
                AddParameter("pNumIdPedido_in", OracleDbType.Int32, IdPedido, ParameterDirection.Input);
                AddParameter(OutParameter.CursorMonedasPedidoSF, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
                #endregion

                #region Invoke                
                ExecuteStoredProcedure(StoredProcedureName.AW_Get_Monedas_PedidoSF);
                
                foreach (DataRow row in GetDtParameter(OutParameter.CursorMonedasPedidoSF).Rows)
                {
                    AmountType objAmountType = new AmountType();
                    objAmountType.CurrencyID = row["MONEDA"].ToString();
                    objAmountType.Value = Convert.ToDecimal(row["MONTO"]);
                    lstMonedas.Add(objAmountType);
                }
                #endregion

                return lstMonedas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PasarelaPago_Pedido> Get_Pedido_XSolicitud(Nullable<int> pIntIdResVue, Nullable<int> pIntIdResPaq, string pStrTipoPaq, Nullable<int> pIntIdWeb, Nullable<int> pIntIdCotSRV, Nullable<int> pIntIdResAuto, Nullable<int> pIntIdResSeguro)
        {
            try
            {
                List<PasarelaPago_Pedido> lstPedidos = new List<PasarelaPago_Pedido>();
                PasarelaPago_Pedido PasarelaPago_Pedido = null;

                #region Parameter             
                if (pIntIdResVue.HasValue)
                    AddParameter("pNumIdResVue_in", OracleDbType.Int32, pIntIdResVue.Value, ParameterDirection.Input);
                else
                    AddParameter("pNumIdResVue_in", OracleDbType.Int32, 0, ParameterDirection.Input);
                if (pIntIdResPaq.HasValue)
                    AddParameter("pNumIdResPaq_in", OracleDbType.Int32, pIntIdResPaq.Value, ParameterDirection.Input);
                else
                    AddParameter("pNumIdResPaq_in", OracleDbType.Int32, 0, ParameterDirection.Input);
                if (!string.IsNullOrEmpty(pStrTipoPaq))
                    AddParameter("pChrTipoPaq_in", OracleDbType.Char, pStrTipoPaq, ParameterDirection.Input, 1);
                else
                    AddParameter("pChrTipoPaq_in", OracleDbType.Char, "0", ParameterDirection.Input, 1);
                if (pIntIdWeb.HasValue)
                    AddParameter("pNumIdWeb_in", OracleDbType.Int32, pIntIdWeb.Value, ParameterDirection.Input);
                else
                    AddParameter("pNumIdWeb_in", OracleDbType.Int32, 0, ParameterDirection.Input);
                if (pIntIdCotSRV.HasValue)
                    AddParameter("pNumIdCotSRV_in", OracleDbType.Int32, pIntIdCotSRV.Value, ParameterDirection.Input);
                else
                    AddParameter("pNumIdCotSRV_in", OracleDbType.Int32, 0, ParameterDirection.Input);
                if (pIntIdResAuto.HasValue)
                    AddParameter("pNumIdResAuto_in", OracleDbType.Int32, pIntIdResAuto.Value, ParameterDirection.Input);
                else
                    AddParameter("pNumIdResAuto_in", OracleDbType.Int32, 0, ParameterDirection.Input);
                if (pIntIdResSeguro.HasValue)
                    AddParameter("pNumIdResSeguro_in", OracleDbType.Int32, pIntIdResSeguro.Value, ParameterDirection.Input);
                else
                    AddParameter("pNumIdResSeguro_in", OracleDbType.Int32, 0, ParameterDirection.Input);

                AddParameter(OutParameter.CursorPedidosBySolicitud, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
                #endregion

                #region Invoke                
                ExecuteStoredProcedure(StoredProcedureName.AW_Get_PedidoXSolicitud);
                
                FormaPagoPedido objFormaPagoPedido;
                RptaPagoVisa objRptaPagoVisa;
                RptaPagoMastercard objRptaPagoMastercard;
                RptaPagoUATP objRptaPagoUATP;
                RptaPagoEfectivoEC objRptaPagoEfectivoEC;

                foreach (DataRow row in GetDtParameter(OutParameter.CursorPedidosBySolicitud).Rows)
                {
                    if(row != null)
                    {
                        PasarelaPago_Pedido = new PasarelaPago_Pedido();
                        PasarelaPago_Pedido.IdPedido = (Int32)(row["NRO_PEDIDO"]);
                        PasarelaPago_Pedido.FechaPedido = (DateTime)row["TX_FECHA"];
                        PasarelaPago_Pedido.TipoMotor = Convert.ToString(row["MOTOR_TIPO"]);
                        PasarelaPago_Pedido.IdWeb = (Int32)(row["WEBS_CID"]);
                        if (row["RES_VUE_ID"] != null)
                            PasarelaPago_Pedido.IdReservaVuelo = (Int32)(row["RES_VUE_ID"]);
                        if (row["RES_PAQ_ID"] != null)
                            PasarelaPago_Pedido.IdReservaPaquete = (Int32)(row["RES_PAQ_ID"]);
                        if (row["RES_PAQ_TIPO"] != null)
                            PasarelaPago_Pedido.TipoPaquete = Convert.ToString(row["RES_PAQ_TIPO"]);
                        if (row["IP_USUARIO"] != null)
                            PasarelaPago_Pedido.IPUsuario = Convert.ToString(row["IP_USUARIO"]);
                        if (row["PAIS_ID"] != null)
                            PasarelaPago_Pedido.IdPaisUsuario = Convert.ToString(row["PAIS_ID"]);
                        if (row["COU_CNAME"] != null)
                            PasarelaPago_Pedido.NomPaisUsuario = Convert.ToString(row["COU_CNAME"]);
                        if (row["BROWSER_USUARIO"] != null)
                            PasarelaPago_Pedido.BrowserUsuario = Convert.ToString(row["BROWSER_USUARIO"]);
                        if (row["OK_PAGO_TARJETA"].ToString() == "1")
                            PasarelaPago_Pedido.OKPagoTarjeta = true;
                        if (row["OK_PAGO_PTOS"].ToString() == "1")
                        {
                            PasarelaPago_Pedido.OKPagoPuntos = true;
                        }                            
                        PasarelaPago_Pedido.IdEstadoPedido = (Int16)(row["ESTPED_ID"]);
                        PasarelaPago_Pedido.NomEstadoPedido = Convert.ToString(row["ESTPED_NOM"]);
                        if (row["MONTO_TARJETA"] != null)
                            PasarelaPago_Pedido.MontoTarjeta = Convert.ToDouble(row["MONTO_TARJETA"]);
                        if (row["COTSRV_ID"] != null)
                            PasarelaPago_Pedido.IdCotSRV = (Int32)(row["COTSRV_ID"]);
                        if (row["MONTO_DSCTO_SUBV"] != null)
                            PasarelaPago_Pedido.MontoDscto = Convert.ToDouble(row["MONTO_DSCTO_SUBV"]);
                        if (row["SERV_DETALLE"] != null)
                            PasarelaPago_Pedido.DetalleServicio = Convert.ToString(row["SERV_DETALLE"]);
                        if (row["ES_UATP"] != null)
                        {
                            if (row["ES_UATP"].ToString() == "1")
                                PasarelaPago_Pedido.EsUATP = true;

                            if (row["FECHA_EXPIRA"] != null)
                                PasarelaPago_Pedido.datFechaexpira = (DateTime)row["FECHA_EXPIRA"];
                        }
                        if (row["FECHA_EXPIRACION"] != null)
                            PasarelaPago_Pedido.datFechaexpiracion = (DateTime)row["FECHA_EXPIRACION"];

                        if (row["ID_LA_VALIDADORA_PED"] != null)
                            PasarelaPago_Pedido.IdLAValidadora = Convert.ToString(row["ID_LA_VALIDADORA_PED"]);
                        else if (row["ID_LA_VALIDADORA"] != null)
                            PasarelaPago_Pedido.IdLAValidadora = Convert.ToString(row["ID_LA_VALIDADORA"]);
                        if (row["ES_FONOPAGO"] != null)
                        {
                            if (row["ES_FONOPAGO"].ToString() == "1")
                                PasarelaPago_Pedido.EsFonoPago = true;
                        }
                        if (row["ID_RECIBO"] != null)
                            PasarelaPago_Pedido.IdRecibo = (Int32)(row["ID_RECIBO"]);
                        if (row["ID_SUCURSAL_RC"] != null)
                            PasarelaPago_Pedido.IdSucursalRC = (Int32)(row["ID_SUCURSAL_RC"]);
                        if (row["FECHA_GENERA_RC"] != null)
                            PasarelaPago_Pedido.FechaGeneraRecibo = (DateTime)row["FECHA_GENERA_RC"];
                        if (row["NRO_PEDIDO_FORMA_PAGO"] != null)
                        {
                            objFormaPagoPedido = new FormaPagoPedido();
                            objFormaPagoPedido.IdPedido = (Int32)(row["NRO_PEDIDO_FORMA_PAGO"]);
                            if (row["CCO_CID"] != null)
                                objFormaPagoPedido.IdTipoTarjeta = Convert.ToString(row["CCO_CID"]);
                            if (row["TOTAL_PTOS_BOL"] != null)
                                objFormaPagoPedido.TotalPtos = (Int32)(row["TOTAL_PTOS_BOL"]);
                            if (row["PTOS_PAGO"] != null)
                                objFormaPagoPedido.PtosPagoCliente = (Int32)(row["PTOS_PAGO"]);
                            if (row["COD_AUTH_PTOS"] != null)
                                objFormaPagoPedido.CodAutorizacionPtos = Convert.ToString(row["COD_AUTH_PTOS"]);
                            if (row["FPAGO_ID"] != null)
                                objFormaPagoPedido.IdFormaPago = (Int16)(row["FPAGO_ID"]);
                            if (row["FPAGO_NOM"] != null)
                                objFormaPagoPedido.NomFormaPago = Convert.ToString(row["FPAGO_NOM"]);
                            if (row["TITULAR_TARJETA"] != null)
                                objFormaPagoPedido.TitularTarjeta = Convert.ToString(row["TITULAR_TARJETA"]);
                            if (row["NRO_CUOTAS_FP"] != null)
                                objFormaPagoPedido.NroCuotas = (Int16)(row["NRO_CUOTAS_FP"]);
                            if (row["TIPo_CUOTAS_FP"] != null)
                            {
                                objFormaPagoPedido.TipoCuotas = Convert.ToString(row["TIPo_CUOTAS_FP"]);
                            }                                
                            PasarelaPago_Pedido.FormaPagoPedido = objFormaPagoPedido;
                        }

                        if (row["NRO_PEDIDO_VI"] != null)
                        {
                            objRptaPagoVisa = new RptaPagoVisa();
                            objRptaPagoVisa.IdPedido = (Int32)(row["NRO_PEDIDO_VI"]);
                            if (row["HOST_RPTA"] != null)
                                objRptaPagoVisa.HostRespuesta = Convert.ToString(row["HOST_RPTA"]);
                            if (row["HOST_COD_TIENDA"] != null)
                                objRptaPagoVisa.CodTienda = Convert.ToString(row["HOST_COD_TIENDA"]);
                            if (row["HOST_COD_ACCION"] != null)
                                objRptaPagoVisa.HostCodAccion = Convert.ToString(row["HOST_COD_ACCION"]);
                            if (row["HOST_PAN"] != null)
                                objRptaPagoVisa.HostNroTarjeta = Convert.ToString(row["HOST_PAN"]);
                            if (row["HOST_ECI"] != null)
                                objRptaPagoVisa.HostECI = Convert.ToString(row["HOST_ECI"]);
                            if (row["HOST_COD_AUTORIZA"] != null)
                                objRptaPagoVisa.HostCodAutoriza = Convert.ToString(row["HOST_COD_AUTORIZA"]);
                            if (row["HOST_NOM_EMISOR"] != null)
                                objRptaPagoVisa.HostNomEmisor = Convert.ToString(row["HOST_NOM_EMISOR"]);
                            else if (row["TARJ_BANCO_EMISOR_VI"] != null)
                                objRptaPagoVisa.HostNomEmisor = Convert.ToString(row["TARJ_BANCO_EMISOR_VI"]);
                            if (row["HOST_IMP_AUTORIZADO"] != null)
                                objRptaPagoVisa.HostImporteAutorizado = Convert.ToDouble(row["HOST_IMP_AUTORIZADO"]);
                            if (row["HOST_MSG_ERROR"] != null)
                                objRptaPagoVisa.HostMensajeError = Convert.ToString(row["HOST_MSG_ERROR"]);
                            if (row["HOST_TARJETA_HABIENTE"] != null)
                            {
                                objRptaPagoVisa.HostTarjetaHabiente = Convert.ToString(row["HOST_TARJETA_HABIENTE"]);
                            }                                
                            PasarelaPago_Pedido.RptaPagoVisa = objRptaPagoVisa;
                        }

                        if (row["NRO_PEDIDO_MC"] != null)
                        {
                            objRptaPagoMastercard = new RptaPagoMastercard();
                            objRptaPagoMastercard.IdPedido = (Int32)(row["NRO_PEDIDO_MC"]);
                            if (row["RESULT_TX"] != null)
                                objRptaPagoMastercard.ResultTx = Convert.ToString(row["RESULT_TX"]);
                            if (row["COD_AUTH"] != null)
                                objRptaPagoMastercard.CodAuth = Convert.ToString(row["COD_AUTH"]);
                            if (row["MONTO_TTL"] != null)
                                objRptaPagoMastercard.MontoTotal = Convert.ToString(row["MONTO_TTL"]);
                            if (row["COD_RPTA"] != null)
                                objRptaPagoMastercard.CodRpta = Convert.ToString(row["COD_RPTA"]);
                            if (row["NRO_TARJETA"] != null)
                                objRptaPagoMastercard.NroTarjeta = Convert.ToString(row["NRO_TARJETA"]);
                            if (row["MENSAJE_RPTA"] != null)
                                objRptaPagoMastercard.MensajeRpta = Convert.ToString(row["MENSAJE_RPTA"]);
                            if (row["MSG_COD_RPTA"] != null)
                                objRptaPagoMastercard.MsgCodRpta = Convert.ToString(row["MSG_COD_RPTA"]);
                            if (row["NRO_CUOTAS"] != null)
                                objRptaPagoMastercard.NroCuotas = (row["NRO_CUOTAS"].ToString());
                            if (row["FEC_PRIMERA_CUOTA"] != null)
                                objRptaPagoMastercard.FecPrimeraCuota = Convert.ToString(row["FEC_PRIMERA_CUOTA"]);
                            if (row["MONEDA_CUOTA"] != null)
                                objRptaPagoMastercard.MonedaCuota = Convert.ToString(row["MONEDA_CUOTA"]);
                            if (row["MONTO_CUOTA"] != null)
                                objRptaPagoMastercard.MontoCuota = Convert.ToString(row["MONTO_CUOTA"]);
                            if (row["MONTO_TTL"] != null)
                                objRptaPagoMastercard.MontoTotal = Convert.ToString(row["MONTO_TTL"]);
                            if (row["TARJ_BANCO_EMISOR_MC"] != null)
                            {
                                objRptaPagoMastercard.NomBancoEmisor = Convert.ToString(row["TARJ_BANCO_EMISOR_MC"]);
                            }                                
                            PasarelaPago_Pedido.RptaPagoMastercard = objRptaPagoMastercard;
                        }

                        if (row["NRO_PEDIDO_UATP"] != null)
                        {
                            objRptaPagoUATP = new RptaPagoUATP();
                            objRptaPagoUATP.IdPedido = (Int32)(row["NRO_PEDIDO_UATP"]);
                            objRptaPagoUATP.FechaRpta = (DateTime)row["FECHA_RPTA_UATP"];
                            objRptaPagoUATP.IdTipoTarjeta = Convert.ToString(row["CCO_CID_UATP"]);
                            objRptaPagoUATP.NroTarjeta = Convert.ToString(row["TARJ_NRO_UATP"]);
                            objRptaPagoUATP.AnioExpiraTarjeta = Convert.ToString(row["TARJ_ANIO_EXPIRA"]);
                            objRptaPagoUATP.MesExpiraTarjeta = Convert.ToString(row["TARJ_MES_EXPIRA"]);
                            objRptaPagoUATP.TitularTarjeta = Convert.ToString(row["TARJ_TITULAR"]);
                            if (row["TARJ_COD_SEGURIDAD"] != null)
                                objRptaPagoUATP.CodSeguridadTarjeta = Convert.ToString(row["TARJ_COD_SEGURIDAD"]);
                            if (row["TARJ_BANCO_EMISOR"] != null)
                                objRptaPagoUATP.BancoEmisor = Convert.ToString(row["TARJ_BANCO_EMISOR"]);
                            if (row["RPTA_MENSAJE_UATP"] != null)
                                objRptaPagoUATP.MensajeRpta = Convert.ToString(row["RPTA_MENSAJE_UATP"]);
                            if (row["FECHA_COBRO_PAYU"] != null)
                                objRptaPagoUATP.Fechacobropayu = Convert.ToString(row["FECHA_COBRO_PAYU"]);
                            if (row["TIPO_DOC_TITULAR"] != null)
                                objRptaPagoUATP.IdTipoDocTitularTarjeta = Convert.ToString(row["TIPO_DOC_TITULAR"]);
                            if (row["NUM_DOC_TITULAR"] != null)
                                objRptaPagoUATP.NumDocTitularTarjeta = Convert.ToString(row["NUM_DOC_TITULAR"]);
                            if (row["RPTA_UATP"] != null)
                            {
                                if (row["RPTA_UATP"].ToString() == "1")
                                    objRptaPagoUATP.ResultadoOK = true;
                            }
                            if (row["TELF_USUARIO"] != null)
                            {
                                objRptaPagoUATP.TelfCliente = Convert.ToString(row["TELF_USUARIO"]);
                            }
                            PasarelaPago_Pedido.RptaPagoUATP = objRptaPagoUATP;
                        }

                        if (row["NRO_PEDIDO_PEEC"] != null)
                        {
                            objRptaPagoEfectivoEC = new RptaPagoEfectivoEC();
                            if (row["CIP"] != null)
                                objRptaPagoEfectivoEC.CIP = Convert.ToString(row["CIP"]);
                            if (row["PE_ESTADO"] != null)
                                objRptaPagoEfectivoEC.Estado = Convert.ToString(row["PE_ESTADO"]);
                            if (row["CIP_ESTADO"] != null)
                                objRptaPagoEfectivoEC.EstadoCIP = Convert.ToString(row["CIP_ESTADO"]);
                            if (row["CIP_ESTADO_CONCILIADO"] != null)
                                objRptaPagoEfectivoEC.EstadoConciliado = Convert.ToString(row["CIP_ESTADO_CONCILIADO"]);
                            if (row["CIP_FECHA_CANCELADO"] != null)
                                objRptaPagoEfectivoEC.FechaCancelado = (DateTime)row["CIP_FECHA_CANCELADO"];
                            if (row["CIP_FECHA_CONCILIADO"] != null)
                                objRptaPagoEfectivoEC.FechaConciliado = (DateTime)row["CIP_FECHA_CONCILIADO"];
                            if (row["FECHA_EXPIRA_PAGO"] != null)
                                objRptaPagoEfectivoEC.FechaExpiraPago = (DateTime)row["FECHA_EXPIRA_PAGO"];
                            if (row["CIP_FECHA_EXTORNO"] != null)
                                objRptaPagoEfectivoEC.FechaExtorno = (DateTime)row["CIP_FECHA_EXTORNO"];
                            if (row["PE_MENSAJE"] != null)
                                objRptaPagoEfectivoEC.MensajeRpta = Convert.ToString(row["PE_MENSAJE"]);
                            if (row["PE_ENVIO_RPTA_PAGO"] != null)
                            {
                                if (row["PE_ENVIO_RPTA_PAGO"].ToString() == "1")
                                    objRptaPagoEfectivoEC.EnvioRpta = true;
                            }
                            PasarelaPago_Pedido.RptaPagoEfectivoEC = objRptaPagoEfectivoEC;
                        }
                        if (row["TOTAL_FEE_VUE"] != null)
                            PasarelaPago_Pedido.FEE_ResVue = Convert.ToDouble(row["TOTAL_FEE_VUE"]);
                        else
                            PasarelaPago_Pedido.FEE_ResVue = 0;

                        if (row["NRO_PEDIDO_SP"] != null)
                        {
                            RptaPagoSafetyPay objRptaPagoSafetyPay = new RptaPagoSafetyPay();
                            if (row["EXPIRATIONDATETIME"] != null)
                                objRptaPagoSafetyPay.ExpirationDateTime = Convert.ToString(row["EXPIRATIONDATETIME"]);
                            if (row["OPERATIONID"] != null)
                                objRptaPagoSafetyPay.OperationId = Convert.ToString(row["OPERATIONID"]);
                            if (row["TRANSACTIONIDENTIFIER"] != null)
                                objRptaPagoSafetyPay.TransaccionIdentifier = Convert.ToString(row["TRANSACTIONIDENTIFIER"]);
                            if (row["ERRORMANAGER_SEVERITY"] != null)
                                objRptaPagoSafetyPay.ErrorManager_Severity = Convert.ToString(row["ERRORMANAGER_SEVERITY"]);
                            if (row["ERRORMANAGER_ERRORNUMBER"] != null)
                                objRptaPagoSafetyPay.ErrorManager_ErrorNumber = Convert.ToString(row["ERRORMANAGER_ERRORNUMBER"]);
                            if (row["ERRORMANAGER_DESCRIPCION"] != null)
                                objRptaPagoSafetyPay.ErrorManager_Description = Convert.ToString(row["ERRORMANAGER_DESCRIPCION"]);
                            if (row["BANK_REDIRECTURL"] != null)
                            {
                                objRptaPagoSafetyPay.BankRedirectUrl = Convert.ToString(row["BANK_REDIRECTURL"]);
                            }                                
                            PasarelaPago_Pedido.RptaPagoSafetyPay = objRptaPagoSafetyPay;
                        }
                        if (row["ESPAYU"] != null)
                        {
                            if (0 < (Int32)(row["ESPAYU"]))
                            {
                                PasarelaPago_Pedido.EsPayu = true;
                            }                                
                        }

                        // RCcancce - MT
                        if (row["PNR_INI"] != null)
                            PasarelaPago_Pedido.PNR_MT = (row["PNR_INI"].ToString().Trim());
                        else
                            PasarelaPago_Pedido.PNR_MT = "";
                        // Fin RC

                        lstPedidos.Add(PasarelaPago_Pedido);

                    }
                }
                #endregion

                return lstPedidos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FormaPagoPedido Get_FormaPagoBy_IdPedido(int IdPedido)
        {
            try
            {
                FormaPagoPedido FormaPagoPedido = null;

                #region Parameter
                AddParameter("pNumIdPedido_in", OracleDbType.Int32, IdPedido, ParameterDirection.Input);
                AddParameter(OutParameter.CursorFormaPagoBy_IdPedido, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
                #endregion

                #region Invoke                
                ExecuteStoredProcedure(StoredProcedureName.AW_Get_FormaPagoBy_IdPedido);

                foreach (DataRow row in GetDtParameter(OutParameter.CursorFormaPagoBy_IdPedido).Rows)
                {
                    if(row != null)
                    {
                        FormaPagoPedido.IdPedido = (Int32)(row["NRO_PEDIDO"]);
                        if (row["CCO_CID"] != null)
                            FormaPagoPedido.IdTipoTarjeta = row["CCO_CID"].ToString();
                        if (row["TOTAL_PTOS_BOL"] != null)
                            FormaPagoPedido.TotalPtos = (Int32)(row["TOTAL_PTOS_BOL"]);
                        if (row["PTOS_PAGO"] != null)
                            FormaPagoPedido.PtosPagoCliente = (Int32)(row["PTOS_PAGO"]);
                        if (row["MONTO_PAGO"] != null)
                            FormaPagoPedido.MontoPago = Convert.ToDouble(row["MONTO_PAGO"]);
                        if (row["PUNTOXDOLAR"] != null)
                            FormaPagoPedido.PuntosXDolar = Convert.ToDouble(row["PUNTOXDOLAR"]);
                        if (row["TIPO_CAMBIO"] != null)
                            FormaPagoPedido.TipoCambio = Convert.ToDouble(row["TIPO_CAMBIO"]);
                        if (row["COD_AUTH_PTOS"] != null)
                            FormaPagoPedido.CodAutorizacionPtos = row["COD_AUTH_PTOS"].ToString();
                        if (row["FPAGO_ID"] != null)
                            FormaPagoPedido.IdFormaPago = (Int16)(row["FPAGO_ID"]);
                        if (row["CCO_CNAME"] != null)
                            FormaPagoPedido.NomTarjeta = row["CCO_CNAME"].ToString();
                        if (row["FPAGO_NOM"] != null)
                            FormaPagoPedido.NomFormaPago = row["FPAGO_NOM"].ToString();
                        if (row["TITULAR_TARJETA"] != null)
                            FormaPagoPedido.TitularTarjeta = row["TITULAR_TARJETA"].ToString();
                        if (row["AUTHORIZATIONCODE"] != null)
                            FormaPagoPedido.AutorizationTarjeta = row["AUTHORIZATIONCODE"].ToString();
                        if (row["TARJ_BANCO_EMISOR"] != null)
                            FormaPagoPedido.BancoEmisorTarjeta = row["TARJ_BANCO_EMISOR"].ToString();
                        if (row["ES_PAYU"] != null)
                            FormaPagoPedido.EsPayu = row["ES_PAYU"].ToString();
                    }
                }
                #endregion

                return FormaPagoPedido;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Auxiliares
        private RptaPagoSafetyPay FillRptaSafetyPay(DataTable dt = null)
        {
            try
            {
                RptaPagoSafetyPay rptaPagoSafety = new RptaPagoSafetyPay();
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        #region Loading
                        rptaPagoSafety.IdPedido = System.Convert.ToInt32(row["NRO_PEDIDO"]);
                        if (row["RESPONSEDATETIME"] != null)
                            rptaPagoSafety.ResponseDateTime = row["RESPONSEDATETIME"].ToString();
                        if (row["ERRORMANAGER_DESCRIPCION"] != null)
                            rptaPagoSafety.ErrorManager_Description = row["ERRORMANAGER_DESCRIPCION"].ToString();
                        if (row["OPERATIONID"] != null)
                            rptaPagoSafety.OperationId = row["OPERATIONID"].ToString();
                        if (row["TRANSACTIONIDENTIFIER"] != null)
                            rptaPagoSafety.TransaccionIdentifier = row["TRANSACTIONIDENTIFIER"].ToString();
                        if (row["EXPIRATIONDATETIME"] != null)
                            rptaPagoSafety.ExpirationDateTime = row["EXPIRATIONDATETIME"].ToString();
                        if (row["FECHA_EXPIRA"] != null)
                            rptaPagoSafety.FechaExpiracion = Convert.ToDateTime(row["FECHA_EXPIRA"]);
                        if (row["SIGNATURE"] != null)
                            rptaPagoSafety.Signature = row["SIGNATURE"].ToString();
                        if (row["ERRORMANAGER_SEVERITY"] != null)
                            rptaPagoSafety.ErrorManager_Severity = row["ERRORMANAGER_SEVERITY"].ToString();
                        if (row["ERRORMANAGER_ERRORNUMBER"] != null)
                            rptaPagoSafety.ErrorManager_ErrorNumber = row["ERRORMANAGER_ERRORNUMBER"].ToString();
                        if (row["BANK_REDIRECTURL"] != null)
                            rptaPagoSafety.BankRedirectUrl = row["BANK_REDIRECTURL"].ToString();
                        #endregion     
                        break;
                    }
                }
                return rptaPagoSafety;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private IEnumerable<PedidosProcesados> FillPedidosProcess(DataTable dt)
        {
            try
            {
                var pedidosProcesadosList = new List<PedidosProcesados>();

                if(dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        #region AddingElement
                        pedidosProcesadosList.Add(new PedidosProcesados()
                        {
                            idPedido = (row["NRO_PEDIDO"] != null ? Convert.ToInt32(row["NRO_PEDIDO"]) : 0),
                            codigoTransaccion = (row["TRANSACTIONIDENTIFIER"] != null ? row["TRANSACTIONIDENTIFIER"].ToString() : string.Empty),
                            idSolicitudPago_SF = (row["ID_OPORTUNIDAD"] != null ? row["ID_OPORTUNIDAD"].ToString() : string.Empty),
                            estadoPago = (row["ESTADO"] != null ? row["ESTADO"].ToString() : string.Empty),
                            estadoProcess = (row["PROCESS_CRM"] != null ? row["PROCESS_CRM"].ToString() : string.Empty)
                        });
                        #endregion
                    }
                }
                
                return pedidosProcesadosList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}