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

        public Operation CreateNM(DatosPedido pedido)
        {
            try
            {
                var operation = new Operation();
                int intIdNewPedido = 0;

                #region Parameter
                AddParameter("pChrMotorTipo_in", OracleDbType.Char, Constantes_Pedido.ID_TIPO_PEDIDO_OTROS, ParameterDirection.Input, 3);
                AddParameter("pVarIdSesion_in", OracleDbType.Varchar2, null, ParameterDirection.Input, 50);
                AddParameter("pNumIdResVue_in", OracleDbType.Int32, null, ParameterDirection.Input);
                AddParameter("pNumIdResPaq_in", OracleDbType.Int32, null, ParameterDirection.Input);
                AddParameter("pChrTipoPaq_in", OracleDbType.Char, null, ParameterDirection.Input, 1);
                AddParameter("pNumIdWeb_in", OracleDbType.Int32, pedido.IdWeb, ParameterDirection.Input);
                AddParameter("pNumIdLang_in", OracleDbType.Int32, pedido.IdLang, ParameterDirection.Input);
                AddParameter("pVarIP_in", OracleDbType.Varchar2, (string.IsNullOrEmpty(pedido.IPUsuario) ? "127.0.0.0" : pedido.IPUsuario), ParameterDirection.Input, 30);
                AddParameter("pVarBrowser_in", OracleDbType.Varchar2, (string.IsNullOrEmpty(pedido.Browser) ? "Servicio Saleforce" : pedido.IPUsuario), ParameterDirection.Input, 200);
                AddParameter("pNumIdCotSRV_in", OracleDbType.Int32, pedido.IdCotVta, ParameterDirection.Input);
                AddParameter("pVarDetalleServ_in", OracleDbType.Varchar2, pedido.DetalleServicio, ParameterDirection.Input, 1000);
                AddParameter("pNumMonto_in", OracleDbType.Double, Convert.ToDouble(pedido.Monto), ParameterDirection.Input);

                AddParameter(OutParameter.IdPedido, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Create_Pedido);

                operation[OutParameter.IdPedido] = GetOutParameter(OutParameter.IdPedido);
                operation[Operation.Result] = ResultType.Success;

                intIdNewPedido = Convert.ToInt32(GetOutParameter(OutParameter.IdPedido).ToString());
                if (pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_TARJETA_UATP)
                {
                    _Update_Pedido_EsUATP(intIdNewPedido, pedido);
                }
                #endregion

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void _Update_Pedido_EsUATP(int intIdNewPedido, DatosPedido pedido)
        {
            try
            {
                #region Parameter
                AddParameter("pNumIdPedido_in", OracleDbType.Int32, intIdNewPedido, ParameterDirection.Input);
                AddParameter("pChrEsUATP_in", OracleDbType.Char, (pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_TARJETA_UATP ? "1" : "0"), ParameterDirection.Input, 1);
                AddParameter("pChrIdLAValidadora_in", OracleDbType.Char, (pedido.CodePasarelaPago == Constantes_MetodoDePago.CODE_FPAGO_TARJETA_UATP ? pedido.LineaAereaValidadora : ""), ParameterDirection.Input, 2);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Update_Pedido_EsUATP);
                #endregion
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

        public void InsertFormaPagoPedidoNM(DatosPedido pedidoRQ, PedidoRS pedidoRS, int intIdFormaPago)
        {
            try
            {
                #region Parameter
                AddParameter("pNumIdPedido_in", OracleDbType.Int32, pedidoRS.IdPedido, ParameterDirection.Input);
                AddParameter("pNumIdFormaPago_in", OracleDbType.Int16, intIdFormaPago);
                //AddParameter("pChrTipoTarj_in", OracleDbType.Char, pedidoRQ.CodePasarelaPago, ParameterDirection.Input, 2);
                AddParameter("pChrTipoTarj_in", OracleDbType.Char, "", ParameterDirection.Input, 2);
                AddParameter("pIntTotalPtos_in", OracleDbType.Int32, 0, ParameterDirection.Input);
                AddParameter("pIntPtosPago_in", OracleDbType.Int32, 0, ParameterDirection.Input);
                AddParameter("pNumMontoPago_in", OracleDbType.Decimal, pedidoRQ.Monto, ParameterDirection.Input, 10);
                AddParameter("pNumPuntoXDolar_in", OracleDbType.Decimal, 0, ParameterDirection.Input, 10);
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

        public void Update_FechaExpira_PedidoNM(DatosPedido pedidoRQ, PedidoRS pedidoRS)
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

        public void Update_Pedido_SolicitudPago_SF(int idPedido, int idSrv, string IdOportunidad_SF, string IdSolicitudpago_SF)
        {
            try
            {
                #region Parameter
                AddParameter("pNumIdPedido_in", OracleDbType.Int32, idPedido, ParameterDirection.Input);
                AddParameter("pCotSrvId_in", OracleDbType.Int32, idSrv, ParameterDirection.Input);
                AddParameter("pIdOportunidad_in", OracleDbType.Varchar2, IdOportunidad_SF, ParameterDirection.Input);
                AddParameter("pIdSolicitudPago_in", OracleDbType.Varchar2, IdSolicitudpago_SF, ParameterDirection.Input);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Update_Pedido_SolicitudPago_SF);
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
                        PasarelaPago_Pedido.IdPedido = Convert.ToInt32(row["NRO_PEDIDO"]);
                        PasarelaPago_Pedido.FechaPedido = (DateTime)row["TX_FECHA"];
                        PasarelaPago_Pedido.TipoMotor = Convert.ToString(row["MOTOR_TIPO"]);
                        PasarelaPago_Pedido.IdWeb = Convert.ToInt32(row["WEBS_CID"].ToString());
                        if (!Convert.IsDBNull(row["RES_VUE_ID"]))
                            PasarelaPago_Pedido.IdReservaVuelo = Convert.ToInt32(row["RES_VUE_ID"]);
                        if (!Convert.IsDBNull(row["RES_PAQ_ID"]))
                            PasarelaPago_Pedido.IdReservaPaquete = Convert.ToInt32(row["RES_PAQ_ID"]);
                        if (!Convert.IsDBNull(row["RES_PAQ_TIPO"]))
                            PasarelaPago_Pedido.TipoPaquete = Convert.ToString(row["RES_PAQ_TIPO"]);
                        if (!Convert.IsDBNull(row["IP_USUARIO"]))
                            PasarelaPago_Pedido.IPUsuario = Convert.ToString(row["IP_USUARIO"]);
                        if (!Convert.IsDBNull(row["PAIS_ID"]))
                            PasarelaPago_Pedido.IdPaisUsuario = Convert.ToString(row["PAIS_ID"]);
                        if (!Convert.IsDBNull(row["COU_CNAME"]))
                            PasarelaPago_Pedido.NomPaisUsuario = Convert.ToString(row["COU_CNAME"]);
                        if (!Convert.IsDBNull(row["BROWSER_USUARIO"]))
                            PasarelaPago_Pedido.BrowserUsuario = Convert.ToString(row["BROWSER_USUARIO"]);
                        if (row["OK_PAGO_TARJETA"].ToString() == "1")
                            PasarelaPago_Pedido.OKPagoTarjeta = true;
                        if (row["OK_PAGO_PTOS"].ToString() == "1")
                        {
                            PasarelaPago_Pedido.OKPagoPuntos = true;
                        }                            
                        PasarelaPago_Pedido.IdEstadoPedido = Convert.ToInt16(row["ESTPED_ID"]);
                        PasarelaPago_Pedido.NomEstadoPedido = Convert.ToString(row["ESTPED_NOM"]);
                        if (!Convert.IsDBNull(row["MONTO_TARJETA"]))
                            PasarelaPago_Pedido.MontoTarjeta = Convert.ToDouble(row["MONTO_TARJETA"]);
                        if (!Convert.IsDBNull(row["COTSRV_ID"]))
                            PasarelaPago_Pedido.IdCotSRV = Convert.ToInt32(row["COTSRV_ID"]);
                        if (!Convert.IsDBNull(row["MONTO_DSCTO_SUBV"]))
                            PasarelaPago_Pedido.MontoDscto = Convert.ToDouble(row["MONTO_DSCTO_SUBV"]);
                        if (!Convert.IsDBNull(row["SERV_DETALLE"]))
                            PasarelaPago_Pedido.DetalleServicio = Convert.ToString(row["SERV_DETALLE"]);
                        if (!Convert.IsDBNull(row["ES_UATP"]))
                        {
                            if (row["ES_UATP"].ToString() == "1")
                                PasarelaPago_Pedido.EsUATP = true;

                            if (!Convert.IsDBNull(row["FECHA_EXPIRA"]))
                                PasarelaPago_Pedido.datFechaexpira = (DateTime)row["FECHA_EXPIRA"];
                        }
                        if (!Convert.IsDBNull(row["FECHA_EXPIRACION"]))
                            PasarelaPago_Pedido.datFechaexpiracion = (DateTime)row["FECHA_EXPIRACION"];

                        if (!Convert.IsDBNull(row["ID_LA_VALIDADORA_PED"]))
                            PasarelaPago_Pedido.IdLAValidadora = Convert.ToString(row["ID_LA_VALIDADORA_PED"]);
                        else if (!Convert.IsDBNull(row["ID_LA_VALIDADORA"]))
                            PasarelaPago_Pedido.IdLAValidadora = Convert.ToString(row["ID_LA_VALIDADORA"]);
                        if (!Convert.IsDBNull(row["ES_FONOPAGO"]))
                        {
                            if (row["ES_FONOPAGO"].ToString() == "1")
                                PasarelaPago_Pedido.EsFonoPago = true;
                        }
                        if (!Convert.IsDBNull(row["ID_RECIBO"]))
                            PasarelaPago_Pedido.IdRecibo = Convert.ToInt32(row["ID_RECIBO"]);
                        if (!Convert.IsDBNull(row["ID_SUCURSAL_RC"]))
                            PasarelaPago_Pedido.IdSucursalRC = Convert.ToInt32(row["ID_SUCURSAL_RC"]);
                        if (!Convert.IsDBNull(row["FECHA_GENERA_RC"]))
                            PasarelaPago_Pedido.FechaGeneraRecibo = (DateTime)row["FECHA_GENERA_RC"];
                        if (!Convert.IsDBNull(row["NRO_PEDIDO_FORMA_PAGO"]))
                        {
                            objFormaPagoPedido = new FormaPagoPedido();
                            objFormaPagoPedido.IdPedido = Convert.ToInt32(row["NRO_PEDIDO_FORMA_PAGO"]);
                            if (!Convert.IsDBNull(row["CCO_CID"]))
                                objFormaPagoPedido.IdTipoTarjeta = Convert.ToString(row["CCO_CID"]);
                            if (!Convert.IsDBNull(row["TOTAL_PTOS_BOL"]))
                                objFormaPagoPedido.TotalPtos = Convert.ToInt32(row["TOTAL_PTOS_BOL"]);
                            if (!Convert.IsDBNull(row["PTOS_PAGO"]))
                                objFormaPagoPedido.PtosPagoCliente = Convert.ToInt32(row["PTOS_PAGO"]);
                            if (!Convert.IsDBNull(row["COD_AUTH_PTOS"]))
                                objFormaPagoPedido.CodAutorizacionPtos = Convert.ToString(row["COD_AUTH_PTOS"]);
                            if (!Convert.IsDBNull(row["FPAGO_ID"]))
                                objFormaPagoPedido.IdFormaPago = Convert.ToInt16(row["FPAGO_ID"]);
                            if (!Convert.IsDBNull(row["FPAGO_NOM"]))
                                objFormaPagoPedido.NomFormaPago = Convert.ToString(row["FPAGO_NOM"]);
                            if (!Convert.IsDBNull(row["TITULAR_TARJETA"]))
                                objFormaPagoPedido.TitularTarjeta = Convert.ToString(row["TITULAR_TARJETA"]);
                            if (!Convert.IsDBNull(row["NRO_CUOTAS_FP"]))
                                objFormaPagoPedido.NroCuotas = Convert.ToInt16(row["NRO_CUOTAS_FP"]);
                            if (!Convert.IsDBNull(row["TIPo_CUOTAS_FP"]))
                            {
                                objFormaPagoPedido.TipoCuotas = Convert.ToString(row["TIPo_CUOTAS_FP"]);
                            }                                
                            PasarelaPago_Pedido.FormaPagoPedido = objFormaPagoPedido;
                        }

                        if (!Convert.IsDBNull(row["NRO_PEDIDO_VI"]))
                        {
                            objRptaPagoVisa = new RptaPagoVisa();
                            objRptaPagoVisa.IdPedido = Convert.ToInt32(row["NRO_PEDIDO_VI"]);
                            if (!Convert.IsDBNull(row["HOST_RPTA"]))
                                objRptaPagoVisa.HostRespuesta = Convert.ToString(row["HOST_RPTA"]);
                            if (!Convert.IsDBNull(row["HOST_COD_TIENDA"]))
                                objRptaPagoVisa.CodTienda = Convert.ToString(row["HOST_COD_TIENDA"]);
                            if (!Convert.IsDBNull(row["HOST_COD_ACCION"]))
                                objRptaPagoVisa.HostCodAccion = Convert.ToString(row["HOST_COD_ACCION"]);
                            if (!Convert.IsDBNull(row["HOST_PAN"]))
                                objRptaPagoVisa.HostNroTarjeta = Convert.ToString(row["HOST_PAN"]);
                            if (!Convert.IsDBNull(row["HOST_ECI"]))
                                objRptaPagoVisa.HostECI = Convert.ToString(row["HOST_ECI"]);
                            if (!Convert.IsDBNull(row["HOST_COD_AUTORIZA"]))
                                objRptaPagoVisa.HostCodAutoriza = Convert.ToString(row["HOST_COD_AUTORIZA"]);
                            if (!Convert.IsDBNull(row["HOST_NOM_EMISOR"]))
                                objRptaPagoVisa.HostNomEmisor = Convert.ToString(row["HOST_NOM_EMISOR"]);
                            else if (!Convert.IsDBNull(row["TARJ_BANCO_EMISOR_VI"]))
                                objRptaPagoVisa.HostNomEmisor = Convert.ToString(row["TARJ_BANCO_EMISOR_VI"]);
                            if (!Convert.IsDBNull(row["HOST_IMP_AUTORIZADO"]))
                                objRptaPagoVisa.HostImporteAutorizado = Convert.ToDouble(row["HOST_IMP_AUTORIZADO"]);
                            if (!Convert.IsDBNull(row["HOST_MSG_ERROR"]))
                                objRptaPagoVisa.HostMensajeError = Convert.ToString(row["HOST_MSG_ERROR"]);
                            if (!Convert.IsDBNull(row["HOST_TARJETA_HABIENTE"]))
                            {
                                objRptaPagoVisa.HostTarjetaHabiente = Convert.ToString(row["HOST_TARJETA_HABIENTE"]);
                            }                                
                            PasarelaPago_Pedido.RptaPagoVisa = objRptaPagoVisa;
                        }

                        if (!Convert.IsDBNull(row["NRO_PEDIDO_MC"]))
                        {
                            objRptaPagoMastercard = new RptaPagoMastercard();
                            objRptaPagoMastercard.IdPedido = Convert.ToInt32(row["NRO_PEDIDO_MC"]);
                            if (!Convert.IsDBNull(row["RESULT_TX"]))
                                objRptaPagoMastercard.ResultTx = Convert.ToString(row["RESULT_TX"]);
                            if (!Convert.IsDBNull(row["COD_AUTH"]))
                                objRptaPagoMastercard.CodAuth = Convert.ToString(row["COD_AUTH"]);
                            if (!Convert.IsDBNull(row["MONTO_TTL"]))
                                objRptaPagoMastercard.MontoTotal = Convert.ToString(row["MONTO_TTL"]);
                            if (!Convert.IsDBNull(row["COD_RPTA"]))
                                objRptaPagoMastercard.CodRpta = Convert.ToString(row["COD_RPTA"]);
                            if (!Convert.IsDBNull(row["NRO_TARJETA"]))
                                objRptaPagoMastercard.NroTarjeta = Convert.ToString(row["NRO_TARJETA"]);
                            if (!Convert.IsDBNull(row["MENSAJE_RPTA"]))
                                objRptaPagoMastercard.MensajeRpta = Convert.ToString(row["MENSAJE_RPTA"]);
                            if (!Convert.IsDBNull(row["MSG_COD_RPTA"]))
                                objRptaPagoMastercard.MsgCodRpta = Convert.ToString(row["MSG_COD_RPTA"]);
                            if (!Convert.IsDBNull(row["NRO_CUOTAS"]))
                                objRptaPagoMastercard.NroCuotas = (row["NRO_CUOTAS"].ToString());
                            if (!Convert.IsDBNull(row["FEC_PRIMERA_CUOTA"]))
                                objRptaPagoMastercard.FecPrimeraCuota = Convert.ToString(row["FEC_PRIMERA_CUOTA"]);
                            if (!Convert.IsDBNull(row["MONEDA_CUOTA"]))
                                objRptaPagoMastercard.MonedaCuota = Convert.ToString(row["MONEDA_CUOTA"]);
                            if (!Convert.IsDBNull(row["MONTO_CUOTA"]))
                                objRptaPagoMastercard.MontoCuota = Convert.ToString(row["MONTO_CUOTA"]);
                            if (!Convert.IsDBNull(row["MONTO_TTL"]))
                                objRptaPagoMastercard.MontoTotal = Convert.ToString(row["MONTO_TTL"]);
                            if (!Convert.IsDBNull(row["TARJ_BANCO_EMISOR_MC"]))
                            {
                                objRptaPagoMastercard.NomBancoEmisor = Convert.ToString(row["TARJ_BANCO_EMISOR_MC"]);
                            }                                
                            PasarelaPago_Pedido.RptaPagoMastercard = objRptaPagoMastercard;
                        }

                        if (!Convert.IsDBNull(row["NRO_PEDIDO_UATP"]))
                        {
                            objRptaPagoUATP = new RptaPagoUATP();
                            objRptaPagoUATP.IdPedido = Convert.ToInt32(row["NRO_PEDIDO_UATP"]);
                            objRptaPagoUATP.FechaRpta = (DateTime)row["FECHA_RPTA_UATP"];
                            objRptaPagoUATP.IdTipoTarjeta = Convert.ToString(row["CCO_CID_UATP"]);
                            objRptaPagoUATP.NroTarjeta = Convert.ToString(row["TARJ_NRO_UATP"]);
                            objRptaPagoUATP.AnioExpiraTarjeta = Convert.ToString(row["TARJ_ANIO_EXPIRA"]);
                            objRptaPagoUATP.MesExpiraTarjeta = Convert.ToString(row["TARJ_MES_EXPIRA"]);
                            objRptaPagoUATP.TitularTarjeta = Convert.ToString(row["TARJ_TITULAR"]);
                            if (!Convert.IsDBNull(row["TARJ_COD_SEGURIDAD"]))
                                objRptaPagoUATP.CodSeguridadTarjeta = Convert.ToString(row["TARJ_COD_SEGURIDAD"]);
                            if (!Convert.IsDBNull(row["TARJ_BANCO_EMISOR"]))
                                objRptaPagoUATP.BancoEmisor = Convert.ToString(row["TARJ_BANCO_EMISOR"]);
                            if (!Convert.IsDBNull(row["RPTA_MENSAJE_UATP"]))
                                objRptaPagoUATP.MensajeRpta = Convert.ToString(row["RPTA_MENSAJE_UATP"]);
                            if (!Convert.IsDBNull(row["FECHA_COBRO_PAYU"]))
                                objRptaPagoUATP.Fechacobropayu = Convert.ToString(row["FECHA_COBRO_PAYU"]);
                            if (!Convert.IsDBNull(row["TIPO_DOC_TITULAR"]))
                                objRptaPagoUATP.IdTipoDocTitularTarjeta = Convert.ToString(row["TIPO_DOC_TITULAR"]);
                            if (!Convert.IsDBNull(row["NUM_DOC_TITULAR"]))
                                objRptaPagoUATP.NumDocTitularTarjeta = Convert.ToString(row["NUM_DOC_TITULAR"]);
                            if (!Convert.IsDBNull(row["RPTA_UATP"]))
                            {
                                if (row["RPTA_UATP"].ToString() == "1")
                                    objRptaPagoUATP.ResultadoOK = true;
                            }
                            if (!Convert.IsDBNull(row["TELF_USUARIO"]))
                            {
                                objRptaPagoUATP.TelfCliente = Convert.ToString(row["TELF_USUARIO"]);
                            }
                            PasarelaPago_Pedido.RptaPagoUATP = objRptaPagoUATP;
                        }

                        if (!Convert.IsDBNull(row["NRO_PEDIDO_PEEC"]))
                        {
                            objRptaPagoEfectivoEC = new RptaPagoEfectivoEC();
                            if (!Convert.IsDBNull(row["CIP"]))
                                objRptaPagoEfectivoEC.CIP = Convert.ToString(row["CIP"]);
                            if (!Convert.IsDBNull(row["PE_ESTADO"]))
                                objRptaPagoEfectivoEC.Estado = Convert.ToString(row["PE_ESTADO"]);
                            if (!Convert.IsDBNull(row["CIP_ESTADO"]))
                                objRptaPagoEfectivoEC.EstadoCIP = Convert.ToString(row["CIP_ESTADO"]);
                            if (!Convert.IsDBNull(row["CIP_ESTADO_CONCILIADO"]))
                                objRptaPagoEfectivoEC.EstadoConciliado = Convert.ToString(row["CIP_ESTADO_CONCILIADO"]);
                            if (!Convert.IsDBNull(row["CIP_FECHA_CANCELADO"]))
                                objRptaPagoEfectivoEC.FechaCancelado = (DateTime)row["CIP_FECHA_CANCELADO"];
                            if (!Convert.IsDBNull(row["CIP_FECHA_CONCILIADO"]))
                                objRptaPagoEfectivoEC.FechaConciliado = (DateTime)row["CIP_FECHA_CONCILIADO"];
                            if (!Convert.IsDBNull(row["FECHA_EXPIRA_PAGO"]))
                                objRptaPagoEfectivoEC.FechaExpiraPago = (DateTime)row["FECHA_EXPIRA_PAGO"];
                            if (!Convert.IsDBNull(row["CIP_FECHA_EXTORNO"]))
                                objRptaPagoEfectivoEC.FechaExtorno = (DateTime)row["CIP_FECHA_EXTORNO"];
                            if (!Convert.IsDBNull(row["PE_MENSAJE"]))
                                objRptaPagoEfectivoEC.MensajeRpta = Convert.ToString(row["PE_MENSAJE"]);
                            if (!Convert.IsDBNull(row["PE_ENVIO_RPTA_PAGO"]))
                            {
                                if (row["PE_ENVIO_RPTA_PAGO"].ToString() == "1")
                                    objRptaPagoEfectivoEC.EnvioRpta = true;
                            }
                            PasarelaPago_Pedido.RptaPagoEfectivoEC = objRptaPagoEfectivoEC;
                        }
                        if (!Convert.IsDBNull(row["TOTAL_FEE_VUE"]))
                            PasarelaPago_Pedido.FEE_ResVue = Convert.ToDouble(row["TOTAL_FEE_VUE"]);
                        else
                            PasarelaPago_Pedido.FEE_ResVue = 0;

                        if (!Convert.IsDBNull(row["NRO_PEDIDO_SP"]))
                        {
                            RptaPagoSafetyPay objRptaPagoSafetyPay = new RptaPagoSafetyPay();
                            if (!Convert.IsDBNull(row["EXPIRATIONDATETIME"]))
                                objRptaPagoSafetyPay.ExpirationDateTime = Convert.ToString(row["EXPIRATIONDATETIME"]);
                            if (!Convert.IsDBNull(row["OPERATIONID"]))
                                objRptaPagoSafetyPay.OperationId = Convert.ToString(row["OPERATIONID"]);
                            if (!Convert.IsDBNull(row["TRANSACTIONIDENTIFIER"]))
                                objRptaPagoSafetyPay.TransaccionIdentifier = Convert.ToString(row["TRANSACTIONIDENTIFIER"]);
                            if (!Convert.IsDBNull(row["ERRORMANAGER_SEVERITY"]))
                                objRptaPagoSafetyPay.ErrorManager_Severity = Convert.ToString(row["ERRORMANAGER_SEVERITY"]);
                            if (!Convert.IsDBNull(row["ERRORMANAGER_ERRORNUMBER"]))
                                objRptaPagoSafetyPay.ErrorManager_ErrorNumber = Convert.ToString(row["ERRORMANAGER_ERRORNUMBER"]);
                            if (!Convert.IsDBNull(row["ERRORMANAGER_DESCRIPCION"]))
                                objRptaPagoSafetyPay.ErrorManager_Description = Convert.ToString(row["ERRORMANAGER_DESCRIPCION"]);
                            if (!Convert.IsDBNull(row["BANK_REDIRECTURL"]))
                            {
                                objRptaPagoSafetyPay.BankRedirectUrl = Convert.ToString(row["BANK_REDIRECTURL"]);
                            }                                
                            PasarelaPago_Pedido.RptaPagoSafetyPay = objRptaPagoSafetyPay;
                        }
                        if (!Convert.IsDBNull(row["ESPAYU"]))
                        {
                            if (0 < Convert.ToInt32(row["ESPAYU"]))
                            {
                                PasarelaPago_Pedido.EsPayu = true;
                            }                                
                        }

                        // RCcancce - MT
                        if (!Convert.IsDBNull(row["PNR_INI"]))
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
                FormaPagoPedido FormaPagoPedido = new FormaPagoPedido();

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
                        FormaPagoPedido.IdPedido = Convert.ToInt32(row["NRO_PEDIDO"]);
                        if (!Convert.IsDBNull(row["CCO_CID"]))
                            FormaPagoPedido.IdTipoTarjeta = row["CCO_CID"].ToString();
                        if (!Convert.IsDBNull(row["TOTAL_PTOS_BOL"]))
                            FormaPagoPedido.TotalPtos = Convert.ToInt32(row["TOTAL_PTOS_BOL"]);
                        if (!Convert.IsDBNull(row["PTOS_PAGO"]))
                            FormaPagoPedido.PtosPagoCliente = Convert.ToInt32(row["PTOS_PAGO"]);
                        if (!Convert.IsDBNull(row["MONTO_PAGO"]))
                            FormaPagoPedido.MontoPago = Convert.ToDouble(row["MONTO_PAGO"]);
                        if (!Convert.IsDBNull(row["PUNTOXDOLAR"]))
                            FormaPagoPedido.PuntosXDolar = Convert.ToDouble(row["PUNTOXDOLAR"]);
                        if (!Convert.IsDBNull(row["TIPO_CAMBIO"]))
                            FormaPagoPedido.TipoCambio = Convert.ToDouble(row["TIPO_CAMBIO"]);
                        if (!Convert.IsDBNull(row["COD_AUTH_PTOS"]))
                            FormaPagoPedido.CodAutorizacionPtos = row["COD_AUTH_PTOS"].ToString();
                        if (!Convert.IsDBNull(row["FPAGO_ID"]))
                            FormaPagoPedido.IdFormaPago = Convert.ToInt16(row["FPAGO_ID"]);
                        if (!Convert.IsDBNull(row["CCO_CNAME"]))
                            FormaPagoPedido.NomTarjeta = row["CCO_CNAME"].ToString();
                        if (!Convert.IsDBNull(row["FPAGO_NOM"]))
                            FormaPagoPedido.NomFormaPago = row["FPAGO_NOM"].ToString();
                        if (!Convert.IsDBNull(row["TITULAR_TARJETA"]))
                            FormaPagoPedido.TitularTarjeta = row["TITULAR_TARJETA"].ToString();
                        if (!Convert.IsDBNull(row["AUTHORIZATIONCODE"]))
                            FormaPagoPedido.AutorizationTarjeta = row["AUTHORIZATIONCODE"].ToString();
                        if (!Convert.IsDBNull(row["TARJ_BANCO_EMISOR"]))
                            FormaPagoPedido.BancoEmisorTarjeta = row["TARJ_BANCO_EMISOR"].ToString();
                        if (!Convert.IsDBNull(row["ES_PAYU"]))
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
                            idSolicitudPago_SF = (row["IDSOLICITUDPAGO_CRM"] != null ? row["IDSOLICITUDPAGO_CRM"].ToString() : string.Empty),
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