using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NguyenThiMaiTrinh_PRN232_A01_FE.Services
{
    public class ApiClientHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClientHelper(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpClient CreateAuthorizedClient()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");

            var client = _httpClientFactory.CreateClient("api");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        public HttpClient CreateODataAuthorizedClient()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");

            var client = _httpClientFactory.CreateClient("odata");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }
    }
}
