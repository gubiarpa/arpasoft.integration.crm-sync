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
    public class File_IA_Repository : OracleBase<AgenciaPnr>, IFileRepository
    {
        #region Constructor
        public File_IA_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.InterAgencias) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetNewAgenciaPnr()
        {
            var operation = new Operation();

            #region Parameters
            // (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (3) P_CLIENTE_PNR
            AddParameter(OutParameter.CursorAgenciaPnr, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.IA_Read_AgenciaPnr);
            operation[OutParameter.CursorAgenciaPnr] = ToAgenciaPnr(GetDtParameter(OutParameter.CursorAgenciaPnr));
            #endregion

            return operation;
        }

        public Operation GetNewFile(AgenciaPnr entity)
        {
            var operation = new Operation();

            #region Parameters
            // (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (3) P_PNR
            var pnr = entity.PNR;
            AddParameter("P_PNR", OracleDbType.Varchar2, pnr);
            // (4) P_ID_FILE
            var id_file = entity.IdFile;
            AddParameter("P_ID_FILE", OracleDbType.Int32, id_file);
            // (5) P_ID_SUCURSAL
            var id_sucursal = entity.IdSucursal;
            AddParameter("P_ID_SUCURSAL", OracleDbType.Int32, id_sucursal);
            // (6) P_ID_OPORTUNIDAD_CRM
            var id_oportunidad_crm = entity.IdOportunidadCrm;
            AddParameter("P_ID_OPORTUNIDAD_CRM", OracleDbType.Varchar2, id_oportunidad_crm);
            // (7) P_FILE
            AddParameter(OutParameter.CursorFile, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            // (8) P_BOLETO
            AddParameter(OutParameter.CursorBoleto, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.IA_Read_File);
            operation[OutParameter.CursorFile] = ToAgenciaPnr(GetDtParameter(OutParameter.CursorAgenciaPnr));
            #endregion

            return operation;
        }
        #endregion

        #region Auxiliar
        public IEnumerable<AgenciaPnr> ToAgenciaPnr(DataTable dt)
        {
            try
            {
                var agenciaPnrList = new List<AgenciaPnr>();
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
                    agenciaPnrList.Add(new AgenciaPnr()
                    {
                        DkAgencia = dk_agencia,
                        PNR = pnr,
                        IdFile = id_file,
                        IdSucursal = id_sucursal,
                        IdOportunidadCrm = id_oportunidad_crm
                    });
                    #endregion
                }
                return agenciaPnrList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<File> ToFile(DataTable dt)
        {
            try
            {
                var fileBoletoList = new List<File>();
                foreach (DataRow row in dt.Rows)
                {
                    fileBoletoList.Add(new File());


                    #region Loading

                    #endregion

                    #region AddingElement
                    //fileBoletoList.Add(new FileBoleto()
                    //{
                    //    IdOportunidad = id_oportunidad,
                    //    Objeto = objeto,
                    //    Accion = accion,
                    //    IdFile = id_file,
                    //    EstadoFile = estado_file,
                    //    UnidadNegocio = unidad_negocio,
                    //    Sucursal = sucursal,
                    //    EjecutivaCuenta = eje
                    //});
                    #endregion
                }
                return fileBoletoList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}