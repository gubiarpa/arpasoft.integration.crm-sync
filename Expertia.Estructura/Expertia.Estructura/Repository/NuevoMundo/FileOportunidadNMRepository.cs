using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.NuevoMundo
{
    public class FileOportunidadNMRepository : OracleBase<FileOportunidadNM>
    {
        public FileOportunidadNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

        #region PublicMethods
        public Operation GetFilesAsociadosSRV_NM()
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_DETALLEPASAJEROSNM
            AddParameter(OutParameter.CursorFileAsociadossNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_FileOportunidadNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorFileAsociadossNM] = ToFileOportunidadNM(GetDtParameter(OutParameter.CursorFileAsociadossNM));
            #endregion

            return operation;
        }

        public Operation Update(RptaFileNM_SF RptaFileNM)
        {
            var operation = new Operation();

            #region Parameters  
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, RptaFileNM.CodigoError, ParameterDirection.Input, 2);
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, RptaFileNM.MensajeError, ParameterDirection.Input, 1000);            
            AddParameter(OutParameter.IdIdentificadorNM, OracleDbType.Int64, Convert.ToInt64(RptaFileNM.Identificador_NM));
            AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Upd_FileOportunidadNM);
            operation[OutParameter.IdActualizados] = GetOutParameter(OutParameter.IdActualizados);
            #endregion

            return operation;
        }

        public string _Select_IdVendedorPTABy_IdUsuWeb(int pIntIdUsuWeb, Int16 pIntIdBD)
        {           
            string strParamReturn = string.Empty;
            try
            {
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, pIntIdUsuWeb, ParameterDirection.Input);
                AddParameter("pNumIdEmpresaPTA_in", OracleDbType.Int32, pIntIdBD, ParameterDirection.Input);
                AddParameter("pVarIdVendPTA_out", OracleDbType.Varchar2, null, ParameterDirection.Output, 3);

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Get_XIdVendPTA_Usuario);
                strParamReturn = GetOutParameter("pVarIdVendPTA_out").ToString();
                #endregion                                               
                 
                return strParamReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            
        }

        #endregion

        #region Parse
        private IEnumerable<FileOportunidadNM> ToFileOportunidadNM(DataTable dt)
        {
            try
            {
                var fileNMList = new List<FileOportunidadNM>();

                foreach (DataRow row in dt.Rows)
                {
                    fileNMList.Add(new FileOportunidadNM()
                    {
                        idOportunidad_SF = row.StringParse("idoportunidad_sf"),
                        Identificador_NM = row.StringParse("identificador_nm"),
                        idCotSrv_SF = row.IntParse("idcotsrv_sf"),
                        numeroFile = row.IntParse("numero_de_file"),
                        importe = (Convert.IsDBNull(row["importe"]) == false ? row.StringParse("importe") : null),
                        sucursal = Convert.ToInt16(row.StringParse("sucursal")),
                        fecha = (Convert.IsDBNull(row["fecha"]) == false ? row.StringParse("fecha") : null),
                        accion_SF = row.StringParse("accion_sf")
                    });
                }
                return fileNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion        
    }
}