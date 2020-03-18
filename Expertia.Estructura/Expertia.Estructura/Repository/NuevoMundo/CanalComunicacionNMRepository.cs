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
    public class CanalComunicacionNMRepository : OracleBase<object>, ICanalComunicacionNMRepository
    {
        #region Constructor
        public CanalComunicacionNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
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
            AddParameter(OutParameter.CursorCanalComunicacionNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_CanalComunicacionNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorCanalComunicacionNM] = ToCanalComunicacionNM(GetDtParameter(OutParameter.CursorCanalComunicacionNM));
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<CanalComunicacionNM> ToCanalComunicacionNM(DataTable dt)
        {
            try
            {
                var canalComunicacionNMList = new List<CanalComunicacionNM>();

                foreach (DataRow row in dt.Rows)
                {
                    canalComunicacionNMList.Add(new CanalComunicacionNM()
                    {
                        idCotSrv_SF = row.StringParse("ACCION"),
                        texto = row.StringParse("DK_CUENTA")
                    });
                }
                return canalComunicacionNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}