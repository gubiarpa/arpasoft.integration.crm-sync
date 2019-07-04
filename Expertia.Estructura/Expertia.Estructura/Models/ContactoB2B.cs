using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class ContactoB2B : Cuenta
    {
        public string RazonSocial { get; set; } // Razón Social
        public string Alias { get; set; } // Alias
        public string CondicionPago { get; set; } // Condición de Pago
        public List<string> TipoMonedaDeLineaCredito { get; set; } // Tipo de moneda de Linea de credito
        public string MontoLineaCredito { get; set; } // Monto de Línea de Crédito
        public string FechaMaximaPagoLDC { get; set; } // Fecha Máxima de Pago de LDC
    }
}