﻿using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface ISendRepository<T>
    {
        Operation Read();

        Operation Update(T entity);
    }
}
