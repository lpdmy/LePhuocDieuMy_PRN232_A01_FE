using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.NewsArticles
{
    public class UpdateModel : PageModel
    {
        private readonly ApiClientHelper _apiHelper;

        [BindProperty]
        public NewsArticleDTO Article { get; set; } = new();

        public UpdateModel(ApiClientHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _apiHelper.CreateAuthorizedClient();
            var response = await client.GetAsync($"NewsArticles/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }

            Article = await response.Content.ReadFromJsonAsync<NewsArticleDTO>() ?? new NewsArticleDTO();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _apiHelper.CreateAuthorizedClient();
            var response = await client.PutAsJsonAsync($"NewsArticles/{id}", Article);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Update failed: " + error);
                return Page();
            }
        }
    }
}
