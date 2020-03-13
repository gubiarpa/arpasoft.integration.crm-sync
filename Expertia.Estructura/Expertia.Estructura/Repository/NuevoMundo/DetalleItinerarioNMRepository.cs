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
    public class DetalleItinerarioNMRepository : OracleBase<object>, IDetalleItinerarioNMRepository
    {
        #region Constructor
        public DetalleItinerarioNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
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
            /// (3) P_DETALLEITINERARIONM
            AddParameter(OutParameter.CursorDetalleItinerarioNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_DetalleItinerarioNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorDetalleItinerarioNM] = ToDetalleItinerarioNM(GetDtParameter(OutParameter.CursorDetalleItinerarioNM));
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<DetalleItinerarioNM> ToDetalleItinerarioNM(DataTable dt)
        {
            try
            {
                var detalleItinerarioNMList = new List<DetalleItinerarioNM>();

                foreach (DataRow row in dt.Rows)
                {
                    detalleItinerarioNMList.Add(new DetalleItinerarioNM()
                    {
                        lAerea = row.StringParse("ACCION"),
                        origen = row.StringParse("DK_CUENTA")
                    });
                }

                return detalleItinerarioNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}