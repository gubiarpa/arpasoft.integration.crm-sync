using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class RptaPagoSafetyPay
    {
        public int IdPedido { get; set; }
        public string ResponseDateTime { get; set; }
        public string ErrorManager_Description { get; set; }
        public string ErrorManager_ErrorNumber { get; set; }
        public string ErrorManager_Severity { get; set; }
        public string OperationId { get; set; }
        public string TransaccionIdentifier { get; set; }
        public string ExpirationDateTime { get; set; }
        public string Signature { get; set; }
        public string BankRedirectUrl { get; set; }
        public List<AmountType> lstAmountType { get; set; }
        public List<PaymentLocationType> lstPaymentLocationType { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public string IdPedidoEncriptado { get; set; }
    }

    public class PaymentLocationType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public PaymentInstructionType[] PaymentInstructions { get; set; }
        public PaymentStepType[] PaymentSteps { get; set; }
        public List<PaymentStepType> lstPaymentStepType { get; set; }
    }

    public class PaymentInstructionType
    {
        public string Name { get; set; }
        public string Value { get; set; }     
    }

    public class PaymentStepType
    {
        public short Step { get; set; }
        public bool StepSpecified { get; set; }
        public string Value { get; set; }
    }

    public class AmountType
    {
        public string CurrencyID { get; set; }        
        public decimal Value { get; set; }
    }

    public enum CurrencyEnumType
    {
        AFN,
        ALL,
        AMD,
        ANG,
        AOA,
        ARS,
        AUD,
        AWG,
        AZN,
        BAM,
        BBD,
        BDT,
        BGN,
        BHD,
        BIF,
        BMD,
        BND,
        BOB,
        BOV,
        BRL,
        BSD,
        BTN,
        BWP,
        BYR,
        BZD,
        CAD,
        CDF,
        CHE,
        CHF,
        CHW,
        CLF,
        CLP,
        CNY,
        COP,
        COU,
        CRC,
        CUC,
        CUP,
        CVE,
        CZK,
        DJF,
        DKK,
        DOP,
        DZD,
        ECS,
        ECV,
        EGP,
        ERN,
        ETB,
        EUR,
        FJD,
        FKP,
        GBP,
        GEL,
        GHS,
        GIP,
        GMD,
        GNF,
        GTQ,
        GYD,
        HKD,
        HNL,
        HRK,
        HTG,
        HUF,
        IDR,
        ILS,
        INR,
        IQD,
        IRR,
        ISK,
        JMD,
        JOD,
        JPY,
        KES,
        KGS,
        KHR,
        KMF,
        KPW,
        KRW,
        KWD,
        KYD,
        KZT,
        LAK,
        LBP,
        LKR,
        LRD,
        LSL,
        LTL,
        LVL,
        LYD,
        MAD,
        MDL,
        MGA,
        MKD,
        MMK,
        MNT,
        MOP,
        MRO,
        MUR,
        MVR,
        MWK,
        MXN,
        MXV,
        MYR,
        MZN,
        NAD,
        NGN,
        NIO,
        NOK,
        NPR,
        NZD,
        OMR,
        PAB,
        PEN,
        PGK,
        PHP,
        PKR,
        PLN,
        PYG,
        QAR,
        RON,
        RSD,
        RUB,
        RWF,
        SAR,
        SBD,
        SCR,
        SDG,
        SEK,
        SGD,
        SHP,
        SLL,
        SOS,
        SRD,
        SSP,
        STD,
        SYP,
        SZL,
        THB,
        TJS,
        TMT,
        TND,
        TOP,
        TRY,
        TTD,
        TWD,
        TZS,
        UAH,
        UGX,
        USD,
        USN,
        USS,
        UYI,
        UYU,
        UZS,
        VEF,
        VND,
        VUV,
        WST,
        XAF,
        XBA,
        XBB,
        XBC,
        XBD,
        XCD,
        XDR,
        XFU,
        XOF,
        XPF,
        YER,
        ZAR,
        ZMW,
        ZWL
    }
}