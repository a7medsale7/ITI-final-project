using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Training_ITI.Models;
using Training_ITI.Repositories;
using Training_ITI.ViewModels;

namespace Training_ITI.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IUnitOfWork _uow;
        public CoursesController(IUnitOfWork uow) => _uow = uow;

        public async Task<IActionResult> Index(string? search, string? category)
        {
            var query = _uow.Courses.GetAll(c => c.Instructor);
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c => c.Name.Contains(search));
            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(c => c.Category.Contains(category));

            var courses = await query.OrderBy(c => c.Name).ToListAsync();
            ViewBag.Search = search;
            ViewBag.Category = category;
            return View(courses);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new CourseFormVM
            {
                Instructors = await _uow.Users.GetAll()
                    .Where(u => u.Role == UserRole.Instructor)
                    .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name })
                    .ToListAsync()
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                await FillInstructors(vm);
                return View(vm);
            }

            if (await _uow.Courses.AnyAsync(c => c.Name == vm.Name))
            {
                ModelState.AddModelError(nameof(vm.Name), "Course name must be unique.");
                await FillInstructors(vm);
                return View(vm);
            }

            var course = new Course
            {
                Name = vm.Name,
                Category = vm.Category,
                InstructorId = vm.InstructorId
            };

            await _uow.Courses.AddAsync(course);
            await _uow.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var course = await _uow.Courses.GetByIdAsync(id);
            if (course == null) return NotFound();

            var vm = new CourseFormVM
            {
                Id = course.Id,
                Name = course.Name,
                Category = course.Category,
                InstructorId = course.InstructorId,
                Instructors = await _uow.Users.GetAll()
                    .Where(u => u.Role == UserRole.Instructor)
                    .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name })
                    .ToListAsync()
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseFormVM vm)
        {
            if (id != vm.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                await FillInstructors(vm);
                return View(vm);
            }

            if (await _uow.Courses.AnyAsync(c => c.Name == vm.Name && c.Id != id))
            {
                ModelState.AddModelError(nameof(vm.Name), "Course name must be unique.");
                await FillInstructors(vm);
                return View(vm);
            }

            var course = await _uow.Courses.GetByIdAsync(id);
            if (course == null) return NotFound();

            course.Name = vm.Name;
            course.Category = vm.Category;
            course.InstructorId = vm.InstructorId;

            _uow.Courses.Update(course);
            await _uow.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var course = await _uow.Courses.GetByIdAsync(id, c => c.Instructor);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _uow.Courses.DeleteAsync(id);
            await _uow.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task FillInstructors(CourseFormVM vm)
        {
            vm.Instructors = await _uow.Users.GetAll()
                .Where(u => u.Role == UserRole.Instructor)
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name })
                .ToListAsync();
        }
    }
}
