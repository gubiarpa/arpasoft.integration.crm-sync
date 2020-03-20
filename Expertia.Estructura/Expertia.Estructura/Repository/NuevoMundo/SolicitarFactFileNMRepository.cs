using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.NuevoMundo
{
    public class SolicitarFactFileNMRepository : OracleBase<GenCodigoPagoNM>
    {
        public SolicitarFactFileNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

        public Operation SolicitarFactFile(SolicitarFactFileNM solicitarFacFile)
        {
            var operation = new Operation();

            #region Parameter
            /// (01) P_CODIGO_ERROR
            AddParameter(OutParameter.SF_Codigo, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (02) P_MENSAJE_ERROR
            AddParameter(OutParameter.SF_Mensaje, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (03) P_ID_OPORTUNIDAD_SF
            AddParameter("P_DETALLE_SERVICIO", OracleDbType.Varchar2, solicitarFacFile.idDatosFacturacion);
            
            #endregion

            #region Invoke
            try
            {
                ExecuteStoredProcedure(StoredProcedureName.AW_Solicitar_Facturacion_FileNM);

                operation[OutParameter.SF_Codigo] = GetOutParameter(OutParameter.SF_Codigo);
                operation[OutParameter.SF_Mensaje] = GetOutParameter(OutParameter.SF_Mensaje);
            }
            catch (Exception)
            {

                operation[OutParameter.SF_Codigo] = "OK, Esto es codigo duro";
                operation[OutParameter.SF_Mensaje] = "Mensaje: Esto es codigo duro";
            }
            
            #endregion

            return operation;
        }
    }
}