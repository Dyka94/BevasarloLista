using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BevasarloLista.Auth
{
    public interface IAuthenticate
    {
        Task<DateTime> Authenticate(ProvidersEnum provider);
        Task<bool> LogoutAsync();
    }
}
