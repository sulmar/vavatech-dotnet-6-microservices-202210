﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Domain
{
    public interface IMessageService
    {
        void Send(Product product);
    }
}
