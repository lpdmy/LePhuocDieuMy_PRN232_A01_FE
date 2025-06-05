using NguyenThiMaiTrinh_PRN232_A01_FE.DTOs;
using NguyenThiMaiTrinh_PRN232_A01_FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenThiMaiTrinh_PRN232_A01_FE.Pages.Tags
{
    public class IndexModel : PageModel
    {
        private readonly ApiClientHelper _api;
        public List<TagDTO> Tags { get; set; } = new();

        public IndexModel(ApiClientHelper api)
        {
            _api = api;
        }

        public async Task OnGetAsync()
        {
            var client = _api.CreateAuthorizedClient();
            Tags = await client.GetFromJsonAsync<List<TagDTO>>("Tag") ?? new();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _api.CreateAuthorizedClient();
            var response = await client.DeleteAsync($"Tag/{id}");
            if (!response.IsSuccessStatusCode)
            {
                // B?n c� th? log l?i ho?c hi?n th? th�ng b�o th?t b?i t?i ��y
            }

            return RedirectToPage(); // Refresh l?i trang
        }
    }
}
