using CommercialRental.Data;
using CommercialRental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Web.Helpers;

namespace CommercialRental.Pages
{
    [AllowAnonymous]
    public class SearchModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public List<Advertisment> Advertisments { get; set; }
        public Advertisment Advertisment { get; set; }
        public bool ShowContacts { get; set; }
        public User SlideOwner { get; set; }
        public int? SlideId { get; set; }
        [BindProperty]
        public int? GoToSlideId { get; set; }
        public List<AppFile> Images { get; set; }
        [BindProperty]
        public FilterModel Filter { get; set; }
        public SearchModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public class FilterModel
        {
            public string Region { get; set; }
            public string City { get; set; }
            public bool Sell { get; set; }
            public bool LongTerm { get; set; }
            public bool Daily { get; set; }
            public bool NewBulding { get; set; }
            public bool Secondary { get; set; }
            public bool Commercial { get; set; }
            public bool Special { get; set; }
            public bool Office { get; set; }
            public bool FromOwner { get; set; }
            public bool FromAgent { get; set; }
            public bool FromBuilder { get; set; }
            public int SquareFrom { get; set; } = 0;
            public int SquareUpTo { get; set; } = int.MaxValue;
            public bool HasFurniture { get; set; }
            public bool VideoChecked { get; set; }
            public bool ExpertChecked { get; set; }
        }

        public void OnGet()
        {
            if (SlideId == null)
            {
                SlideId = WebCache.Get("id");
                if (SlideId == null)
                {
                    SlideId = 0;
                    WebCache.Set("id", SlideId);
                }
            }
            if (Filter == null)
            {
                Filter = WebCache.Get("filter");
                if (Filter == null)
                {
                    Filter = new FilterModel();
                    WebCache.Set("filter", Filter);
                }
            }
            if (GoToSlideId == null)
            {
                GoToSlideId = WebCache.Get("gotoid");
                if (GoToSlideId != null && GoToSlideId != 0)
                {
                    SlideId = 0;
                    WebCache.Set("id", SlideId);
                }
            }

            OnPageLoad();
            LoadSlide(SlideId.Value);
        }

