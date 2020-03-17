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
        public Operation Send(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs)
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
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_DetallePasajerosNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorDetalleHotelNM] = ToDetalleHotelNM(GetDtParameter(OutParameter.CursorDetalleHotelNM));
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
                        hotel = row.StringParse("ACCION"),
                        direccion = row.StringParse("DK_CUENTA")
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