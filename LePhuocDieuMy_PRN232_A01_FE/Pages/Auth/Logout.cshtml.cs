using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LePhuocDieuMy_PRN232_A01_FE.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnPost()
        {
            HttpContext.Session.Remove("JWToken");
            HttpContext.Session.Remove("UserEmail");
            return RedirectToPage("/Auth/Login");
        }
    }
}
