using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.NewsArticles
{
    public class IndexModel : PageModel
    {
        private readonly ApiClientHelper _apiHelper;
        public List<NewsArticleDTO> NewsArticles { get; set; } = new();

        public IndexModel(ApiClientHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task OnGetAsync()
        {
            var client = _apiHelper.CreateAuthorizedClient();
            var response = await client.GetAsync("NewsArticles");

            if (response.IsSuccessStatusCode)
            {
                NewsArticles = await response.Content.ReadFromJsonAsync<List<NewsArticleDTO>>() ?? new();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _apiHelper.CreateAuthorizedClient();
            var response = await client.DeleteAsync($"NewsArticles/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Delete failed: " + error);
            }

            return RedirectToPage();
        }
    }
}
