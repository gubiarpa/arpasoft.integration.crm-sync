using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    /// <summary>
    /// Contacto B2B
    /// </summary>
    public class CuentaB2B : Cuenta
    {
        /// <summary>
        /// Razón Social
        /// </summary>
        public string RazonSocial { get; set; } // Razón Social
        /// <summary>
        /// Alias
        /// </summary>
        public string Alias { get; set; } // Alias
        /// <summary>
        /// Condición de Pago
        /// </summary>
        public string CondicionPago { get; set; } // Condición de Pago
        /// <summary>
        /// Tipo de Moneda
        /// </summary>
        public string TipoMonedaDeLineaCredito { get; set; } // Tipo de moneda de Linea de credito
        /// <summary>
        /// Monto de Línea de Crédito
        /// </summary>
        public decimal MontoLineaCredito { get; set; } // Monto de Línea de Crédito
        /// <summary>
        /// Fecha Máxima de Pago de Línea de Crédito
        /// </summary>
        public DateTime? FechaMaximaPagoLDC { get; set; } // Fecha Máxima de Pago de LDC
    }
}