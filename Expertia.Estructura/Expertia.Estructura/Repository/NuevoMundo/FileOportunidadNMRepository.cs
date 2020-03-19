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
    public class FileOportunidadNMRepository : OracleBase<FileOportunidadNM>
    {
        public FileOportunidadNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

        public Operation AsociarFileOportunidad(FileOportunidadNM fileOportunidad)
        {
            var operation = new Operation();

            #region Parameter
            /// (01) P_CODIGO_ERROR
            AddParameter(OutParameter.SF_Codigo, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (02) P_MENSAJE_ERROR
            AddParameter(OutParameter.SF_Mensaje, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (03) P_ID_OPORTUNIDAD_SF
            AddParameter("P_ID_OPORTUNIDAD_SF", OracleDbType.Varchar2, fileOportunidad.idOportunidad_SF);
            /// (04) P_ID_COT_SRV_SF
            AddParameter("P_ID_COT_SRV_SF", OracleDbType.Varchar2, fileOportunidad.idCotSrv_SF);
            /// (05) P_FILE
            AddParameter("P_FILE", OracleDbType.Varchar2, fileOportunidad.file);
            /// (06) P_IMPORTE
            AddParameter("P_IMPORTE", OracleDbType.Varchar2, fileOportunidad.importe);
            /// (07) P_SUCURSAL
            AddParameter("P_SUCURSAL", OracleDbType.Varchar2, fileOportunidad.sucursal);
            /// (08) P_FECHA
            AddParameter("P_FECHA", OracleDbType.Varchar2, fileOportunidad.fecha);
            #endregion

            #region Invoke
            try
            {
                ExecuteStoredProcedure(StoredProcedureName.AW_Asociar_FileOportunidad);

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