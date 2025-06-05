using System.Net.Http.Json;
using NguyenThiMaiTrinh_PRN232_A01_FE.DTOs;
using NguyenThiMaiTrinh_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NguyenThiMaiTrinh_PRN232_A01_FE.Pages.NewsArticles
{
    public class UpdateModel : PageModel
    {
        private readonly ApiClientHelper _clientFactory;

        public UpdateModel(ApiClientHelper clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public NewsArticleDTO UpdatedArticle { get; set; } = new();

        public List<CategoryDTO> Categories { get; set; } = new();
        public MultiSelectList TagSelectList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _clientFactory.CreateAuthorizedClient();

            var article = await client.GetFromJsonAsync<NewsArticleDTO>($"NewsArticles/{id}");
            if (article == null) return NotFound();

            UpdatedArticle = article;
            article.NewsArticleId = id;

            // Load categories
            Categories = await client.GetFromJsonAsync<List<CategoryDTO>>("Category");

            // Load all tags
            var tags = await client.GetFromJsonAsync<List<TagDTO>>("Tag");
            TagSelectList = new MultiSelectList(tags, "TagId", "TagName", UpdatedArticle.TagIds);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var client = _clientFactory.CreateAuthorizedClient();
            UpdatedArticle.AccountId = (int)HttpContext.Session.GetInt32("UserId");

            var response = await client.PutAsJsonAsync($"NewsArticles/{UpdatedArticle.NewsArticleId}", UpdatedArticle);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }

            // Reload dropdowns if update failed
            Categories = await client.GetFromJsonAsync<List<CategoryDTO>>("Category");
            var tags = await client.GetFromJsonAsync<List<TagDTO>>("Tag");
            TagSelectList = new MultiSelectList(tags, "TagId", "TagName", UpdatedArticle.TagIds);

            ModelState.AddModelError(string.Empty, "Update failed.");
            return Page();
        }
    }
}
