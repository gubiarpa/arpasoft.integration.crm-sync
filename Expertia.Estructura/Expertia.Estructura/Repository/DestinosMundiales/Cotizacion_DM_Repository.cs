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

namespace Expertia.Estructura.Repository.DestinosMundiales
{
    public class Cotizacion_DM_Repository : OracleBase<Cotizacion>, ICrud<Cotizacion>
    {
        #region Constructor
        public Cotizacion_DM_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.DestinosMundiales) : base(ConnectionKeys.DMConnKey, unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation Generate(Cotizacion entity)
        {
            try
            {
                Operation operation = new Operation();
                object value;

                #region Parameters
                // (01) P_CODIGO_ERROR
                value = DBNull.Value;
                AddParameter("P_CODIGO_ERROR", OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
                // (02) P_MENSAJE_ERROR
                value = DBNull.Value;
                AddParameter("P_MENSAJE_ERROR", OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.DM_Generate_Cotizacion);

                operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
                operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
                //operation[OutParameter.IdCuenta] = entity.IdCuenta = GetOutParameter(OutParameter.IdCuenta).ToString();
                //operation[OutParameter.IdCotizacion] = entity.IdCotizacion = int.Parse(GetOutParameter(OutParameter.IdCotizacion).ToString());
                operation[Operation.Result] = ResultType.Success;
                #endregion

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation Asociate(Cotizacion entity)
        {
            Operation operation = new Operation();
            object value;

            #region Parameters
            // (01) P_CODIGO_ERROR
            value = DBNull.Value;
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (02) P_MENSAJE_ERROR
            value = DBNull.Value;
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (09) P_NOMBRE_PUNTO_VENTA
            value = DBNull.Value;
            AddParameter(OutParameter.NombrePuntoVenta, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (10) P_NUMERO_SUBCODIGO
            value = DBNull.Value;
            AddParameter(OutParameter.NumeroSubcodigo, OracleDbType.Int32, value, ParameterDirection.Output);
            // (11) P_NOMBRE_GRUPO
            value = DBNull.Value;
            AddParameter(OutParameter.NombreGrupo, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (12) P_FECHA_SALIDA
            value = DBNull.Value;
            AddParameter(OutParameter.FechaSalida, OracleDbType.Date, value, ParameterDirection.Output);
            // (13) P_FECHA_RETORNO
            value = DBNull.Value;
            AddParameter(OutParameter.FechaRetorno, OracleDbType.Date, value, ParameterDirection.Output);
            // (14) P_NOMBRE_ORIGEN
            value = DBNull.Value;
            AddParameter(OutParameter.NombreOrigen, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (15) P_NOMBRE_PAIS
            value = DBNull.Value;
            AddParameter(OutParameter.NombrePais, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (14) P_NOMBRE_CIUDAD
            value = DBNull.Value;
            AddParameter(OutParameter.NombreCiudad, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (15) P_NOMBRE_VENDEDOR_COUNTER
            value = DBNull.Value;
            AddParameter(OutParameter.NombreVendedorCounter, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (16) P_NOMBRE_VENDEDOR_COTIZADOR
            value = DBNull.Value;
            AddParameter(OutParameter.NombreVendedorCotizador, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (17) P_NOMBRE_VENDEDOR_RESERVA
            value = DBNull.Value;
            AddParameter(OutParameter.NombreVendedorReserva, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (18) P_ID_CUENTA
            value = DBNull.Value;
            AddParameter(OutParameter.IdCuenta, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (19) P_ID_COTIZACION
            value = DBNull.Value;
            AddParameter(OutParameter.IdCotizacion, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.DM_Asociate_Cotizacion);

            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.NombrePuntoVenta] = GetOutParameter(OutParameter.NombrePuntoVenta).ToString();
            operation[OutParameter.NumeroSubcodigo] = (int.TryParse(GetOutParameter(OutParameter.NumeroSubcodigo).ToString(), out int subCodigo)) ? subCodigo : 0;
            operation[OutParameter.NombreGrupo] = GetOutParameter(OutParameter.NombreGrupo).ToString();
            operation[OutParameter.FechaSalida] = (DateTime.TryParse(GetOutParameter(OutParameter.FechaSalida).ToString(), out DateTime fechaSalida)) ? fechaSalida : OutParameter.MinDate;
            operation[OutParameter.FechaRetorno] = (DateTime.TryParse(GetOutParameter(OutParameter.FechaRetorno).ToString(), out DateTime fechaRetorno)) ? fechaRetorno : OutParameter.MinDate;
            operation[OutParameter.NombreOrigen] = GetOutParameter(OutParameter.NombreOrigen).ToString();
            operation[OutParameter.NombrePais] = GetOutParameter(OutParameter.NombrePais).ToString();
            operation[OutParameter.NombreCiudad] = GetOutParameter(OutParameter.NombreCiudad).ToString();
            operation[OutParameter.NombreVendedorCounter] = GetOutParameter(OutParameter.NombreVendedorCounter).ToString();
            operation[OutParameter.NombreVendedorCotizador] = GetOutParameter(OutParameter.NombreVendedorCotizador).ToString();
            operation[OutParameter.NombreVendedorReserva] = GetOutParameter(OutParameter.NombreVendedorReserva).ToString();
            operation[Operation.Result] = ResultType.Success;
            #endregion

            return operation;
        }

        public Operation GetAllModified()
        {
            Operation operation = new Operation();
            object value;

            #region Parameters
            // (1) P_CODIGO_ERROR
            value = DBNull.Value;
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (2) P_MENSAJE_ERROR
            value = DBNull.Value;
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (3) P_COTIZACION
            value = DBNull.Value;
            AddParameter(OutParameter.CursorCotizacion, OracleDbType.RefCursor, value, ParameterDirection.Output);
            // (4) P_COTIZACION_DETALLE
            value = DBNull.Value;
            AddParameter(OutParameter.CursorCotizacionDet, OracleDbType.RefCursor, value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.DM_Send_Cotizacion);

            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorCotizacion] = ToCotizacion(GetDtParameter(OutParameter.CursorCotizacion));
            operation[OutParameter.CursorCotizacionDet] = GetDtParameter(OutParameter.CursorCotizacionDet);
            operation[Operation.Result] = ResultType.Success;
            #endregion

            return operation;
        }
        #endregion

        #region Auxiliar
        public IEnumerable<Cotizacion> ToCotizacion(DataTable dt)
        {
            try
            {
                var cotizaciones = new List<Cotizacion>();
                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    if (!int.TryParse(row["COTIZACION"].ToString(), out int id_cotizacion)) id_cotizacion = 0;
                    var propietario = (row["PROPIETARIO_CUENTA"] ?? string.Empty).ToString();
                    var estado = (row["ESTADO"] ?? string.Empty).ToString();
                    var unidad_negocio = (row["UNIDAD_NEGOCIO"] ?? string.Empty).ToString();
                    var branch = (row["BRANCH"] ?? string.Empty).ToString();
                    var referencia = (row["REFERENCIA"] ?? string.Empty).ToString();
                    if (!int.TryParse((row["NUM_ADULTOS"] ?? "0").ToString(), out var num_adultos)) num_adultos = 0;
                    if (!int.TryParse((row["NUM_NINOS"] ?? "0").ToString(), out var num_ninos)) num_ninos = 0;
                    var rango = (row["RANGO"] ?? string.Empty).ToString();
                    var cliente = (row["CLIENTE"] ?? string.Empty).ToString();
                    var contacto = (row["CONTACTO"] ?? string.Empty).ToString();
                    var pais = (row["PAIS"] ?? string.Empty).ToString();
                    if (!DateTime.TryParse(row["FECHA_COTIZACION"].ToString(), out var fecha_cotizacion)) fecha_cotizacion = DateTime.Today;
                    if (!DateTime.TryParse(row["FECHA_INI_VIGENCIA"].ToString(), out var fecha_ini_vigencia)) fecha_ini_vigencia = DateTime.Today;
                    if (!DateTime.TryParse(row["FECHA_FIN_VIGENCIA"].ToString(), out var fecha_fin_vigencia)) fecha_fin_vigencia = DateTime.Today;
                    if (!DateTime.TryParse(row["FECHA_INI_SERVICIO"].ToString(), out var fecha_ini_servicio)) fecha_ini_servicio = DateTime.Today;
                    if (!DateTime.TryParse(row["FECHA_FIN_SERVICIO"].ToString(), out var fecha_fin_servicio)) fecha_fin_servicio = DateTime.Today;
                    if (!DateTime.TryParse(row["FECHA_SOLICITUD"].ToString(), out var fecha_solicitud)) fecha_solicitud = DateTime.Today;
                    if (!float.TryParse((row["MARKUP_HOTEL"] ?? "0").ToString(), out var markup_hotel)) markup_hotel = 0;
                    var liberados_venta = (row["LIBERADOS_VENTA"] ?? string.Empty).ToString();
                    if (!float.TryParse((row["COSTO_FINAL"] ?? "0").ToString(), out var costo_final)) costo_final = 0;
                    if (!float.TryParse((row["VENTA_FINAL"] ?? "0").ToString(), out var venta_final)) venta_final = 0;
                    var tipo_vuelo = (row["TIPO_VUELO"] ?? string.Empty).ToString();
                    var destino = (row["DESTINO"] ?? string.Empty).ToString();
                    var clase = (row["CLASE"] ?? string.Empty).ToString();
                    var punto_venta = (row["PUNTO_VENTA"] ?? string.Empty).ToString();
                    var servicio = (row["SERVICIO"] ?? string.Empty).ToString();
                    var canal_venta = (row["CANAL_VENTA"] ?? string.Empty).ToString();
                    var seguimiento = (row["SEGUIMIENTO"] ?? string.Empty).ToString();
                    var punto_de_contacto = (row["PUNTO_DE_CONTACTO"] ?? string.Empty).ToString();
                    var counter = (row["COUNTER"] ?? string.Empty).ToString();
                    var reservado_por = (row["RESERVADO_POR"] ?? string.Empty).ToString();
                    var registrada_por = (row["REGISTRADA_POR"] ?? string.Empty).ToString();
                    if (!bool.TryParse((row["ES_BLOQUEO"] ?? "0").ToString(), out var es_bloqueo)) es_bloqueo = false;
                    if (!bool.TryParse((row["ES_GRUPO"] ?? "0").ToString(), out var es_grupo)) es_grupo = false;
                    var condicion_pago = (row["CONDICION_PAGO"] ?? string.Empty).ToString();
                    var motivo_no_venta = (row["MOTIVO_NO_VENTA"] ?? string.Empty).ToString();
                    var notas = (row["NOTAS"] ?? string.Empty).ToString();
                    #endregion

                    #region AddingElement
                    cotizaciones.Add(new Cotizacion()
                    {
                        Cliente_Cliente = cliente,
                       
                    });
                    #endregion
                }
                return cotizaciones;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region NotImplemented
        public Operation Create(Cotizacion entity)
        {
            throw new NotImplementedException();
        }

        public Operation Update(Cotizacion entity)
        {
            throw new NotImplementedException();
        }

        public Operation Read(Cotizacion entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}