using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApiClientHelper _apiHelper;
        public List<CategoryDTO> Categories { get; set; } = new();

        public IndexModel(ApiClientHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task OnGetAsync()
        {
            var client = _apiHelper.CreateAuthorizedClient();
            var response = await client.GetAsync("Category");

            if (response.IsSuccessStatusCode)
            {
                Categories = await response.Content.ReadFromJsonAsync<List<CategoryDTO>>() ?? new();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _apiHelper.CreateAuthorizedClient();
            var response = await client.DeleteAsync($"Category/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Delete failed: " + error);
            }

            return RedirectToPage();
        }
    }
}