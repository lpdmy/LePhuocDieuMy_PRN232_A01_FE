using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.NewsArticles
{
    public class CreateModel : PageModel
    {
        private readonly ApiClientHelper _api;
        [BindProperty]
        public NewsArticleDTO NewArticle { get; set; } = new();
        public List<CategoryDTO> Categories { get; set; } = new();
        public MultiSelectList TagSelectList { get; set; }
        public CreateModel(ApiClientHelper api)
        {
            _api = api;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var client = _api.CreateAuthorizedClient();
            NewArticle.AccountId = (int) HttpContext.Session.GetInt32("UserId");
            var response = await client.PostAsJsonAsync("NewsArticles", NewArticle);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Create failed");
            var responseCate = await client.GetAsync("Category");

            if (responseCate.IsSuccessStatusCode)
            {
                Categories = await response.Content.ReadFromJsonAsync<List<CategoryDTO>>();
            }
            else
            {
                Categories = new List<CategoryDTO>();
            }
            var tags = await client.GetFromJsonAsync<List<TagDTO>>("Tag");
            TagSelectList = new MultiSelectList(tags, "TagId", "TagName");
            return Page();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _api.CreateAuthorizedClient();
            var response = await client.GetAsync("Category");

            if (response.IsSuccessStatusCode)
            {
                Categories = await response.Content.ReadFromJsonAsync<List<CategoryDTO>>();
            }
            else
            {
                Categories = new List<CategoryDTO>();
            }
            var tags = await client.GetFromJsonAsync<List<TagDTO>>("Tag");
            TagSelectList = new MultiSelectList(tags, "TagId", "TagName");
            return Page();
        }
    }
}
