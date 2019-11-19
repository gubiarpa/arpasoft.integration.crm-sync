using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.InterAgencias
{
    public class Subcodigo_IA_Repository : OracleBase<Subcodigo>, ISubcodigoRepository
    {
        #region Constructor
        public Subcodigo_IA_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.Interagencias) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation Create(Subcodigo subcodigo)
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
                AddParameter("P_NOMBRE_USUARIO", OracleDbType.Varchar2, subcodigo.Usuario);
                /// (04) P_ACCION
                AddParameter("P_ACCION", OracleDbType.Varchar2, subcodigo.Accion);
                /// (05) P_ID_CUENTA
                AddParameter("P_ID_CUENTA", OracleDbType.Varchar2, subcodigo.DkAgencia);
                /// (06) P_NOMBRE_SUCURSAL
                AddParameter("P_NOMBRE_SUCURSAL", OracleDbType.Varchar2, subcodigo.NombreSucursal);
                /// (07) P_DIRECCION_SUCURSAL
                AddParameter("P_DIRECCION_SUCURSAL", OracleDbType.Varchar2, subcodigo.DireccionSucursal);
                /// (08) P_NOMBRE_CONDICION_PAGO
                AddParameter("P_NOMBRE_CONDICION_PAGO", OracleDbType.Varchar2, subcodigo.CondicionPago);
                /// (09) P_ESTADO_SUCURSAL
                AddParameter("P_ESTADO_SUCURSAL", OracleDbType.Varchar2, subcodigo.EstadoSucursal);
                /// (10) P_PROMOTOR
                AddParameter("P_NOMBRE_PROMOTOR", OracleDbType.Varchar2, subcodigo.Promotor);
                /// (11) CORRELATIVO_SUBCODIGO
                AddParameter(OutParameter.IdSubcodigo, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                #endregion

                #region Invoke
                var spName = string.Empty;
                switch (_unidadNegocio)
                {
                    case UnidadNegocioKeys.DestinosMundiales:
                        spName = StoredProcedureName.DM_Create_Subcodigo;
                        break;
                    case UnidadNegocioKeys.Interagencias:
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

        public Operation Read()
        {
            try
            {
                var operation = new Operation();
                object value;

                #region Parameters
                /// (01) P_CODIGO_ERROR
                value = DBNull.Value;
                AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (02) P_MENSAJE_ERROR
                value = DBNull.Value;
                AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (03) P_SUBCODIGO
                value = DBNull.Value;
                AddParameter(OutParameter.CursorSubcodigo, OracleDbType.RefCursor, value, ParameterDirection.Output);
                #endregion

                #region Invoke
                var spName = string.Empty;
                switch (_unidadNegocio)
                {
                    case UnidadNegocioKeys.DestinosMundiales:
                        spName = StoredProcedureName.DM_Read_Subcodigo;
                        break;
                    case UnidadNegocioKeys.Interagencias:
                        spName = StoredProcedureName.IA_Read_Subcodigo;
                        break;
                    default:
                        break;
                }

                ExecuteStoredProcedure(spName);

                operation[OutParameter.CursorSubcodigo] = ToSubcodigo(GetDtParameter(OutParameter.CursorSubcodigo));
                operation[Operation.Result] = ResultType.Success;
                #endregion

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation Update(Subcodigo subcodigo)
        {
            try
            {
                var operation = new Operation();

                #region Parameters
                /// (01) P_CODIGO_ERROR
                AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (02) P_MENSAJE_ERROR
                AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (03) P_ID_CLIENTE
                AddParameter("P_ID_CLIENTE", OracleDbType.Int32, subcodigo.DkAgencia);
                /// (04) P_ID_SUBCODIGO
                AddParameter("P_ID_SUBCODIGO", OracleDbType.Int32, subcodigo.CorrelativoSubcodigo);
                /// (05) P_ES_ATENCION
                AddParameter("P_ES_ATENCION", OracleDbType.Varchar2, subcodigo.CodigoError);
                /// (06) P_DESCRIPCION
                AddParameter("P_DESCRIPCION", OracleDbType.Varchar2, subcodigo.MensajeError);
                /// (07) P_ACTUALIZADOS
                AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.IA_Update_Subcodigo);
                #endregion

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Auxiliar
        private IEnumerable<Subcodigo> ToSubcodigo(DataTable dt)
        {
            try
            {
                var subcodigos = new List<Subcodigo>();
                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    var nombre_usuario = row.StringParse("P_NOMBRE_USUARIO");
                    var accion = row.StringParse("P_ACCION");
                    var id_cuenta = row.IntParse("P_ID_CUENTA");
                    var nombre_sucursal = row.StringParse("P_NOMBRE_SUCURSAL");
                    var direccion_sucursal = row.StringParse("P_DIRECCION_SUCURSAL");
                    var nombre_condicion_pago = row.StringParse("P_NOMBRE_CONDICION_PAGO");
                    var nombre_promotor = row.StringParse("P_NOMBRE_PROMOTOR");
                    var id_subcodigo = row.IntParse("P_ID_SUBCODIGO");
                    var estado_sucursal = row.StringParse("P_ESTADO_SUCURSAL");
                    #endregion

                    #region AddingElement
                    subcodigos.Add(new Subcodigo()
                    {
                        UnidadNegocio = _unidadNegocio.ToLongName(),
                        Accion = accion,
                        DkAgencia = id_cuenta,
                        CorrelativoSubcodigo = id_subcodigo,
                        NombreSucursal = nombre_sucursal,
                        DireccionSucursal = direccion_sucursal,
                        EstadoSucursal = estado_sucursal,
                        Promotor = nombre_promotor,
                        CondicionPago = nombre_condicion_pago,
                        Usuario = nombre_usuario
                    });
                    #endregion
                }
                return subcodigos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}