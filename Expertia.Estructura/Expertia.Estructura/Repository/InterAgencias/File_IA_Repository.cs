using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.InterAgencias
{
    public class File_IA_Repository : OracleBase<File>, ICrud<File>
    {
        #region Constructor
        public File_IA_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.InterAgencias) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation Read(File entity)
        {
            var operation = new Operation();

            #region Parameters
            // (1) P_CODIGO_ERROR
            var codigoError = DBNull.Value;
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, codigoError, ParameterDirection.Output, OutParameter.DefaultSize);
            // (2) P_MENSAJE_ERROR
            var mensajeError = DBNull.Value;
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, mensajeError, ParameterDirection.Output, OutParameter.DefaultSize);
            // (3) P_CLIENTE_PNR
            var cliente_pnr = DBNull.Value;
            AddParameter(OutParameter.CursorFile, OracleDbType.RefCursor, cliente_pnr, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.IA_Read_File);

            operation[OutParameter.CursorFile] = ToFile(GetDtParameter(OutParameter.CursorFile));
            #endregion

            return operation;
        }
        #endregion

        #region Auxiliar
        public IEnumerable<File> ToFile(DataTable dt)
        {
            try
            {
                var files = new List<File>();
                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    var dk_agencia = row.IntParse("DK_AGENCIA");
                    var pnr = row.StringParse("PNR");
                    var id_file = row.IntParse("ID_FILE");
                    var id_sucursal = row.IntParse("ID_SUCURSAL");
                    var id_oportunidad_crm = row.StringParse("ID_OPORTUNIDAD_CRM");
                    #endregion

                    #region AddingElement
                    files.Add(new File()
                    {
                        DkAgencia = dk_agencia,
                        PNR = pnr,
                        IdFile = id_file,
                        IdSucursal = id_sucursal,
                        IdOportunidadCrm = id_oportunidad_crm
                    });
                    #endregion
                }
                return files;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region NotImplemented
        public Operation Asociate(File entity)
        {
            throw new NotImplementedException();
        }

        public Operation Create(File entity)
        {
            throw new NotImplementedException();
        }

        public Operation Generate(File entity)
        {
            throw new NotImplementedException();
        }

        public Operation Update(File entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}