        private void OnPageLoad()
        {
            if (_context.Advertisments.Any())
            {
                var reqsId = _context.RequestsRent
                    .Include(u => u.User)
                    .Where(u => u.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                    .Select(r => r.AdvertismentId)
                    .ToList();

                var advs = _context.Advertisments
                    .Include(u => u.User)
                    .Where(u => !u.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                    .Where(a => !reqsId.Contains(a.Id))
                    .ToList();

                if (GoToSlideId == null || GoToSlideId == 0)
                {
                    if (!string.IsNullOrEmpty(Filter.Region))
                    {
                        advs.RemoveAll(r => !r.Region.Equals(Filter.Region));
                    }
                    if (!string.IsNullOrEmpty(Filter.City))
                    {
                        advs.RemoveAll(r => !r.City.Equals(Filter.City));
                    }
                    if (Filter.Sell || Filter.Daily || Filter.LongTerm)
                    {
                        if (!Filter.Sell) advs.RemoveAll(i => i.OperationType.Equals("Продати"));
                        if (!Filter.LongTerm) advs.RemoveAll(i => i.OperationType.Equals("Довгостроково"));
                        if (!Filter.Daily) advs.RemoveAll(i => i.OperationType.Equals("Подобово"));
                    }
                    if (Filter.NewBulding || Filter.Secondary)
                    {
                        if (!Filter.NewBulding) advs.RemoveAll(i => i.PropertyType.Equals("Новобудова"));
                        if (!Filter.Secondary) advs.RemoveAll(i => i.PropertyType.Equals("Вторинна"));
                    }
                    if (Filter.Special || Filter.Office || Filter.Commercial)
                    {
                        if (!Filter.Special) advs.RemoveAll(i => i.BuildingType.Equals("Спеціальне"));
                        if (!Filter.Office) advs.RemoveAll(i => i.BuildingType.Equals("Офісне"));
                        if (!Filter.Commercial) advs.RemoveAll(i => i.BuildingType.Equals("Комерційне"));
                    }
                    if (Filter.FromAgent || Filter.FromOwner || Filter.FromBuilder)
                    {
                        if (!Filter.FromAgent) advs.RemoveAll(i => i.OfferType.Equals("Від посередника"));
                        if (!Filter.FromOwner) advs.RemoveAll(i => i.OfferType.Equals("Від власника"));
                        if (!Filter.FromBuilder) advs.RemoveAll(i => i.OfferType.Equals("Від забудовника"));
                    }
                    if (Filter.SquareFrom > 0 || Filter.SquareUpTo < int.MaxValue)
                    {
                        if (Filter.SquareFrom > 0) advs.RemoveAll(i => i.Square < Filter.SquareFrom);
                        if (Filter.SquareUpTo < int.MaxValue) advs.RemoveAll(i => i.Square > Filter.SquareUpTo);
                    }
                    if (Filter.HasFurniture) advs.RemoveAll(i => string.IsNullOrEmpty(i.Furniture));
                    if (Filter.VideoChecked || Filter.ExpertChecked)
                    {
                        if (!Filter.VideoChecked) advs.RemoveAll(i => i.Checked.Equals("Перевірено по відеозвінку"));
                        if (!Filter.ExpertChecked) advs.RemoveAll(i => i.Checked.Equals("Перевірено експертом"));
                    }

                    Advertisments = advs;

                    if (Advertisments.Any())
                    {
                        Advertisment = Advertisments.ElementAt(SlideId.Value);
                        SlideOwner = _context.Advertisments.Include(a => a.User).FirstOrDefault(a => a.Id == Advertisment.Id)?.User;

                        if (_context.Files.Any(i => i.AdvertismentId.Equals(Advertisment.Id)))
                        {
                            Images = _context.Files.Where(i => i.AdvertismentId.Equals(Advertisment.Id)).ToList();
                        }
                    }
                }
                else
                {
                    Advertisments = advs.Where(i => i.Id.Equals(GoToSlideId)).ToList();
                    if (Advertisments.Any())
                    {
                        Advertisment = Advertisments.ElementAt(0);
                        SlideOwner = _context.Advertisments.Include(a => a.User).FirstOrDefault(a => a.Id == Advertisment.Id)?.User;

                        if (_context.Files.Any(i => i.AdvertismentId.Equals(Advertisment.Id)))
                        {
                            Images = _context.Files.Where(i => i.AdvertismentId.Equals(Advertisment.Id)).ToList();
                        }
                    }

                    if (Advertisment == null)
                    {
                        GoToSlideId = 0;
                        SlideId = 0;
                        WebCache.Set("id", 0);
                        WebCache.Set("gotoid", 0);

                        OnPageLoad();
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostNextSlide()
        {
            SlideId = WebCache.Get("id");
            SlideId++;
            if (SlideId == null)
            {
                SlideId = 0;
            }
            WebCache.Set("id", SlideId);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostPrevSlide()
        {
            SlideId = WebCache.Get("id");
            SlideId--;
            if (SlideId == null || SlideId < 0)
            {
                SlideId = 0;
            }
            WebCache.Set("id", SlideId);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostGoToAdvice()
        {
            if (GoToSlideId == null)
            {
                return BadRequest();
            }
            else
            {
                WebCache.Set("gotoid", GoToSlideId);
            }


            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostClearGoTo()
        {
            GoToSlideId = 0;
            WebCache.Set("gotoid", GoToSlideId);

            return RedirectToPage();
        }

        private bool LoadSlide(int slideId)
        {
            if (Advertisments != null && Advertisments.Count > 0 && slideId < Advertisments.Count && slideId >= 0)
            {
                Advertisment = Advertisments.ElementAt(slideId);
                SlideOwner = _context.Advertisments.Include(a => a.User).Where(b => !b.IsRented).FirstOrDefault(a => a.Id == Advertisment.Id)?.User;

                return true;
            }

            return false;
        }

        public async Task<IActionResult> OnPostClearFilter()
        {
            Filter = new FilterModel();
            WebCache.Set("filter", Filter);
            WebCache.Set("id", 0);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostApplyFilter()
        {
            WebCache.Set("filter", Filter);
            WebCache.Set("id", 0);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                var reqs = await _context.RequestsRent
                    .Where(i => i.AdvertismentId.Equals(id))
                    .Include(u => u.User)
                    .FirstOrDefaultAsync(u => u.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)));

                if (reqs != null)
                {
                    return RedirectToPage();
                }

                RequestRent request = new RequestRent();

                request.AdvertismentId = id;
                request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                request.RequestDate = DateTime.Now;

                await _context.RequestsRent.AddAsync(request);
                await _context.SaveChangesAsync();

                return LocalRedirect("/my-list");
            }
            else
            {
                return LocalRedirect("/login");
            }
        }
    }
}