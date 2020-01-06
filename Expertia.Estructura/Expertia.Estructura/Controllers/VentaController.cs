using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.Condor;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.Ventas)]
    public class VentaController : BaseController<object>
    {
        #region Properties
        private IVentaCT _ventaCTRepository;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Read)]
        public IHttpActionResult Read(VentasRequest ventasRequest)
        {
            var error = string.Empty;
            try
            {
                RepositoryByBusiness(ventasRequest.Region.ToUnidadNegocioByCountry());
                var ventasOperation = _ventaCTRepository.GetVentasCT(ventasRequest);
                var ventas = new VentasResponse()
                {
                    CodigoRetorno = ventasOperation[OutParameter.CodigoError].ToString(),
                    MensajeRetorno = ventasOperation[OutParameter.MensajeError].ToString(),
                    VentaRowList = (IEnumerable<VentasRow>)ventasOperation[OutParameter.CursorVentas]
                };
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    Error = error,
                    Body = ventasRequest
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliar
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            _ventaCTRepository = new Venta_CT_Repository(unidadNegocioKey);
            return unidadNegocioKey;
        }
        #endregion
    }
}
