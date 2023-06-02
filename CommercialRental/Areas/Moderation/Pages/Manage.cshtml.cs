using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.InteropServices;

namespace CommercialRental.Areas.Moderation
{
    [Authorize(Roles = "Admin")]
    public class ManageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
