﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
    }
}
