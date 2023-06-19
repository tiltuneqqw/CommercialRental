using CommercialRental.Data;
using CommercialRental.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CommercialRental.Pages
{
    [Authorize]
    [ValidateAntiForgeryToken]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        [BindProperty]
        public Advertisment Advertisment { get; set; } = new Advertisment();
        public bool Error { get; set; }
        [BindProperty]
        public FileUploadModel FileUpload { get; set; }

        public class FileUploadModel
        {
            [Required]
            [Display(Name = "Зображення")]
            public List<IFormFile> Files { get; set; }
        }

        public CreateModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostCreateAdvertisment(List<IFormFile> files)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("model is invalid");
                return RedirectToPage();
            }

            List<Advertisment> advs = LoadArrays();
            if (advs != null && advs.Any(s => s.FullStreetName.Equals(Advertisment.FullStreetName)) &&
                advs.Any(s => s.City.Equals(Advertisment.City)) && advs.Any(s => s.Region.Equals(Advertisment.Region)))
            {
                Error = true;
            }
            else if (Advertisment != null)
            {
                _logger.LogInformation("started adding adv");
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Advertisment.UserId = userId;

                Advertisment.CreationDate = DateTime.Now;

                await _context.Advertisments.AddAsync(Advertisment);
                await _context.SaveChangesAsync();

                if (files != null && files.Count > 0)
                {
                    foreach (var formFile in files)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await formFile.CopyToAsync(memoryStream);

                            var file = new AppFile()
                            {
                                Content = memoryStream.ToArray(),
                                AdvertismentId = Advertisment.Id
                            };

                            await _context.Files.AddAsync(file);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                _logger.LogInformation($"added adv: {Advertisment.Id}");

                return LocalRedirect("/my-list");
            }

            _logger.LogInformation("something went wrong");
            return Page();
        }

        private List<Advertisment> LoadArrays()
        {
            if (_context.Advertisments.Any())
            {
                return _context.Advertisments.Where(i => i.User.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).ToList();
            }

            return new List<Advertisment>();
        }
    }
}
