﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class UpdateUserRoleDto
    {
        public string UserId { get; set; } = string.Empty;
        public string NewRole { get; set; } = string.Empty;
    }
}
