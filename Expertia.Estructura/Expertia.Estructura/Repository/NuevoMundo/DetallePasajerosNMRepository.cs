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
    public class DetallePasajerosNMRepository : OracleBase<object>, IDetallePasajerosNMRepository
    {
        #region Constructor
        public DetallePasajerosNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
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
            AddParameter(OutParameter.CursorDetallePasajerosNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_DetallePasajerosNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorDetallePasajerosNM] = ToDetallePasajerosNM(GetDtParameter(OutParameter.CursorDetallePasajerosNM));
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<DetallePasajerosNM> ToDetallePasajerosNM(DataTable dt)
        {
            try
            {
                var detallePasajerosNMList = new List<DetallePasajerosNM>();

                foreach (DataRow row in dt.Rows)
                {
                    detallePasajerosNMList.Add(new DetallePasajerosNM()
                    {
                        tipo = row.StringParse("ACCION"),
                        pais = row.StringParse("DK_CUENTA")
                    });
                }

                return detallePasajerosNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}