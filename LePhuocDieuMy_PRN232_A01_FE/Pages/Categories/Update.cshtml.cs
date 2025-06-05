using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class UpdateModel : PageModel
{
    private readonly ApiClientHelper _apiHelper;

    public UpdateModel(ApiClientHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    [BindProperty]
    public CategoryDTO UpdatedCategory { get; set; } = new();
    public List<CategoryDTO> Categories { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var client = _apiHelper.CreateAuthorizedClient();
        var category = await client.GetFromJsonAsync<CategoryDTO>($"Category/{id}");

        if (category == null) return NotFound();

        UpdatedCategory = new CategoryDTO
        {
            CategoryName = category.CategoryName,
            CategoryDescription = category.CategoryDescription,
            ParentCategoryId = category.ParentCategoryId,
            IsActive = category.IsActive
        };

        var response = await client.GetAsync("Category");

        if (response.IsSuccessStatusCode)
        {
            Categories = await response.Content.ReadFromJsonAsync<List<CategoryDTO>>();
        }
        else
        {
            Categories = new List<CategoryDTO>(); // ð?m b?o không null
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var client = _apiHelper.CreateAuthorizedClient();

        var response = await client.PutAsJsonAsync($"Category/{Id}", UpdatedCategory);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("Index");
        }

        var error = await response.Content.ReadAsStringAsync();
        ModelState.AddModelError(string.Empty, "Update failed: " + error);
        return Page();
    }
}
