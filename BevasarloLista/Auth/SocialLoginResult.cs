using System;
using System.Collections.Generic;
using System.Text;

namespace BevasarloLista.Auth
{
    public class SocialLoginResult
    {

        public string SocialId
        {
            get { return string.IsNullOrEmpty(Sub) ? Id : Sub; }
        }

        public string Email { get; set; }
        public string Sub { get; set; }
        public string Id { get; set; }
    }
}

