using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.Condor
{
    public class CuentaB2B_RbRepository : OracleBase<CuentaB2B>, ICrud<CuentaB2B>
    {
        public CuentaB2B_RbRepository() : base(ConnectionKeys.CondorConnKey)
        {
        }

        public Operation Create(CuentaB2B entity)
        {
            Operation operation = new Operation();
            try
            {
                /* (01) P_CODIGO_ERROR */
                AddParameter("P_CODIGO_ERROR", null, ParameterDirection.Output);
                /* (02) P_MENSAJE_ERROR */
                AddParameter("P_MENSAJE_ERROR", null, ParameterDirection.Output);
                /* (03) P_NOMBRE_USUARIO */
                AddParameter("P_NOMBRE_USUARIO"); // ◄ No hay campos de auditoría
                /* (04) P_NOMBRE_EMPRESA */
                AddParameter("P_NOMBRE_EMPRESA"); // ◄ Especificar el nombre
                /* (05) P_BRANCH */
                if (entity.Branches != null)
                    AddParameter("P_BRANCH", entity.Branches.ToList()[0].RegionMercadoBranch); // ◄ No se tiene ID, sino descripción
                else
                    AddParameter("P_BRANCH");
                /* (06) P_COD_CLIENTE_MDM */
                AddParameter("P_COD_CLIENTE_MDM", entity.ID);
                /* (07) P_COD_CLIENTE_CRM */
                AddParameter("P_COD_CLIENTE_CRM", entity.IdSalesForce);
                /* (08) P_NOMBRE_CLIENTE */
                AddParameter("P_NOMBRE_CLIENTE", entity.RazonSocial);
                /* (09) P_ALIAS_CLIENTE */
                AddParameter("P_ALIAS_CLIENTE", entity.Alias);
                /* (10) P_NOMBRE_TIPO_CLIENTE */
                AddParameter("P_NOMBRE_TIPO_CLIENTE", entity.TipoCuenta); // ◄ No se tiene ID, sino descripción
                /* (11) P_ESTADO_CLIENTE */
                AddParameter("P_ESTADO_CLIENTE", entity.Estado);
                /* (12) P_NOMBRE_CONDICION_PAGO */
                AddParameter("P_NOMBRE_CONDICION_PAGO", entity.CondicionPago.ID);
                /* (13) P_NOMBRE_TIPO_PERSONA */
                AddParameter("P_NOMBRE_TIPO_PERSONA", entity.TipoPersona);
                /* (14) P_NOMBRE_PAIS */
                AddParameter("P_NOMBRE_PAIS", entity.PaisProcedencia); // ¿Este campo o de la dirección?
                /* (15) P_NOMBRE_CIUDAD */
                if (entity.Direcciones != null)
                    AddParameter("P_NOMBRE_CIUDAD", entity.Direcciones.ToList()[0].Ciudad);
                else
                    AddParameter("P_NOMBRE_CIUDAD");
                /* (16) P_NOMBRE_IDIOMA */
                if (entity.IdiomasComunicCliente != null)
                    AddParameter("P_NOMBRE_IDIOMA", entity.IdiomasComunicCliente.ToList()[0].ID);
                else
                    AddParameter("P_NOMBRE_IDIOMA");
                if (entity.Correos != null)
                {
                /* (17) P_EMAIL1 */
                    if (entity.Correos.ToList().Count > 0)
                        AddParameter("P_EMAIL1", entity.Correos.ToList()[0].Descripcion);
                    else
                        AddParameter("P_EMAIL1");
                /* (18) P_EMAIL2 */
                    if (entity.Correos.ToList().Count > 1)
                        AddParameter("P_EMAIL2", entity.Correos.ToList()[1].Descripcion);
                    else
                        AddParameter("P_EMAIL2");
                }
                if (entity.Telefonos != null)
                {
                /* (19) P_TELEFONO_OFICINA */
                    if (entity.Telefonos.ToList().Count > 0)
                        AddParameter("P_TELEFONO_OFICINA", entity.Telefonos.ToList()[0].Numero);
                    else
                        AddParameter("P_TELEFONO_OFICINA");
                /* (20) P_TELEFONO_CELULAR */
                    if (entity.Telefonos.ToList().Count > 1)
                        AddParameter("P_TELEFONO_CELULAR", entity.Telefonos.ToList()[1].Numero);
                    else
                        AddParameter("P_TELEFONO_CELULAR");
                /* (21) P_TELEFONO_ADICIONAL */
                    if (entity.Telefonos.ToList().Count > 2)
                        AddParameter("P_TELEFONO_ADICIONAL", entity.Telefonos.ToList()[2].Numero);
                    else
                        AddParameter("P_TELEFONO_ADICIONAL");
                /* (22) P_TELEFONO_EMERGENCIA */
                    if (entity.Telefonos.ToList().Count > 3)
                        AddParameter("P_TELEFONO_EMERGENCIA", entity.Telefonos.ToList()[3].Numero);
                    else
                        AddParameter("P_TELEFONO_EMERGENCIA");
                }
                /* (23) P_SITIO_WEB */
                if (entity.Sitios != null)
                    AddParameter("P_SITIO_WEB", entity.Sitios.ToList()[0].Descripcion);
                else
                    AddParameter("P_SITIO_WEB");
                /* (24) P_DIRECCION */
                if (entity.Direcciones != null)
                    AddParameter("P_DIRECCION", entity.Direcciones.ToList()[0].Descripcion);
                else
                    AddParameter("P_DIRECCION");
                /* (25) P_NOTAS */
                AddParameter("P_NOTAS", entity.Comentarios);

                ExecuteSPWithoutResults("CRM_PKG.SP_CREAR_CLIENTE");

                operation[Operation.Result] = ResultType.Success;
            }
            catch (Exception ex)
            {
                operation[Operation.Result] = ResultType.Fail;
                operation[Operation.ErrorMessage] = ex.Message;
            }
            return operation;
        }

        public Operation Delete(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }

        public Operation Read(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }

        public Operation Update(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<CuentaB2B> DataTableToEnumerable(DataTable dt)
        {
            throw new NotImplementedException();
        }
    }
}