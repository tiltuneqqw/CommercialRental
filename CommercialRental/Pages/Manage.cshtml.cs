using CommercialRental.Data;
using CommercialRental.Data.Models;
using ContosoUniversity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CommercialRental.Pages
{
    [Authorize(Roles = "Admin")]
    public class ManageModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 16;
        public bool ShowPrevious => CurrentPage > 1;
        public bool ShowNext => CurrentPage < TotalPages;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public string NewChecked = string.Empty;
        public PaginatedList<Advertisment> Advertisments { get; set; }

        ApplicationDbContext _context;
        IConfiguration _configuration;

        public ManageModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task OnGetAsync(int? pageIndex)
        {
            var pageSize = _configuration.GetValue("ModerationPageSize", 15);
            Advertisments = await PaginatedList<Advertisment>.CreateAsync(_context.Advertisments.Include(u => u.User).AsNoTracking(), pageIndex ?? 1, pageSize);
        }

        public async Task<IActionResult> OnPostRemoveAdv(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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

        public async Task<IActionResult> OnPostSaveChecked(int? id, string selectedValue)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adv = await _context.Advertisments.FirstOrDefaultAsync(i => i.Id == id);

            if (adv != null && selectedValue != null)
            {
                adv.Checked = selectedValue;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
