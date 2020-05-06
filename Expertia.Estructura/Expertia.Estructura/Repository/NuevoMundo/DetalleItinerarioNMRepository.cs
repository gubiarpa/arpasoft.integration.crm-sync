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
    public class DetalleItinerarioNMRepository : OracleBase, IDetalleItinerarioNMRepository
    {
        #region Constructor
        public DetalleItinerarioNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetItinerarios()
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_DETALLEITINERARIONM
            AddParameter(OutParameter.CursorDetalleItinerarioNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_DetalleItinerarioNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorDetalleItinerarioNM] = ToDetalleItinerarioNM(GetDtParameter(OutParameter.CursorDetalleItinerarioNM));
            #endregion

            return operation;
        }

        public Operation Update(RptaItinerarioSF RptaItinerarioNM)
        {
            var operation = new Operation();

            #region Parameters            
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, RptaItinerarioNM.CodigoError, ParameterDirection.Input, 2);
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, RptaItinerarioNM.MensajeError, ParameterDirection.Input, 1000);
            AddParameter(OutParameter.SF_IDOPORTUNIDAD_NM, OracleDbType.Varchar2, RptaItinerarioNM.idOportunidad_SF);            
            AddParameter(OutParameter.SF_IDITINERARIO_NM, OracleDbType.Varchar2, RptaItinerarioNM.idItinerario_SF);
            AddParameter(OutParameter.IdIdentificadorNM, OracleDbType.Int64, Convert.ToInt64(RptaItinerarioNM.Identificador_NM));
            AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Upd_DetalleItinerarioNM);
            operation[OutParameter.IdActualizados] = GetOutParameter(OutParameter.IdActualizados);
            #endregion

            return operation;
        }

        #endregion

        #region Parse
        private IEnumerable<DetalleItinerarioNM> ToDetalleItinerarioNM(DataTable dt)
        {
            try
            {
                var detalleItinerarioNMList = new List<DetalleItinerarioNM>();

                foreach (DataRow row in dt.Rows)
                {
                    detalleItinerarioNMList.Add(new DetalleItinerarioNM()
                    {
                        idOportunidad_SF = row.StringParse("idOportunidad_SF"),
                        Identificador_NM = row.StringParse("Identificador_NM"),
                        id_reserva = row.IntParse("IdReserva"),
                        id_itinerario = row.StringParse("IdPosItinerario"),
                        LAerea = row.StringParse("LAerea"),
                        Origen = row.StringParse("Origen"),
                        Salida = row.StringParse("Salida"),
                        Destino = row.StringParse("Destino"),
                        llegada = row.StringParse("Llegada"),
                        numeroVuelo = row.IntParse("NumeroVuelo"),
                        Clase = row.StringParse("Clase"),
                        fareBasis = row.StringParse("FareBasis"),
                        OperadoPor = row.StringParse("OperadoPor"),
                        esRetornoItinerario = row.StringParse("esRetornoItinerario"),
                        accion_SF = row.StringParse("Accion_SF")
                    });
                }

                return detalleItinerarioNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}