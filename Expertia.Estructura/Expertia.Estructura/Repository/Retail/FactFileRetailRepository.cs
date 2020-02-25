using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.Retail
{
    public class FactFileRetailRepository : OracleBase<FactFileRetailReq>
    {
        public FactFileRetailRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

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
            switch (_unidadNegocio)
            {
                case UnidadNegocioKeys.DestinosMundiales:
                    spName = StoredProcedureName.DM_Update_Oportunidad;
                    break;
                case UnidadNegocioKeys.Interagencias:
                    spName = StoredProcedureName.IA_Update_Oportunidad;
                    break;
                case UnidadNegocioKeys.AppWebs:
                    if(model.IdDatosFacturacion!=0)
                        spName = StoredProcedureName.AW_Upd_factFileRetail;
                    else
                        spName = StoredProcedureName.AW_Ins_factFileRetail;
                    break;
            }
            ExecuteStoredProcedure(spName);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.NumId] = GetOutParameter(OutParameter.NumId);
            operation[OutParameter.IdDatosFactura] = operation[OutParameter.NumId];
            #endregion

            return operation;
        }
        #endregion
    }
}