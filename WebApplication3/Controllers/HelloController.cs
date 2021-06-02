using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    [Authorize]
    public class HelloController : Controller
    {
        private readonly ApiClient _apiClient;
        private readonly IConfiguration _configuration;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HelloController(IConfiguration configuration, ITokenAcquisition tokenAcquisition, HttpClient client, IHttpContextAccessor httpContextAccessor)
        {            
            this._configuration = configuration;
            this._tokenAcquisition = tokenAcquisition;
            this._client = client;
            this._httpContextAccessor = httpContextAccessor;

            this._apiClient = new ApiClient(this._tokenAcquisition, this._client, this._configuration, this._httpContextAccessor);
        }

        //[AuthorizeForScopes(Scopes = new[] { "https://kurttesting.onmicrosoft.com/f475343a-be0e-4d21-920e-507dbff744b3/Files.Read" })]
        public async Task<IActionResult> Index()
        {
            ViewData["Content"] = await this._apiClient.GetAsync(this._configuration["AzureAd:ApiScope"], this._configuration["AzureAd:ApiUrl"]);            

            return View();
        }
    }
}
