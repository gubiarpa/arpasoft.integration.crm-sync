using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.NuevoMundo;
using Expertia.Estructura.Repository.AppWebs;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.General;
using Expertia.Estructura.Repository.NuevoMundo;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    /// CRMEC007 - Envío de boletos y/o documentos de emisión
    [RoutePrefix(RoutePrefix.DocumentosEmisionNM)]
    public class DocumentosEmisionNMController : BaseController
    {
        #region Properties
        private IOportunidadVentaNMRepository _oportunidadVentaNMRepository;
        private ICotizacionSRV_Repository _cotizacionSRV_Repository;        
        private DatosUsuario _datosUsuario;
        #endregion

        protected override ControllerName _controllerName => ControllerName.DocumentoEmisionNM;

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(DocumentosEmisionNM DocumentosEmision)
        {
            DocEmisionRS _DocEmisionRS = new DocEmisionRS();
            string exceptionMsg = string.Empty;

            try
            {
                UsuarioLogin usuarioLogin = null;
                CotizacionVta DtsCotizacionVta = null;

                ValidateNM(DocumentosEmision, ref _DocEmisionRS, ref usuarioLogin, ref DtsCotizacionVta);
                if (string.IsNullOrEmpty(_DocEmisionRS.codigo) == false) return Ok(new { respuesta = _DocEmisionRS });

                /**Llamamos al servicio del SRV**/

                _DocEmisionRS.codigo = "OK";
                _DocEmisionRS.mensaje = "Se envio el correo satisfactoriamente";
                return Ok(new { respuesta = _DocEmisionRS }); ;
            }
            catch (Exception ex)
            {
                exceptionMsg = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    Response = _DocEmisionRS,                                        
                    Exception = exceptionMsg,
                    LegacySystems = DocumentosEmision
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        private void ValidateNM(DocumentosEmisionNM _docsEmisionNM, ref DocEmisionRS _docEmisionRS, ref UsuarioLogin UserLogin, ref CotizacionVta CotizacionVta)
        {
            string mensajeError = string.Empty;

            if (_docsEmisionNM == null)
            {                
                cargarError(ref _docEmisionRS, "Envie correctamente los parametros de entrada - RQ Nulo|");
                return;
            } 
            if (string.IsNullOrEmpty(_docsEmisionNM.idOportunidad_SF))
            {
                mensajeError += "La oportunidad SF es un campo obligatorio|";
            }                                
            if (_docsEmisionNM.IdUsuarioSrv_SF <= 0)
            {
                mensajeError += "El IdUsuarioSrv_SF es un campo obligatorio|";
            }           
            if (_docsEmisionNM.idCotizacionSRV <= 0)
            {
                mensajeError += "El idCotizacionSRV es un campo obligatorio|";
            }
            if (string.IsNullOrEmpty(_docsEmisionNM.correo))
            {
                mensajeError += "El correo es un campo obligatorio|";
            }
            else if (ValidateProcess.validarEmail(_docsEmisionNM.correo) == false)
            {
                mensajeError += "El formato del correo es incorrecto|";
            }            
            
            if (string.IsNullOrEmpty(mensajeError))
            {
                /*Cargamos Datos del Usuario*/
                RepositoryByBusiness(null);
                UserLogin = _datosUsuario.Get_Dts_Usuario_Personal(_docsEmisionNM.IdUsuarioSrv_SF);
                if (UserLogin == null) { mensajeError += "ID del Usuario no registrado|"; }

                /*Validacion Oportunidad*/
                int intCotizacion_SF = _oportunidadVentaNMRepository._Select_CotId_X_OportunidadSF(_docsEmisionNM.idOportunidad_SF);
                if (intCotizacion_SF <= 0) { mensajeError += "No existe la oportunidad en el ambiente de Expertia|"; }
                else if (intCotizacion_SF > 0 && intCotizacion_SF != _docsEmisionNM.idCotizacionSRV) { mensajeError += "La cotizacion enviada es diferente a la registrada|"; }

                if (string.IsNullOrEmpty(mensajeError))
                {
                    CotizacionVta = _cotizacionSRV_Repository.Get_Datos_CotizacionVta((int)_docsEmisionNM.idCotizacionSRV);

                    if (CotizacionVta == null || CotizacionVta.IdCot == 0)
                    {
                        mensajeError = "No existe informacion de la cotizacion enviada";                        
                    }else if(CotizacionVta.IdEstado != (short)ENUM_ESTADOS_COT_VTA.Facturado)
                    {
                        mensajeError = "La cotizacion no se encuentra facturado, no es posible enviar documentos";                        
                    }
                    else if (!(CotizacionVta.IdReservaVuelos != null && CotizacionVta.IdReservaVuelos > 0))
                    {
                        mensajeError = "No se puede enviar ningún documento porque no está asociada a una reserva automática.";                        
                    }                                       
                }
            }

            if (string.IsNullOrEmpty(mensajeError) == false)
            {
                _docEmisionRS.codigo = "ER";
                _docEmisionRS.mensaje = "VA: " + mensajeError;
            }
        }

        private void cargarError(ref DocEmisionRS _docEmisionRS, string errorText)
        {
            _docEmisionRS.codigo = "ER";
            _docEmisionRS.mensaje = "VA: " + errorText;
        }


        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _datosUsuario = new DatosUsuario();
            _cotizacionSRV_Repository = new CotizacionSRV_AW_Repository();
            _oportunidadVentaNMRepository = new OportunidadVentaNMRepository();
            return unidadNegocioKey;
        }
    }
}
