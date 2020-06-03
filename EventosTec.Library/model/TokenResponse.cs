﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EventosTec.Library.model
{
    class TokenResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime ExpirationLocal => Expiration.ToLocalTime();
    }
}