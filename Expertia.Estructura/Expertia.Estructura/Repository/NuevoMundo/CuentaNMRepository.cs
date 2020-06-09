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
        public Operation GetCuentas()
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

        public Operation Update(RptaCuentaSF RptaCuentaNM)
        {
            var operation = new Operation();

            #region Parameters            
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, RptaCuentaNM.CodigoError, ParameterDirection.Input, 2);
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, RptaCuentaNM.MensajeError, ParameterDirection.Input, 1000);
            AddParameter(OutParameter.SF_IDCUENTA_NM, OracleDbType.Varchar2, RptaCuentaNM.idCuenta_SF);
            AddParameter(OutParameter.IdIdentificadorNM, OracleDbType.Int64, Convert.ToInt64(RptaCuentaNM.Identificador_NM));
            AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Upd_CuentaNM);
            operation[OutParameter.IdActualizados] = GetOutParameter(OutParameter.IdActualizados);
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<CuentaNM> ToCuentaNM(DataTable dt)
        {
            try
            {
                Int32 IdTempo = 0;
                var cuentaNMList = new List<CuentaNM>();

                foreach (DataRow row in dt.Rows)
                {
                    if (IdTempo != row.IntParse("Identificador_NM"))
                    {
                        cuentaNMList.Add(new CuentaNM()
                        {
                            idCuenta_SF = (Convert.IsDBNull(row["idCuentaSrv_SF"]) == false ? row.StringParse("idCuentaSrv_SF") : null),
                            idCuenta_NM = row.StringParse("Identificador_NM"),
                            nombreCliente = row.StringParse("nombreCliente"),
                            apellidoCliente = row.StringParse("apellidoCliente"),
                            apeMatCli = row.StringParse("apellidoMaternoCliente"),
                            emailCliente = row.StringParse("EmailCli"),
                            enviarPromociones = row.BoolParse("EnviarPromociones"),
                            direccion = row.StringParse("Direccion"),                           
                            idUsuarioSrv_SF = row.IntParse("IdUsuarioSrv_SF"),
                            accion_SF = row.StringParse("accion_SF")
                        });
                    }

                    if (Convert.IsDBNull(row["tipoTelefono"]) == false)
                    {
                        switch (row.IntParse("position"))
                        {
                            case 1:
                                cuentaNMList[cuentaNMList.Count - 1].tipoTelefono1 = row.StringParse("tipoTelefono");
                                cuentaNMList[cuentaNMList.Count - 1].pais1 = row.StringParse("codPais");                                
                                cuentaNMList[cuentaNMList.Count - 1].codArea1 = row.StringParse("codArea");
                                cuentaNMList[cuentaNMList.Count - 1].Anexo1 = row.StringParse("Anexo");
                                cuentaNMList[cuentaNMList.Count - 1].numero1 = row.StringParse("Numero");
                                break;
                            case 2:
                                cuentaNMList[cuentaNMList.Count - 1].tipoTelefono2 = row.StringParse("tipoTelefono");
                                cuentaNMList[cuentaNMList.Count - 1].pais2 = row.StringParse("codPais");
                                cuentaNMList[cuentaNMList.Count - 1].codArea2 = row.StringParse("codArea");
                                cuentaNMList[cuentaNMList.Count - 1].Anexo2 = row.StringParse("Anexo");
                                cuentaNMList[cuentaNMList.Count - 1].numero2 = row.StringParse("Numero");
                                break;
                            case 3:
                                cuentaNMList[cuentaNMList.Count - 1].tipoTelefono3 = row.StringParse("tipoTelefono");
                                cuentaNMList[cuentaNMList.Count - 1].pais3 = row.StringParse("codPais");
                                cuentaNMList[cuentaNMList.Count - 1].codArea3 = row.StringParse("codArea");
                                cuentaNMList[cuentaNMList.Count - 1].Anexo3 = row.StringParse("Anexo");
                                cuentaNMList[cuentaNMList.Count - 1].numero3 = row.StringParse("Numero");
                                break;
                            default:                                
                                break;
                        }                       
                    }

                    IdTempo = row.IntParse("Identificador_NM");
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