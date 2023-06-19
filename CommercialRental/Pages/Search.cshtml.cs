using CommercialRental.Data;
using CommercialRental.Data.Models;
using ContosoUniversity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
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
        [BindProperty]
        public Advertisment Advertisment { get; set; }
        public int SlideId { get; set; }
        [BindProperty]
        public int GoToSlideId { get; set; }
        public List<AppFile> Images { get; set; }
        [BindProperty]
        public FilterModel Filter { get; set; }
        public bool IsLast { get; set; }
        public bool IsFirst { get; set; }
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

        public void OnGet(int addPage = 0)
        {
            var slideId = WebCache.Get("id");
            if (slideId == null)
            {
                SlideId = 0;
                WebCache.Set("id", SlideId);
            }
            else
            {
                SlideId = slideId + addPage;
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

            var gotoid = WebCache.Get("gotoid");
            if (gotoid == null)
            {
                GoToSlideId = 0;
                WebCache.Set("gotoid", GoToSlideId);
            }
            else
            {
                GoToSlideId = gotoid;
            }

            OnPageLoad();
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

                if (SlideId < 0)
                {
                    SlideId = 0;
                }
                if (SlideId >= advs.Count)
                {
                    SlideId = advs.Count - 1;
                }

                if (GoToSlideId == 0)
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
                        Advertisment = Advertisments.ElementAt(SlideId);

                        if (_context.Files.Any(i => i.AdvertismentId.Equals(Advertisment.Id)))
                        {
                            Images = _context.Files.Where(i => i.AdvertismentId.Equals(Advertisment.Id)).ToList();
                        }
                    }

                    if (SlideId == 0)
                    {
                        IsFirst = true;
                    }
                    if (SlideId == advs.Count - 1)
                    {
                        IsLast = true;
                    }
                }
                else
                {
                    Advertisment = advs.FirstOrDefault(i => i.Id.Equals(GoToSlideId));
                    if (Advertisment != null && _context.Files.Any(i => i.AdvertismentId.Equals(Advertisment.Id)))
                    {
                        Images = _context.Files.Where(i => i.AdvertismentId.Equals(Advertisment.Id)).ToList();
                    }

                    IsLast = true;
                    IsFirst = true;
                }
            }
        }

        public IActionResult OnPostNextSlide()
        {
            var slideId = WebCache.Get("id");
            if (slideId == null)
            {
                SlideId = 0;
            }
            else
            {
                SlideId = slideId + 1;
            }
            WebCache.Set("id", SlideId);

            return RedirectToPage();
        }

        public IActionResult OnPostPrevSlide()
        {
            var slideId = WebCache.Get("id");
            if (slideId == null)
            {
                SlideId = 0;
            }
            else
            {
                SlideId = slideId - 1;
            }
            WebCache.Set("id", SlideId);

            return RedirectToPage();
        }

        public IActionResult OnPostGoToAdvice()
        {
            WebCache.Set("gotoid", GoToSlideId);

            return RedirectToPage();
        }

        public IActionResult OnPostClearGoTo()
        {
            WebCache.Set("gotoid", 0);
            WebCache.Set("id", 0);

            return RedirectToPage();
        }

        public IActionResult OnPostClearFilter()
        {
            Filter = new FilterModel();
            WebCache.Set("filter", Filter);
            WebCache.Set("id", 0);
            WebCache.Set("gotoid", 0);

            return RedirectToPage();
        }

        public IActionResult OnPostApplyFilter()
        {
            WebCache.Set("filter", Filter);
            WebCache.Set("id", 0);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRent(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false && !User.IsInRole("Admin"))
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