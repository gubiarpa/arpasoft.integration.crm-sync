using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.MDM
{
    public class CuentaB2B_MdmRepository : OracleBase<CuentaB2B>, ICrud<CuentaB2B>
    {
        #region Properties
        protected CuentaB2B_FK_MdmRepository _fkMdm;
        #endregion

        #region Constructor
        public CuentaB2B_MdmRepository() : base(ConnectionKeys.MDMConnKey)
        {
            _fkMdm = new CuentaB2B_FK_MdmRepository();
        }
        #endregion

        #region ICrud
        public Operation Create(CuentaB2B entity)
        {
            Operation operationResult = new Operation();
            try
            {
                #region Simples
                #region Cuenta
                //AddParameter("P_IDSALESFORCE", entity.IdSalesForce);
                //AddInParameter("P_TIPOPERSONA", entity.TipoPersona, CuentaB2B_FK.TipoPersona);
                //AddParameter("P_FECHANACIMORANIV", entity.FechaNacimOrAniv);
                //AddParameter("P_LOGOFOTO", entity.LogoFoto);
                //AddInParameter("P_PUNTOCONTACTO", entity.PuntoContacto, CuentaB2B_FK.PuntoContacto);
                //AddParameter("P_RECIBIRINFORMACION", entity.RecibirInformacion);
                //AddInParameter("P_NIVELIMPORTANCIA", entity.NivelImportancia, CuentaB2B_FK.NivelImportancia);
                //AddParameter("P_FECHAINIRELACIONCOMERCIAL", entity.FechaIniRelacionComercial);
                //AddParameter("P_COMENTARIOS", entity.FechaIniRelacionComercial);
                //AddInParameter("P_TIPOCUENTA", entity.TipoCuenta, CuentaB2B_FK.TipoCuenta);
                //AddInParameter("P_ESTADO", entity.Estado, CuentaB2B_FK.Estado);
                //AddInParameter("P_PAISPROCEDENCIA", entity.PaisProcedencia, CuentaB2B_FK.PaisProcedencia);
                #endregion

                #region CuentaB2B
                //AddParameter("P_RAZONSOCIAL", entity.RazonSocial);
                //AddParameter("P_ALIAS", entity.Alias);
                //AddInParameter("P_CONDICIONPAGO", entity.CondicionPago, CuentaB2B_FK.CondicionPago);
                //AddInParameter("P_TIPOMONEDADELINEACREDITO", entity.TipoMonedaDeLineaCredito, CuentaB2B_FK.TipoMonedaDeLineaCredito);
                //AddParameter("P_MONTOLINEACREDITO", entity.MontoLineaCredito);
                #endregion

                #region Ejecución
                //ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B");
                #endregion
                #endregion

                #region Multiples
                //if (entity.Documentos != null)
                //    foreach (var documento in entity.Documentos)
                //    {
                //        try
                //        {
                //            AddParameter("P_IDSALESFORCE", entity.IdSalesForce);
                //            AddInParameter("P_TIPO", documento.Tipo, CuentaB2B_FK.TipoDocumento);
                //            AddParameter("P_NUMERO", documento.Numero);

                //            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_DOCUMENTO");
                //        }
                //        catch (Exception ex)
                //        {
                //            throw ex;
                //        }
                //    }
                //if (entity.Direcciones != null)
                //    foreach (var direccion in entity.Direcciones)
                //    {
                //        try
                //        {
                //            AddParameter("P_IDSALESFORCE", entity.IdSalesForce);
                //            AddInParameter("P_TIPO", direccion.Tipo, CuentaB2B_FK.TipoDireccion);
                //            AddParameter("P_DIRECCION", direccion.Descripcion);
                //            AddInParameter("P_CIUDAD", direccion.Ciudad, CuentaB2B_FK.Ciudad);
                //            AddInParameter("P_DEPARTAMENTO", direccion.Departamento, CuentaB2B_FK.Departamento);
                //            AddInParameter("P_PAIS", direccion.Pais, CuentaB2B_FK.Pais);

                //            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_DIRECCION");
                //        }
                //        catch (Exception ex)
                //        {
                //            throw ex;
                //        }
                //    }
                //if (entity.Telefonos != null)
                //    foreach (var telefono in entity.Telefonos)
                //    {
                //        try
                //        {
                //            AddParameter("P_IDSALESFORCE", entity.IdSalesForce);
                //            AddInParameter("P_TIPO", telefono.Tipo, CuentaB2B_FK.TipoTelefono);
                //            AddParameter("P_NUMERO", telefono.Numero);

                //            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_TELEFONO");
                //        }
                //        catch (Exception ex)
                //        {
                //            throw ex;
                //        }
                //    }
                //if (entity.Sitios != null)
                //    foreach (var sitio in entity.Sitios)
                //    {
                //        try
                //        {
                //            AddParameter("P_IDSALESFORCE", entity.IdSalesForce);
                //            AddInParameter("P_TIPO", sitio.Tipo, CuentaB2B_FK.TipoSitio);
                //            AddParameter("P_DESCRIPCION", sitio.Descripcion);

                //            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_SITIO");
                //        }
                //        catch (Exception ex)
                //        {
                //            throw ex;
                //        }
                //    }
                //if (entity.Correos != null)
                //    foreach (var correo in entity.Correos)
                //    {
                //        try
                //        {
                //            AddParameter("P_IDSALESFORCE", entity.IdSalesForce);
                //            AddInParameter("P_TIPO", correo.Tipo, CuentaB2B_FK.TipoCorreo);
                //            AddParameter("P_DESCRIPCION", correo.Descripcion);

                //            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_CORREO");
                //        }
                //        catch (Exception ex)
                //        {
                //            throw ex;
                //        }
                //    }
                //if (entity.Participantes != null)
                //    foreach (var participante in entity.Participantes)
                //    {
                //        try
                //        {
                //            AddParameter("P_IDSALESFORCE", entity.IdSalesForce);
                //            AddInParameter("P_EMPLOREJECRESPONS", participante.EmpleadoOrEjecutivoResponsable, CuentaB2B_FK.EmpleadoEjecResponsable);
                //            AddInParameter("P_SUPERVISORKAM", participante.SupervisorKam, CuentaB2B_FK.SupervisorKam);
                //            AddInParameter("P_GERENTE", participante.Gerente, CuentaB2B_FK.Gerente);
                //            AddInParameter("P_UNIDADNEGOCIO", participante.UnidadNegocio, CuentaB2B_FK.UnidadNegocio);
                //            AddInParameter("P_GRUPOCOLABBEJECREGIONBRANCH", participante.GrupoColabEjecRegionBranch, CuentaB2B_FK.GrupoColabEjecRegionBranch);
                //            AddParameter("P_FLAGPRINCIPAL", participante.FlagPrincipal);

                //            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_PARTICIPANTE");
                //        }
                //        catch (Exception ex)
                //        {
                //            throw ex;
                //        }
                //    }
                //if (entity.InteresesProdActiv != null)
                //    foreach (var interes in entity.InteresesProdActiv)
                //    {
                //        try
                //        {
                //            AddParameter("P_IDSALESFORCE", entity.IdSalesForce);
                //            AddInParameter("P_TIPO", interes.Tipo, CuentaB2B_FK.TipoInteresProdActiv);

                //            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_INTERESPRODACTIV");
                //        }
                //        catch (Exception ex)
                //        {
                //            throw ex;
                //        }
                //    }
                //if (entity.CanalesRecibirInfo != null)
                //    foreach (var canalInfo in entity.CanalesRecibirInfo)
                //    {
                //        try
                //        {
                //            AddParameter("P_IDSALESFORCE", entity.IdSalesForce);
                //            AddParameter("P_DESCRIPCION", canalInfo.Descripcion);

                //            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_CANALESRECIBIRINFO");
                //        }
                //        catch (Exception ex)
                //        {
                //            throw ex;
                //        }
                //    }
                //if (entity.Branches != null)
                //    foreach (var branch in entity.Branches)
                //    {
                //        try
                //        {
                //            AddParameter("P_IDSALESFORCE", entity.IdSalesForce);
                //            AddInParameter("P_DESCRIPCION", branch.RegionMercadoBranch, CuentaB2B_FK.RegionMercadoBranch);

                //            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_BRANCH");
                //        }
                //        catch (Exception ex)
                //        {
                //            throw;
                //        }
                //    }
                //if (entity.IdiomasComunicCliente != null)
                //    foreach (var idioma in entity.IdiomasComunicCliente)
                //    {
                //        try
                //        {
                //            AddParameter("P_IDSALESFORCE", entity.IdSalesForce);
                //            AddInParameter("P_ID", idioma.ID, CuentaB2B_FK.IdiomaComunicCliente);

                //            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_IDIOMACOMUNICCLIENTE");
                //        }
                //        catch (Exception ex)
                //        {
                //            throw ex;
                //        }
                //    }
                #endregion

                operationResult[Operation.Result] = ResultType.Success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return operationResult;
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
        #endregion

        #region SQLMethod
        private void AddInParameter(string paramName, dynamic description, CuentaB2B_FK foreignKey)
        {
            AddParameter(paramName, _fkMdm.LookUpByDescription(foreignKey, description));
            /*
                try
                {
                    dynamicField.ID = _fkMdm.LookUpByDescription(foreignKey, dynamicField.Descripcion).ToString();
                    AddInParameter(name, dynamicField.ID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            */
        }

        protected override IEnumerable<CuentaB2B> DataTableToEnumerable(DataTable dt)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}