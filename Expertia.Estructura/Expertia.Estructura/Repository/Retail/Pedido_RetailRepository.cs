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

        private List<AmountType> Get_Monedas_PedidoSafetyPay(int IdPedido)
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
        #endregion

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
    }
}