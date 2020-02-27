using Expertia.Estructura.Models;
using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.AppWebs;
using Expertia.Estructura.Repository.General;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Expertia.Estructura.Models.Behavior;
using System.Collections;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.FileRetail)]
    public class FileRetailController : BaseController<object>
    {
        #region Properties               
        private IDatosUsuario _datosUsuario;
        private ICotizacionSRV_Repository _CotizacionSRV_Repository;        
        #endregion

        #region Constructor
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            if (unidadNegocioKey == null)
            {
                unidadNegocioKey = UnidadNegocioKeys.AppWebs;
            }

            _datosUsuario = new DatosUsuario(unidadNegocioKey);
            return unidadNegocioKey;
        }
        #endregion
      
        #region Metodos Publicos
        [Route(RouteAction.Asociate)]
        public IHttpActionResult AsociateFile(AssociateFile FileAssociate)
        {
            AssociateFileRS _responseAsociate = new AssociateFileRS();
            CotizacionVta DtsCotizacionVta = new CotizacionVta();
            UsuarioLogin DtsUsuarioLogin = null;

            List<FilePTACotVta> lstFilesPTACotVta = null;
            List<FilePTACotVta> lstFilesPTAOld = null;

            Nullable<double> dblMontoEstimadoFile = null;

            ArrayList lstFechasCotVta = new ArrayList(); /*Duda*/
            bool bolCambioEstado = false; /*Si luego aplica el cambio de estado agregarlo al RQ*/

            /*Datos que se quitaran, solo lo agregamos para tener una mejor vision*/
            //bool bolEsCounterAdministrativo = objUsuarioSession.EsCounterAdminSRV;

            string ErrorAsociate = string.Empty;
            try
            {
                /*Validaciones*/
                validacionAssociate(ref FileAssociate, ref _responseAsociate, ref DtsUsuarioLogin);
                if (string.IsNullOrEmpty(_responseAsociate.CodigoError) == false) return Ok(_responseAsociate);

                /*Obtenemos los datos del SRV, etc*/
                DtsCotizacionVta = _CotizacionSRV_Repository.Get_Datos_CotizacionVta(FileAssociate.idCotSRV_SF);

                /*Realizamos la asociacion*/
                if(bolCambioEstado == true) { } else{/*Logica si tiene asociado un servicio*/}
                if (DtsCotizacionVta.IdEstado == Convert.ToInt16(ENUM_ESTADOS_COT_VTA.Facturado))
                {
                    lstFilesPTACotVta = new List<FilePTACotVta>();
                    FilePTACotVta _FilePTACotVta = null;

                    foreach (FileSRV fileSRV in FileAssociate.LstFiles)
                    {
                        if (!string.IsNullOrEmpty(fileSRV.IdFilePTA.ToString()))
                        {
                            _FilePTACotVta = new FilePTACotVta();
                            _FilePTACotVta.IdCot = FileAssociate.idCotSRV_SF;
                            _FilePTACotVta.IdSuc = Convert.ToInt16(fileSRV.Sucursal);
                            _FilePTACotVta.IdFilePTA = fileSRV.IdFilePTA;
                            _FilePTACotVta.ImporteFacturado = fileSRV.ImporteFact;
                            _FilePTACotVta.Moneda = fileSRV.Moneda;
                            lstFilesPTACotVta.Add(_FilePTACotVta);
                        }
                    }
                    /*Logica si tiene asociado un servicio*/

                    /*Obtenemos los Files anteriores (Si los tiene)*/
                    lstFilesPTAOld = _CotizacionSRV_Repository._SelectFilesPTABy_IdCot(FileAssociate.idCotSRV_SF, 0, 0, 0);

                    /*Insertamos el POST*/
                    string strTextoPost = "<span>Se asocio los files a la cotizacion</span>";                    
                    Post_SRV _PostSRV_RQ = new Post_SRV()
                    {
                        IdCot = FileAssociate.idCotSRV_SF,
                        TipoPost = Convert.ToInt16(Constantes_SRV.ID_TIPO_POST_SRV_USUARIO),
                        TextoPost = strTextoPost,
                        IPUsuCrea = "127.0.0.0",
                        LoginUsuCrea = DtsUsuarioLogin.LoginUsuario,
                        IdUsuWeb = DtsUsuarioLogin.IdUsuario,
                        IdDep = DtsUsuarioLogin.IdDep,
                        IdOfi = DtsUsuarioLogin.IdOfi,
                        Archivos = null,
                        LstFilesPTA = lstFilesPTACotVta,
                        IdEstado = DtsCotizacionVta.IdEstado,
                        CambioEstado = bolCambioEstado,
                        LstFechasCotVta = null,
                        EsAutomatico = false,
                        ArchivoMail = null,
                        EsCounterAdmin = DtsUsuarioLogin.EsCounterAdminSRV,
                        IdUsuWebCounterCrea = (DtsUsuarioLogin.EsCounterAdminSRV == true ? DtsCotizacionVta.IdUsuWeb : DtsUsuarioLogin.IdUsuario),
                        IdOfiCounterCrea = (DtsUsuarioLogin.EsCounterAdminSRV == true ? DtsCotizacionVta.IdOfi : DtsUsuarioLogin.IdOfi),
                        IdDepCounterCrea = (DtsUsuarioLogin.EsCounterAdminSRV == true ? DtsCotizacionVta.IdDep : DtsUsuarioLogin.IdDep),
                        EsUrgenteEmision = null,
                        FecPlazoEmision = null,
                        IdMotivoNoCompro = null,
                        OtroMotivoNoCompro = null,
                        MontoEstimadoFile = null
                    };
                    _CotizacionSRV_Repository.ProcesosPostCotizacion(_PostSRV_RQ);

                }

                return Ok(_responseAsociate);
            }
            catch (Exception ex)
            {
                ErrorAsociate = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    Request = FileAssociate,
                    Response = _responseAsociate,
                    Exception = ErrorAsociate
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliares
        private void validacionAssociate(ref AssociateFile _fileAssociate, ref AssociateFileRS _responseFile, ref UsuarioLogin UserLogin)
        {
            string mensajeError = string.Empty;
            //_pedido.IdLang = 1; _pedido.IdWeb = 0;

            if (_fileAssociate.idCotSRV_SF <= 0)
            {
                mensajeError += "Envie el codigo de SRV|";
            }
            if (_fileAssociate.idoportunidad_SF <= 0)
            {
                mensajeError += "Envie el codigo de Oportunidad|";
            }
            if (_fileAssociate.LstFiles == null)
            {
                mensajeError += "Envie la lista de files a asociar|";
            }
            else if (_fileAssociate.LstFiles.Count > 3)
            {
                mensajeError += "El maximo de files asociar es 3|";
            }
            else
            {
                int posListFile = 0;
                foreach (FileSRV fileSRV in _fileAssociate.LstFiles)
                {
                    if (fileSRV == null)
                    {
                        mensajeError += "Envie el registro " + posListFile + " de la lista de files a asociar|";
                        break;
                    }
                    if (fileSRV.IdFilePTA <= 0)
                    {
                        mensajeError += "Envie el IdFile del registro " + posListFile + " de la lista de files a asociar|";
                    }
                    if (fileSRV.Fecha == null)
                    {
                        mensajeError += "Envie la Fecha del registro " + posListFile + " de la lista de files a asociar|";
                    }
                    if (fileSRV.ImporteFact > 0)
                    {
                        mensajeError += "Envie el importe del registro " + posListFile + " de la lista de files a asociar|";
                    }
                    if (string.IsNullOrEmpty(fileSRV.Cliente))
                    {
                        mensajeError += "Envie el cliente del registro " + posListFile + " de la lista de files a asociar|";
                    }
                    if (string.IsNullOrEmpty(fileSRV.Moneda))
                    {
                        mensajeError += "Envie la moneda del registro " + posListFile + " de la lista de files a asociar|";
                    }

                    if (string.IsNullOrEmpty(mensajeError) == false) { break; }
                    posListFile++;
                }
            }

            if (_fileAssociate.idUsuario > 0)
            {
                /*Cargamos Datos del Usuario*/
                RepositoryByBusiness(null);
                UserLogin = _datosUsuario.Get_Dts_Usuario_Personal(Convert.ToInt32(_fileAssociate.idUsuario));
                if (UserLogin == null) { mensajeError += "ID del Usuario no registrado|"; }
            }
            else
            {
                mensajeError += "Envie el ID del Usuario correctamente|";
            }

            if (string.IsNullOrEmpty(mensajeError) == false)
            {
                _responseFile.CodigoError = "VA";
                _responseFile.MensajeError = mensajeError;
            }
        }
        #endregion
    }
}
