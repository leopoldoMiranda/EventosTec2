using EventosTec.Library.model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventosTec.Library.Services
{
    public interface IApiServices
    {
        //REALIZADO
        Task<Response> GetTokenAsync(string urlBase,
           string servicePrefix, string controller, TokenRequest request);
    }
}
