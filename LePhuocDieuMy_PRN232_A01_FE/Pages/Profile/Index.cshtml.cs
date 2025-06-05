using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApiClientHelper _apiHelper;

        [BindProperty]
        public SystemAccountDto Account { get; set; } = new();

        public IndexModel(IHttpClientFactory clientFactory, ApiClientHelper apiHelper)
        {
            _clientFactory = clientFactory;
            _apiHelper = apiHelper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _apiHelper.CreateAuthorizedClient();
            var id = HttpContext.Session.GetInt32("UserId");
            var account = await client.GetFromJsonAsync<SystemAccountDto>($"Account/{id}");

            if (account == null)
            {
                return NotFound();
            }

            Account = account;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _apiHelper.CreateAuthorizedClient();

            var response = await client.PutAsJsonAsync("Account", Account);

            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("UserEmail", Account.AccountName);
                return RedirectToPage("/Index");
            }
            var errorContent = await response.Content.ReadAsStringAsync();
            ViewData["Error"] = "Updated failed: Duplicate email";
            return Page();
        }
    }
}
