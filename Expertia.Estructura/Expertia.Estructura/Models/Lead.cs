using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class Lead
    {
        public string IdLeads_SF { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string Titulo_profesional { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Fax { get; set; }
        public string Comentarios_Lead { get; set; }
        public string Nombre_empresa { get; set; }
        public string Cliente_de { get; set; }
        public string Idioma_preferencia { get; set; }
        public bool Cliente_Trabajado { get; set; }
        public string Fuente { get; set; }
        public string Address { get; set; }
        public bool Are_you { get; set; }
        public string Best_Time { get; set; }
        public string Country_Residence { get; set; }
        public string Country_Visit { get; set; }
        public string Departure_City { get; set; }
        public string Departure_Date { get; set; }
        public int Duration_Trip { get; set; }
        public string Group_Private { get; set; }
        public string Nivel_acomodacion { get; set; }
        public float Ideal_Budget { get; set; }
        public string Ubicacion_IP { get; set; }
        public int Numero_Adultos { get; set; }
        public int num_kids0_24m { get; set; }
        public int num_kids2_5y { get; set; }
        public int num_kids6_12 { get; set; }
        public string Referring_URL { get; set; }
        public string Skype { get; set; }
        public string Touchpoint { get; set; }
        public string Tour_Interest { get; set; }
        public string Propietario_oportunidad { get; set; }
        public string Ejecutivo { get; set; }
    }

    public class LeadResponse
    {
        public string MensajeRetorno { get; set; }
        public string CodigoRetorno { get; set; }
        public string IdLeadSf { get; set; }
    }

}