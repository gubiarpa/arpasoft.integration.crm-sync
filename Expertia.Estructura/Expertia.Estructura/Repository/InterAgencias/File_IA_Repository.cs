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

            throw new NotImplementedException();
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
                    var id_cliente = (row["P_ID_CLIENTE"] ?? string.Empty).ToString();
                    var id_file = (row["P_ID_FILE"] ?? string.Empty).ToString();
                    if (!int.TryParse(row["ID_SUCURSAL"].ToString(), out int id_sucursal)) id_sucursal = 0;
                    if (!int.TryParse(row["COD_RESERVA"].ToString(), out int cod_reserva)) cod_reserva = 0;
                    #endregion

                    #region AddingElement
                    files.Add(new File()
                    {
                        IdSucursal = id_sucursal,
                        CodReserva = cod_reserva
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