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

namespace Expertia.Estructura.Repository.Retail
{
    public class FactFileRetailRepository : OracleBase<FactFileRetailReq>, IFactFileRepository
    {
        #region Constructor
        public FactFileRetailRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GuardarDatosFacturacion(FactFileRetailReq model)
        {
            var operation = new Operation();
            int IdDatosFactura;

            #region Parameters
            if (model.IdDatosFacturacion != 0)
            {
                AddParameter("pIdDatosFacturacion", OracleDbType.Varchar2, model.IdDatosFacturacion);
            }

            AddParameter("pEstado", OracleDbType.Int32, model.Estado);
            AddParameter("pDK", OracleDbType.Varchar2, model.DK);
            AddParameter("pSubCodigo", OracleDbType.Varchar2, model.SubCodigo);
            AddParameter("pEjecutiva", OracleDbType.Varchar2, model.Ejecutiva);
            AddParameter("pNumfileNM", OracleDbType.Varchar2, model.NumFile_NM);
            AddParameter("pNumfileDM", OracleDbType.Varchar2, model.NUmFile_DM);
            AddParameter("pCCB", OracleDbType.Varchar2, model.CCB);
            AddParameter("pRUC", OracleDbType.Varchar2, model.RUC);
            AddParameter("pRAZON", OracleDbType.Varchar2, model.RAZON);
            AddParameter("pTipoDocumento", OracleDbType.Varchar2, model.TipoDocumento);
            AddParameter("PDoc_Cid", OracleDbType.Varchar2, model.Doc_cid);
            AddParameter("pDOCUMENTO", OracleDbType.Varchar2, model.Documento);
            AddParameter("pNombre", OracleDbType.Varchar2, model.Nombre);
            AddParameter("pApellidoP", OracleDbType.Varchar2, model.ApellidoPaterno);
            AddParameter("pApellidoM", OracleDbType.Varchar2, model.ApellidoMateno);
            AddParameter("pOARippley", OracleDbType.Varchar2, model.OARipley);
            AddParameter("pMontoOA", OracleDbType.Decimal, model.MontoOA);
            AddParameter("pIdUsuario", OracleDbType.Int32, model.IdUsuario);
            AddParameter("pCot_Id", OracleDbType.Int32, model.Cot_Id);
            AddParameter("pCampania", OracleDbType.Varchar2, model.Campania);
            AddParameter("pCorreo", OracleDbType.Varchar2, model.Correo);
            AddParameter("pBanco", OracleDbType.Varchar2, model.Banco);
            AddParameter("pCantidadMillas", OracleDbType.Varchar2, model.CantidadMillas);
            AddParameter("pMontoMillas", OracleDbType.Decimal, model.MontoMillas);
            AddParameter("pObservacion", OracleDbType.Clob, model.Observacion);
            if (model.IdDatosFacturacion != 0)
            {
                IdDatosFactura = model.IdDatosFacturacion;
            }
            else
            {
                AddParameter("pNumId_out", OracleDbType.Int32, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                
            }
            #endregion

            #region Invoke
            var spName = string.Empty;
            
            if(model.IdDatosFacturacion!=0)
                spName = StoredProcedureName.AW_Upd_factFileRetail;
            else
                spName = StoredProcedureName.AW_Ins_factFileRetail;
                    
            
            ExecuteStoredProcedure(spName);
            
            operation["pNumId_out"] = GetOutParameter("pNumId_out");
            #endregion

            return operation;
        }

        public void EliminarDetalleTarifa(int IdDatosFacturacion)
        {
            #region parameters
            AddParameter("pIdDatosFacturacion", OracleDbType.Int32, IdDatosFacturacion);
            #endregion

            #region Invoke
            var spName = string.Empty;
                spName = StoredProcedureName.AW_Del_DetalleTarifa;

            ExecuteStoredProcedure(spName);

            #endregion
        }

        public void EliminarDetalleNoRecibos(int IdDatosFacturacion)
        {
            #region parameters
            AddParameter("pIdDatosFacturacion", OracleDbType.Int32, IdDatosFacturacion);
            #endregion

            #region Invoke
            var spName = string.Empty;
            spName = StoredProcedureName.AW_Del_DetalleRecibos;

            ExecuteStoredProcedure(spName);

            #endregion
        }

        public void GuardarDetalleTarifa(FactFileRetailReq model, int IdDatosFacturacion)
        {
            List<TarifaDetalle> lstDetalleTarifas = new List<TarifaDetalle>();
            lstDetalleTarifas = model.TarifaDetalle;

            try
            {
                foreach (TarifaDetalle Item in lstDetalleTarifas)
                {
                    #region parameters
                    AddParameter("pCantidadADT", OracleDbType.Int32, Item.CantidadADT);
                    AddParameter("pTarifaPorADT", OracleDbType.Decimal, Convert.ToDouble(Item.CantidadADT));
                    AddParameter("pCatindadCHD", OracleDbType.Int32, Item.CantidadADT);
                    AddParameter("pTarifaPorCHD", OracleDbType.Decimal, Convert.ToDouble(Item.CantidadADT));
                    AddParameter("pCantidadINF", OracleDbType.Int32, Item.CantidadADT);
                    AddParameter("pTarifaPorINF", OracleDbType.Decimal, Convert.ToDouble(Item.CantidadADT));
                    AddParameter("pIdDatosFacturacion", OracleDbType.Int32, Item.CantidadADT);
                    AddParameter("pIdGrupoServicio", OracleDbType.Int32, Item.CantidadADT);
                    AddParameter("pMontoPorADT", OracleDbType.Decimal, Convert.ToDouble(Item.CantidadADT));
                    AddParameter("pMontoPorCHD", OracleDbType.Decimal, Convert.ToDouble(Item.CantidadADT));
                    AddParameter("pMontoPorINF", OracleDbType.Decimal, Convert.ToDouble(Item.CantidadADT));
                    AddParameter("pGrupoServicio", OracleDbType.Varchar2, Item.CantidadADT);
                    #endregion
                    #region Invoke
                    var spName = string.Empty;
                    spName = StoredProcedureName.AW_Ins_Tarifa;

                    ExecuteStoredProcedure(spName);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        public void GuardarDetalleNoRecibo(FactFileRetailReq model, int IdDatosFacturacion)
        {
            var lstDetalleNoRecibo = model.ReciboDetalle;

            try
            {
                foreach (ReciboDetalle Item in lstDetalleNoRecibo)
                {
                    #region parameters
                    AddParameter("pNoRecibo", OracleDbType.Varchar2, Item.NoRecibo);
                    AddParameter("pMontoRecibo", OracleDbType.Decimal, Convert.ToDouble(Item.MontoRecibo));
                    AddParameter("pEstado", OracleDbType.Int32, 1);
                    AddParameter("pIdSucursal", OracleDbType.Int32, Item.IdSucursal);
                    AddParameter("pIdDatosFacturacion", OracleDbType.Int32, IdDatosFacturacion);
                    AddParameter("pSucursal", OracleDbType.Varchar2, Item.Sucursal);
                    #endregion
                    #region Invoke
                    var spName = string.Empty;
                    spName = StoredProcedureName.AW_Ins_NoRecibo;

                    ExecuteStoredProcedure(spName);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion
    }
}