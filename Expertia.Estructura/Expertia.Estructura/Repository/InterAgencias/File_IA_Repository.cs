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

        public Operation GetFileAndBoleto(AgenciaPnr agenciaPnr)
        {
            var operation = new Operation();
            #region Loading
            var pnr = agenciaPnr.PNR;
            var id_file = agenciaPnr.IdFile;
            var id_sucursal = agenciaPnr.Sucursal;
            var id_oportunidad = agenciaPnr.IdOportunidad;
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

        public Operation UpdateFile(File file)
        {
            var operation = new Operation();
            #region Loading
            var sucursal = file.Sucursal;
            var numeroFile = file.NumeroFile;
            var codigoError = file.CodigoError;
            var mensajeError = file.MensajeError;
            #endregion

            #region Parameters
            // (01) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (02) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (03) P_NOMBRE_SUCURSAL
            AddParameter("P_NOMBRE_SUCURSAL", OracleDbType.Varchar2, sucursal);
            // (04) P_ID_FILE
            AddParameter("P_ID_FILE", OracleDbType.Varchar2, numeroFile);
            // (05) P_ES_ATENCION
            AddParameter("P_ES_ATENCION", OracleDbType.Varchar2, codigoError);
            // (06) P_DESCRIPCION
            AddParameter("P_DESCRIPCION", OracleDbType.Varchar2, mensajeError);
            // (07) P_ACTUALIZADOS
            AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.IA_Update_File);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.IdActualizados] = GetOutParameter(OutParameter.IdActualizados);
            #endregion

            return operation;
        }

        public Operation UpdateBoleto(Boleto boleto)
        {
            var operation = new Operation();
            #region Loading
            var sucursal = boleto.Sucursal;
            var numeroFile = boleto.NumeroFile;
            var numeroBoleto = boleto.NumeroBoleto;
            var codigoError = boleto.CodigoError;
            var mensajeError = boleto.MensajeError;
            #endregion

            #region Parameters
            // (01) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (02) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (03) P_NOMBRE_SUCURSAL
            AddParameter("P_NOMBRE_SUCURSAL", OracleDbType.Varchar2, sucursal);
            // (04) P_ID_FILE
            AddParameter("P_ID_FILE", OracleDbType.Varchar2, numeroFile);
            // (05) P_NUMERO_DE_BOLETO
            AddParameter("P_NUMERO_DE_BOLETO", OracleDbType.Varchar2, numeroBoleto);
            // (06) P_ES_ATENCION
            AddParameter("P_ES_ATENCION", OracleDbType.Varchar2, codigoError);
            // (07) P_DESCRIPCION
            AddParameter("P_DESCRIPCION", OracleDbType.Varchar2, mensajeError);
            // (08) P_ACTUALIZADOS
            AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.IA_Update_Boleto);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.IdActualizados] = GetOutParameter(OutParameter.IdActualizados);
            #endregion

            return operation;
        }
        #endregion

        #region Auxiliar
        private IEnumerable<AgenciaPnr> ToAgenciaPnr(DataTable dt)
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
                    var id_oportunidad = row.StringParse("ID_OPORTUNIDAD_CRM");
                    #endregion

                    #region AddingElement
                    agenciaPnrList.Add(new AgenciaPnr()
                    {
                        DkAgencia = dk_agencia,
                        PNR = pnr,
                        IdFile = id_file,
                        Sucursal = nombre_sucursal,
                        IdOportunidad = id_oportunidad,
                        Files = new List<File>(),
                        Boletos = new List<Boleto>()
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

        private IEnumerable<File> ToFile(DataTable dt)
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
                        NumeroFile = idFile,
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

        private IEnumerable<Boleto> ToBoleto(DataTable dt)
        {
            try
            {
                var boletoList = new List<Boleto>();
                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    var idOportunidad = row.StringParse("ID_OPORTUNIDAD");
                    var accion = row.StringParse("accion");
                    var idFile = row.IntParse("id_file");
                    var sucursal = row.StringParse("sucursal");
                    var numeroBoleto = row.StringParse("boleto");
                    var estadoBoleto = row.StringParse("estado_boleto");
                    var pnr = row.StringParse("pnr");
                    var tipoBoleto = row.StringParse("tipo_boleto");
                    var lineaAerea = row.StringParse("linea_aerea");
                    var ruta = row.StringParse("ruta");
                    var tipoRuta = row.StringParse("tipo_ruta");
                    var ciudadDestino = row.StringParse("ciudad_destino");
                    var puntoEmision = row.StringParse("punto_de_emision");
                    var nombrePasajero = row.StringParse("nombre_pasajero");
                    var infanteAdulto = row.StringParse("infante_con_adulto");
                    var fechaEmision = row.DateTimeParse("fecha_emision");
                    var emitidoCanje = row.StringParse("emitido_canje");
                    var agenteQuienEmite = row.StringParse("agente_quien_emite");
                    var montoTarifa = row.FloatParse("monto_tarifa");
                    var montoComision = row.FloatParse("monto_comision");
                    var montoTotal = row.FloatParse("monto_total");
                    var formaPago = row.StringParse("forma_pago");
                    var reembolsado = row.StringParse("reembolsado");
                    var pagoConTarjeta = row.StringParse("pago_con_tarjeta");
                    var tieneWaiver = row.StringParse("tiene_waiver");
                    var tipoWaiver = row.StringParse("tipo_waiver");
                    var montoWaiver = row.FloatParse("monto_waiver");
                    var pagado = row.StringParse("pagado");
                    var comprobante = row.StringParse("comprobante");
                    #endregion

                    #region AddingElement
                    boletoList.Add(new Boleto()
                    {
                        IdOportunidad = idOportunidad,
                        Accion = accion,
                        NumeroFile = idFile,
                        Sucursal = sucursal,
                        NumeroBoleto = numeroBoleto,
                        EstadoBoleto = estadoBoleto,
                        Pnr = pnr,
                        TipoBoleto = tipoBoleto,
                        LineaAerea = lineaAerea,
                        Ruta = ruta,
                        TipoRuta = tipoRuta,
                        CiudadDestino = ciudadDestino,
                        PuntoEmision = puntoEmision,
                        NombrePasajero = nombrePasajero,
                        InfanteAdulto = infanteAdulto,
                        FechaEmision = fechaEmision,
                        EmitidoCanje = emitidoCanje,
                        AgenteQuienEmite = agenteQuienEmite,
                        MontoTarifa = montoTarifa,
                        MontoComision = montoComision,
                        MontoTotal = montoTotal,
                        FormaPago = formaPago,
                        Reembolsado = reembolsado,
                        PagoConTarjeta = pagoConTarjeta,
                        TipoWaiver = tipoWaiver,
                        MontoWaiver = montoWaiver,
                        Pagado = pagado,
                        Comprobante = comprobante
                    });
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