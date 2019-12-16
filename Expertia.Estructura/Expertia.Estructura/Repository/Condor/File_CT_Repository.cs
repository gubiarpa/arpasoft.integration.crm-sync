using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.Condor
{
    public class File_CT_Repository : OracleBase<FileCT>, IFileCT
    {
        #region Constructor
        public File_CT_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.CondorTravel) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetFileCT(FileCTRequest fileCTRequest)
        {
            var operation = new Operation();
            #region Loading
            var idOportunidadSf = fileCTRequest.IdOportunidadSf;
            var idCotizacionSf = fileCTRequest.IdCotizacionSf;
            var region = fileCTRequest.Region;
            var fileCT = fileCTRequest.File;
            var subfileCT = fileCTRequest.subfile;
            #endregion

            #region Parameters
            /// (1) P_CODIGO_ERROR
            //AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            //AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_ID_OPORTUNIDAD_SF
            AddParameter("P_ID_OPORTUNIDAD_SF", OracleDbType.Varchar2, idOportunidadSf);
            /// (4) P_ID_FILE_SF
            AddParameter("P_ID_FILE_SF", OracleDbType.Varchar2, idCotizacionSf);
            /// (5) P_FILE
            AddParameter("P_FILE", OracleDbType.Varchar2, fileCT);
            /// (6) P_CUR_COTIZACION
            AddParameter("P_SUBFILE", OracleDbType.Int32, subfileCT);
            AddParameter(OutParameter.CursorCotizacion, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.CT_Obtiene_FileCT);
            operation[OutParameter.CursorCotizacion] = ToCotizacion(GetDtParameter(OutParameter.CursorCotizacion));
            #endregion

            return operation;
        }
        #endregion

        #region Auxiliar
        public IEnumerable<FileCT> ToCotizacion(DataTable dt)
        {
            try
            {
                var files = new List<FileCT>();
                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    //var grupo = row.

                    var file = row.StringParse("XFILE");
                    var subfile = row.IntParse("Subfile");
                    var cliente = row.StringParse("Cliente");
                    var cliente_Cliente = row.StringParse("Cliente_Cliente");
                    var ejecutivo = row.StringParse("EJECUTIVA_CUENTA");
                    var branch = row.StringParse("BRANCH");
                    var fecha_Apertura = row.DateTimeParse("FEC_APERTURA");
                    var fecha_Inicio = row.DateTimeParse("FEC_INICIO");
                    var fecha_Fin = row.DateTimeParse("FEC_FIN");
                    var num_Pasajero = row.StringParse("CANT_PAX");

                    #endregion

                    #region AddingElement
                    files.Add(new FileCT()
                    {
                        File = file,
                        Subfile = subfile,
                        Cliente = cliente,
                        Cliente_Cliente = cliente_Cliente,
                        Ejecutivo = ejecutivo,
                        Branch = branch,
                        Fecha_Apertura = fecha_Apertura,
                        Fecha_Inicio = fecha_Inicio,
                        Fecha_Fin = fecha_Fin,
                        Num_Pasajero = num_Pasajero
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




    }
}