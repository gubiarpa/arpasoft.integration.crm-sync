using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
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
                // (03) P_NOMBRE_USUARIO
                value = entity.Usuario.Descripcion.Coalesce();
                AddParameter("P_NOMBRE_USUARIO", OracleDbType.Varchar2, value);
                // (04) P_ID_CUENTA_SALESFORCE
                value = entity.IdCuentaSalesforce.Coalesce();
                AddParameter("P_ID_CUENTA_SALESFORCE", OracleDbType.Varchar2, value);
                // (05) P_ID_OPORTUNIDAD_SALESFORCE
                value = entity.IdOportunidadSalesforce.Coalesce();
                AddParameter("P_ID_OPORTUNIDAD_SALESFORCE", OracleDbType.Varchar2, value);
                // (06) P_ID_COTIZACION_SALESFORCE
                value = entity.IDCotizacionSalesforce.Coalesce();
                AddParameter("P_ID_COTIZACION_SALESFORCE", OracleDbType.Varchar2, value);
                // (07) P_NOMBRE_SUCURSAL
                value = entity.Surcursal.Descripcion.Coalesce();
                AddParameter("P_NOMBRE_SUCURSAL", OracleDbType.Varchar2, value);
                // (08) P_NOMBRE_PUNTO_VENTA
                value = entity.PuntoVenta.Descripcion.Coalesce();
                AddParameter("P_NOMBRE_PUNTO_VENTA", OracleDbType.Varchar2, value);
                // (09) P_NOMBRE_GRUPO
                value = entity.Grupo.Descripcion.Coalesce();
                AddParameter("P_NOMBRE_GRUPO", OracleDbType.Varchar2, value);
                // (10) P_FECHA_SALIDA
                value = entity.FechaSalida;
                AddParameter("P_FECHA_SALIDA", OracleDbType.Date, value);
                // (11) P_FECHA_RETORNO
                value = entity.FechaRetorno;
                AddParameter("P_FECHA_RETORNO", OracleDbType.Date, value);
                // (12) P_NOMBRE_ORIGEN
                value = entity.Origen.Descripcion.Coalesce();
                AddParameter("P_NOMBRE_ORIGEN", OracleDbType.Varchar2, value);
                // (13) P_NOMBRE_PAIS
                value = entity.Pais.Descripcion.Coalesce();
                AddParameter("P_NOMBRE_PAIS", OracleDbType.Varchar2, value);
                // (14) P_NOMBRE_CIUDAD
                value = entity.Ciudad.Descripcion.Coalesce();
                AddParameter("P_NOMBRE_CIUDAD", OracleDbType.Varchar2, value);
                // (15) P_NOMBRE_VENDEDOR_COUNTER
                value = entity.VendedorCounter.Descripcion.Coalesce();
                AddParameter("P_NOMBRE_VENDEDOR_COUNTER", OracleDbType.Varchar2, value);
                // (16) P_NOMBRE_VENDEDOR_COTIZADOR
                value = entity.VendedorCotizador.Descripcion.Coalesce();
                AddParameter("P_NOMBRE_VENDEDOR_COTIZADOR", OracleDbType.Varchar2, value);
                // (17) P_NOMBRE_VENDEDOR_RESERVA
                value = entity.VendedorReserva.Descripcion.Coalesce();
                AddParameter("P_NOMBRE_VENDEDOR_RESERVA", OracleDbType.Varchar2, value);
                // (18) P_ID_CUENTA
                value = entity.IdCuenta.Coalesce();
                AddParameter("P_ID_CUENTA", OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
                // (19) P_ID_COTIZACION
                value = entity.IdCotizacion.Coalesce();
                AddParameter("P_ID_COTIZACION", OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.DM_Generate_Cotizacion);

                operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
                operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
                operation[OutParameter.IdCuenta] = entity.IdCuenta = GetOutParameter(OutParameter.IdCuenta).ToString();
                operation[OutParameter.IdCotizacion] = entity.IdCotizacion = GetOutParameter(OutParameter.IdCotizacion).ToString();
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
            // (03) P_NOMBRE_USUARIO
            value = entity.Usuario.Descripcion.Coalesce();
            AddParameter("P_NOMBRE_USUARIO", OracleDbType.Varchar2, value);
            // (04) P_ID_CUENTA_SALESFORCE
            value = entity.IdCuentaSalesforce.Coalesce();
            AddParameter("P_ID_CUENTA_SALESFORCE", OracleDbType.Varchar2, value);
            // (05) P_ID_OPORTUNIDAD_SALESFORCE
            value = entity.IdOportunidadSalesforce.Coalesce();
            AddParameter("P_ID_OPORTUNIDAD_SALESFORCE", OracleDbType.Varchar2, value);
            // (06) P_ID_COTIZACION_SALESFORCE
            value = entity.IDCotizacionSalesforce.Coalesce();
            AddParameter("P_ID_COTIZACION_SALESFORCE", OracleDbType.Varchar2, value);
            // (07) P_NOMBRE_SUCURSAL
            value = entity.Surcursal.Descripcion.Coalesce();
            AddParameter("P_NOMBRE_SUCURSAL", OracleDbType.Varchar2, value);
            // (08) P_ID_COTIZACION_PTA
            value = entity.IdCotizacionPta;
            AddParameter("P_ID_COTIZACION_PTA", OracleDbType.Int32, value);
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
            operation[OutParameter.IdCuenta] = entity.IdCuenta = GetOutParameter(OutParameter.IdCuenta).ToString();
            operation[OutParameter.IdCotizacion] = entity.IdCotizacion = GetOutParameter(OutParameter.IdCotizacion).ToString();
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
            AddParameter(OutParameter.CurCotizacion, OracleDbType.RefCursor, value, ParameterDirection.Output);
            // (4) P_COTIZACION_DETALLE
            value = DBNull.Value;
            AddParameter(OutParameter.CurCotizacionDet, OracleDbType.RefCursor, value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.DM_Send_Cotizacion);

            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CurCotizacion] = GetDtParameter(OutParameter.CurCotizacion);
            operation[OutParameter.CurCotizacionDet] = GetDtParameter(OutParameter.CurCotizacionDet);
            operation[Operation.Result] = ResultType.Success;
            #endregion

            return operation;
        }
        #endregion

        #region Auxiliar
        public Operation ExecuteOperation(Cotizacion entity, string SPName, string userName)
        {
            throw new NotImplementedException();
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
        #endregion
    }
}