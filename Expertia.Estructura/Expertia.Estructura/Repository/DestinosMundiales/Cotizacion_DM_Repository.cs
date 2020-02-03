using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.DestinosMundiales
{
    public class Cotizacion_DM_Repository : OracleBase<Cotizacion>, ICotizacion_DM
    {
        #region Constructor
        public Cotizacion_DM_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.DestinosMundiales) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetCotizaciones()
        {
            try
            {
                var operation = new Operation();

                #region Parameters
                /// (01) P_CODIGO_ERROR
                AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (02) P_MENSAJE_ERROR
                AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (03) P_COTIZACION
                AddParameter(OutParameter.CursorCotizacionDM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.DM_Read_Cotizacion);

                operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
                operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
                operation[OutParameter.CursorCotizacionDM] = ToCotizacion(GetDtParameter(OutParameter.CursorCotizacionDM));
                #endregion

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation UpdateCotizacion(Cotizacion_DM cotizacion)
        {
            return null;
        }
        #endregion

        #region Auxiliar
        private IEnumerable<Cotizacion_DM> ToCotizacion(DataTable dt)
        {
            try
            {
                var cotizaciones = new List<Cotizacion_DM>();
                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    var idOportunidadSf = row.StringParse("ID_OPORTUNIDAD_SF");
                    var idCotizacionSf = row.StringParse("ID_COTIZACION_SF");
                    var idCotizacion = row.IntNullParse("COTIZACION");
                    var montoCotizacion = row.FloatNullParse("MONTO_COTIZACION");
                    var montoComision = row.FloatNullParse("MONTO_COMISION");
                    var estadoCotizacion = row.StringParse("ESTADO_COTIZACION");
                    var nombreCotizacion = row.StringParse("NOMBRE_COTIZACION");
                    var numPasajerosAdult = row.IntNullParse("NUM_PASAJEROS_ADL");
                    var numPasajerosChild = row.IntNullParse("NUM_PASAJEROS_CHD");
                    var numPasajerosTotal = row.IntNullParse("NUM_PASAJEROS_TOT");
                    #endregion

                    #region AddingElement
                    cotizaciones.Add(new Cotizacion_DM()
                    {
                        IdOportunidadSf = idOportunidadSf,
                        IdCotizacionSf = idCotizacionSf,
                        IdCotizacion = idCotizacion,
                        MontoCotizacion = montoCotizacion,
                        MontoComision = montoComision,
                        EstadoCotizacion = estadoCotizacion,
                        NombreCotizacion = nombreCotizacion,
                        NumPasajerosAdult = numPasajerosAdult,
                        NumPasajerosChild = numPasajerosChild,
                        NumPasajerosTotal = numPasajerosTotal
                    });
                    #endregion
                }
                return cotizaciones;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}