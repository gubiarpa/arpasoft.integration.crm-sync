using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Retail;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.Retail
{
    public class FileSRVRetailRepository : OracleBase<object>, IFileSRVRetailRepository
    {
        #region Constructor
        public FileSRVRetailRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetFilesAsociadosSRV()
        {
            var operation = new Operation();
            try
            {
                #region Parameters
                /// (01) P_CODIGO_ERROR
                AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (02) P_MENSAJE_ERROR
                AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (03) P_FILES_ASOCIADOS_SRV
                AddParameter(OutParameter.CursorFilesAsociadosSRV, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                #endregion

                #region Invoke              
                ExecuteStoredProcedure(StoredProcedureName.AW_Read_FileAsociadosSRV);
                operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
                operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
                operation[OutParameter.CursorFilesAsociadosSRV] = FillFilesAsociadosSRV(GetDtParameter(OutParameter.CursorFilesAsociadosSRV));
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return operation;
        }

        public Operation Actualizar_EnvioCotRetail(FilesAsociadosSRVResponse FileAsociadosSRVResponse)
        {
            var operation = new Operation();

            #region Parameter
            /// (01) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (02) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (03) P_OPORTUNIDAD_CRM
            AddParameter("P_OPORTUNIDAD_CRM", OracleDbType.Varchar2, FileAsociadosSRVResponse.id_oportunidad_sf);
            /// (04) P_ES_ATENCION
            AddParameter("P_ES_ATENCION", OracleDbType.Varchar2, FileAsociadosSRVResponse.codigo_error);
            /// (05) P_DESCRIPCION
            AddParameter("P_DESCRIPCION", OracleDbType.Varchar2, FileAsociadosSRVResponse.mensaje_error);
            /// (06) P_ACTUALIZADOS
            AddParameter(OutParameter.NumeroActualizados, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Upd_EnvioCotRetail);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.NumeroActualizados] = GetOutParameter(OutParameter.NumeroActualizados);
            #endregion

            return operation;
        }

        #endregion

        #region Auxiliares

        private IEnumerable<FilesAsociadosSRV> FillFilesAsociadosSRV(DataTable dt)
        {
            try
            {
                var FilesAsociadosSRVList = new List<FilesAsociadosSRV>();

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        #region AddingElement
                        FilesAsociadosSRVList.Add(new FilesAsociadosSRV()
                                                {
                                                    id_oportunidad = row.StringParse("ID_OPORTUNIDAD"),
                                                    cot_id = row.IntParse("COT_ID"),
                                                    suc_id = row.IntParse("SUC_ID"),
                                                    file_id = row.IntParse("FILE_ID"),
                                                    fpta_fecha = row.DateTimeParse("FPTA_FECHA"),
                                                    fpta_imp_fact = row.IntParse("FPTA_IMP_FACT")
                                                });
                        #endregion
                    }
                }

                return FilesAsociadosSRVList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}