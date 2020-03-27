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
            AddParameter(OutParameter.CursorCuentaNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_CuentaNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorCuentaNM] = ToCuentaNM(GetDtParameter(OutParameter.CursorCuentaNM));
            #endregion

            return operation;
        }

        public Operation Update(CuentaNM cuentaNM)
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_IDCUENTA_SF
            AddParameter("P_IDCUENTA_SF", OracleDbType.Varchar2, cuentaNM.idCuenta_Sf);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Upd_CuentaNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<CuentaNM> ToCuentaNM(DataTable dt)
        {
            try
            {
                var cuentaNMList = new List<CuentaNM>();

                foreach (DataRow row in dt.Rows)
                {
                    cuentaNMList.Add(new CuentaNM()
                    {
                        nombreCli = row.StringParse("NombreCli"),
                        apePatCli = row.StringParse("ApePatCli"),
                        apeMatCli = row.StringParse("ApeMatCli"),
                        idCuenta_Sf = row.StringParse("idCuenta_SF"),
                        eMailCli = row.StringParse("EmailCli"),
                        enviarPromociones = row.BoolParse("EnviarPromociones"),
                        tipoTelefono1 = row.StringParse("tipoTelefono1"),
                        codPais1 = row.IntParse("codPais1"),
                        numero1 = row.IntParse("Numero1"),
                        tipoTelefono2 = row.StringParse("TipoTelefono2"),
                        codPais2 = row.IntParse("codPais2"),
                        numero2 = row.IntParse("Numero2"),
                        tipoTelefono3 = row.StringParse("TipoTelefono3"),
                        codPais3 = row.IntParse("codPais3"),
                        numero3 = row.IntParse("Numero3"),
                        direccion = row.StringParse("Dirección"),
                        razonSocial = row.StringParse("razonSocial"),
                        aceptarPoliticas = row.BoolParse("aceptarPoliticas"),
                        ruc = row.IntParse("RUC"),
                        idUsuarioSrv_Sf = row.IntParse("IdUsuarioSrv_SF"),
                        accion_Sf = row.StringParse("accion_SF")

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