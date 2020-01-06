using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.Condor
{
    public class Venta_CT_Repository : OracleBase<VentasRequest>, IVentaCT
    {
        public Venta_CT_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.CondorTravel) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

        public Operation GetVentasCT(VentasRequest ventasRequest)
        {
            var operation = new Operation();

            #region Loading
            var usuario = ventasRequest.Usuario;
            var idCuentaSf = ventasRequest.IdCuentaSf;
            #endregion

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_NOMBRE_USUARIO
            AddParameter("P_NOMBRE_USUARIO", OracleDbType.Varchar2, usuario);
            /// (4) P_ID_CUENTA_SF
            AddParameter("P_ID_CUENTA_SF", OracleDbType.Varchar2, idCuentaSf);
            /// (5) P_CUR_RESU_VENTAS
            AddParameter(OutParameter.CursorVentas, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.CT_Get_VentasResumen);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorVentas] = ToVentasRow(GetDtParameter(OutParameter.CursorVentas));
            #endregion

            return operation;
        }

        private IEnumerable<VentasRow> ToVentasRow(DataTable dt)
        {
            try
            {
                var ventas = new List<VentasRow>();
                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    var indicador = row.StringParse("INDICADOR");
                    var region = row.StringParse("REGION");
                    var monto = row.FloatNullParse("MONTO");
                    var comparativo = row.FloatNullParse("COMPARATIVO");
                    #endregion

                    #region AddingElement
                    ventas.Add(new VentasRow()
                    {
                        Indicador = indicador,
                        Region = region,
                        Monto = monto,
                        Comparativo = comparativo
                    });
                    #endregion
                }

                return ventas;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}