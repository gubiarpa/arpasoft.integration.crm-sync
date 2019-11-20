using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Behavior;
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
        #region Properties
        private IEnumerable<File> _files;
        #endregion

        #region Constructor
        public File_IA_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.Interagencias) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
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

        public Operation GetNewFileBoleto(AgenciaPnr entity)
        {
            var operation = new Operation();
            #region Loading
            var pnr = entity.PNR;
            var id_file = entity.IdFile;
            var id_sucursal = entity.Sucursal.Descripcion;
            var id_oportunidad = entity.IdOportunidad;
            #endregion

            #region Parameters
            // (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (3) P_PNR
            AddParameter("P_PNR", OracleDbType.Varchar2, pnr);
            // (4) P_ID_FILE
            AddParameter("P_ID_FILE", OracleDbType.Int32, id_file);
            // (5) P_ID_SUCURSAL
            AddParameter("P_ID_SUCURSAL", OracleDbType.Varchar2, id_sucursal);
            // (6) P_ID_OPORTUNIDAD_CRM
            AddParameter("P_ID_OPORTUNIDAD_CRM", OracleDbType.Varchar2, id_oportunidad);
            // (7) P_FILE
            AddParameter(OutParameter.CursorFile, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            // (8) P_BOLETO
            AddParameter(OutParameter.CursorBoleto, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.IA_Read_File);
            operation[OutParameter.CursorFile] = ToFile(GetDtParameter(OutParameter.CursorFile));
            operation[OutParameter.CursorBoleto] = ToBoleto(GetDtParameter(OutParameter.CursorBoleto));
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
                    var nombre_sucursal = row.StringParse("NOMBRE_SUCURSAL");
                    var id_oportunidad_crm = row.StringParse("ID_OPORTUNIDAD_CRM");
                    #endregion

                    #region AddingElement
                    agenciaPnrList.Add(new AgenciaPnr()
                    {
                        DkAgencia = dk_agencia,
                        PNR = pnr,
                        IdFile = id_file,
                        Sucursal = new SimpleDesc(nombre_sucursal),
                        IdOportunidad = id_oportunidad_crm
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
                var fileList = new List<File>();
                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    var idOportunidad = row.StringParse("ID_OPORTUNIDAD");
                    var accion = row.StringParse("ACCION");
                    var idFile = row.IntParse("ID_FILE");
                    var estadoFile = row.StringParse("ESTADO_FILE");
                    var unidadNegocio = row.StringParse("UNIDAD_NEGOCIO");
                    var sucursal = row.StringParse("SUCURSAL");
                    var nombreGrupo = row.StringParse("NOMBRE_GRUPO");
                    var counter = row.StringParse("COUNTER");
                    var fechaApertura = row.DateTimeParse("FECHA_APERTURA");
                    var fechaInicio = row.DateTimeParse("FECHA_INICIO");
                    var fechaFin = row.DateTimeParse("FECHA_FIN");
                    var cliente = row.StringParse("CLIENTE");
                    var subcodigo = row.StringParse("SUBCODIGO");
                    var contacto = row.StringParse("CONTACTO");
                    var condicionPago = row.StringParse("CONDICION_PAGO");
                    var numPasajeros = row.IntParse("NUM_PASAJEROS");
                    var costo = row.FloatParse("COSTO");
                    var venta = row.FloatParse("VENTA");
                    var comisionAgencia = row.FloatParse("COMISION_AGENCIA");
                    #endregion

                    #region AddingElement
                    fileList.Add(new File()
                    {
                        IdOportunidad = idOportunidad,
                        Accion = accion,
                        IdFile = idFile,
                        EstadoFile = estadoFile,
                        UnidadNegocio = unidadNegocio,
                        Sucursal = sucursal,
                        NombreGrupo = nombreGrupo,
                        Counter = counter,
                        FechaApertura = fechaApertura,
                        FechaInicio = fechaInicio,
                        FechaFin = fechaFin,
                        Cliente = cliente,
                        Subcodigo = subcodigo,
                        Contacto = contacto,
                        CondicionPago = condicionPago,
                        NumPasajeros = numPasajeros,
                        Costo = costo,
                        Venta = venta,
                        ComisionAgencia = comisionAgencia
                    });
                    #endregion
                }
                return fileList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Boleto> ToBoleto(DataTable dt)
        {
            try
            {
                var boletoList = new List<Boleto>();
                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    #endregion

                    #region AddingElement
                    boletoList.Add(new Boleto() { });
                    #endregion
                }
                return boletoList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}