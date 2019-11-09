using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Linq;

namespace Expertia.Estructura.Repository.InterAgencias
{
    public class Subcodigo_IA_Repository : OracleBase<Subcodigo>, ICrud<Subcodigo>
    {
        public Subcodigo_IA_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.InterAgencias) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

        public Operation Asociate(Subcodigo entity)
        {
            throw new NotImplementedException();
        }

        public Operation Create(Subcodigo entity)
        {
            try
            {
                var operation = new Operation();
                object value;

                #region Parameter
                /// (01) P_CODIGO_ERROR
                value = DBNull.Value;
                AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (02) P_MENSAJE_ERROR
                value = DBNull.Value;
                AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (03) P_NOMBRE_USUARIO
                value = entity.Usuario.Descripcion;
                AddParameter("P_NOMBRE_USUARIO", OracleDbType.Varchar2, value);
                /// (00) P_ACCION
                value = entity.Accion.Descripcion;
                AddParameter("P_ACCION", OracleDbType.Varchar2, value);
                /// (00) P_ID_CUENTA
                value = entity.IdCuentas.ToList().FirstOrDefault(p => p.UnidadNegocio.Descripcion.ToUnidadNegocio().Equals(_unidadNegocio)).Descripcion;
                AddParameter("P_ID_CUENTA", OracleDbType.Varchar2, value);
                /// (00) P_NOMBRE_SUCURSAL
                value = entity.NombreSucursal;
                AddParameter("P_NOMBRE_SUCURSAL", OracleDbType.Varchar2, value);
                /// (00) P_DIRECCION_SUCURSAL
                value = entity.DireccionSucursal;
                AddParameter("P_DIRECCION_SUCURSAL", OracleDbType.Varchar2, value);
                /// (00) P_NOMBRE_CONDICION_PAGO
                value = entity.CondicionesPago.ToList().FirstOrDefault(p => p.UnidadNegocio.Descripcion.ToUnidadNegocio().Equals(_unidadNegocio)).Descripcion;
                AddParameter("P_NOMBRE_CONDICION_PAGO", OracleDbType.Varchar2, value);
                /// (00) P_ESTADO_SUCURSAL
                value = entity.EstadoSucursal.Descripcion;
                AddParameter("P_ESTADO_SUCURSAL", OracleDbType.Varchar2, value);
                /// (00) P_PROMOTOR
                value = entity.Promotores.ToList().FirstOrDefault(p => p.UnidadNegocio.Descripcion.ToUnidadNegocio().Equals(_unidadNegocio)).Descripcion;
                AddParameter("P_NOMBRE_PROMOTOR", OracleDbType.Varchar2, value);
                /// (00) CORRELATIVO_SUBCODIGO
                value = DBNull.Value;
                AddParameter(OutParameter.IdSubcodigo, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
                #endregion

                #region Invoke
                var spName = string.Empty;
                switch (_unidadNegocio)
                {
                    case UnidadNegocioKeys.DestinosMundiales:
                        spName = StoredProcedureName.DM_Create_Subcodigo;
                        break;
                    case UnidadNegocioKeys.InterAgencias:
                        spName = StoredProcedureName.IA_Create_Subcodigo;
                        break;
                    default:
                        break;
                }

                ExecuteStoredProcedure(spName);

                operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
                operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
                operation[OutParameter.IdSubcodigo] = GetOutParameter(OutParameter.IdSubcodigo);
                operation[Operation.Result] = ResultType.Success;
                #endregion

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation Generate(Subcodigo entity)
        {
            throw new NotImplementedException();
        }

        public Operation Read(Subcodigo entity)
        {
            throw new NotImplementedException();
        }

        public Operation Update(Subcodigo entity)
        {
            throw new NotImplementedException();
        }
    }
}