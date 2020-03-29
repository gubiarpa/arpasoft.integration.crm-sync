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
    public class DetalleItinerarioNMRepository : OracleBase, ISendRepository<DetalleItinerarioNM>
    {
        #region Constructor
        public DetalleItinerarioNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation Read()
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

        public Operation Update(DetalleItinerarioNM entity)
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_IDOPORTUNIDAD_SF
            AddParameter("P_IDOPORTUNIDAD_SF", OracleDbType.Varchar2, entity.idOportunidad_SF);
            /// (4) P_IDITINERARIO_SF
            AddParameter("P_IDITINERARIO_SF", OracleDbType.Varchar2, entity.idItinerario_SF);
            /// (5) P_ACTUALIZADOS
            AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Set_DetalleItinerarioNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
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
                        lAerea = row.StringParse("LAerea"),
                        origen = row.StringParse("Origen"),
                        salida = row.StringParse("Salida"),
                        destino = row.StringParse("Destino"),
                        llegada = row.StringParse("Llegada"),
                        numeroVuelo = row.IntParse("NumeroVuelo"),
                        clase = row.StringParse("Clase"),
                        fareBasis = row.StringParse("FareBasis"),
                        operadoPor = row.StringParse("OperadoPor")
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