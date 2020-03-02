using Expertia.Estructura.Models.Journeyou;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.Journeyou
{
    public class CotizacionJYRepository : OracleBase<Cotizacion_JY>
    {
        public CotizacionJYRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.CondorTravel) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

        public Operation GetCotizaciones(Cotizacion_JY cotizacion)
        {
            var operation = new Operation();

            #region Parameter
            /// (01) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (02) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (03) P_ID_OPORTUNIDAD_SF
            AddParameter("P_ID_OPORTUNIDAD_SF", OracleDbType.Varchar2, cotizacion.Id_Oportunidad_Sf);
            /// (04) P_ID_COTIZACION_SF
            AddParameter("P_ID_COTIZACION_SF", OracleDbType.Varchar2, cotizacion.Id_Cotizacion_Sf);
            /// (05) P_ID_CUENTA_SF
            AddParameter("P_ID_CUENTA_SF", OracleDbType.Varchar2, cotizacion.Id_Cuenta_Sf);
            /// (06) P_USUARIO
            AddParameter("P_USUARIO", OracleDbType.Varchar2, cotizacion.Usuario);
            /// (07) P_ACCION
            AddParameter("P_ACCION", OracleDbType.Varchar2, cotizacion.Accion);
            /// (08) P_TIPO_DOCUMENTO
            AddParameter("P_TIPO_DOCUMENTO", OracleDbType.Varchar2, cotizacion.Tipo_Documento);
            /// (09) P_DOCUMENTO
            AddParameter("P_DOCUMENTO", OracleDbType.Varchar2, cotizacion.Documento);
            /// (10) P_CORREO
            AddParameter("P_CORREO", OracleDbType.Varchar2, cotizacion.Correo);
            /// (11) P_COTIZACION
            AddParameter("P_COTIZACION", OracleDbType.Varchar2, cotizacion.Cotizacion);
            /// (12) P_FILE
            AddParameter("P_FILE", OracleDbType.Varchar2, cotizacion.File);
            /// (13) P_NUMERO_PAXS
            AddParameter("P_NUMERO_PAXS", OracleDbType.Varchar2, cotizacion.Numero_Paxs);
            /// (14) P_CUR_COTIZACION_ASOCIADA
            AddParameter(OutParameter.CursorCotizacionAsociada, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);

            #endregion



            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.CT_Obtiene_NovedadCotizacion);

            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorCotizacionAsociada] = ToCotizacionJY(GetDtParameter(OutParameter.CursorCotizacionAsociada));
            #endregion

            return operation;
        }

        private IEnumerable<CotizacionJYResponse> ToCotizacionJY(DataTable dt)
        {
            try
            {
                var cotizaciones = new List<CotizacionJYResponse>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        cotizaciones.Add(new CotizacionJYResponse()
                        {
                            Grupo = row.StringParse("Grupo"),
                            Estado = row.StringParse("Estado"),
                            VentaEstimada = row.FloatParse("Venta_Estimada"),
                            FileSubfile = row.StringParse("File_SubFile"),
                            VentaFile = row.FloatParse("Venta_File"),
                            MargenFile = row.FloatParse("Margen_File"),
                            PaxsFile = row.IntParse("Paxs_File"),
                            EstadoFile = row.StringParse("Estado_File"),
                            FechaInicioViaje = row.DateTimeParse("Fecha_Inicio_Viaje")
                        });
                    }
                }
                return cotizaciones;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Operation Lista_CotizacionB2C()
        {
            var operation = new Operation();

            #region Parameter
            /// (01) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (02) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (03) P_CUR_COTIZACION_B2C
            AddParameter(OutParameter.CursorCotizacionB2C, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);

            #endregion
                       
            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.CT_Lista_CotizacionB2C);

            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorCotizacionB2C] = ToCotizacionJYUpd(GetDtParameter(OutParameter.CursorCotizacionB2C));
            #endregion

            return operation;
        }

        private IEnumerable<CotizacionJYUpdResponse> ToCotizacionJYUpd(DataTable dt)
        {
            try
            {
                var cotizaciones = new List<CotizacionJYUpdResponse>();

                foreach (DataRow row in dt.Rows)
                {
                    cotizaciones.Add(new CotizacionJYUpdResponse()
                    {
                        ID_OPORTUNIDAD_SF = row.StringParse("id_oportunidad_crm"),
                        ID_COTIZACION_SF = row.StringParse("id_cotizacion_crm"),
                        ID_CUENTA_SF = row.StringParse("id_cuenta_crm"),
                        Cotizacion = row.StringParse("Cotizacion"),
                        GRUPO = row.StringParse("Grupo"),
                        ESTADO = row.StringParse("Estado"),
                        VENTA_ESTIMADA = row.FloatParse("Venta_Estimada"),
                        FILE_SUBFILE = row.StringParse("File_SubFile"),
                        VENTA_FILE = row.FloatParse("Venta_File"),
                        MARGEN_FILE = row.FloatParse("Margen_File"),
                        PAXS_FILE = row.IntParse("Paxs_File"),
                        ESTADO_FILE = row.StringParse("Estado_File"),
                        FECHA_INICIO_VIAJE = row.DateTimeParse("Fecha_Inicio_Viaje"),
                        Pais = row.StringParse("Pais")
                    });
                }
                return cotizaciones;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Operation Actualizar_EnvioCotizacionB2C(CotizacionJYUpd Cotizacion)
        {
            var operation = new Operation();

            #region Parameter
            /// (01) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (02) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (03) P_COTIZACION
            AddParameter("P_COTIZACION", OracleDbType.Varchar2, Cotizacion.Cotizacion);
            /// (04) P_FILE
            AddParameter("P_FILE", OracleDbType.Varchar2, Cotizacion.File);
            /// (05) P_ES_ATENCION
            AddParameter("P_ES_ATENCION", OracleDbType.Varchar2, Cotizacion.Es_Atencion);
            /// (06) P_DESCRIPCION
            AddParameter("P_DESCRIPCION", OracleDbType.Varchar2, Cotizacion.Descripcion);
            /// (07) P_ACTUALIZADOS
            AddParameter(OutParameter.NumeroActualizados, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            #endregion



            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.CT_Actualziar_EnvioCotizacionB2C);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.NumeroActualizados] = GetOutParameter(OutParameter.NumeroActualizados);
            #endregion

            return operation;
        }
    }
}