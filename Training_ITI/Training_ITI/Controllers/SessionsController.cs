using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Training_ITI.Models;
using Training_ITI.Repositories;
using Training_ITI.ViewModels;

namespace Training_ITI.Controllers
{
    public class SessionsController : Controller
    {
        private readonly IUnitOfWork _uow;
        public SessionsController(IUnitOfWork uow) => _uow = uow;

        public async Task<IActionResult> Index(string? course)
        {
            var query = _uow.Sessions.GetAll(s => s.Course);
            if (!string.IsNullOrWhiteSpace(course))
                query = query.Where(s => s.Course!.Name.Contains(course));

            var sessions = await query.OrderByDescending(s => s.StartDate).ToListAsync();
            ViewBag.Course = course;
            return View(sessions);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new SessionFormVM
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(2),
                Courses = await _uow.Courses.GetAll()
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToListAsync()
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SessionFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                await FillCourses(vm);
                return View(vm);
            }

            var session = new Session
            {
                CourseId = vm.CourseId,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate
            };

            await _uow.Sessions.AddAsync(session);
            await _uow.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var s = await _uow.Sessions.GetByIdAsync(id);
            if (s == null) return NotFound();

            var vm = new SessionFormVM
            {
                Id = s.Id,
                CourseId = s.CourseId,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Courses = await _uow.Courses.GetAll()
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToListAsync()
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SessionFormVM vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                await FillCourses(vm);
                return View(vm);
            }

            var s = await _uow.Sessions.GetByIdAsync(id);
            if (s == null) return NotFound();

            s.CourseId = vm.CourseId;
            s.StartDate = vm.StartDate;
            s.EndDate = vm.EndDate;

            _uow.Sessions.Update(s);
            await _uow.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var s = await _uow.Sessions.GetByIdAsync(id, x => x.Course);
            if (s == null) return NotFound();
            return View(s);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _uow.Sessions.DeleteAsync(id);
            await _uow.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task FillCourses(SessionFormVM vm)
        {
            vm.Courses = await _uow.Courses.GetAll()
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();
        }
    }
}
