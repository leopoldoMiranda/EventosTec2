using EventosTec.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventosTec.Common.Service
{
    public interface IApiServices
    {
        //REALIZADO
        Task<Response> GetTokenAsync(string urlBase,
           string servicePrefix, string controller, TokenRequest request);
    }
}
