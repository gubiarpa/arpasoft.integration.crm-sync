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
    public class Cotizacion_DM_Repository : OracleBase<Cotizacion>
    {
        #region Constructor
        public Cotizacion_DM_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.DestinosMundiales) : base(ConnectionKeys.DMConnKey, unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetCotizacionesDM()
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
        #endregion

        #region Auxiliar
        private IEnumerable<CotizacionDM> ToCotizacion(DataTable dt)
        {
            try
            {
                var cotizaciones = new List<CotizacionDM>();
                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    var idOportunidadSf = row.StringParse("");
                    var idCotizacionSf = row.StringParse("");
                    var idCotizacion = row.IntNullParse("");
                    var montoCotizacion = row.FloatNullParse("");
                    var montoComision = row.FloatNullParse("");
                    var estadoCotizacion = row.StringParse("");
                    var nombreCotizacion = row.StringParse("");
                    var numPasajerosAdult = row.IntNullParse("");
                    var numPasajerosChild = row.IntNullParse("");
                    var numPasajerosTotal = row.IntNullParse("");
                    #endregion

                    #region AddingElement
                    cotizaciones.Add(new CotizacionDM()
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