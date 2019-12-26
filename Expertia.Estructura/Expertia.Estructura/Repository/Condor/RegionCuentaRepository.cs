using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.Condor
{
    public class RegionCuentaRepository : OracleBase<RegionCuenta>, IRegionCuentaRepository
    {
        public RegionCuentaRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.CondorTravel) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

        public Operation Register(RegionCuenta regionCuenta)
        {
            try
            {
                Operation operation = new Operation();
                
                /// (01) P_CODIGO_ERROR
                AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (02) P_MENSAJE_ERROR
                AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (03) P_USUARIO
                AddParameter("P_USUARIO", OracleDbType.Varchar2, regionCuenta.Usuario);
                /// (04) P_ID_CUENTA_SF
                AddParameter("P_ID_CUENTA_SF", OracleDbType.Varchar2, regionCuenta.IdCuentaSf);
                /// (05) PESTADO_CLIENTE
                AddParameter("P_ESTADO_CLIENTE", OracleDbType.Varchar2, regionCuenta.Estado);
                

                ExecuteStoredProcedure(StoredProcedureName.CT_Register_RegionCuenta);

                operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
                operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}