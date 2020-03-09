using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Retail;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IFileSRVRetailRepository
    {
        Operation GetFilesAsociadosSRV();
        Operation Actualizar_EnvioCotRetail(FilesAsociadosSRVResponse FileAsociadosSRVResponse);

    }
}
