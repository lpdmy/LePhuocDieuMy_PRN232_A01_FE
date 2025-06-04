using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.NewsArticles
{
    public class CreateModel : PageModel
    {
        private readonly ApiClientHelper _apiHelper;

        [BindProperty]
        public NewsArticleDTO NewArticle { get; set; } = new();

        public CreateModel(ApiClientHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _apiHelper.CreateAuthorizedClient();

            var response = await client.PostAsJsonAsync("NewsArticles", NewArticle);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Create failed: " + errorContent);
                return Page();
            }
        }
    }
}
