using NguyenThiMaiTrinh_PRN232_A01_FE.DTOs;
using NguyenThiMaiTrinh_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenThiMaiTrinh_PRN232_A01_FE.Pages.Accounts
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApiClientHelper _apiHelper;

        [BindProperty]
        public SystemAccountDto Account { get; set; } = new();

        public EditModel(IHttpClientFactory clientFactory, ApiClientHelper apiHelper)
        {
            _clientFactory = clientFactory;
            _apiHelper = apiHelper;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _apiHelper.CreateAuthorizedClient();

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
                return RedirectToPage("Index");
            }
            var errorContent = await response.Content.ReadAsStringAsync();
            ViewData["Error"] = "Updated failed: Duplicate email";
            return Page();
        }
    }
}
