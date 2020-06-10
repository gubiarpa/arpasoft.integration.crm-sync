using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.NuevoMundo
{
    public class DetallePasajerosNMRepository : OracleBase<object>, IDetallePasajerosNMRepository
    {
        #region Constructor
        public DetallePasajerosNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetPasajeros()
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_DETALLEPASAJEROSNM
            AddParameter(OutParameter.CursorDetallePasajerosNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_DetallePasajerosNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorDetallePasajerosNM] = ToDetallePasajerosNM(GetDtParameter(OutParameter.CursorDetallePasajerosNM));
            #endregion

            return operation;
        }

        public Operation Update(RptaPasajeroSF RptaPasajeroNM)
        {
            var operation = new Operation();

            #region Parameters            
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, RptaPasajeroNM.CodigoError, ParameterDirection.Input, 2);
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, RptaPasajeroNM.MensajeError, ParameterDirection.Input, 1000);
            AddParameter(OutParameter.SF_IDOPORTUNIDAD_NM, OracleDbType.Varchar2, RptaPasajeroNM.idOportunidad_SF);
            AddParameter(OutParameter.SF_IDPASAJERO_NM, OracleDbType.Varchar2, RptaPasajeroNM.idPasajero_SF);            
            AddParameter(OutParameter.IdCodeIdentifyNM, OracleDbType.Varchar2, (RptaPasajeroNM.Identificador_NM.Contains("-") ? RptaPasajeroNM.Identificador_NM.Split('-')[0] : ""));
            AddParameter(OutParameter.IdIdentificadorNM, OracleDbType.Int64, (RptaPasajeroNM.Identificador_NM.Contains("-") ? Convert.ToInt64(RptaPasajeroNM.Identificador_NM.Split('-')[1]) : -1));
            AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Upd_DetallePasajerosNM);
            operation[OutParameter.IdActualizados] = GetOutParameter(OutParameter.IdActualizados);
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<DetallePasajerosNM> ToDetallePasajerosNM(DataTable dt)
        {
            try
            {
                var detallePasajerosNMList = new List<DetallePasajerosNM>();

                foreach (DataRow row in dt.Rows)
                {
                    detallePasajerosNMList.Add(new DetallePasajerosNM()
                    {
                        idOportunidad_SF = row.StringParse("idOportunidad_SF"),
                        idPasajero_SF = (Convert.IsDBNull(row["idPasajero_SF"]) == false ? row.StringParse("idPasajero_SF") : null),
                        Identificador_NM = row.StringParse("Identificador_NM"),
                        id_reserva = row.IntParse("IdReserva"),
                        IdPasajero = row.StringParse("IdPasajero"),
                        tipo = row.StringParse("Tipo"),
                        pais = row.StringParse("Pais"),
                        apellidos = row.StringParse("Apellidos"),
                        nombre = row.StringParse("Nombre"),
                        tipoDocumento = row.StringParse("TipoDocumento"),
                        nroDocumento = row.StringParse("NroDocumento"),
                        fechaNacimiento = row.StringParse("FechaNacimiento"),                        
                        Genero = row.StringParse("Genero"),
                        FOID = row.StringParse("FOID"),                        
                        NombreReniec = row.StringParse("NombreReniec"),
                        numHabitacionPaquete = row.StringParse("NumHabitacionPaquete"),
                        accion_SF = row.StringParse("Accion_SF")
                    });
                    if(Convert.IsDBNull(row["FEE"]) == false)
                    {
                        detallePasajerosNMList[detallePasajerosNMList.Count - 1].FEE = row.FloatParse("FEE");
                    }                    
                }

                return detallePasajerosNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}