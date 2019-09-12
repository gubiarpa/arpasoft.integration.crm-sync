using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using Expertia.Estructura.Models.Foreign;
using System;

namespace Expertia.Estructura.Models
{
    public class Pago : IUnique, ISalesForce, IAuditable
    {
        #region Properties
        public string ID { get; set; }
        public string IdSalesForce { get; set; }
        public string IDPago { get; set; }
        public string Pasarela { get; set; }
        public string Sitio { get; set; }
        public string TipoGeneracion { get; set; }
        public string CotizacionPnr { get; set; }
        public DateTime? FechaPedido { get; set; }
        public string Estado { get; set; }
        public bool Autorizado { get; set; }
        public DateTime? FechaAutorizacion { get; set; }
        #endregion

        #region Foreignkey
        public FormaPago FormaPago { get; set; }
        #endregion

        #region Auditoria
        public Auditoria Auditoria { get; set; }
        #endregion
    }
}