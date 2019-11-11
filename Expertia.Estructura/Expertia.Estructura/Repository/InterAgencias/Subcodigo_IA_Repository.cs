using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Behavior;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Expertia.Estructura.Repository.InterAgencias
{
    public class Subcodigo_IA_Repository : OracleBase<Subcodigo>, ICrud<Subcodigo>
    {
        #region Constructor
        public Subcodigo_IA_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.InterAgencias) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
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

        public Operation Read(Subcodigo entity)
        {
            var operation = new Operation();
            object value;

            #region Parameters
            // (1) P_CODIGO_ERROR
            value = DBNull.Value;
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (2) P_MENSAJE_ERROR
            value = DBNull.Value;
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
            // (3) P_SUBCODIGO
            value = DBNull.Value;
            AddParameter(OutParameter.CursorSubcodigo, OracleDbType.RefCursor, value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.IA_Read_Subcodigo);

            operation[OutParameter.CursorSubcodigo] = ToSubcodigo(GetDtParameter(OutParameter.CursorSubcodigo));
            operation[Operation.Result] = ResultType.Success;
            #endregion

            return operation;
        }
        #endregion

        #region Auxiliar
        public IEnumerable<Subcodigo> ToSubcodigo(DataTable dt)
        {
            try
            {
                var subcodigos = new List<Subcodigo>();
                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    var nombre_usuario = (row["P_NOMBRE_USUARIO"] ?? string.Empty).ToString();
                    var accion = (row["P_ACCION"] ?? string.Empty).ToString();
                    if (!int.TryParse(row["P_ID_CUENTA"].ToString(), out int id_cuenta)) id_cuenta = 0;
                    if (!int.TryParse(row["P_ID_SUBCODIGO"].ToString(), out int id_subcodigo)) id_subcodigo = 0;
                    var nombre_sucursal = (row["P_NOMBRE_SUCURSAL"] ?? string.Empty).ToString();
                    var direccion_sucursal = (row["P_DIRECCION_SUCURSAL"] ?? string.Empty).ToString();
                    var nombre_promotor = (row["P_NOMBRE_PROMOTOR"] ?? string.Empty).ToString();
                    var nombre_condicion_pago = (row["P_NOMBRE_CONDICION_PAGO"] ?? string.Empty).ToString();
                    var estado_sucursal = (row["P_ESTADO_SUCURSAL" ?? string.Empty]).ToString();
                    #endregion
                     
                    #region AddingElement
                    subcodigos.Add(new Subcodigo()
                    {
                        Usuario = new SimpleDesc(nombre_usuario),
                        Accion = new SimpleDesc(accion),
                        IdCuentas = new List<SimpleNegocioDesc>() { new SimpleNegocioDesc(id_cuenta.ToString())},
                        IdSubcodigo = id_subcodigo.ToString(),
                        NombreSucursal = nombre_sucursal,
                        DireccionSucursal = direccion_sucursal,
                        Promotores = new List<SimpleNegocioDesc>() { new SimpleNegocioDesc(nombre_promotor) },
                        CondicionesPago = new List<SimpleNegocioDesc>() { new SimpleNegocioDesc(nombre_condicion_pago) },
                        EstadoSucursal = new SimpleDesc(estado_sucursal)
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

        #region NotImplemented
        public Operation Asociate(Subcodigo entity)
        {
            throw new NotImplementedException();
        }

        public Operation Generate(Subcodigo entity)
        {
            throw new NotImplementedException();
        }

        public Operation Update(Subcodigo entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}