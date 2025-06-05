using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
public class IndexModel : PageModel
{
    public string? UserEmail { get; private set; }
    public string? Role { get; private set; }
    public string? UserId { get; private set; }



    public void OnGet()
    {
        var token = HttpContext.Session.GetString("JWToken");
        if (!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var emailClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Name || c.Type == "email");

            var roleClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Role || c.Type == "role");

            var idClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier);

            UserEmail = emailClaim?.Value;

            Role = roleClaim?.Value;

            UserId = idClaim?.Value;

            if (string.IsNullOrEmpty(UserEmail))
            {
                UserEmail = "Admin";
            }

            // Lưu email vào Session
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail"))) {
                HttpContext.Session.SetString("UserEmail", UserEmail);
            }
            if (UserEmail != "Admin")
            {
                HttpContext.Session.SetInt32("UserId", UserId != null ? Convert.ToInt32(UserId) : 0);
            }

            ViewData[nameof(UserEmail)] = UserEmail;
            ViewData[nameof(Role)] = Role;

        }
    }

}
