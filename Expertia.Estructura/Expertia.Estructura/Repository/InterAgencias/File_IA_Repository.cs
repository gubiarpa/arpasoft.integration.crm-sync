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
            AddParameter(OutParameter.CursorFile, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.IA_Read_AgenciaPnr);
            operation[OutParameter.CursorFile] = ToAgenciaPnr(GetDtParameter(OutParameter.CursorFile));
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
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.IA_Read_File);
            operation[OutParameter.CursorFile] = ToAgenciaPnr(GetDtParameter(OutParameter.CursorFile));
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

        public IEnumerable<FileBoleto> ToFileBoleto(DataTable dt)
        {
            try
            {
                var fileBoletoList = new List<FileBoleto>();
                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    var id_oportunidad = row.StringParse("ID_OPORTUNIDAD");
                    var objeto = row.StringParse("OBJETO");
                    var accion = row.StringParse("ACCION");
                    var id_file = row.IntParse("ID_FILE");
                    var estado_file = row.StringParse("ESTADO_FILE");
                    var unidad_negocio = row.StringParse("UNIDAD_NEGOCIO");
                    var sucursal = row.StringParse("SUCURSAL");
                    var nombre_grupo = row.StringParse("NOMBRE_GRUPO");
                    var counter = row.StringParse("COUNTER");
                    var fecha_apertura = row.DateTimeParse("FECHA_APERTURA");
                    var fecha_inicio = row.DateTimeParse("FECHA_INICIO");
                    var fecha_fin = row.DateTimeParse("FECHA_FIN");
                    var cliente = row.StringParse("CLIENTE");
                    var subcodigo = row.StringParse("SUBCODIGO");
                    var contacto = row.StringParse("CONTACTO");
                    var condicion_pago = row.StringParse("CONDICION_PAGO");
                    var num_pasajeros = row.IntParse("NUM_PASAJEROS");
                    var costo = row.IntParse("COSTO");
                    var venta = row.IntParse("VENTA");
                    var comision_agencia = row.IntParse("COMISION_AGENCIA");
                    var boleto = row.IntParse("BOLETO");
                    var estado_boleto = row.StringParse("ESTADO_BOLETO");
                    var pnr = row.StringParse("PNR");
                    var tipo_boleto = row.StringParse("TIPO_BOLETO");
                    var linea_aerea = row.StringParse("LINEA_AEREA");
                    var ruta = row.StringParse("RUTA");
                    var tipo_ruta = row.StringParse("TIPO_RUTA");
                    var ciudad_destino = row.StringParse("CIUDAD_DESTINO");
                    var punto_de_emision = row.StringParse("PUNTO_DE_EMISION");
                    var nombre_pasajero = row.StringParse("NOMBRE_PASAJERO");
                    var infante_con_adulto = row.StringParse("INFANTE_CON_ADULTO");
                    var fecha_emision = row.DateTimeParse("FECHA_EMISION");
                    var emitido_canje = row.StringParse("EMITIDO_CANJE");
                    var agente_quien_emite = row.StringParse("AGENTE_QUIEN_EMITE");
                    var monto_tarifa = row.IntParse("MONTO_TARIFA");
                    var monto_comision = row.IntParse("MONTO_COMISION");
                    var monto_total = row.IntParse("MONTO_TOTAL");
                    var forma_pago = row.StringParse("FORMA_PAGO");
                    var reembolsado = row.StringParse("REEMBOLSADO");
                    var pago_con_tarjeta = row.StringParse("PAGO_CON_TARJETA");
                    var tiene_waiver = row.StringParse("TIENE_WAIVER");
                    var tipo_waiver = row.StringParse("TIPO_WAIVER");
                    var monto_waiver = row.IntParse("MONTO_WAIVER");
                    var pagado = row.StringParse("PAGADO");
                    var comprobante = row.StringParse("COMPROBANTE");
                    #endregion

                    #region AddingElement
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