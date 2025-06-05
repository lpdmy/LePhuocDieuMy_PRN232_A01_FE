using NguyenThiMaiTrinh_PRN232_A01_FE.DTOs;
using NguyenThiMaiTrinh_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenThiMaiTrinh_PRN232_A01_FE.Pages.Tags
{
    public class UpdateModel : PageModel
    {
        private readonly ApiClientHelper _api;

        public UpdateModel(ApiClientHelper api)
        {
            _api = api;
        }

        [BindProperty]
        public TagDTO UpdatedTag { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _api.CreateAuthorizedClient();
            var tag = await client.GetFromJsonAsync<TagDTO>($"Tag/{id}");
            if (tag == null) return NotFound();

            UpdatedTag = tag;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _api.CreateAuthorizedClient();
            var response = await client.PutAsJsonAsync($"Tag/{UpdatedTag.TagId}", UpdatedTag);
            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Update failed.");
            return Page();
        }
    }
}
