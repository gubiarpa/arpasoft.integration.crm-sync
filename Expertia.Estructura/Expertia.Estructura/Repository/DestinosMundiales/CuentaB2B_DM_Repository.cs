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
    public class CuentaB2B_DM_Repository : OracleBase<CuentaB2B>, ICrud<CuentaB2B>
    {
        public CuentaB2B_DM_Repository() : base(ConnectionKeys.DMConnKey)
        {
        }

        public Operation Create(CuentaB2B entity)
        {
            try
            {
                Operation operation = new Operation();
                object value;

                #region Parameters
                // (01) P_CODIGO_ERROR
                AddParameter("P_CODIGO_ERROR", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, 4000);
                // (02) P_MENSAJE_ERROR
                AddParameter("P_MENSAJE_ERROR", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, 4000);
                // (03) P_NOMBRE_USUARIO
                AddParameter("P_NOMBRE_USUARIO", OracleDbType.Varchar2, entity.Auditoria.CreateUser.Descripcion); // ◄ No hay campos de auditoría
                // (04) P_RAZON_SOCIAL
                AddParameter("P_RAZON_SOCIAL", OracleDbType.Varchar2, entity.RazonSocial);
                // (05) P_FE_ANIVERSARIO
                AddParameter("P_FE_ANIVERSARIO", OracleDbType.Varchar2, entity.FechaNacimOrAniv);
                // (06) P_NOMBRE_PAIS_PROCEDENCIA
                AddParameter("P_NOMBRE_PAIS_PROCEDENCIA", OracleDbType.Varchar2, entity.PaisProcedencia.Descripcion);
                // (07) P_TIPO_DOCUMENTO_IDENTIDAD
                if ((entity.Documentos != null) && (entity.Documentos.ToList().Count > 0)) value = entity.Documentos.ToList()[0].Tipo.Descripcion; else value = DBNull.Value;
                AddParameter("P_TIPO_DOCUMENTO_IDENTIDAD", OracleDbType.Varchar2, value);
                // (08) P_NUMERO_DOCUMENTO
                if ((entity.Documentos != null) && (entity.Documentos.ToList().Count > 0)) value = entity.Documentos.ToList()[0].Numero; else value = DBNull.Value;
                AddParameter("P_NUMERO_DOCUMENTO", OracleDbType.Varchar2, value);
                // (09) P_TIPO_DIRECCION
                if ((entity.Direcciones != null) && (entity.Direcciones.ToList().Count > 0)) value = entity.Documentos.ToList()[0].Tipo.Descripcion; else value = DBNull.Value;
                AddParameter("P_TIPO_DIRECCION", OracleDbType.Varchar2, value);
                // (10) P_DIRECCION
                if ((entity.Direcciones != null) && (entity.Direcciones.ToList().Count > 0)) value = entity.Direcciones.ToList()[0].Descripcion; else value = DBNull.Value;
                AddParameter("P_DIRECCION", OracleDbType.Varchar2, value);
                // (11) P_NOMBRE_PAIS
                AddParameter("P_NOMBRE_PAIS", OracleDbType.Varchar2, entity.Direcciones.ToList()[0].Pais.Descripcion);
                // (12) P_DISTRITO
                // (13) P_DIRECCION_FISCAL
                // (14) P_TIPO_TELEFONO
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 0)) value = entity.Telefonos.ToList()[0].Tipo.Descripcion; else value = DBNull.Value;
                AddParameter("P_TIPO_TELEFONO", OracleDbType.Varchar2, value);
                // (15) P_TELEFONO
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 0)) value = entity.Telefonos.ToList()[0].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO", OracleDbType.Varchar2, value);
                // (16) P_TIPO_TELEFONO_1
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 1)) value = entity.Telefonos.ToList()[1].Tipo.Descripcion; else value = DBNull.Value;
                AddParameter("P_TIPO_TELEFONO_1", OracleDbType.Varchar2, value);
                // (17) P_TELEFONO_1
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 1)) value = entity.Telefonos.ToList()[1].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO_1", OracleDbType.Varchar2, value);
                // (18) P_TIPO_TELEFONO_2
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 2)) value = entity.Telefonos.ToList()[2].Tipo.Descripcion; else value = DBNull.Value;
                AddParameter("P_TIPO_TELEFONO_2", OracleDbType.Varchar2, value);
                // (19) P_TELEFONO_2
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 2)) value = entity.Telefonos.ToList()[2].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO_2", OracleDbType.Varchar2, value);
                // (20) P_SITIO_WEB
                if ((entity.Sitios != null) && (entity.Sitios.ToList().Count > 0)) value = entity.Telefonos.ToList()[0].Numero; else value = DBNull.Value;
                AddParameter("P_SITIO_WEB", OracleDbType.Varchar2, value);
                // (21) P_CORREO
                if ((entity.Correos != null) && (entity.Correos.ToList().Count > 0)) value = entity.Correos.ToList()[0].Descripcion; else value = DBNull.Value;
                AddParameter("P_CORREO", OracleDbType.Varchar2, value);
                // (22) P_PROPIETARIO
                // (23) P_NOMBRE_CONDICION_PAGO → ¿Se envía sólo el 1er elemento?
                // (24) P_COMENTARIO
                AddParameter("P_COMENTARIO", OracleDbType.Varchar2, entity.Comentarios);
                // (25) P_TIPO_CUENTA → ¿Si es B2B o B2C? ¿Qué valor debería ingresar?
                // (26) P_CATEG_VALOR
                AddParameter("P_CATEG_VALOR", OracleDbType.Varchar2, entity.CategoriaValor.Descripcion);
                // (27) P_CATEG_PERFIL_ACTITUD_TEC
                AddParameter("P_CATEG_PERFIL_ACTITUD_TEC", OracleDbType.Varchar2, entity.CategoriaPerfilActitudTecnologica.Descripcion);
                // (28) P_CATEG_PERFIL_FIDELIDAD
                AddParameter("P_CATEG_PERFIL_FIDELIDAD", OracleDbType.Varchar2, entity.CategoriaPerfilFidelidad.Descripcion);
                // (29) P_INCENTIVO
                AddParameter("P_INCENTIVO", OracleDbType.Varchar2, entity.CategoriaPerfilFidelidad.Descripcion);
                // (30) P_ESTADO_ACTIVACION → Confirmar si es la propiedad
                AddParameter("P_ESTADO_ACTIVACION", OracleDbType.Varchar2, entity.Estado.Descripcion);
                // (31) P_REFE_EXTERENA_2 → ¿Qué campo es éste?
                // (32) P_REFE_EXTERENA_3 → ¿Qué campo es éste?
                #endregion

                #region Invoke
                ExecuteSPWithoutResults("NUEVOMUNDO.CRM_PKG.SP_CREAR_CLIENTE");

                operation["P_CODIGO_ERROR"] = GetOutParameter("P_CODIGO_ERROR");
                operation["P_MENSAJE_ERROR"] = GetOutParameter("P_MENSAJE_ERROR");
                operation[Operation.Result] = ResultType.Success;
                #endregion

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation Delete(CuentaB2B entity)
        {
            throw new System.NotImplementedException();
        }

        public Operation Read(CuentaB2B entity)
        {
            throw new System.NotImplementedException();
        }

        public Operation Update(CuentaB2B entity)
        {
            throw new System.NotImplementedException();
        }
    }
}