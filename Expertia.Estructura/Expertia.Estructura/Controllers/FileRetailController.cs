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
using System.Text;
using System.Data;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.FileRetail)]
    public class FileRetailController : BaseController<object>
    {
        #region Properties               
        private IDatosUsuario _datosUsuario;
        private ICotizacionSRV_Repository _CotizacionSRV_Repository;
        private IPedidoRepository _PedidoRetail_Repository;
        #endregion

        #region Constructor
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            if (unidadNegocioKey == null)
            {
                unidadNegocioKey = UnidadNegocioKeys.AppWebs;
            }
            _CotizacionSRV_Repository = new CotizacionSRV_AW_Repository();
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
            CotizacionSRV_AW_Repository _CotizacionSRV_Repository = new CotizacionSRV_AW_Repository();

            List<FilePTACotVta> lstFilesPTACotVta = null;
            List<FilePTACotVta> lstFilesPTAOld = null;
            
            ArrayList lstFechasCotVta = new ArrayList(); /*Duda*/

            /*Se realizara el cambio de Estado a Facturado*/
            Int16 EstadoSeleccionado = (Int16)ENUM_ESTADOS_COT_VTA.Facturado;
            bool bolCambioEstado = true;

            /*Datos que se quitaran, solo lo agregamos para tener una mejor vision*/
            //bool bolEsCounterAdministrativo = objUsuarioSession.EsCounterAdminSRV;

            string ErrorAsociate = string.Empty;
            try
            {
                /*Validaciones*/
                validacionAssociate(ref FileAssociate, ref _responseAsociate, ref DtsUsuarioLogin);
                if (string.IsNullOrEmpty(_responseAsociate.CodigoError) == false) return Ok(_responseAsociate);

                //_responseAsociate.Cliente = "ER";
                //_responseAsociate.Cliente = FileAssociate.LstFiles[0].Cliente;
                //_responseAsociate.NumFile = FileAssociate.LstFiles[0].IdFilePTA;
                //_responseAsociate.Importe = Convert.ToString(FileAssociate.LstFiles[0].ImporteFact);

                if (FileAssociate.LstFiles[0].IdFilePTA == 896798)
                {
                    _responseAsociate.CodigoError = "OK";
                    _responseAsociate.MensajeError = "Informacion del numero de file.";
                    _responseAsociate.NumFile = FileAssociate.LstFiles[0].IdFilePTA;
                    _responseAsociate.Importe = 287.48;
                    _responseAsociate.Cliente = "ELENA PAUCAR IBAÑEZ";
                }
                else
                {
                    _responseAsociate.CodigoError = "ER";
                    _responseAsociate.MensajeError = "No se ha encontrado información con el número de file ingresado en la sucursal seleccionada.";
                    if (_responseAsociate.CodigoError == "ER")
                    {
                        _responseAsociate.Cliente = "";
                    }
                    _responseAsociate.NumFile = FileAssociate.LstFiles[0].IdFilePTA;
                    _responseAsociate.Importe = FileAssociate.LstFiles[0].ImporteFact;
                }


                /*Obtenemos los datos del SRV, etc*/
                //DtsCotizacionVta = _CotizacionSRV_Repository.Get_Datos_CotizacionVta(FileAssociate.idCotSRV_SF);

                ///*Realizamos la asociacion*/
                //if (EstadoSeleccionado == (Int16)ENUM_ESTADOS_COT_VTA.Facturado || DtsCotizacionVta.IdEstado == (Int16)(ENUM_ESTADOS_COT_VTA.Facturado))
                //{
                //    lstFilesPTACotVta = new List<FilePTACotVta>();
                //    FilePTACotVta _FilePTACotVta = null;

                //    foreach (FileSRV fileSRV in FileAssociate.LstFiles)
                //    {
                //        if (!string.IsNullOrEmpty(fileSRV.IdFilePTA.ToString()))
                //        {
                //            _FilePTACotVta = new FilePTACotVta();
                //            _FilePTACotVta.IdCot = FileAssociate.idCotSRV_SF;
                //            _FilePTACotVta.IdSuc = Convert.ToInt16(fileSRV.Sucursal);
                //            _FilePTACotVta.IdFilePTA = fileSRV.IdFilePTA;
                //            _FilePTACotVta.ImporteFacturado = fileSRV.ImporteFact;
                //            _FilePTACotVta.Moneda = fileSRV.Moneda;
                //            lstFilesPTACotVta.Add(_FilePTACotVta);
                //        }
                //    }
                //    /*Logica si tiene asociado un servicio*/

                //    /*Obtenemos los Files anteriores (Si los tiene)*/
                //    lstFilesPTAOld = _CotizacionSRV_Repository._SelectFilesPTABy_IdCot(FileAssociate.idCotSRV_SF, 0, 0, 0);

                //    /*Insertamos el POST*/
                //    string strTextoPost = "<span class='texto_cambio_estado'>Cambio de estado a <strong>Facturado</strong> y asociación de Files a la cotización</span><br><br>";                                        
                //    _CotizacionSRV_Repository.Inserta_Post_Cot(FileAssociate.idCotSRV_SF, Constantes_SRV.ID_TIPO_POST_SRV_USUARIO, strTextoPost,
                //        "127.0.0.0", DtsUsuarioLogin.LoginUsuario, DtsUsuarioLogin.IdUsuario, DtsUsuarioLogin.IdDep, DtsUsuarioLogin.IdOfi, null, lstFilesPTACotVta,
                //        EstadoSeleccionado, bolCambioEstado, null, false, null, DtsUsuarioLogin.EsCounterAdminSRV,
                //        (DtsUsuarioLogin.EsCounterAdminSRV == true ? DtsCotizacionVta.IdUsuWeb : DtsUsuarioLogin.IdUsuario),
                //        (DtsUsuarioLogin.EsCounterAdminSRV == true ? DtsCotizacionVta.IdOfi : DtsUsuarioLogin.IdOfi),
                //        (DtsUsuarioLogin.EsCounterAdminSRV == true ? DtsCotizacionVta.IdDep : DtsUsuarioLogin.IdDep), null, null, null, null, null);                    
                //}


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
            if (Convert.ToString(_fileAssociate.idoportunidad_SF) == null)
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
                foreach (FileSRV fileSRV in _fileAssociate.LstFiles) /*Aqui se tendra que validar si existe registro del File con la Sucursal asociadas al vendedor (configurado en base al UsuarioId)*/
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
                    /*if (fileSRV.Fecha == null)
                    {
                        mensajeError += "Envie la Fecha del registro " + posListFile + " de la lista de files a asociar|";
                    }
                    if (fileSRV.ImporteFact > 0)
                    {
                        mensajeError += "Envie el importe del registro " + posListFile + " de la lista de files a asociar|";
                    }*/
                    if (string.IsNullOrEmpty(fileSRV.Cliente))
                    {
                        mensajeError += "Envie el cliente del registro " + posListFile + " de la lista de files a asociar|";
                    }
                    /*if (string.IsNullOrEmpty(fileSRV.Moneda))
                    {
                        mensajeError += "Envie la moneda del registro " + posListFile + " de la lista de files a asociar|";
                    }*/

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
                _responseFile.CodigoError = "ER";
                _responseFile.MensajeError = "VA " + mensajeError;
            }
        }

        public FileSRV CargarInfoFile(int sucursal, int idfile)
        {
            _CotizacionSRV_Repository = new CotizacionSRV_AW_Repository();
            FileSRV fileSRV = new FileSRV();

            string strIdFile = Convert.ToString(idfile);
            string strNumFile = "1"; // Si es el File 1, 2 o 3
            string strIdEstCot = "1";
            string strTipo = "1";

            strIdFile = strIdFile.Replace(",", "");

            DataTable dtImporteFile;
            int intIdOfiCotVta = 23;//CAMBIAR POR LA OFICINA 

            if (intIdOfiCotVta == 6)//si corresponde a destinos mundiales(6)
            {
                dtImporteFile = null;//dtImporteFile = objPTAFileBO.Obtiene_InfoFile_DM(sucursal, idfile);
            }
            else
            {
                dtImporteFile = _CotizacionSRV_Repository._Select_InfoFile(sucursal, idfile);

                //dtImporteFile = objPTAFileBO.Obtiene_InfoFile(sucursal, idfile);
            }

            if (dtImporteFile == null)
            {
               //aca retornar el el codigo 'ER' y mensaje "lo que muestra este aviso"

                //objNMWebUtility.ExecuteJavaScript(this, "sin_info1", "<script>alert('No se ha encontrado información con el número de file ingresado en la sucursal seleccionada.'); </script>");
                //objNMWebUtility = null;
                //LimpiarDatosFile();
                //btnConfirmFile.Disabled = true;
            }
            else if (dtImporteFile.Rows.Count == 0)
            {
                //aca retornar el el codigo 'ER' y mensaje "lo que muestra este aviso"
                //objNMWebUtility.ExecuteJavaScript(this, "sin_info2", "<script>alert('No se ha encontrado información con el número de file ingresado en la sucursal seleccionada.'); </script>");
                //objNMWebUtility = null;
                //LimpiarDatosFile();
                //btnConfirmFile.Disabled = true;
            }
            else
            {
                DataRow drCliente = dtImporteFile.Rows[0];
                double dblImporteSumaUSD = 0;
                double dblImporteSumaSOL = 0;
                string cliente = "";
                double monto = 0;
                string moneda = "";
                // Dim dblImporteRestaUSD As Double = 0
                string strIdMoneda = drCliente["ID_MONEDA"].ToString();

                foreach (DataRow drImporteFile in dtImporteFile.Rows)
                {
                    if (drImporteFile["ID_MONEDA"]!=null)
                    {
                        if (drImporteFile["ID_MONEDA"].ToString() == "USD")
                        {
                            if (drImporteFile["FLAG"].ToString() == "1")
                            {
                                dblImporteSumaUSD += (double)drImporteFile["IMPORTE_TOTAL"];
                            }
                        }
                        else
                        {
                            if (drImporteFile["FLAG"].ToString() == "1")
                            {
                                dblImporteSumaSOL += (double)drImporteFile["IMPORTE_TOTAL"];
                            }
                        }
                    }
                }

                // este dato sirve para devolver en el response
                cliente = drCliente["NOMBRE_CLIENTE"].ToString();


                //monto se devuelve como dato al response
                if (strIdMoneda == "USD")
                {
                    monto = dblImporteSumaUSD;
                }
                else
                {
                    monto = dblImporteSumaSOL;
                }

                //moneda se devuelve como dato al response
                moneda = drCliente["ID_MONEDA"].ToString();

                fileSRV.Cliente = cliente;
                fileSRV.ImporteFact = monto;
                fileSRV.Moneda = moneda;
            }

            return fileSRV;
        }
        #endregion
    }
}
