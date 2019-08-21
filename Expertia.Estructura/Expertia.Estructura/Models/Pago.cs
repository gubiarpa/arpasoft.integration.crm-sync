using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class Pago : IUnique, ISalesForce, IAuditable
    {
        #region Properties
        public string ID { get; set; }
        public string IdSalesForce { get; set; }
        #endregion

        #region Auditoria
        public Auditoria Auditoria { get; set; }
        #endregion
    }
}