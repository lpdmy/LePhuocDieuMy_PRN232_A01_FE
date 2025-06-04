using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.Accounts
{

    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApiClientHelper _apiHelper;
        [BindProperty]
        public SystemAccountDto Account { get; set; } = new();
        private ApiClientHelper _apiClientHelper { get; set; }
        public DeleteModel(IHttpClientFactory clientFactory, ApiClientHelper apiClientHelper)
        {
            _clientFactory = clientFactory;
            _apiClientHelper = apiClientHelper;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _apiClientHelper.CreateAuthorizedClient();

            var account = await client.GetFromJsonAsync<SystemAccountDto>($"Account/{id}");

            if (account == null)
            {
                return NotFound();
            }

            Account = account;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _apiClientHelper.CreateAuthorizedClient();

            var account = await client.GetFromJsonAsync<SystemAccountDto>($"Account/{id}");

            var response = await client.DeleteAsync($"Account/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Delete failed");
            return Page();
        }
    }
}