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
            /// (3) P_CUENTANM
            AddParameter(OutParameter.CursorCuentaNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_CuentaNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorCuentaNM] = ToDetalleItinerarioNM(GetDtParameter(OutParameter.CursorCuentaNM));
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<CuentaNM> ToDetalleItinerarioNM(DataTable dt)
        {
            try
            {
                var cuentaNMList = new List<CuentaNM>();

                foreach (DataRow row in dt.Rows)
                {
                    cuentaNMList.Add(new CuentaNM()
                    {
                        nombreCli = row.StringParse("ACCION"),
                        apePatCli = row.StringParse("DK_CUENTA")
                    });
                }

                return cuentaNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}