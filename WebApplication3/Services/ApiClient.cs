using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WebApplication3.Services
{
    public class ApiClient
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClient(ITokenAcquisition tokenAcquisition, HttpClient client, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
        {
            this._tokenAcquisition = tokenAcquisition;
            this._client = client;
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetAsync(string scope, string apiUrl)
        {
            List<string> scopes = new List<string> { scope };

            string accessToken = await this._tokenAcquisition.GetAccessTokenForUserAsync(scopes);

            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await this._client.GetStringAsync(apiUrl);

        }
    }
}
