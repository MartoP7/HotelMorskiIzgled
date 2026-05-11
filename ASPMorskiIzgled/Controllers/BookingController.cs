using ASPMorskiIzgled.Data;
using ASPMorskiIzgled.Models;
using ASPMorskiIzgled.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPMorskiIzgled.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new RoomSearchView
            {
                DateIn = DateTime.Today,
                DateOut = DateTime.Today.AddDays(1),
                Adults = 1,
                Children = 0,
                Babies = 0
            };

            model.AvailableRooms = _context.Rooms.Include(r => r.RoomTypes).ToList();
            return View(model);
            //var applicationDbContext = _context.Rooms.Include(r => r.RoomTypes);
            //return View(await applicationDbContext.ToListAsync());
            
        }

        [HttpPost]
        public async Task<IActionResult> Index(RoomSearchView model)
        {
            if (model.DateIn < DateTime.Today)
            {
                ModelState.AddModelError("DateIn", "Началната дата не може да е в миналото.");
            }

            if (model.DateOut <= model.DateIn)
            {
                ModelState.AddModelError("DateOut", "Крайната дата трябва да е след началната.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int totalGuests = model.Adults + model.Children;

            var roomsQuery = _context.Rooms
                .Include(r => r.RoomTypes)
                .AsQueryable();

            // Филтър по тип стая според името на RoomType
            roomsQuery = roomsQuery.Where(r =>
                (r.RoomTypes.Name == "Единична стая" && totalGuests <= 1) ||
                (r.RoomTypes.Name == "Двойна стая" && totalGuests <= 2) ||
                (r.RoomTypes.Name == "Студио" && totalGuests <= 3) ||
                (r.RoomTypes.Name == "Апартамент" && totalGuests <= 4)
            );

            // Ако има бебета -> стаята трябва да има кошара
            if (model.Babies > 0)
            {
                roomsQuery = roomsQuery.Where(r => r.SleepingCot);
            }

            // Свободни стаи за периода
            roomsQuery = roomsQuery.Where(r => !_context.Reservations.Any(res =>
                res.RoomId == r.Id &&
                res.DateIn < model.DateOut &&
                res.DateOut > model.DateIn
            ));

            model.AvailableRooms = await roomsQuery.ToListAsync();

            return View(model);
        }
    }
}