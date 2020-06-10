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
    public class DetalleHotelNMRepository : OracleBase<object>, IDetalleHotelNMRepository
    {
        #region Constructor
        public DetalleHotelNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetDetalleHoteles()
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_DETALLEPASAJEROSNM
            AddParameter(OutParameter.CursorDetalleHotelNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_DetalleHotelNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorDetalleHotelNM] = ToDetalleHotelNM(GetDtParameter(OutParameter.CursorDetalleHotelNM));
            #endregion

            return operation;
        }

        public Operation Update(RptaHotelSF RptaHotelNM)
        {
            var operation = new Operation();

            #region Parameters            
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, RptaHotelNM.CodigoError, ParameterDirection.Input, 2);
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, RptaHotelNM.MensajeError, ParameterDirection.Input, 1000);
            AddParameter(OutParameter.SF_IDOPORTUNIDAD_NM, OracleDbType.Varchar2, RptaHotelNM.idOportunidad_SF);
            AddParameter(OutParameter.SF_IDHOTEL_NM, OracleDbType.Varchar2, RptaHotelNM.idDetalleHotel_SF);            
            AddParameter(OutParameter.IdIdentificadorNM, OracleDbType.Int64, Convert.ToInt64(RptaHotelNM.Identificador_NM));
            AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Upd_DetalleHotelNM);
            operation[OutParameter.IdActualizados] = GetOutParameter(OutParameter.IdActualizados);
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<DetalleHotelNM> ToDetalleHotelNM(DataTable dt)
        {
            try
            {
                var detalleHotelNMList = new List<DetalleHotelNM>();

                foreach (DataRow row in dt.Rows)
                {
                    detalleHotelNMList.Add(new DetalleHotelNM()
                    {
                        idOportunidad_SF = row.StringParse("IdOportunidad_SF"),
                        idDetalleHotel_SF = (Convert.IsDBNull(row["idDetalleHotel_SF"]) == false ? row.StringParse("idDetalleHotel_SF") : null),
                        Identificador_NM = row.StringParse("Identificador_NM"),                        
                        hotel = row.StringParse("Hotel"),
                        direccion = row.StringParse("Direccion"),
                        destino = row.StringParse("Destino"),
                        categoria = row.StringParse("Categoria"),
                        fechaIngreso = row.StringParse("FechaIngreso"),
                        fechaSalida = row.StringParse("FechaSalida"),
                        fechaCancelacion = row.StringParse("FechaCancelacion"),
                        codigoReservaNemo = row.StringParse("CodigoReservaNemo"),
                        Proveedor = row.StringParse("Proveedor"),                        
                        accion_SF = row.StringParse("Accion_SF")
                    });
                }

                return detalleHotelNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}