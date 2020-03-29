﻿using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IChatterNMRepository
    {
        Operation Send(UnidadNegocioKeys? unidadNegocio);
        Operation Update(ChatterNM chatterNM);
    }
}
