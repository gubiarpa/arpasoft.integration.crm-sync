using Expertia.Estructura.Models.Retail;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.Retail
{
    public class OportunidadRetailRepository : OracleBase<OportunidadRetailReq>
    {
        public OportunidadRetailRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

        #region PublicMethods
        public Operation ObtienePersonalXId_TrustWeb(
            int pIntIdUsuWeb
            )
        {
            try
            {
                var operation = new Operation();

                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int64, pIntIdUsuWeb);
                AddParameter("pCurResult_out", OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);

                ExecuteStoredProcedure("APPWEBS.PKG_PERSONAL.SP_PERS_OBTIENE_TRUSTWEB");

                var personalRow = GetDtParameter("pCurResult_out").Rows[0];
                operation["pCurResult_out"] = ToPersonal(personalRow);

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation ObtieneUsuarioWebXId(
            int pIntIdUsuWeb
            )
        {
            try
            {
                var operation = new Operation();

                AddParameter("pNumIdUsuWeb", OracleDbType.Int64, pIntIdUsuWeb);
                AddParameter("pCurResult_out", OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);

                ExecuteStoredProcedure("APPWEBS.PKG_USUARIO_WEB.SP_UW_OBTIENE_X_ID");

                var usuarioWebRow = GetDtParameter("pCurResult_out").Rows[0];
                operation["pCurResult_out"] = ToUsuarioWeb(usuarioWebRow);

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Operation SelectByDocumento(
            string pStrTipoDoc,
            string pStrNumDoc
            )
        {
            try
            {
                var operation = new Operation();

                AddParameter("pVarIdTipoDoc_in", OracleDbType.Varchar2, pStrTipoDoc, ParameterDirection.Input, 3);
                AddParameter("pVarNumDoc_in", OracleDbType.Varchar2, pStrNumDoc, ParameterDirection.Input, 30);
                AddParameter("pCurResult_out", OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);

                ExecuteStoredProcedure("APPWEBS.PKG_CLIENTE_COT.SP_GET_X_DOC");

                operation["pCurResult_out"] = ToClienteCot(GetDtParameter("pCurResult_out"));

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation SelectByEmail(
            string strCorreo
            )
        {
            try
            {
                var operation = new Operation();

                AddParameter("pMail_in", OracleDbType.Varchar2, strCorreo);
                AddParameter("pCurResult_out", OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);

                ExecuteStoredProcedure("APPWEBS.PKG_CLIENTE_COT.SP_GET_X_MAIL");

                operation["pCurResult_out"] = ToClienteCot(GetDtParameter("pCurResult_out"));

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation InsertaClienteCotizacion(
            string pStrNomCliCot,
            string pStrApePatCliCot,
            string pStrApeMatCliCot,
            string pStrEmailCliCot,
            string pStrEmailAlterCliCot,
            List<TelefonoCliCot> pLstTelefonos,
            bool pBolRecibePromo,
            string pStrDirecCliCot,
            string pStrNumDocCliCot,
            string pStrIdTipDoc,
            int pIntIdUsuWeb,
            int pIntIdWeb,
            List<ArchivoCliCot> pLstArchivos,
            bool pBolEsAdicional,
            int? pIntIdCot,
            DateTime? pDatFecNac
            )
        {
            try
            {
                var operation = new Operation();

                AddParameter("pVarNomCliCot_in", OracleDbType.Varchar2, pStrNomCliCot);
                AddParameter("pVarApeCliCot_in", OracleDbType.Varchar2, pStrApePatCliCot);
                AddParameter("pVarApeMatCliCot_in", OracleDbType.Varchar2, pStrApeMatCliCot);
                AddParameter("pVarEmailCliCot_in", OracleDbType.Varchar2, pStrEmailCliCot);
                AddParameter("pVarEmailAlterCliCot_in", OracleDbType.Varchar2, pStrEmailAlterCliCot);
                AddParameter("pChrRecibePromo_in", OracleDbType.Varchar2, pBolRecibePromo ? "1" : "0");
                AddParameter("pVarDirecCliCot_in", OracleDbType.Varchar2, pStrDirecCliCot);
                AddParameter("pChrIdTipDoc_in", OracleDbType.Varchar2, pStrIdTipDoc.Trim().Equals("-1") ? null : pStrIdTipDoc);
                AddParameter("pVarNumDocCliCot_in", OracleDbType.Varchar2, pStrNumDocCliCot);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Varchar2, pIntIdUsuWeb);
                AddParameter("pNumIdWeb_in", OracleDbType.Varchar2, pIntIdWeb);
                AddParameter("pDatFecNacCli_in", OracleDbType.Date, pDatFecNac.HasValue ? pDatFecNac : null);
                AddParameter("pNumIdNewCliCot_out", OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);

                ExecuteStoredProcedure("APPWEBS.PKG_CLIENTE_COT.SP_INSERTA_CLI");
 
                if (!int.TryParse(GetOutParameter("pNumIdNewCliCot_out").ToString(), out int pNumIdNewCliCot_out)) pNumIdNewCliCot_out = 0;
                operation["pNumIdNewCliCot_out"] = pNumIdNewCliCot_out;

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation InsertaTelefClienteCotizacion(
            int pIntIdCliCot,
            string pStrNumTelf,
            string pStrAnexoTelf,
            bool pBolEsTelfPrinc,
            int pIntIdTipoTelf,
            Int16? pIntCodPais,
            Int16? pIntCodArea
            )
        {
            try
            {
                var operation = new Operation();

                AddParameter("pNumIdCliCot_in", OracleDbType.Int32, pIntIdCliCot);
                AddParameter("pVarNumTelf_in", OracleDbType.Varchar2, pStrNumTelf);
                AddParameter("pVarNumAnexoTelf_in", OracleDbType.Varchar2, pStrAnexoTelf);
                AddParameter("pChrTelfPrinc_in", OracleDbType.Char, pBolEsTelfPrinc ? "1" : "0");
                AddParameter("pNumIdTipoTelf_in", OracleDbType.Int16, pIntIdTipoTelf);
                AddParameter("pNumCodPaisTelf_in", OracleDbType.Int16, pIntCodPais);
                AddParameter("pNumCodAreaTelf_in", OracleDbType.Int16, pIntCodArea);

                ExecuteStoredProcedure("APPWEBS.PKG_CLIENTE_COT.SP_INSERTA_TELF_CLI");

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation InsertaArchivoClienteCotizacion(
            int pIntIdCliCot,
            string pStrRutaServArchivo,
            string pStrRutaURLArchivo,
            string pStrNomArchivo,
            string pStrExtArchivo,
            List<Byte> pBytArchivo,
            int pIntIdUsuWeb
            )
        {
            try
            {
                var operation = new Operation();

                AddParameter("pNumIdCliCot_in", OracleDbType.Int32, pIntIdCliCot);
                AddParameter("pVarRutaServArchivo_in", OracleDbType.Varchar2, pStrRutaServArchivo);
                AddParameter("pVarRutaURLArchivo_in", OracleDbType.Varchar2, pStrRutaURLArchivo);
                AddParameter("pVarNomArchivo_in", OracleDbType.Varchar2, pStrNomArchivo);
                AddParameter("pVarExtArchivo_in", OracleDbType.Char, pStrExtArchivo);
                AddParameter("pBlbArchivo_in", OracleDbType.Blob, pBytArchivo);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, pIntIdUsuWeb);

                ExecuteStoredProcedure("APPWEBS.PKG_CLIENTE_COT.SP_INSERTA_ARCHIVO_CLI");
                
                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation ActualizaClienteCotizacion(
            int pIntIdCliCot,
            string pStrNomCliCot,
            string pStrApeCliCot,
            string pStrApeMatCliCot,
            string pStrEmailCliCot,
            int pIntIdUsuWeb
            )
        {
            try
            {
                var operation = new Operation();

                AddParameter("pNumIdCliCot_in", OracleDbType.Int32, pIntIdCliCot);
                AddParameter("pVarNomCliCot_in", OracleDbType.Varchar2, pStrNomCliCot);
                AddParameter("pVarApeCliCot_in", OracleDbType.Varchar2, pStrApeCliCot);
                AddParameter("pVarApeMatCliCot_in", OracleDbType.Varchar2, pStrApeMatCliCot);
                AddParameter("pVarEmailCliCot_in", OracleDbType.Varchar2, pStrEmailCliCot);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, pIntIdUsuWeb);

                ExecuteStoredProcedure("APPWEBS.PKG_CLIENTE_COT.SP_UPD_CLI_2");

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation InsertaCotizacionVenta(
            short pIntModoIng,
            string pStrTextoSol,
            string pStrNomUsuCrea,
            string pStrLoginUsuCrea,
            string pStrIPUsuCrea,
            int pIntIdCliCotVta,
            int pIntIdUsuWeb,
            int pIntIdDep,
            int pIntIdOfi,
            int pIntIdWeb,
            int pIntIdLang,
            short pIntIdCanalVta,
            string[] pStrArrayServicios,
            string pStrCodIATAPrinc,
            int? pIntIdEmpCot,
            short pIntIdEstOtro,
            string pStrDestinosPref,
            DateTime? pDatFecSalida,
            DateTime? pDatFecRegreso,
            short? pIntCantPaxAdulto,
            short? pIntCantPaxNino,
            string pStrPaisResidencia,
            int? pIntIdReservaVuelos,
            short? pIntIdSucursalPaq,
            int? pIntIdWebPaq,
            int? pintIdCotResPaq,
            string pStrTipoPaq,
            int? pintIdReservaAuto,
            int? pintIdReservaSeguro,
            int? pintIdReservaHotel,
            string pStrNomGrupo,
            decimal? pNumMontoDscto,
            int pIntIdOAtencion = 0,
            int pIntIdEvento = 0
            )
        {
            try
            {
                var operation = new Operation();

                AddParameter("pChrModoIng_in", OracleDbType.Int16, pIntModoIng);
                AddParameter("pClbTextSol_in", OracleDbType.Clob, pStrTextoSol);
                AddParameter("pVarNomUsuCrea_in", OracleDbType.Varchar2, pStrNomUsuCrea);
                AddParameter("pVarLoginUsuCrea_in", OracleDbType.Varchar2, pStrLoginUsuCrea);
                AddParameter("pVarIPUsuCrea_in", OracleDbType.Varchar2, pStrIPUsuCrea);
                AddParameter("pNumIdCliCot_in", OracleDbType.Int32, pIntIdCliCotVta);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, pIntIdUsuWeb);
                AddParameter("pNumIdDep_in", OracleDbType.Int32, pIntIdDep);
                AddParameter("pNumIdOfi_in", OracleDbType.Int32, pIntIdOfi);
                AddParameter("pNumIdWeb_in", OracleDbType.Int32, pIntIdWeb);
                AddParameter("pNumIdLang_in", OracleDbType.Int32, pIntIdLang);
                AddParameter("pNumIdCanalVta_in", OracleDbType.Int16, pIntIdCanalVta);
                AddParameter("pChrIATAPrinc_in", OracleDbType.Char, pStrCodIATAPrinc);
                AddParameter("pNumIdEmpCot_in", OracleDbType.Int32, pIntIdEmpCot);
                AddParameter("pNumIdEstOtro_in", OracleDbType.Int16, pIntIdEstOtro);
                AddParameter("pVarDestinosPref_in", OracleDbType.Varchar2, pStrDestinosPref);
                AddParameter("pDatFecSalida_in", OracleDbType.Date, pDatFecSalida);
                AddParameter("pDatFecRegreso_in", OracleDbType.Date, pDatFecRegreso);
                AddParameter("pNumCantPaxAdulto_in", OracleDbType.Int16, pIntCantPaxAdulto);
                AddParameter("pNumCantPaxNino_in", OracleDbType.Int16, pIntCantPaxNino);
                AddParameter("pVarPaisResidencia_in", OracleDbType.Varchar2, pStrPaisResidencia);
                AddParameter("pNumIdReservaVue_in", OracleDbType.Int32, pIntIdReservaVuelos);
                AddParameter("pNumIdSucPaq_in", OracleDbType.Int16, pIntIdSucursalPaq);
                AddParameter("pNumIdWebPaq_in", OracleDbType.Int32, pIntIdWebPaq);
                AddParameter("pNumIdCotResPaq_in", OracleDbType.Int32, pintIdCotResPaq);
                AddParameter("pVarTipoPaq_in", OracleDbType.Varchar2, pStrTipoPaq);
                AddParameter("pVarNomGrupo_in", OracleDbType.Varchar2, pStrNomGrupo);
                AddParameter("pNumMontoDscto_in", OracleDbType.Double, pNumMontoDscto);
                AddParameter("pNumIdNewCot_out", OracleDbType.Double, null, ParameterDirection.Output);

                ExecuteStoredProcedure("APPWEBS.PKG_COTIZACION_VTA_WFF.SP_INSERTA_COT_CRM");

                if (!int.TryParse(GetOutParameter("pNumIdNewCot_out").ToString(), out int pNumIdNewCliCot_out)) pNumIdNewCliCot_out = 0;
                operation["pNumIdNewCot_out"] = pNumIdNewCliCot_out;

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation InsertaIngresoCliente(
            DateTime pDatFecRegistro,
            string pStrNomCli,
            string pStrApeCli,
            string pStrApeMatCli,
            string pStrEmailCli,
            short pIntIdMotivo,
            string pStrOtroMotivo,
            string pStrDestino,
            int pIntIdUsuWebCrea,
            int pIntIdOfiUsuWebCrea,
            int pIntIdDepUsuWebCrea,
            DateTime? pDatFecAtiende,
            int? pIntIdUsuWebAtiende,
            int? pIntIdOfiUsuWebAtiende,
            int? pIntIdDepUsuWebAtiende,
            string pStrComentarios,
            string pStrIP,
            long? pIntIdCotSRV,
            int? pIntIdCliCot,
            string pStrIdTipoDoc,
            string pStrNumDoc,
            DateTime? pDatFecSalida
            )
        {
            try
            {
                var operation = new Operation();

                AddParameter("pDatFecha_in", OracleDbType.Date, pDatFecRegistro, ParameterDirection.Input);
                AddParameter("pVarNomCli_in", OracleDbType.Varchar2, pStrNomCli, ParameterDirection.Input, 50);
                AddParameter("pVarApeCli_in", OracleDbType.Varchar2, pStrApeCli, ParameterDirection.Input, 50);
                AddParameter("pVarApeMatCli_in", OracleDbType.Varchar2, pStrApeMatCli, ParameterDirection.Input, 50);
                AddParameter("pVarEmailCli_in", OracleDbType.Varchar2, pStrEmailCli, ParameterDirection.Input, 100);
                AddParameter("pNumIdMotivo_in", OracleDbType.Varchar2, pIntIdMotivo, ParameterDirection.Input);
                AddParameter("pVarDescMotivo_in", OracleDbType.Varchar2, pStrOtroMotivo, ParameterDirection.Input, 30);
                AddParameter("pVarDestino_in", OracleDbType.Varchar2, pStrDestino, ParameterDirection.Input, 200);
                AddParameter("pNumIdUsuWebCrea_in", OracleDbType.Int32, pIntIdUsuWebCrea, ParameterDirection.Input);
                AddParameter("pNumIdOfiCrea_in", OracleDbType.Int32, pIntIdOfiUsuWebCrea, ParameterDirection.Input);
                AddParameter("pNumIdDepCrea_in", OracleDbType.Int32, pIntIdDepUsuWebCrea, ParameterDirection.Input);
                AddParameter("pDatFechaAtt_in", OracleDbType.Date, pDatFecAtiende, ParameterDirection.Input);
                AddParameter("pNumIdUsuWebAtt_in", OracleDbType.Int32, pIntIdUsuWebAtiende, ParameterDirection.Input);
                AddParameter("pNumIdOfiAtt_in", OracleDbType.Int32, pIntIdOfiUsuWebAtiende, ParameterDirection.Input);
                AddParameter("pNumIdDepAtt_in", OracleDbType.Int32, pIntIdDepUsuWebAtiende, ParameterDirection.Input);
                AddParameter("pVarComentarios_in", OracleDbType.Varchar2, pStrComentarios, ParameterDirection.Input, 500);
                AddParameter("pVarIP_in", OracleDbType.Varchar2, pStrIP, ParameterDirection.Input, 30);
                AddParameter("pNumIdCotSRV_in", OracleDbType.Int64, pIntIdCotSRV, ParameterDirection.Input);
                AddParameter("pNumIdCliCot_in", OracleDbType.Int32, pIntIdCliCot, ParameterDirection.Input);
                AddParameter("pVarNumIdDoc_in", OracleDbType.Varchar2, pStrIdTipoDoc, ParameterDirection.Input, 3);
                AddParameter("pVarNumDoc_in", OracleDbType.Varchar2, pStrNumDoc, ParameterDirection.Input, 30);
                AddParameter("pDatFecSalida_in", OracleDbType.Date, pDatFecSalida, ParameterDirection.Input);
                AddParameter("pNumIdOcurr_out", OracleDbType.Int32, null, ParameterDirection.Output);

                ExecuteStoredProcedure("APPWEBS.PKG_ATENCION_CLIENTE.SP_INSERTA");

                if (!int.TryParse(GetOutParameter("pNumIdOcurr_out").ToString(), out int pNumIdOcurr_out)) pNumIdOcurr_out = 0;
                operation["pNumIdOcurr_out"] = pNumIdOcurr_out;

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ValidarEnvioPromociones(
            int pIntIdOcurrencia,
            string pEnviaPromo_in
            )
        {
            try
            {
                AddParameter("pNumIdOcurr_in", OracleDbType.Int32, pIntIdOcurrencia, ParameterDirection.Input);
                AddParameter("pEnviaPromo_in", OracleDbType.Int32, pEnviaPromo_in, ParameterDirection.Input);

                ExecuteStoredProcedure("APPWEBS.PKG_ENVIO_PROMOCIONES.SP_ACTUALIZA_ENVIAPROMOCIONES");

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void EnviarPromociones(
            Personal objPersonal,
            OportunidadRetailReq oportunidadRetailReq,
            bool chEnviarPromociones,
            string strNomCli,
            string strApeCli,
            string strEmailCli
            )
        {
            int intIdOfi = objPersonal.IdOficina; // objUsuarioSession.IdOfi;
            int intIdDep = objPersonal.IdDepartamento;  // objUsuarioSession.IdDep;
            try
            {
                if (oportunidadRetailReq.EnviarPromociones.Equals("0") & chEnviarPromociones)
                {
                    if (
                        intIdDep != ConstantesOportunidadRetail.INT_ID_DEP_INTERNO ||
                        intIdDep != ConstantesOportunidadRetail.INT_ID_DEP_RECEPTIVO
                        )
                        NMMail.Mail_AgregaEmailListaBoletinNMV(7, 1, (strNomCli + " " + strApeCli).ReplaceSpecialChars(), strEmailCli, Constantes_Mail.EMAIL_BOLETIN_NMV, true);

                    if (
                        (intIdOfi == Constantes_SRV.INT_ID_OFI_CORPORATIVO_VACACIONAL && intIdDep == Constantes_SRV.INT_ID_DEP_COUNTER) ||
                        (intIdOfi == Constantes_SRV.INT_ID_OFI_NMV & intIdDep == Constantes_SRV.INT_ID_DEP_COUNTER)
                        )
                        NMMail.Mail_AgregaEmailListaBoletinNMV(7, 1, (strNomCli + " " + strApeCli).ReplaceSpecialChars(), strEmailCli, Constantes_Mail.EMAIL_BOLETIN_CORPORATIVO_VACACIONAL, true);
                }
                else if (oportunidadRetailReq.EnviarPromociones.Equals("1") & !chEnviarPromociones)
                {
                    // Estaba suscrito y ahora ya no lo va a estar
                    NMMail.Mail_AgregaEmailListaBoletinNMV(7, 1, (strNomCli + " " + strApeCli).ReplaceSpecialChars(), strEmailCli, Constantes_Mail.EMAIL_BOLETIN_NMV, false);

                    if (
                        (intIdOfi == Constantes_SRV.INT_ID_OFI_CORPORATIVO_VACACIONAL && intIdDep == Constantes_SRV.INT_ID_DEP_COUNTER) ||
                        (intIdOfi == Constantes_SRV.INT_ID_OFI_NMV & intIdDep == Constantes_SRV.INT_ID_DEP_COUNTER)
                        )
                        NMMail.Mail_AgregaEmailListaBoletinNMV(7, 1, (strNomCli + " " + strApeCli).ReplaceSpecialChars(), strEmailCli, Constantes_Mail.EMAIL_BOLETIN_CORPORATIVO_VACACIONAL, false);
                }
                else if (oportunidadRetailReq.EnviarPromociones.Equals("") & chEnviarPromociones)
                {
                    if (
                        intIdDep != ConstantesOportunidadRetail.INT_ID_DEP_INTERNO ||
                        intIdDep != ConstantesOportunidadRetail.INT_ID_DEP_RECEPTIVO
                        )
                        NMMail.Mail_AgregaEmailListaBoletinNMV(7, 1, (strNomCli + " " + strApeCli).ReplaceSpecialChars(), strEmailCli, Constantes_Mail.EMAIL_BOLETIN_NMV, true);

                    if (
                        (intIdOfi == Constantes_SRV.INT_ID_OFI_CORPORATIVO_VACACIONAL && intIdDep == Constantes_SRV.INT_ID_DEP_COUNTER) ||
                        (intIdOfi == Constantes_SRV.INT_ID_OFI_NMV && intIdDep == Constantes_SRV.INT_ID_DEP_COUNTER)
                        )
                        NMMail.Mail_AgregaEmailListaBoletinNMV(7, 1, (strNomCli + " " + strApeCli).ReplaceSpecialChars(), strEmailCli, Constantes_Mail.EMAIL_BOLETIN_CORPORATIVO_VACACIONAL, true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Inserta_Post_Cot(
            int pIntIdCot,
            string pStrTipoPost,
            string pStrTextoPost,
            string pStrIPUsuCrea,
            string pStrLoginUsuCrea,
            int pIntIdUsuWeb,
            int pIntIdDep,
            int pIntIdOfi,
            List<ArchivoPostCot> pLstArchivos,
            List<FilePTACotVta> pLstFilesPTA,
            short pIntIdEstado,
            bool pBolCambioEstado,
            ArrayList pLstFechasCotVta,
            bool pBolEsAutomatico,
            byte[] pBytArchivoMail,
            bool pBolEsCounterAdmin,
            int? pIntIdUsuWebCounterCrea,
            int? pIntIdOfiCounterCrea,
            int? pIntIdDepCounterCrea,
            bool? pBolEsUrgenteEmision,
            DateTime? pDatFecPlazoEmision,
            short? pIntIdMotivoNoCompro,
            string pStrOtroMotivoNoCompro,
            double? pDblMontoEstimadoFile
            )
        {
            try
            {
                AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntIdCot);
                AddParameter("pChrTipoPost_in", OracleDbType.Char, pStrTipoPost);
                AddParameter("pClbTextoPost_in", OracleDbType.Clob, pStrTextoPost);
                AddParameter("pVarIPUsuCrea_in", OracleDbType.Varchar2, pStrIPUsuCrea);
                AddParameter("pVarLoginUsuCrea_in", OracleDbType.Varchar2, pStrLoginUsuCrea);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, pIntIdUsuWeb);
                AddParameter("pNumIdDep_in", OracleDbType.Int32, pIntIdDep);
                AddParameter("pNumIdOfi_in", OracleDbType.Int32, pIntIdOfi);
                AddParameter("pChrCambioEst_in", OracleDbType.Char, pBolCambioEstado ? "S" : "N");
                AddParameter("pNumIdEst_in", OracleDbType.Int16, pBolCambioEstado ? (int?)pIntIdEstado : null);
                AddParameter("pChrEsAutomatico_in", OracleDbType.Char, pBolEsAutomatico ? "1" : "0");
                AddParameter("pChrEsUrgenteEmision_in", OracleDbType.Char, pBolEsUrgenteEmision != null ? ((bool)pBolEsUrgenteEmision ? "1" : "0") : null);
                AddParameter("pDatFecPlazoEmision_in", OracleDbType.Date, pDatFecPlazoEmision);
                AddParameter("pNumIdNewPost_out", OracleDbType.Int32, null, ParameterDirection.Output);

                ExecuteStoredProcedure("APPWEBS.PKG_COTIZACION_VTA_WFF.SP_INSERTA_POST_COT", true);

                if (!int.TryParse(GetOutParameter("pNumIdNewPost_out").ToString(), out int pNumIdOcurr_out)) pNumIdOcurr_out = 0;
                return pNumIdOcurr_out;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Inserta_Archivo_Post_Cot(
            int pIntIdCot,
            int pIntIdPost,
            string pStrRutaArchivo,
            string pStrNomArchivo,
            string pStrExtArchivo,
            byte[] pBytArchivo
            )
        {

        }

        public void Update_Estado_Cot_Vta(
            int pIntIdCot,
            string pStrLoginUsuCrea,
            string pStrIPUsuCrea,
            short pIntIdEstado,
            int pIntIdUsuWeb,
            int pIntIdDep,
            int pIntIdOfi,
            bool pBolEsAutomatico,
            int? pIntIdUsuWebCounterAdmin
            )
        {
            try
            {
                string strDetalleAccion = string.Empty;
                if (pBolEsAutomatico)
                    strDetalleAccion = "Usuario " + pIntIdUsuWeb + " cambia estado de cotización '" + pIntIdCot + "' a " + pIntIdEstado + "(automático)";
                else
                    if (pIntIdUsuWebCounterAdmin.HasValue)
                    strDetalleAccion = "Usuario " + pIntIdUsuWebCounterAdmin.Value + " cambia estado de cotización '" + pIntIdCot + "' a " + pIntIdEstado;
                else
                    strDetalleAccion = "Usuario " + pIntIdUsuWeb + " cambia estado de cotización '" + pIntIdCot + "' a " + pIntIdEstado;

                AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntIdCot, ParameterDirection.Input);
                AddParameter("pVarLoginUsuCrea_in", OracleDbType.Varchar2, pStrLoginUsuCrea, ParameterDirection.Input, 50);
                AddParameter("pVarIPUsuCrea_in", OracleDbType.Varchar2, pStrIPUsuCrea, ParameterDirection.Input, 20);
                AddParameter("pVarDetAccion_in", OracleDbType.Varchar2, strDetalleAccion, ParameterDirection.Input, 300);
                AddParameter("pNumIdEstado_in", OracleDbType.Int16, pIntIdEstado, ParameterDirection.Input);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, pIntIdUsuWeb, ParameterDirection.Input);
                AddParameter("pNumIdDep_in", OracleDbType.Int32, pIntIdDep, ParameterDirection.Input);
                AddParameter("pNumIdOfi_in", OracleDbType.Int32, pIntIdOfi, ParameterDirection.Input);

                ExecuteStoredProcedure("APPWEBS.PKG_COTIZACION_VTA_WFF.SP_ACTUALIZA_EST_COT", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update_MotivoNoCompro(
            int pIntIdCot,
            short? pIntIdMotivoNoCompro,
            string pStrOtroMotivoNoCompro
            )
        {
            try
            {
                AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntIdCot, ParameterDirection.Input);
                AddParameter("pNumIdMotivo_in", OracleDbType.Int32, pIntIdMotivoNoCompro, ParameterDirection.Input);
                AddParameter("pVarOtroMotivo_in", OracleDbType.Varchar2, pStrOtroMotivoNoCompro, ParameterDirection.Input, 100);

                ExecuteStoredProcedure("APPWEBS.PKG_COTIZACION_VTA_WFF.SP_UPD_MOTIVO_NO_COMPRO");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Parse
        private IEnumerable<Personal> ToPersonal(DataTable dt)
        {
            try
            {
                var personalList = new List<Personal>();
                foreach (DataRow row in dt.Rows) personalList.Add(ToPersonal(row));
                return personalList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private Personal ToPersonal(DataRow dr)
        {
            try
            {
                return new Personal()
                {
                    IdUsuWeb = dr.IntParse("USUWEB_ID"),
                    LoginPer = dr.StringParse("USUWEB_LOGIN"),
                    ApePatPer = dr.StringParse("PER_APEPAT"),
                    ApeMatPer = dr.StringParse("PER_APEMAT"),
                    NomPer = dr.StringParse("PER_NOM"),
                    NomCompletoPer = string.Format("{0} {1} {2}", dr.StringParse("PER_NOM"), dr.StringParse("PER_APEPAT"), dr.StringParse("PER_APEMAT")),
                    DirecPer = dr.StringParse("PER_DIREC"),
                    CodPostalPer = dr.StringParse("PER_COD_POS"),
                    EmailPer = dr.StringParse("PER_EMAIL"),
                    TelfCasaPer = dr.StringParse("PER_TELF_CASA"),
                    TelfDirectoPer = dr.StringParse("PER_TELF_DIR"),
                    TelfMovilPer = dr.StringParse("PER_TELF_MOV"),
                    TelfMovilEmpresaPer = dr.StringParse("PER_TELF_MOV_EMP"),
                    TelfNextelPer = dr.StringParse("PER_TELF_NEXTEL"),
                    TelfOficinaPer = dr.StringParse("PER_TELF_OFI"),
                    AnexoPer = dr.StringParse("PER_ANEXO"),
                    NumDocPer = dr.StringParse("PER_NRO_DOC"),
                    FechaNacPer = dr.StringParse("PER_FEC_NAC"),
                    EstadoPer = dr.IntParse("PER_ESTADO"),
                    IdDoc = dr.StringParse("DOC_CID"),
                    IdPais = dr.StringParse("PAIS_ID"),
                    IdCiudad = dr.StringParse("CIU_ID"),
                    IdDistrito = dr.IntNullParse("DIS_ID") ?? -1,
                    IdOficina = dr.IntParse("OFI_ID"),
                    IdDepartamento = dr.IntParse("DEP_ID"),
                    IdEmpresa = dr.IntParse("EMP_ID"),
                    IdUsuWebSybase = dr.IntNullParse("USUWEB_ID_SYBASE") ?? 0,
                    IdCargo = dr.IntNullParse("CAR_ID") ?? 0,
                    EsSupervisorSRV = dr.StringParse("PER_ES_SUPERVISOR").Equals("1"),
                    VarDespegar = dr.StringParse("PER_VARDESPEGUE").Equals("1"),
                    VarImgDespegar = dr.StringParse("PER_VARIMGDESPEGUE").Equals("1"),
                    PCCPer = dr.StringParse("PER_PCC"),
                    IdEmpresaPlanilla = dr.IntNullParse("ID_EMPRESA_PLANILLA"),
                    IdPlanilla = dr.IntNullParse("ID_CODIGO_PLANILLA"),
                    AutoLoginTrp = dr.StringNullParse("AUTOLOGIN_TRP"),
                    EsCounterAdmin = dr.StringParse("ES_COUNTER_ADMIN").Equals("1"),
                    TieneRpteMetas = dr.StringParse("PER_TIENE_RP_METAS").Equals("1")
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IEnumerable<UsuarioWeb> ToUsuarioWeb(DataTable dt)
        {
            try
            {
                var usuarioWebList = new List<UsuarioWeb>();
                foreach (DataRow row in dt.Rows) usuarioWebList.Add(ToUsuarioWeb(row));
                return usuarioWebList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private UsuarioWeb ToUsuarioWeb(DataRow dr)
        {
            try
            {
                return new UsuarioWeb()
                {
                    IdUsuWeb = dr.IntParse("USUWEB_ID"),
                    LoginUsuWeb = dr.StringParse("USUWEB_LOGIN"),
                    TipoUsuario = dr.StringParse("USUWEB_TIPO").Equals("P") ? UsuarioWeb.TIPO_USUARIO.PERSONAL : UsuarioWeb.TIPO_USUARIO.CLIENTE,
                    IdUsuWebSybase = dr.IntParse("USUWEB_ID_SYBASE")
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IEnumerable<ClienteCot> ToClienteCot(DataTable dt)
        {
            try
            {
                var clienteCotList = new List<ClienteCot>();

                foreach (DataRow row in dt.Rows)
                {
                    clienteCotList.Add(new ClienteCot()
                    {
                        IdCliCot = row.IntParse("CLICOT_ID"),
                        NomCliCot = row.StringParse("CLICOT_NOM"),
                        ApeCliCot = row.StringParse("CLICOT_APE"),
                        EmailCliCot = row.StringParse("CLICOT_EMAIL"),
                        ApeMatCliCot = row.StringParse("CLICOT_APE_MAT"),
                        RecibePromo = row.BoolParse("CLICOT_RECIBIR_PROMO")
                    });
                }

                return clienteCotList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}