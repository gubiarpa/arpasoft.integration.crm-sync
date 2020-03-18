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
    public class InformacionPagoNMRepository : OracleBase<object>, IInformacionPagoNMRepository
    {
        #region Constructor
        public InformacionPagoNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
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
            AddParameter(OutParameter.CursorInformacionPagoNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_SolicitudPagoNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorInformacionPagoNM] = ToInformacionPagoNM(GetDtParameter(OutParameter.CursorInformacionPagoNM));
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<InformacionPagoNM> ToInformacionPagoNM(DataTable dt)
        {
            try
            {
                var informacionPagoNMList = new List<InformacionPagoNM>();

                foreach (DataRow row in dt.Rows)
                {
                    informacionPagoNMList.Add(new InformacionPagoNM()
                    {
                        tipoServicio = row.StringParse("ACCION"),
                        tipoPasajero = row.StringParse("DK_CUENTA")
                    });
                }
                return informacionPagoNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}