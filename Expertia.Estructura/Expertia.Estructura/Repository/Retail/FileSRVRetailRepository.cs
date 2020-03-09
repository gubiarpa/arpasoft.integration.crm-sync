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
                AddParameter(OutParameter.CursorFilesAsociadosSRV, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                #endregion

                #region Invoke              
                ExecuteStoredProcedure(StoredProcedureName.AW_Read_FileAsociadosSRV);
                operation[OutParameter.CursorFilesAsociadosSRV] = FillFilesAsociadosSRV(GetDtParameter(OutParameter.CursorFilesAsociadosSRV));
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
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