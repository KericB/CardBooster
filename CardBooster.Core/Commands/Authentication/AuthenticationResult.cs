﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Core.Commands.Authentication
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; } 
        public string? RefreshToken { get; set; } 
    }
}
