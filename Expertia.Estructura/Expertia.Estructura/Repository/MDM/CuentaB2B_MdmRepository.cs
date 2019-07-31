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
        public OperationResult Create(CuentaB2B entity)
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                #region Simples
                #region Cuenta
                AddInParameter("P_IDSALESFORCE", entity.IdSalesForce);
                AddInParameter("P_TIPOPERSONA", entity.TipoPersona, CuentaB2B_FK.TipoPersona);
                AddInParameter("P_FECHANACIMORANIV", entity.FechaNacimOrAniv);
                AddInParameter("P_LOGOFOTO", entity.LogoFoto);
                AddInParameter("P_PUNTOCONTACTO", entity.PuntoContacto, CuentaB2B_FK.PuntoContacto);
                AddInParameter("P_RECIBIRINFORMACION", entity.RecibirInformacion);
                AddInParameter("P_NIVELIMPORTANCIA", entity.NivelImportancia, CuentaB2B_FK.NivelImportancia);
                AddInParameter("P_FECHAINIRELACIONCOMERCIAL", entity.FechaIniRelacionComercial);
                AddInParameter("P_COMENTARIOS", entity.FechaIniRelacionComercial);
                AddInParameter("P_TIPOCUENTA", entity.TipoCuenta, CuentaB2B_FK.TipoCuenta);
                AddInParameter("P_ESTADO", entity.Estado, CuentaB2B_FK.Estado);
                AddInParameter("P_PAISPROCEDENCIA", entity.PaisProcedencia, CuentaB2B_FK.PaisProcedencia);
                #endregion

                #region CuentaB2B
                AddInParameter("P_RAZONSOCIAL", entity.RazonSocial);
                AddInParameter("P_ALIAS", entity.Alias);
                AddInParameter("P_CONDICIONPAGO", entity.CondicionPago, CuentaB2B_FK.CondicionPago);
                AddInParameter("P_TIPOMONEDADELINEACREDITO", entity.TipoMonedaDeLineaCredito, CuentaB2B_FK.TipoMonedaDeLineaCredito);
                AddInParameter("P_MONTOLINEACREDITO", entity.MontoLineaCredito);
                #endregion

                #region Ejecución
                ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B");
                #endregion
                #endregion
                
                #region Multiples
                if (entity.Documentos != null)
                    foreach (var documento in entity.Documentos)
                    {
                        try
                        {
                            AddInParameter("P_IDSALESFORCE", entity.IdSalesForce);
                            AddInParameter("P_TIPO", documento.Tipo, CuentaB2B_FK.TipoDocumento);
                            AddInParameter("P_NUMERO", documento.Numero);

                            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_DOCUMENTO");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                if (entity.Direcciones != null)
                    foreach (var direccion in entity.Direcciones)
                    {
                        try
                        {
                            AddInParameter("P_IDSALESFORCE", entity.IdSalesForce);
                            AddInParameter("P_TIPO", direccion.Tipo, CuentaB2B_FK.TipoDireccion);
                            AddInParameter("P_DIRECCION", direccion.Descripcion);
                            AddInParameter("P_CIUDAD", direccion.Ciudad, CuentaB2B_FK.Ciudad);
                            AddInParameter("P_DEPARTAMENTO", direccion.Departamento, CuentaB2B_FK.Departamento);
                            AddInParameter("P_PAIS", direccion.Pais, CuentaB2B_FK.Pais);

                            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_DIRECCION");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                if (entity.Telefonos != null)
                    foreach (var telefono in entity.Telefonos)
                    {
                        try
                        {
                            AddInParameter("P_IDSALESFORCE", entity.IdSalesForce);
                            AddInParameter("P_TIPO", telefono.Tipo, CuentaB2B_FK.TipoTelefono);
                            AddInParameter("P_NUMERO", telefono.Numero);

                            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_TELEFONO");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                if (entity.Sitios != null)
                    foreach (var sitio in entity.Sitios)
                    {
                        try
                        {
                            AddInParameter("P_IDSALESFORCE", entity.IdSalesForce);
                            AddInParameter("P_TIPO", sitio.Tipo, CuentaB2B_FK.TipoSitio);
                            AddInParameter("P_DESCRIPCION", sitio.Descripcion);

                            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_SITIO");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                if (entity.Correos != null)
                    foreach (var correo in entity.Correos)
                    {
                        try
                        {
                            AddInParameter("P_IDSALESFORCE", entity.IdSalesForce);
                            AddInParameter("P_TIPO", correo.Tipo, CuentaB2B_FK.TipoCorreo);
                            AddInParameter("P_DESCRIPCION", correo.Descripcion);

                            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_CORREO");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                if (entity.Participantes != null)
                    foreach (var participante in entity.Participantes)
                    {
                        try
                        {
                            AddInParameter("P_IDSALESFORCE", entity.IdSalesForce);
                            AddInParameter("P_EMPLOREJECRESPONS", participante.EmpleadoOrEjecutivoResponsable, CuentaB2B_FK.EmpleadoEjecResponsable);
                            AddInParameter("P_SUPERVISORKAM", participante.SupervisorKam, CuentaB2B_FK.SupervisorKam);
                            AddInParameter("P_GERENTE", participante.Gerente, CuentaB2B_FK.Gerente);
                            AddInParameter("P_UNIDADNEGOCIO", participante.UnidadNegocio, CuentaB2B_FK.UnidadNegocio);
                            AddInParameter("P_GRUPOCOLABBEJECREGIONBRANCH", participante.GrupoColabEjecRegionBranch, CuentaB2B_FK.GrupoColabEjecRegionBranch);
                            AddInParameter("P_FLAGPRINCIPAL", participante.FlagPrincipal);

                            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_PARTICIPANTE");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                if (entity.InteresesProdActiv != null)
                    foreach (var interes in entity.InteresesProdActiv)
                    {
                        try
                        {
                            AddInParameter("P_IDSALESFORCE", entity.IdSalesForce);
                            AddInParameter("P_TIPO", interes.Tipo, CuentaB2B_FK.TipoInteresProdActiv);

                            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_INTERESPRODACTIV");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                if (entity.CanalesRecibirInfo != null)
                    foreach (var canalInfo in entity.CanalesRecibirInfo)
                    {
                        try
                        {
                            AddInParameter("P_IDSALESFORCE", entity.IdSalesForce);
                            AddInParameter("P_DESCRIPCION", canalInfo.Descripcion);

                            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_CANALESRECIBIRINFO");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                if (entity.Branches != null)
                    foreach (var branch in entity.Branches)
                    {
                        try
                        {
                            AddInParameter("P_IDSALESFORCE", entity.IdSalesForce);
                            AddInParameter("P_DESCRIPCION", branch.RegionMercadoBranch, CuentaB2B_FK.RegionMercadoBranch);

                            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_BRANCH");
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                if (entity.IdiomasComunicCliente != null)
                    foreach (var idioma in entity.IdiomasComunicCliente)
                    {
                        try
                        {
                            AddInParameter("P_IDSALESFORCE", entity.IdSalesForce);
                            AddInParameter("P_ID", idioma.ID, CuentaB2B_FK.IdiomaComunicCliente);

                            ExecuteSPWithoutResults("SP_CREAR_CLIENTE_B2B_IDIOMACOMUNICCLIENTE");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                #endregion

                operationResult[OperationResult.Operation] = Operation.Success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return operationResult;
        }

        public OperationResult Delete(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }        

        public OperationResult Read(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }

        public OperationResult Update(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region SQLMethod
        private void AddInParameter(string paramName, dynamic description, CuentaB2B_FK foreignKey)
        {
            AddInParameter(paramName, _fkMdm.LookUpByDescription(foreignKey, description));
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