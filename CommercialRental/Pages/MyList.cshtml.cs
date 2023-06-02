using CommercialRental.Data;
using CommercialRental.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Xml.Linq;

namespace CommercialRental.Pages
{
    [Authorize]
    [ValidateAntiForgeryToken]
    public class MyListModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public List<Advertisment> Advertisments { get; set; }
        public List<RequestRent> Requests { get; set; }
        public int Collapsed { get; set; } = 1;
        public bool CancelAdvVisible { get; set; } = false;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Старий пароль")]
            public string OldPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Новий пароль")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Підтвердіть новий пароль")]
            [Compare("NewPassword", ErrorMessage = "Паролі не співпадають.")]
            public string ConfirmPassword { get; set; }
        }

        public MyListModel(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public void OnGet()
        {
            LoadArrays();
        }

        public void OnPostMyList()
        {
            Collapsed = 1;
            LoadArrays();
        }

        public void OnPostRequests()
        {
            Collapsed = 2;
            LoadArrays();
        }

        public void OnGetRequests()
        {
            Collapsed = 2;
            LoadArrays();
        }

        public void OnPostRented()
        {
            Collapsed = 3;
            LoadArrays(true);
        }

        public void OnPostSettings()
        {
            Collapsed = 4;
            LoadArrays();
        }

        public void OnPostOpenCancel()
        {
            CancelAdvVisible = true;
            LoadArrays();
        }

        public async Task<IActionResult> OnPostRemoveAdvertisment(int id)
        {
            var entity = await _context.Advertisments.FindAsync(id);

            var advs = _context.RequestsRent
                    .Where(a => a.AdvertismentId.Equals(id))
                    .ToList();
            if (entity == null)
            {
                return NotFound();
            }

            _context.RequestsRent.RemoveRange(advs);
            _context.Advertisments.Remove(entity);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public ActionResult OnPostCloseCancel()
        {
            CancelAdvVisible = false;
            LoadArrays();

            return RedirectToPage();
        }

        private void LoadArrays(bool showRented = false)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (_context.Advertisments.Any())
                {
                    Advertisments = _context.Advertisments.Include(u => u.User).Where(i => i.User.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).ToList();
                }
                if (_context.RequestsRent.Any())
                {
                    if (showRented)
                    {
                        Requests = _context.RequestsRent
                            .Include(a => a.RequestAdvertisment)
                            .ThenInclude(u => u.User)
                            .Include(u => u.User)
                            .Where(a => a.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                            .Where(i => i.RequestAdvertisment.IsRented.Equals(showRented))
                            .ToList();
                    }
                    else
                    {
                        Requests = _context.RequestsRent
                            .Include(a => a.RequestAdvertisment)
                            .ThenInclude(u => u.User)
                            .Include(u => u.User)
                            .Where(a => a.RequestAdvertisment.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                            .Where(i => i.RequestAdvertisment.IsRented.Equals(showRented))
                            .ToList();
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostCancelRent(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                var adv = await _context.Advertisments.FindAsync(id);
                var req = await _context.RequestsRent.FirstOrDefaultAsync(i => i.AdvertismentId.Equals(id));

                adv.StartRentDate = DateTime.MinValue;
                adv.IsRented = false;

                _context.RequestsRent.Remove(req);
                await _context.SaveChangesAsync();

                Collapsed = 1;
                LoadArrays();

                return RedirectToPage();
            }
            else
            {
                return LocalRedirect("/login");
            }
        }

        public async Task<IActionResult> OnPostRent(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                var adv = await _context.Advertisments.Where(i => i.Id.Equals(id)).FirstOrDefaultAsync();
                adv.IsRented = true;
                adv.StartRentDate = DateTime.Now;

                await _context.SaveChangesAsync();

                Collapsed = 2;
                LoadArrays();
                return RedirectToPage();
            }
            else
            {
                return LocalRedirect("/login");
            }
        }

        public async Task<IActionResult> OnPostDenyRent(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                var adv = await _context.RequestsRent.Where(i => i.Id.Equals(id)).FirstOrDefaultAsync();
                _context.RequestsRent.Remove(adv);

                await _context.SaveChangesAsync();

                Collapsed = 2;
                LoadArrays();
                return Page();
            }
            else
            {
                return LocalRedirect("/login");
            }
        }

        public async Task<IActionResult> OnPostSettingsPassword()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            Console.WriteLine(Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);

            return RedirectToPage();
        }
    }
}
