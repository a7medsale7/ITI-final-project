using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Training_ITI.Models;
using Training_ITI.Repositories;
using Training_ITI.ViewModels;

namespace Training_ITI.Controllers
{
    public class GradesController : Controller
    {
        private readonly IUnitOfWork _uow;
        public GradesController(IUnitOfWork uow) => _uow = uow;

        public async Task<IActionResult> Index(int? traineeId)
        {
            var query = _uow.Grades.GetAll(g => g.Trainee, g => g.Session!, g => g.Session!.Course!);
            if (traineeId.HasValue)
                query = query.Where(g => g.TraineeId == traineeId.Value);

            var grades = await query.OrderByDescending(g => g.Id).ToListAsync();

            ViewBag.Trainees = await _uow.Users.GetAll()
                .Where(u => u.Role == UserRole.Trainee)
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name })
                .ToListAsync();

            ViewBag.SelectedTraineeId = traineeId;
            return View(grades);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new GradeFormVM
            {
                Sessions = await _uow.Sessions.GetAll(s => s.Course)
                    .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Course!.Name + " (" + s.StartDate.ToString("yyyy-MM-dd") + ")" })
                    .ToListAsync(),
                Trainees = await _uow.Users.GetAll()
                    .Where(u => u.Role == UserRole.Trainee)
                    .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name })
                    .ToListAsync()
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GradeFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                await FillDropdowns(vm);
                return View(vm);
            }

            var grade = new Grade
            {
                SessionId = vm.SessionId,
                TraineeId = vm.TraineeId,
                Value = vm.Value
            };

            await _uow.Grades.AddAsync(grade);
            await _uow.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var g = await _uow.Grades.GetByIdAsync(id, x => x.Session!, x => x.Trainee!);
            if (g == null) return NotFound();

            var vm = new GradeFormVM
            {
                Id = g.Id,
                SessionId = g.SessionId,
                TraineeId = g.TraineeId,
                Value = g.Value
            };
            await FillDropdowns(vm);
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GradeFormVM vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                await FillDropdowns(vm);
                return View(vm);
            }

            var g = await _uow.Grades.GetByIdAsync(id);
            if (g == null) return NotFound();

            g.SessionId = vm.SessionId;
            g.TraineeId = vm.TraineeId;
            g.Value = vm.Value;

            _uow.Grades.Update(g);
            await _uow.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var g = await _uow.Grades.GetByIdAsync(id, x => x.Session!, x => x.Trainee!);
            if (g == null) return NotFound();
            return View(g);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _uow.Grades.DeleteAsync(id);
            await _uow.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task FillDropdowns(GradeFormVM vm)
        {
            vm.Sessions = await _uow.Sessions.GetAll(s => s.Course)
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Course!.Name + " (" + s.StartDate.ToString("yyyy-MM-dd") + ")" })
                .ToListAsync();

            vm.Trainees = await _uow.Users.GetAll()
                .Where(u => u.Role == UserRole.Trainee)
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name })
                .ToListAsync();
        }
    }
}
