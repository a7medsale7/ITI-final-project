using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training_ITI.Models;
using Training_ITI.Repositories;

namespace Training_ITI.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _uow;
        public UsersController(IUnitOfWork uow) => _uow = uow;

        public async Task<IActionResult> Index()
        {
            var users = await _uow.Users.GetAll().OrderBy(u => u.Name).ToListAsync();
            return View(users);
        }

        public IActionResult Create() => View(new User());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid) return View(user);
            await _uow.Users.AddAsync(user);
            await _uow.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var u = await _uow.Users.GetByIdAsync(id);
            if (u == null) return NotFound();
            return View(u);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id) return BadRequest();
            if (!ModelState.IsValid) return View(user);

            _uow.Users.Update(user);
            await _uow.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var u = await _uow.Users.GetByIdAsync(id);
            if (u == null) return NotFound();
            return View(u);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _uow.Users.DeleteAsync(id);
            await _uow.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
