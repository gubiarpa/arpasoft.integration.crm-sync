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
    public class Oportunidad_IA_Repository : OracleBase<Oportunidad>, IOportunidadRepository
    {
        #region Constructor
        public Oportunidad_IA_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.Interagencias) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetOportunidades()
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_OPORTUNIDAD
            AddParameter(OutParameter.CursorOportunidad, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            var spName = string.Empty;
            switch (_unidadNegocio)
            {
                case UnidadNegocioKeys.DestinosMundiales:
                    spName = StoredProcedureName.DM_Read_Oportunidad;
                    break;
                case UnidadNegocioKeys.Interagencias:
                    spName = StoredProcedureName.IA_Read_Oportunidad;
                    break;
                case UnidadNegocioKeys.AppWebs:
                    spName = StoredProcedureName.AW_Read_Oportunidad;
                    break;
            }
            ExecuteStoredProcedure(spName);
            operation[OutParameter.CursorOportunidad] = ToOportunidad(GetDtParameter(OutParameter.CursorOportunidad));
            #endregion

            return operation;
        }

        public Operation Update(Oportunidad oportunidad)
        {
            var operation = new Operation();

            #region Parameters
            /// (01) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (02) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (03) P_PNR
            AddParameter("P_PNR", OracleDbType.Varchar2, oportunidad.Pnr1);
            /// (04) P_ID_CLIENTE
            AddParameter("P_ID_CLIENTE", OracleDbType.Int32, oportunidad.DkCuenta);
            /// (05) P_ID_OPORTUNIDAD_CRM
            AddParameter("P_ID_OPORTUNIDAD_CRM", OracleDbType.Varchar2, oportunidad.IdOportunidad);
            /// (06) P_NOMBRE_SUCURSAL
            AddParameter("P_NOMBRE_SUCURSAL", OracleDbType.Varchar2, oportunidad.Sucursal);
            /// (07) P_ID_FILE
            AddParameter("P_ID_FILE", OracleDbType.Int32, oportunidad.IdFile);
            /// (07) P_ES_ATENCION
            AddParameter("P_ES_ATENCION", OracleDbType.Varchar2, oportunidad.CodigoError);
            /// (08) P_DESCRIPCION
            AddParameter("P_DESCRIPCION", OracleDbType.Varchar2, oportunidad.MensajeError);
            /// (09) P_ACTUALIZADOS
            AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            var spName = string.Empty;
            switch (_unidadNegocio)
            {
                case UnidadNegocioKeys.DestinosMundiales:
                    spName = StoredProcedureName.DM_Update_Oportunidad;
                    break;
                case UnidadNegocioKeys.Interagencias:
                    spName = StoredProcedureName.IA_Update_Oportunidad;
                    break;
                case UnidadNegocioKeys.AppWebs:
                    spName = StoredProcedureName.AW_Update_Oportunidad;
                    break;
            }
            ExecuteStoredProcedure(spName);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.IdActualizados] = GetOutParameter(OutParameter.IdActualizados);
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<Oportunidad> ToOportunidad(DataTable dt)
        {
            try
            {
                var oportunidadList = new List<Oportunidad>();

                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    var idOportunidad = row.StringParse("ID_OPORTUNIDAD");
                    var accion = row.StringParse("ACCION");
                    var etapa = row.StringParse("ETAPA");
                    var dkCuenta = row.StringParse("DK_CUENTA");
                    var unidadNegocio = row.StringParse("UNIDAD_NEGOCIO");
                    var sucursal = row.StringParse("SUCURSAL");
                    var puntoVenta = row.StringParse("PUNTO_VENTA");
                    var subcodigo = row.StringParse("SUBCODIGO");
                    var fechaOportunidad = row.DateTimeParse("FECHA_OPORTUNIDAD");
                    var nombreOportunidad = row.StringParse("NOMBRE_OPORTUNIDAD");
                    var origenOportunidad = row.StringParse("ORIGEN_OPORTUNIDAD");
                    var medioOportunidad = row.StringParse("MEDIO_OPORTUNIDAD");
                    var gds = row.StringParse("GDS");
                    var tipoProducto = row.StringParse("TIPO_PRODUCTO");
                    var rutaViaje = row.StringParse("RUTA_VIAJE");
                    var ciudadOrigen = row.StringParse("CIUDAD_ORIGEN");
                    var ciudadDestino = row.StringParse("CIUDAD_DESTINO");
                    var tipoRuta = row.StringParse("TIPO_RUTA");
                    var numPasajeros = row.IntParse("NUM_PASAJEROS");
                    var fechaInicioViaje1 = row.DateTimeParse("FECHA_INICIO_VIAJE_1");
                    var fechaFinViaje1 = row.DateTimeParse("FECHA_FIN_VIAJE_1");
                    var fechaInicioViaje2 = row.DateTimeParse("FECHA_INICIO_VIAJE_2");
                    var fechaFinViaje2 = row.DateTimeParse("FECHA_FIN_VIAJE_2");
                    var montoEstimado = row.FloatParse("MONTO_ESTIMADO");
                    var montoReal = row.FloatParse("MONTO_REAL");
                    var pnr_1 = row.StringParse("PNR_1");
                    var pnr_2 = row.StringParse("PNR_2");
                    var motivoPerdida = row.StringParse("MOTIVO_PERDIDA");
                    var idFile =
                        (new List<UnidadNegocioKeys?>() { UnidadNegocioKeys.Interagencias, UnidadNegocioKeys.AppWebs }).Contains(_unidadNegocio) ?
                            row.IntNullParse("ID_FILE") : 0;
                    var contacto =
                        (new List<UnidadNegocioKeys?>() { UnidadNegocioKeys.DestinosMundiales }).Contains(_unidadNegocio) ?
                            row.StringParse("CONTACTO") : string.Empty;
                    var counter_ventas =
                        (new List<UnidadNegocioKeys?>() { UnidadNegocioKeys.DestinosMundiales }).Contains(_unidadNegocio) ?
                            row.StringParse("COUNTER_VENTAS") : string.Empty;
                    var counter_admin =
                        (new List<UnidadNegocioKeys?>() { UnidadNegocioKeys.DestinosMundiales }).Contains(_unidadNegocio) ?
                            row.StringParse("COUNTER_ADM") : string.Empty;
                    #endregion

                    #region AddingElement
                    oportunidadList.Add(new Oportunidad()
                    {
                        IdOportunidad = idOportunidad,
                        IdFile = idFile,
                        Accion = accion,
                        Etapa = etapa,
                        DkCuenta = dkCuenta,
                        UnidadNegocio = unidadNegocio,
                        Sucursal = sucursal,
                        PuntoVenta = puntoVenta,
                        Subcodigo = subcodigo,
                        FechaOportunidad = fechaOportunidad,
                        NombreOportunidad = nombreOportunidad,
                        OrigenOportunidad = origenOportunidad,
                        MedioOportunidad = medioOportunidad,
                        GDS = gds,
                        TipoProducto = tipoProducto,
                        RutaViaje = rutaViaje,
                        CiudadOrigen = ciudadOrigen,
                        CiudadDestino = ciudadDestino,
                        TipoRuta = tipoRuta,
                        NumPasajeros = numPasajeros,
                        FechaInicioViaje1 = fechaInicioViaje1,
                        FechaFinViaje1 = fechaFinViaje1,
                        FechaInicioViaje2 = fechaInicioViaje2,
                        FechaFinViaje2 = fechaFinViaje2,
                        MontoEstimado = montoEstimado,
                        MontoReal = montoReal,
                        Pnr1 = pnr_1,
                        Pnr2 = pnr_2,
                        MotivoPerdida = motivoPerdida,
                        Contacto = contacto,
                        CounterVentas = counter_ventas,
                        CounterAdmin = counter_admin
                    });
                    #endregion
                }
                return oportunidadList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}