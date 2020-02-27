using Expertia.Estructura.Models.Retail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IDatosOficina
    {
        Oficina ObtieneOficinaXId(int pIntIdOfi);
    }
}