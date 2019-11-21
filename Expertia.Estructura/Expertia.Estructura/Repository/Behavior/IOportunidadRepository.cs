using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expertia.Estructura.Repository.Behavior
{
    interface IOportunidadRepository
    {
        Operation GetOportunidades();
        Operation Update(Oportunidad oportunidad);
    }
}
