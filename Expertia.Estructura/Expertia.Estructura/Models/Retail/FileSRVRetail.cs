using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models.Retail
{
    public class FileSRVRetail
    {
    }

    public class FilesAsociadosSRV
    {
        public string id_oportunidad { get; set; }
        public int cot_id { get; set; }
        public int suc_id { get; set; }
        public int file_id { get; set; }
        public DateTime fpta_fecha { get; set; }
        public double fpta_imp_fact { get; set; }
    }

    public class FilesAsociadosSRVResponse
    {
        public string mensaje_error { get; set; }
        public string id_oportunidad_sf { get; set; }
        public string codigo_error { get; set; }
    }
}