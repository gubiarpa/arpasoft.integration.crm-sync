using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.NuevoMundo
{
    public class GenCodigoPagoNMRepository : OracleBase<GenCodigoPagoNM>
    {
        public GenCodigoPagoNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

        public void RegistraSolicitudPagoSF(string idSolicitudPagoSF, string idOportunidadSF, int idPedidoNM)
        {
            AddParameter("P_ID_SOLIC_PAGO_SF", OracleDbType.NVarchar2, idSolicitudPagoSF);
            AddParameter("P_ID_OPORTUNIDAD_SF", OracleDbType.NVarchar2, idOportunidadSF);
            AddParameter("P_ID_PEDIDO_NM", OracleDbType.Int32, idPedidoNM);

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Insert_SolicitudPagoSF);
            #endregion
        }

        public void UpdateSolicitudPagoSF(string idSolicitudPagoSF, string idOportunidadSF, int idPedidoNM, string estadoRegistro)
        {
            AddParameter("P_ID_SOLIC_PAGO_SF", OracleDbType.NVarchar2, idSolicitudPagoSF);
            AddParameter("P_ID_OPORTUNIDAD_SF", OracleDbType.NVarchar2, idOportunidadSF);
            AddParameter("P_ID_PEDIDO_NM", OracleDbType.Int32, idPedidoNM);
            AddParameter("P_ST_REGI", OracleDbType.NVarchar2, estadoRegistro);

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Update_SolicitudPagoSF);
            #endregion
        }

        public Operation GenerarCodigoPago(GenCodigoPagoNM genCodigoPago)
        {
            var operation = new Operation();

            #region Parameter
            /// (01) P_CODIGO_ERROR
            AddParameter(OutParameter.SF_Codigo, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (02) P_MENSAJE_ERROR
            AddParameter(OutParameter.SF_Mensaje, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (03) P_ID_OPORTUNIDAD_SF
            AddParameter("P_DETALLE_SERVICIO", OracleDbType.Varchar2, genCodigoPago.detalleServicio);
            /// (04) P_ID_COT_SRV_SF
            AddParameter("P_TOTAL_PAGAR", OracleDbType.Varchar2, genCodigoPago.monto);
            /// (05) P_FILE
            AddParameter("P_TIEMPO_EXPIRACION", OracleDbType.Varchar2, genCodigoPago.tiempoExpiracionCIP);
            /// (06) P_IMPORTE
            AddParameter("P_EMAIL", OracleDbType.Varchar2, genCodigoPago.email);
            /// (07) P_SUCURSAL
            AddParameter("P_ID_OPORTUNIDAD_SF", OracleDbType.Varchar2, genCodigoPago.idOportunidad_SF);
            /// (08) P_FECHA
            AddParameter("P_ID_COT_SRV", OracleDbType.Varchar2, genCodigoPago.idCotVta);
            /// (09) P_FECHA
            AddParameter("P_ID_SOLICITUD_PAGO_SF", OracleDbType.Varchar2, genCodigoPago.idSolicitudPago_SF);
            /// (10) P_FECHA
            AddParameter("P_ID_USUARIO_SRV_SF", OracleDbType.Varchar2, genCodigoPago.idUsuario);
            /// (11) P_FECHA
            AddParameter("P_ID_CANAL_VENTA", OracleDbType.Varchar2, genCodigoPago.idCanalVta);
            /// (12) P_FECHA
            AddParameter("P_NOMBRECLI", OracleDbType.Varchar2, genCodigoPago.NombreClienteCot);
            /// (13) P_FECHA
            AddParameter("P_APEPATCLI", OracleDbType.Varchar2, genCodigoPago.ApellidoClienteCot);
            /// (14) P_FECHA
            AddParameter("P_UNIDAD_NEGOCIO", OracleDbType.Varchar2, "Quitar Esto");
            /// (15) P_FECHA
            AddParameter("P_ID", OracleDbType.Varchar2, genCodigoPago.unidadDeNegocio.ID);
            /// (16) P_FECHA
            AddParameter("P_DESCRIPCION", OracleDbType.Varchar2, genCodigoPago.unidadDeNegocio.Descripcion);
            /// (17) P_FECHA
            AddParameter("P_ID_LANG", OracleDbType.Varchar2, genCodigoPago.idLang);
            /// (18) P_FECHA
            AddParameter("P_ID_WEB", OracleDbType.Varchar2, genCodigoPago.idWeb);
            /// (19) P_FECHA
            AddParameter("P_ID_USUARIO", OracleDbType.Varchar2, genCodigoPago.ipUsuario);
            /// (20) P_FECHA
            AddParameter("P_BROWSER", OracleDbType.Varchar2, genCodigoPago.browser);
            /// (21) P_FECHA
            AddParameter("P_CODE_PASARELA_PAGO", OracleDbType.Varchar2, genCodigoPago.codePasarelaPago);
            /// (22) P_CODIGO_TRANSACCION
            AddParameter(OutParameter.CodigoTransaccion, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            #endregion

            #region Invoke
            try
            {
                ExecuteStoredProcedure(StoredProcedureName.AW_Generar_Codigo_PagoNM);

                operation[OutParameter.SF_Codigo] = GetOutParameter(OutParameter.SF_Codigo);
                operation[OutParameter.SF_Mensaje] = GetOutParameter(OutParameter.SF_Mensaje);
                operation[OutParameter.CodigoTransaccion] = GetOutParameter(OutParameter.CodigoTransaccion);
            }
            catch (Exception)
            {

                operation[OutParameter.SF_Codigo] = "ER";
                operation[OutParameter.SF_Mensaje] = "Error de Prueba";
                operation[OutParameter.CodigoTransaccion] = "0015980025ABC";
            }
            
            #endregion

            return operation;
        }
    }
}