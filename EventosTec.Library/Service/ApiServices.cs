using EventosTec.Library.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventosTec.Library.Services
{
	public class ApiServices : IApiServices
	{

		public async Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request)
		{
			try
			{
				var requestString = JsonConvert.SerializeObject(request);
				var content = new StringContent(requestString, Encoding.UTF8, "application/json");
				var client = new HttpClient
				{
					BaseAddress = new Uri(urlBase)
				};

				var url = $"{servicePrefix}{controller}";
				var response = await client.PostAsync(url, content);
				var result = await response.Content.ReadAsStringAsync();

				if (!response.IsSuccessStatusCode)
				{
					return new Response()
					{
						IsSuccess = false,
						Message = result,
					};
				}

				var token = JsonConvert.DeserializeObject<TokenResponse>(result);
				return new Response()
				{
					IsSuccess = true,
					Result = token,
				};
			}
			catch (Exception ex)
			{
				return new Response()
				{
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}
	}
}
