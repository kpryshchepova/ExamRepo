using EmployeesApp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeesApp.Models;
using EmployeesApp.Shared;

namespace EmployeesApp.Areas.Todo.Controllers
{
    [Area("Todo")]
    [Route("Todo")]
    public class TodoController : BaseController
    {
        private IRepository<Models.Todo> _todosRepository;
        private IRepository<Employee> _employeesRepository;
        public TodoController(IRepository<Models.Todo> todosRepository, IRepository<Employee> employeesRepository)
        {
            _todosRepository = todosRepository;
            _employeesRepository = employeesRepository;
        }

        [Route("TodoList")]
        public async Task<IActionResult> TodoList()
        {
            try
            {
                ViewBag.Employees = await _employeesRepository.GetAllAsync();
                var todos = await _todosRepository.GetAllAsync();
                return View(todos);
            }
            catch
            {//------------TODO ERROR-------------
                return NotFound();
            }
        }

        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            ViewData["Employees"] = await _employeesRepository.GetAllAsync();
            return View("CreateEditTodo");
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Models.Todo todo)
        {
            return await base.Create(todo, _todosRepository, nameof(TodoList));

        }

        [Route("Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id != null)
                {
                    Models.Todo? todo = await _todosRepository.FindByIdAsync(id);
                    if (todo != null)
                    {
                        var employees = await _employeesRepository.GetAllAsync();
                        ViewData["Employees"] = employees;
                        ViewData["CurrentEmployeeId"] = employees.FirstOrDefault(e => e.Id == todo.EmployeeId)?.Id;
                        return View("CreateEditTodo", todo);
                    }
                    
                }
                //------------TODO ERROR-------------
                return NotFound();
            }
            catch
            {//------------TODO ERROR-------------
                return NotFound();
            }

        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(Models.Todo todo)
        {
            return await base.Edit(todo, _todosRepository, nameof(TodoList));
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id != null)
                {
                    Models.Todo todo = new Models.Todo { Id = id.Value };
                    _todosRepository.Delete(todo);

                    await _todosRepository.SaveAsync();
                    return RedirectToAction("TodoList");
                }
            }
            catch
            {
                //------------TODO ERROR-------------
                return NotFound();
            }

            return NotFound();
        }
    }
}
