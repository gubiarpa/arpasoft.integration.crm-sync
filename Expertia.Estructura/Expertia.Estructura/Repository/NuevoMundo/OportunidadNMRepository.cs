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
    public class OportunidadNMRepository : OracleBase<object>, IOportunidadNMRepository
    {
        #region Constructor
        public OportunidadNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetOportunidades()
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_CUENTANM
            AddParameter(OutParameter.CursorOportunidadNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_OportunidadNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorOportunidadNM] = ToCuentaNM(GetDtParameter(OutParameter.CursorOportunidadNM));
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<OportunidadNM> ToCuentaNM(DataTable dt)
        {
            try
            {
                var oportunidadNMList = new List<OportunidadNM>();

                foreach (DataRow row in dt.Rows)
                {
                    oportunidadNMList.Add(new OportunidadNM()
                    {
                        idCuenta_SF = row.StringParse("ACCION"),
                        fechaRegistro = row.StringParse("DK_CUENTA")
                    });
                }

                return oportunidadNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}