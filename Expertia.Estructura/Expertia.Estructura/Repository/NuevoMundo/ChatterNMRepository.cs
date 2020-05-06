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
        public Operation GetPostCotizaciones()
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

        public Operation Update(RptaChatterSF RptaChatterNM)
        {
            var operation = new Operation();

            #region Parameters  
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, RptaChatterNM.CodigoError, ParameterDirection.Input, 2);
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, RptaChatterNM.MensajeError, ParameterDirection.Input, 1000);
            AddParameter(OutParameter.SF_IDOPORTUNIDAD_NM, OracleDbType.Varchar2, RptaChatterNM.idOportunidad_SF);
            AddParameter(OutParameter.SF_IDPOSTCOTSRV_NM, OracleDbType.Varchar2, RptaChatterNM.IdRegPostCotSrv_SF);
            AddParameter(OutParameter.IdIdentificadorNM, OracleDbType.Int64, Convert.ToInt64(RptaChatterNM.Identificador_NM));
            AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Upd_ChatterNM);
            operation[OutParameter.IdActualizados] = GetOutParameter(OutParameter.IdActualizados);            
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
                        idOportunidad_SF = row.StringParse("idOportunidad_SF"),
                        idPostCotSrv = row.IntParse("idPostCotSrv"),
                        Identificador_NM = row.StringParse("Identificador_NM"),
                        cabecera = row.StringParse("cabecera"),
                        texto = row.StringParse("texto"),
                        fecha = row.StringParse("fecha"),
                        accion_SF = row.StringParse("accion_SF")
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