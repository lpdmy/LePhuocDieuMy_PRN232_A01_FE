using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.Admin
{
    public class ReportModel : PageModel
    {
        private readonly ApiClientHelper _apiHelper;

        public List<NewsArticleDTO> NewsArticles { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchKeyword { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public int PageSize { get; set; } = 5;

        public int TotalCount { get; set; }

        public ReportModel(ApiClientHelper apiHelper)
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

            if (StartDate != null)
            {
                filters.Add($"CreatedDate ge {StartDate.Value.ToString("yyyy-MM-ddTHH:mm:ssZ")}");
            }
            if (EndDate != null)
            {
                filters.Add($"CreatedDate le {EndDate.Value.ToString("yyyy-MM-ddTHH:mm:ssZ")}");
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
    }
}
