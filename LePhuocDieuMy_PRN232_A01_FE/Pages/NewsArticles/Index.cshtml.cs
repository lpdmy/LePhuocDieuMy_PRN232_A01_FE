using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json.Serialization;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.NewsArticles
{
    public class IndexModel : PageModel
    {
        private readonly ApiClientHelper _apiHelper;

        public List<NewsArticleDTO> NewsArticles { get; set; } = new();
        [BindProperty(SupportsGet = true)] public string? SearchKeyword { get; set; }
        [BindProperty(SupportsGet = true)] public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalCount { get; set; }

        public IndexModel(ApiClientHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task OnGetAsync()
        {
            var client = _apiHelper.CreateODataAuthorizedClient();

            // Xây dựng query string OData
            var filter = !string.IsNullOrEmpty(SearchKeyword)
                ? $"$filter=contains(tolower(NewsTitle),'{SearchKeyword.ToLower()}') or contains(tolower(Headline),'{SearchKeyword.ToLower()}')&"
                : "";

            var skip = (Page - 1) * PageSize;

            var query = $"NewsArticles?$orderby=CreatedDate desc&{filter}$skip={skip}&$top={PageSize}&$count=true";

            var response = await client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<ODataResponse<NewsArticleDTO>>();
                NewsArticles = json?.Value ?? new();
                TotalCount = json?.Count ?? 0;
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

            return RedirectToPage("Index", new { SearchKeyword, Page });
        }

        public class ODataResponse<T>
        {
            [JsonPropertyName("@odata.context")]
            public string Context { get; set; }

            [JsonPropertyName("@odata.count")]
            public int Count { get; set; }

            [JsonPropertyName("value")]
            public List<T> Value { get; set; }
        }
    }

}
