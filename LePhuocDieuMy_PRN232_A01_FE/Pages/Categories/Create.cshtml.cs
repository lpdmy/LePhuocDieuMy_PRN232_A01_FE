using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ApiClientHelper _apiHelper;
        [BindProperty]

        public List<CategoryDTO> Categories { get; set; } = new();

        public CreateModel(ApiClientHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        [BindProperty]
        public CategoryDTO NewCategory { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _apiHelper.CreateAuthorizedClient();

            var response = await client.PostAsJsonAsync("Category", NewCategory);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, "Create failed: " + errorContent);
            return Page();
        }

        public async Task OnGetAsync()
        {
            var client = _apiHelper.CreateAuthorizedClient();
            var response = await client.GetAsync("Category");

            if (response.IsSuccessStatusCode)
            {
                Categories = await response.Content.ReadFromJsonAsync<List<CategoryDTO>>();
            }
            else
            {
                Categories = new List<CategoryDTO>(); // đảm bảo không null
            }

            NewCategory = new CategoryDTO(); // đảm bảo không null
        }
    }
}
