using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.Accounts
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        [BindProperty]
        public AccountCreateRequest NewAccount { get; set; } = new();
        private readonly ApiClientHelper _apiHelper;

        public CreateModel(IHttpClientFactory clientFactory, ApiClientHelper apiHelper)
        {
            _clientFactory = clientFactory;
            _apiHelper = apiHelper;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _apiHelper.CreateAuthorizedClient();

            var response = await client.PostAsJsonAsync("Account", NewAccount);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ViewData["Error"] = "Duplicate Email";
                return Page();
            }
        }
    }
}
