using ASPMorskiIzgled.Data;
using ASPMorskiIzgled.Models;
using ASPMorskiIzgled.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPMorskiIzgled.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rooms.Include(r => r.RoomTypes);
            return View(await applicationDbContext.ToListAsync());
        }


        // INDEX RESERVATION


        [HttpGet]
        public async Task<IActionResult> IndexReservation()
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
        public async Task<IActionResult> IndexReservation(RoomSearchView model)
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










        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.RoomTypes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "Id", "Name");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,RoomTypeId,Description,SleepingCot,SofaBed,Photo,Price")] Room room)
        {
            room.DateReg = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "Id", "Name", room.RoomTypeId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "Id", "Name", room.RoomTypeId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,RoomTypeId,Description,SleepingCot,SofaBed,Photo,Price")] Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            room.DateReg = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "Id", "Name", room.RoomTypeId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.RoomTypes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
