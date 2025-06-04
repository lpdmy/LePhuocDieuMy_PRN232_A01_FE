using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApiClientHelper _apiHelper;
        public List<SystemAccountDto> Accounts { get; set; } = new();

        public IndexModel(IHttpClientFactory clientFactory, ApiClientHelper apiHelper)
        {
            _clientFactory = clientFactory;
            _apiHelper = apiHelper;
        }

        public async Task OnGetAsync()
        {
            var client = _apiHelper.CreateAuthorizedClient();
            var accounts = await client.GetFromJsonAsync<List<SystemAccountDto>>("Account");
            if (accounts != null)
                Accounts = accounts;
        }
    }

}
