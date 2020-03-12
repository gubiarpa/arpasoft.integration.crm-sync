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
    public class CuentaNMRepository : OracleBase<object>, ICuentaNMRepository
    {
        #region Constructor
        public CuentaNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation Read(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs)
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_CUENTANM
            AddParameter(OutParameter.CursorCuentaPta, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_Cuenta);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            //operation[OutParameter.CursorCuentaPta] = ToCuentaNM(GetDtParameter(OutParameter.CursorCuentaPta));
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<CuentaPta> ToCuentaNM(DataTable dt)
        {
            try
            {
                var cuentaPtaList = new List<CuentaPta>();

                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    var accion = row.StringParse("ACCION");
                    var dkCuenta = row.IntParse("DK_CUENTA");
                    var razonSocial = row.StringParse("RAZON_SOCIAL");
                    var nombreComercial = row.StringParse("NOMBRE_COMERCIAL");
                    var tipoCuenta = row.StringParse("TIPO_CUENTA");
                    var propietario = row.StringParse("PROPIETARIO");
                    var fechaAniversario = row.DateTimeParse("FECHA_ANIVERSARIO");
                    var tipoDocumentoIdentidad = row.StringParse("TIPO_DOCUMENTO_IDENTIDAD");

                    #endregion

                }

                return cuentaPtaList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}