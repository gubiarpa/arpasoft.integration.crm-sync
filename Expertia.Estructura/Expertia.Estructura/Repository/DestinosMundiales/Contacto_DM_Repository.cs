using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Linq;

namespace Expertia.Estructura.Repository.DestinosMundiales
{
    public class Contacto_DM_Repository : OracleBase<Contacto>, ICrud<Contacto>, ISameSPName<Contacto>
    {
        #region GenericConstructor
        public Contacto_DM_Repository(string connKey) : base(connKey)
        {
        }
        #endregion

        public Contacto_DM_Repository() : base(ConnectionKeys.DMConnKey)
        {
        }

        #region PublicMethods
        public Operation Create(Contacto entity)
        {
            return ExecuteOperation(entity, StoredProcedureName.DM_Create_Contacto, entity.Auditoria.CreateUser.Descripcion);
        }

        public Operation Update(Contacto entity)
        {
            return ExecuteOperation(entity, StoredProcedureName.DM_Update_Contacto, entity.Auditoria.ModifyUser.Descripcion);
        }
        #endregion

        #region Auxiliar
        public Operation ExecuteOperation(Contacto entity, string SPName, string userName)
        {
            try
            {
                Operation operation = new Operation();
                object value;

                #region Parameters
                // (01) P_CODIGO_ERROR
                value = DBNull.Value;
                AddParameter("P_CODIGO_ERROR", OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
                // (02) P_MENSAJE_ERROR
                value = DBNull.Value;
                AddParameter("P_MENSAJE_ERROR", OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
                // (03) P_NOMBRE_USUARIO
                value = userName;
                AddParameter("P_NOMBRE_USUARIO", OracleDbType.Varchar2, value);
                // (04) P_ID_CUENTA_SALESFORCE
                value = entity.IdCuentaSalesForce;
                AddParameter("P_ID_CUENTA_SALESFORCE", OracleDbType.Varchar2, value);
                // (05) P_ID_CONTACTO_SALESFORCE
                value = entity.IdSalesForce;
                AddParameter("P_ID_CONTACTO_SALESFORCE", OracleDbType.Varchar2, value);
                // (06) P_CARGO
                value = entity.CargoEmpresa.Descripcion;
                AddParameter("P_CARGO", OracleDbType.Varchar2, value);
                // (07) P_NOMBRES
                value = entity.Nombre;
                AddParameter("P_NOMBRES", OracleDbType.Varchar2, value);
                // (08) P_APELLIDOPATERNO
                value = entity.ApePaterno;
                AddParameter("P_APELLIDOPATERNO", OracleDbType.Varchar2, value);
                // (09) P_APELLIDOMATERNO
                value = entity.ApeMaterno;
                AddParameter("P_APELLIDOMATERNO", OracleDbType.Varchar2, value);
                // (10) P_TIPO_DOCUMENTO_IDENTIDAD
                if ((entity.Documentos != null) && (entity.Documentos.ToList().Count > 0)) value = entity.Documentos.ToList()[0].Tipo.Descripcion; else value = DBNull.Value;
                AddParameter("P_TIPO_DOCUMENTO_IDENTIDAD", OracleDbType.Varchar2, value);
                // (11) P_NUMERO_DOCUMENTO
                if ((entity.Documentos != null) && (entity.Documentos.ToList().Count > 0)) value = entity.Documentos.ToList()[0].Numero; else value = DBNull.Value;
                AddParameter("P_NUMERO_DOCUMENTO", OracleDbType.Varchar2, value);
                // (12) P_DIRECCION
                if ((entity.Direcciones != null) && (entity.Direcciones.ToList().Count > 0)) value = entity.Direcciones.ToList()[0].Descripcion; else value = DBNull.Value;
                AddParameter("P_DIRECCION", OracleDbType.Varchar2, value);
                // (13) P_TELEFONO
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 0)) value = entity.Telefonos.ToList()[0].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO", OracleDbType.Varchar2, value);
                // (14) P_TELEFONO_CELULAR
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 1)) value = entity.Telefonos.ToList()[1].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO_CELULAR", OracleDbType.Varchar2, value);
                // (15) P_CORREO
                if ((entity.Correos != null) && (entity.Correos.ToList().Count > 0)) value = entity.Correos.ToList()[0].Descripcion; else value = DBNull.Value;
                AddParameter("P_CORREO", OracleDbType.Varchar2, value);
                // (16) P_ESTADO
                value = entity.Estado.Descripcion;
                AddParameter("P_ESTADO", OracleDbType.Varchar2, value);
                // (17) P_FECHA_CUMPLE
                if (entity.FechaNacimiento == null) value = DBNull.Value; else value = entity.FechaNacimiento;
                AddParameter("P_FECHA_CUMPLE", OracleDbType.Date, value);
                // (18) P_ID_CUENTA
                value = DBNull.Value;
                AddParameter("P_ID_CUENTA", OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
                // (19) P_ID_CONTACTO
                value = DBNull.Value;
                AddParameter("P_ID_CONTACTO", OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
                #endregion

                #region Invoke
                ExecuteSPWithoutResults(SPName);

                operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
                operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
                operation[OutParameter.IdCuenta] = entity.IDCuenta = GetOutParameter(OutParameter.IdCuenta).ToString();
                operation[OutParameter.IdContacto] = entity.ID = GetOutParameter(OutParameter.IdContacto).ToString();
                operation[Operation.Result] = ResultType.Success;
                #endregion

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region NonImplemented
        public Operation Delete(Contacto entity)
        {
            throw new NotImplementedException();
        }

        public Operation Read(Contacto entity)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}