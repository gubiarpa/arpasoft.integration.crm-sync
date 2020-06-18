using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.NuevoMundo
{
    public class SolicitarFactFileNMRepository : OracleBase<GenCodigoPagoNM>
    {
        public SolicitarFactFileNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

        #region Desglose
        public int GuardarDesgloseCA(SolicitarFactFileNM solicitarFactFile)
        {
            UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs;
            OracleTransaction objTx = null; OracleConnection objConn = null;

            try
            {
                ExecuteConexionBegin(unidadNegocio.ToConnectionKey(), ref objTx, ref objConn);

                var result = GuardarDatosFacturacion(solicitarFactFile, objTx, objConn);

                if (solicitarFactFile.existeIdDatosFacturacion)
                {
                    EliminarDetalleTarifa(solicitarFactFile, objTx, objConn);
                    EliminarDetalleNoRecibos(solicitarFactFile, objTx, objConn);
                }

                GuardarDetalleTarifa(solicitarFactFile, objTx, objConn);
                GuardarDetalleNoRecibo(solicitarFactFile, objTx, objConn);

                objTx.Commit();
                return result;
            }
            catch (Exception ex)
            {
                objTx.Rollback();
                throw ex;
            }
        }

        private int GuardarDatosFacturacion(SolicitarFactFileNM solicitarFactFile, OracleTransaction objTx, OracleConnection objConn)
        {
            var spName = solicitarFactFile.existeIdDatosFacturacion ?
                    "APPWEBS.PKG_DESGLOSE_CA.SP_ACTUALIZAR_DATOSFACTURACION" :
                    "APPWEBS.PKG_DESGLOSE_CA.SP_INSERTAR_DATOSFACTURACION";

            if (solicitarFactFile.existeIdDatosFacturacion)
                AddParameter("pIdDatosFacturacion", OracleDbType.Int32, solicitarFactFile.iddatosfacturacion);

            AddParameter("pEstado", OracleDbType.Int32, solicitarFactFile.estado);
            AddParameter("pDK", OracleDbType.NVarchar2, solicitarFactFile.dk);
            AddParameter("pSubCodigo", OracleDbType.NVarchar2, solicitarFactFile.subcodigo);
            AddParameter("pEjecutiva", OracleDbType.NVarchar2, solicitarFactFile.comisionista); // (!) Orig., model.Ejecutiva
            AddParameter("pNumfileNM", OracleDbType.NVarchar2, solicitarFactFile.numfilenm);
            AddParameter("pNumfileDM", OracleDbType.NVarchar2, solicitarFactFile.numfiledm);
            AddParameter("pCCB", OracleDbType.NVarchar2, solicitarFactFile.ccb);
            AddParameter("pRUC", OracleDbType.NVarchar2, string.Empty); // (!) Orig., model.RUC
            AddParameter("pRAZON", OracleDbType.NVarchar2, solicitarFactFile.razonsocial);
            AddParameter("pTipoDocumento", OracleDbType.NVarchar2, solicitarFactFile.tipodocidentidad);
            AddParameter("PDoc_Cid", OracleDbType.NVarchar2, string.Empty);
            AddParameter("pDOCUMENTO", OracleDbType.NVarchar2, solicitarFactFile.numdocidentidad); // (!) Orig., model.Documento
            AddParameter("pNombre", OracleDbType.NVarchar2, solicitarFactFile.nombre);
            AddParameter("pApellidoP", OracleDbType.NVarchar2, solicitarFactFile.apepaterno);
            AddParameter("pApellidoM", OracleDbType.NVarchar2, solicitarFactFile.apemateno);
            AddParameter("pOARippley", OracleDbType.NVarchar2, solicitarFactFile.oaripley);
            AddParameter("pMontoOA", OracleDbType.Decimal, solicitarFactFile.oamonto);
            AddParameter("pIdUsuario", OracleDbType.Int32, solicitarFactFile.idusuario);
            AddParameter("pCot_Id", OracleDbType.Int32, solicitarFactFile.cotid);
            AddParameter("pCampania", OracleDbType.NVarchar2, solicitarFactFile.campania);
            AddParameter("pCorreo", OracleDbType.NVarchar2, solicitarFactFile.correo);
            AddParameter("pBanco", OracleDbType.NVarchar2, solicitarFactFile.banco);
            AddParameter("pCantidadMillas", OracleDbType.NVarchar2, solicitarFactFile.cantidadmillas);
            AddParameter("pMontoMillas", OracleDbType.Decimal, solicitarFactFile.montomillas);
            AddParameter("pObservacion", OracleDbType.Clob, solicitarFactFile.observaciones);

            var idDatosFactura = string.Empty;

            if (solicitarFactFile.existeIdDatosFacturacion)
            {
                ExecuteStorePBeginCommit(spName, objTx, objConn);
                idDatosFactura = solicitarFactFile.iddatosfacturacion;
            }
            else
            {
                AddParameter("pNumId_out", OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
                ExecuteStorePBeginCommit(spName, objTx, objConn);
                idDatosFactura = GetOutParameter("pNumId_out").ToString();
            }

            return int.Parse(idDatosFactura);
        }

        private void EliminarDetalleTarifa(SolicitarFactFileNM solicitarFactFile, OracleTransaction objTx, OracleConnection objConn)
        {
            var spName = "APPWEBS.PKG_DESGLOSE_CA.SP_ELIMINAR_DETALLETARIFA";

            AddParameter("pIdDatosFacturacion", OracleDbType.Int32, solicitarFactFile.iddatosfacturacion);

            ExecuteStorePBeginCommit(spName, objTx, objConn);
        }

        private void EliminarDetalleNoRecibos(SolicitarFactFileNM solicitarFactFile, OracleTransaction objTx, OracleConnection objConn)
        {
            var spName = "APPWEBS.PKG_DESGLOSE_CA.SP_ELIMINAR_DETALLENORECIBOS";

            AddParameter("pIdDatosFacturacion", OracleDbType.Int32, solicitarFactFile.iddatosfacturacion);

            ExecuteStorePBeginCommit(spName, objTx, objConn);
        }

        private void GuardarDetalleTarifa(SolicitarFactFileNM solicitarFactFile, OracleTransaction objTx, OracleConnection objConn)
        {
            // Protección
            if (solicitarFactFile.TarifaDetalleList == null || solicitarFactFile.TarifaDetalleList.Count == 0) return;

            // Invocación
            foreach (var tarifaDetalle in solicitarFactFile.TarifaDetalleList)
            {
                var spName = "APPWEBS.PKG_DESGLOSE_CA.SP_INSERTAR_TARIFA";

                AddParameter("pCantidadADT", OracleDbType.Int32, tarifaDetalle.CantidadADT);
                AddParameter("pTarifaPorADT", OracleDbType.Decimal, tarifaDetalle.TarifaPorADT);
                AddParameter("pCatindadCHD", OracleDbType.Int32, tarifaDetalle.CantidadCHD);
                AddParameter("pTarifaPorCHD", OracleDbType.Decimal, tarifaDetalle.TarifaPorCHD);
                AddParameter("pCantidadINF", OracleDbType.Int32, tarifaDetalle.CantidadCHD);
                AddParameter("pTarifaPorINF", OracleDbType.Decimal, tarifaDetalle.TarifaINF);
                AddParameter("pIdDatosFacturacion", OracleDbType.Int32, tarifaDetalle.IdDatosFacturacion);
                AddParameter("pIdGrupoServicio", OracleDbType.Int32, tarifaDetalle.IdGrupoServicio);
                AddParameter("pMontoPorADT", OracleDbType.Decimal, tarifaDetalle.MontoPorADT);
                AddParameter("pMontoPorCHD", OracleDbType.Decimal, tarifaDetalle.MontoPorCHD);
                AddParameter("pMontoPorINF", OracleDbType.Decimal, tarifaDetalle.MontoPorINF);
                AddParameter("pGrupoServicio", OracleDbType.NVarchar2, tarifaDetalle.GrupoServicio);

                ExecuteStorePBeginCommit(spName, objTx, objConn);
            }
        }

        private void GuardarDetalleNoRecibo(SolicitarFactFileNM solicitarFactFile, OracleTransaction objTx, OracleConnection objConn)
        {
            // Protección
            if (solicitarFactFile.ReciboDetalleList == null || solicitarFactFile.ReciboDetalleList.Count == 0) return;

            // Invocación
            foreach (var reciboDetalle in solicitarFactFile.ReciboDetalleList)
            {
                var spName = "APPWEBS.PKG_DESGLOSE_CA.SP_INSERTAR_NORECIBO";

                AddParameter("pNoRecibo", OracleDbType.NVarchar2, reciboDetalle.NoRecibo);
                AddParameter("pMontoRecibo", OracleDbType.Decimal, reciboDetalle.MontoRecibo);
                AddParameter("pEstado", OracleDbType.Int32, 1);
                AddParameter("pIdSucursal", OracleDbType.Int32, reciboDetalle.IdSucursal);
                AddParameter("pIdDatosFacturacion", OracleDbType.Int32, reciboDetalle.IdDatosFacturacion);
                AddParameter("pSucursal", OracleDbType.NVarchar2, reciboDetalle.Sucursal);

                ExecuteStorePBeginCommit(spName, objTx, objConn);
            }
        }
        #endregion

        #region Archivo
        public void GuardarArchivo(SolicitarFactFileNM solicitarFactFile, int idDatosFacturacion, int idUsuario)
        {
            // Protección
            if (solicitarFactFile.ArchivoList == null || solicitarFactFile.ArchivoList.Count == 0) return;
            
            // Invocación
            var spName = "APPWEBS.PKG_Desglose_CA.SP_INSERTAR_ARCHIVOS";
            foreach (var archivo in solicitarFactFile.ArchivoList)
            {
                if (!archivo.IsValid) continue;

                AddParameter("pRutaArchivo", OracleDbType.NVarchar2, archivo.RutaArchivo);
                AddParameter("pNombreArchivo", OracleDbType.NVarchar2, archivo.NomArchivo);
                AddParameter("pExtensionArchivo", OracleDbType.NVarchar2, archivo.ExtArchivo); // (!) Item.ExtensionArchivo
                AddParameter("pIdDatosFacturacion", OracleDbType.Int32, idDatosFacturacion);
                AddParameter("pIdUsuWebCrea", OracleDbType.Int32, idUsuario);
                AddParameter("pUrlArchivo", OracleDbType.NVarchar2, archivo.UrlArchivo);

                ExecuteStoredProcedure(spName);
            }
        }

        public List<Archivo> ObtenerArchivos(string idDatosFacturacion)
        {
            var spName = "APPWEBS.PKG_DESGLOSE_CA.SP_OBTIENE_ARCHIVOS";

            AddParameter("pId_DatosFacturacion", OracleDbType.Int32, idDatosFacturacion);
            AddParameter("pCurResult_out", OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);

            ExecuteStoredProcedure(spName);

            var result = ToArchivoList(GetDtParameter("pCurResult_out"));
            return result;
        }

        private List<Archivo> ToArchivoList(DataTable dt)
        {
            try
            {
                var archivoList = new List<Archivo>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        archivoList.Add(new Archivo()
                        {
                            IdArchivo = row.IntParse("IDARCHIVO"),
                            RutaArchivo = row.StringParse("RUTAARCHIVO"),
                            NomArchivo = row.StringParse("NOMBREARCHIVO"),
                            ExtArchivo = row.StringParse("EXTENSIONARCHIVO"),
                            IdDatosFacturacion = row.IntParse("IDDATOSFACTURACION"),
                            IdUsuWeb = row.IntParse("IDUSUWEBCREA"),
                            UrlArchivo = row.StringParse("URLARCHVIO")
                        });
                    }
                }
                return archivoList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}