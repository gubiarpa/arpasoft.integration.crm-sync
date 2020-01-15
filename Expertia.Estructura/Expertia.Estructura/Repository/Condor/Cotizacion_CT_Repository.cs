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
    public class Cotizacion_CT_Repository : OracleBase<Cotizacion>, ICotizacionCT
    {
        #region Constructor
        public Cotizacion_CT_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.CondorTravel) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetCotizacionCT(CotizacionRequest cotizacionRequest)
        {
            var operation = new Operation();

            #region Loading
            var usuario = cotizacionRequest.Usuario;
            var cuentaSf = cotizacionRequest.IdCuentaSf;
            var idOportunidadSf = cotizacionRequest.IdOportunidadSf;
            var idCotizacionSf = cotizacionRequest.IdCotizacionSf;
            var cotizacion = cotizacionRequest.Cotizacion;
            var accion = cotizacionRequest.Accion;
            #endregion

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_NOMBRE_USUARIO
            AddParameter("P_NOMBRE_USUARIO", OracleDbType.Varchar2, usuario);
            /// (4) P_ID_CUENTA_SF
            AddParameter("P_ID_CUENTA_SF", OracleDbType.Varchar2, cuentaSf);
            /// (5) P_ID_OPORTUNIDAD_SF
            AddParameter("P_ID_OPORTUNIDAD_SF", OracleDbType.Varchar2, idOportunidadSf);
            /// (6) P_ID_COTIZACION_SF
            AddParameter("P_ID_COTIZACION_SF", OracleDbType.Varchar2, idCotizacionSf);
            /// (7) P_COTIZACION
            AddParameter("P_COTIZACION", OracleDbType.Varchar2, cotizacion);
            /// (8) P_ACCION
            AddParameter("P_ACCION", OracleDbType.Varchar2, accion);
            /// (9) P_RECORDSET
            AddParameter(OutParameter.CursorCotizacion, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.CT_Obtiene_Cotizacion);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorCotizacion] = ToCotizacionResponse(GetDtParameter(OutParameter.CursorCotizacion));
            #endregion

            return operation;
        }

        public Operation GetOperationJourney()
        {
            var operation = new Operation();

            #region Parameter
            /// (01) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (02) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (03) P_NOMBRE_USUARIO
            AddParameter(OutParameter.CursorCotizacion, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.CT_Obtiene_NovedadCotizacion);

            operation[OutParameter.CursorCotizacion] = ToCotizacionJY(GetDtParameter(OutParameter.CursorCotizacion));
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
                    var grupo = row.StringParse("Grupo");
                    var cliente = row.StringParse("Cliente");
                    var cliente_Cliente = row.StringParse("Cliente_cliente");
                    var ejecutivo = row.StringParse("Ejecutivo");
                    var unidad_negocio = row.StringParse("UNIDAD_NEGOCIO");
                    var branch = row.StringParse("BRANCH");
                    var fecha_Apertura = row.DateTimeParse("FECHA_COTIZ");
                    var fecha_Inicio = row.DateTimeParse("FECHA_INI_SERVICIO");
                    var fecha_Fin = row.DateTimeParse("FECHA_FIN_SERVICIO");        
                    #endregion

                    #region AddingElement
                    cotizaciones.Add(new Cotizacion()
                    {   
                        Grupo = grupo,
                        Cliente = cliente,
                        Cliente_Cliente = cliente_Cliente,
                        Ejecutivo = ejecutivo,
                        Unidad_Negocio = unidad_negocio,
                        Branch = branch,
                        Fecha_Apertura = fecha_Apertura,
                        Fecha_Inicio = fecha_Inicio,
                        Fecha_Fin = fecha_Fin

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

        public IEnumerable<CotizacionJYResponse> ToCotizacionJY(DataTable dt)
        {
            try
            {
                var cotizaciones = new List<CotizacionJYResponse>();
                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    var id_oportunidad_sf = row.StringParse("");
                    var id_cotizacion_sf = row.StringParse("");
                    var id_cuenta_sf = row.StringParse("");
                    var cotizacion = row.StringParse("");
                    var grupo = row.StringParse("");
                    var venta_estimada = row.FloatParse("");
                    var elegida = row.StringParse("").Equals("");
                    var file_subfile = row.StringParse("");
                    var venta_file = row.FloatParse("");
                    var margen_file = row.FloatParse("");
                    var paxs_file = row.IntParse("");
                    var estado_file = row.StringParse("");
                    #endregion

                    #region AddingElement
                    cotizaciones.Add(new CotizacionJYResponse()
                    {
                        IdOportunidadSf = id_oportunidad_sf,
                        IdCotizacionSf = id_cotizacion_sf,
                        IdCuentaSf = id_cuenta_sf,
                        Cotizacion = cotizacion,
                        Grupo = grupo,
                        VentaEstimada = venta_estimada,
                        Elegida = elegida,
                        FileSubfile = file_subfile,
                        VentaFile = venta_file,
                        MargenFile = margen_file,
                        PaxsFile = paxs_file,
                        EstadoFile = estado_file
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

        public IEnumerable<CotizacionResponse> ToCotizacionResponse(DataTable dt)
        {
            try
            {
                var cotizaciones = new List<CotizacionResponse>();

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        #region Loading
                        var grupo = row.StringParse("GRUPO");
                        var estado = row.StringParse("ESTADO");
                        var ventaEstimada = row.FloatParse("VENTA_ESTIMADA");
                        var elegida = row.StringParse("ELEGIDA").Equals(ApiResponseCode.SI);
                        var file = row.StringParse("FILE_SUBFILE");
                        var venta_file = row.FloatParse("VENTA_FILE");
                        var margen_file = row.FloatParse("MARGEN_FILE");
                        var paxs_file = row.IntParse("PAXS_FILE");
                        var estado_file = row.StringParse("ESTADO_FILE");
                        #endregion

                        #region AddingElement
                        cotizaciones.Add(new CotizacionResponse()
                        {
                            Grupo = grupo,
                            Estado = estado,
                            VentaEstimada = ventaEstimada,
                            Elegida = elegida,
                            File = file,
                            VentaFile = venta_file,
                            MargenFile = margen_file,
                            PaxsFile = paxs_file,
                            EstadoFile = estado_file
                        });
                        #endregion
                    }
                }

                return cotizaciones;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion            
    }
}