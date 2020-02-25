using System;
using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    interface ICotizacionSRV_Repository
    {
        int ProcesosPostCotizacion(Post_SRV RQ_General_PostSRV);
    }
}
