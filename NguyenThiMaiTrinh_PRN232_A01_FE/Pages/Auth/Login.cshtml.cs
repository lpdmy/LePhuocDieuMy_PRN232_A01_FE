using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace NguyenThiMaiTrinh_PRN232_A01_FE.Pages.Auth
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        public string? ErrorMessage { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            var loginRequest = new { Email = Email, Password = Password };

            try
            {
                var response = await client.PostAsJsonAsync("https://localhost:7015/api/auth/login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<LoginResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (result != null && !string.IsNullOrEmpty(result.Token))
                    {
                        HttpContext.Session.SetString("JWToken", result.Token);

                        return RedirectToPage("/Index");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ErrorMessage = "Invalid email or password.";
                }
                else
                {
                    ErrorMessage = "An error occurred. Please try again.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Exception: {ex.Message}";
            }

            return Page();
        }

        private class LoginResponse
        {
            public string Token { get; set; } = string.Empty;
        }
    }
}
