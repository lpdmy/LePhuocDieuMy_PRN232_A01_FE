using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.NewsArticles
{
    public class HistoryModel : PageModel
    {
        private readonly ApiClientHelper _apiHelper;

        public List<NewsArticleDTO> NewsArticles { get; set; } = new();

        [BindProperty(SupportsGet = true)] public string? SearchKeyword { get; set; }
        [BindProperty(SupportsGet = true)] public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalCount { get; set; }

        public HistoryModel(ApiClientHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task OnGetAsync()
        {
            var client = _apiHelper.CreateODataAuthorizedClient();

            var filters = new List<string>();

            if (!string.IsNullOrEmpty(SearchKeyword))
            {
                var keyword = SearchKeyword.ToLower();
                filters.Add($"contains(tolower(NewsTitle),'{keyword}') or contains(tolower(Headline),'{keyword}')");
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                filters.Add($"CreatedById eq {userId}");
            }

            var filterString = filters.Count > 0 ? $"$filter={string.Join(" and ", filters)}&" : "";

            var skip = (PageNumber - 1) * PageSize;
            var query = $"NewsArticles?$orderby=CreatedDate desc&{filterString}$skip={skip}&$top={PageSize}&$count=true";

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

            return RedirectToPage("Index", new { SearchKeyword, PageNumber });
        }
    }
}
