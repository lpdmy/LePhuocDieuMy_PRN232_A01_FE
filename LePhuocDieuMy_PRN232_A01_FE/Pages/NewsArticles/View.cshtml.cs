using System.Net.Http.Json;
using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.NewsArticles
{
    public class ViewModel : PageModel
    {
        private readonly ApiClientHelper _clientFactory;

        public ViewModel(ApiClientHelper clientFactory)
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

    }
}
