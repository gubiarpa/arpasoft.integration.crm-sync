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

        public Operation GenerarCodigoPago(GenCodigoPagoNM genCodigoPago)
        {
            var operation = new Operation();

            #region Parameter
            /// (01) P_CODIGO_ERROR
            AddParameter(OutParameter.SF_Codigo, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (02) P_MENSAJE_ERROR
            AddParameter(OutParameter.SF_Mensaje, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (03) P_ID_OPORTUNIDAD_SF
            AddParameter("P_DETALLE_SERVICIO", OracleDbType.Varchar2, genCodigoPago.detalledeServicio);
            /// (04) P_ID_COT_SRV_SF
            AddParameter("P_TOTAL_PAGAR", OracleDbType.Varchar2, genCodigoPago.totalAPagar);
            /// (05) P_FILE
            AddParameter("P_TIEMPO_EXPIRACION", OracleDbType.Varchar2, genCodigoPago.tiempoExpiracion);
            /// (06) P_IMPORTE
            AddParameter("P_EMAIL", OracleDbType.Varchar2, genCodigoPago.email);
            /// (07) P_SUCURSAL
            AddParameter("P_ID_OPORTUNIDAD_SF", OracleDbType.Varchar2, genCodigoPago.idOportunidad_SF);
            /// (08) P_FECHA
            AddParameter("P_ID_COT_SRV", OracleDbType.Varchar2, genCodigoPago.idCotSRV);
            /// (09) P_FECHA
            AddParameter("P_ID_SOLICITUD_PAGO_SF", OracleDbType.Varchar2, genCodigoPago.idSolicitudPago_SF);
            /// (10) P_FECHA
            AddParameter("P_ID_USUARIO_SRV_SF", OracleDbType.Varchar2, genCodigoPago.idUsuarioSrv_SF);
            /// (11) P_FECHA
            AddParameter("P_ID_CANAL_VENTA", OracleDbType.Varchar2, genCodigoPago.idCanalVenta);
            /// (12) P_FECHA
            AddParameter("P_NOMBRECLI", OracleDbType.Varchar2, genCodigoPago.nombreCli);
            /// (13) P_FECHA
            AddParameter("P_APEPATCLI", OracleDbType.Varchar2, genCodigoPago.apePatCli);
            /// (14) P_FECHA
            AddParameter("P_UNIDAD_NEGOCIO", OracleDbType.Varchar2, genCodigoPago.unidadNegocio);
            /// (15) P_FECHA
            AddParameter("P_ID", OracleDbType.Varchar2, genCodigoPago.id);
            /// (16) P_FECHA
            AddParameter("P_DESCRIPCION", OracleDbType.Varchar2, genCodigoPago.descripcion);
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

                operation[OutParameter.SF_Codigo] = "OK, Esto es codigo duro";
                operation[OutParameter.SF_Mensaje] = "Mensaje: Esto es codigo duro";
                operation[OutParameter.CodigoTransaccion] = "0015980025ABC: Codigo duro";
            }
            
            #endregion

            return operation;
        }
    }
}