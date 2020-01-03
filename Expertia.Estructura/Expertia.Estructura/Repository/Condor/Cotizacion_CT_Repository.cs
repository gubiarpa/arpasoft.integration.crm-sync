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
            var idOportunidadSf = cotizacionRequest.IdOportunidadSf;
            var idCotizacionSf = cotizacionRequest.IdCotizacionSf;
            var cotizacion = cotizacionRequest.Cotizacion;
            #endregion

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_ID_OPORTUNIDAD_SF
            AddParameter("P_ID_OPORTUNIDAD_SF", OracleDbType.Varchar2, idOportunidadSf);
            /// (4) P_ID_FILE_SF
            AddParameter("P_ID_FILE_SF", OracleDbType.Varchar2, idCotizacionSf);
            /// (5) P_COTIZACION
            AddParameter("P_COTIZACION", OracleDbType.Varchar2, cotizacion);
            /// (6) P_CUR_COTIZACION
            AddParameter(OutParameter.CursorCotizacion, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.CT_Obtiene_Cotizacion);
            operation[OutParameter.CursorCotizacion] = ToCotizacion(GetDtParameter(OutParameter.CursorCotizacion));
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
        #endregion            
    }
}