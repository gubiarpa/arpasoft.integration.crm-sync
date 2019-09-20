using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace Expertia.Estructura.Repository.DestinosMundiales
{
    public class Contacto_DM_Repository : OracleBase<Contacto>, ICrud<Contacto>, ISameSPName<Contacto>
    {
        public Contacto_DM_Repository() : base(ConnectionKeys.DMConnKey)
        {
        }

        #region PublicMethods
        public Operation Create(Contacto entity)
        {
            throw new NotImplementedException();
        }

        public Operation Update(Contacto entity)
        {
            throw new NotImplementedException();
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
                AddParameter("P_CODIGO_ERROR", OracleDbType.Varchar2, value, ParameterDirection.Output, 4000);
                // (02) P_MENSAJE_ERROR
                value = DBNull.Value;
                AddParameter("P_MENSAJE_ERROR", OracleDbType.Varchar2, value, ParameterDirection.Output, 4000);
                // (03) P_ID_CARGO
                value = entity.CargoEmpresa.Descripcion;
                AddParameter("P_ID_CARGO", OracleDbType.Varchar2, value);
                // (04) P_NOMBRES
                value = entity.Nombre;
                AddParameter("P_NOMBRES", OracleDbType.Varchar2, value);
                // (05) P_APELLIDOPATERNO
                value = entity.ApePaterno;
                AddParameter("P_APELLIDOPATERNO", OracleDbType.Varchar2, value);
                // (06) P_APELLIDOMATERNO
                value = entity.ApeMaterno;
                AddParameter("P_APELLIDOMATERNO", OracleDbType.Varchar2, value);
                // (07) P_ID_TIPO_DOCUMENTO_IDENTIDAD
                //value = 
                // (08) P_DOCUMENTO
                // (09) P_DIRECCION
                // (10) P_TELEFONO
                // (11) P_TELEFONO_CELULAR
                // (12) P_EMAIL
                // (13) P_EN_DESUSO
                // (15) P_FECHA_CUMPLE
                // (16) P_ANEXO
                // (17) P_ID_TIPO_DOCU_IDENTIDAD_CLIENTE
                // (18) P_DOCUMENTO_CLIENTE
                // (19) P_ID_CLIENTE
                // (20) P_CORRELATIVO
                #endregion

                #region Invoke
                ExecuteSPWithoutResults(SPName);

                operation["P_CODIGO_ERROR"] = GetOutParameter("P_CODIGO_ERROR");
                operation["P_MENSAJE_ERROR"] = GetOutParameter("P_MENSAJE_ERROR");
                operation["P_REFE_EXTERNA_2"] = GetOutParameter("P_REFE_EXTERNA_2");
                operation["P_REFE_EXTERNA_3"] = GetOutParameter("P_REFE_EXTERNA_3");
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