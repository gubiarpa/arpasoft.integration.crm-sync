using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.NuevoMundo
{
    public class ChatterNMRepository : OracleBase<object>, IChatterNMRepository
    {
        #region Constructor
        public ChatterNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation Send(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs)
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_DETALLEPASAJEROSNM
            AddParameter(OutParameter.CursorChatterNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_ChatterNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorChatterNM] = ToChatterNM(GetDtParameter(OutParameter.CursorChatterNM));
            #endregion

            return operation;
        }

        public Operation Update(ChatterNM chatterNM)
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_IDCUENTA_SF
            AddParameter("P_IDCOTSRV_SF", OracleDbType.Varchar2, chatterNM.idCotSrv_SF);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Upd_ChatterNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<ChatterNM> ToChatterNM(DataTable dt)
        {
            try
            {
                var chatterNMList = new List<ChatterNM>();

                foreach (DataRow row in dt.Rows)
                {
                    chatterNMList.Add(new ChatterNM()
                    {
                        idOportunidad_SF = row.StringParse("IdOportunidad_SF"),
                        idCotSrv_SF = row.StringParse("IdCotSrv_SF"),
                        texto = row.StringParse("Texto"),
                        accion_SF = row.StringParse("Accion_SF")
                    });
                }
                return chatterNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}