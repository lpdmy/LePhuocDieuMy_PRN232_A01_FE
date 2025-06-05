using LePhuocDieuMy_PRN232_A01_FE.DTOs;
using LePhuocDieuMy_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.Tags
{
    public class CreateModel : PageModel
    {
        private readonly ApiClientHelper _api;

        public CreateModel(ApiClientHelper api)
        {
            _api = api;
        }

        [BindProperty]
        public TagDTO NewTag { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _api.CreateAuthorizedClient();
            var response = await client.PostAsJsonAsync("Tag", NewTag);
            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Failed to create tag.");
            return Page();
        }
    }
}
