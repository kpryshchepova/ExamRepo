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
        public int pageSize = 5;
        private IRepository<Models.Todo> _todosRepository;
        private IRepository<Employee> _employeesRepository;
        public TodoController(IRepository<Models.Todo> todosRepository, IRepository<Employee> employeesRepository)
        {
            _todosRepository = todosRepository;
            _employeesRepository = employeesRepository;
        }

        [Route("TodoList")]
        public IActionResult TodoList()
        {
            return View();
        }

        [Route("GetAllTodosAsync")]
        public async Task<JsonResult> GetAllTodosAsync(string txtSearch, int? page)
        {
            IEnumerable<Models.Todo> todos = await _todosRepository.GetAllAsync();
            IEnumerable<Employee> employees = await _employeesRepository.GetAllAsync();

            if (!String.IsNullOrEmpty(txtSearch))
            {
                todos = todos.Where(todo =>
                    todo.Name.Contains(txtSearch) ||
                    todo.Description.ToString().Contains(txtSearch));
            }
            if (page <= 0)
            {
                page = page;
            }
            else
            {
                page = 1;
            }
            int start = (int)(page - 1) * pageSize;
            ViewBag.pageCurrent = page;

            int totalPage = todos.Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);

            ViewBag.numSize = numSize;

            var dataTodos = todos.OrderByDescending(x => x.Id).Skip(start).Take(pageSize);
            List<Models.Todo> listTodos = new List<Models.Todo>();
            listTodos = dataTodos.ToList();

            return Json(new { todos = listTodos, pageCurrent = page, numSize, employees });
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
        public IActionResult Edit(int? id)
        {
            ViewBag.Id = id;
            return View("CreateEditTodo");
        }

        [Route("GetByIdAsync")]
        public async Task<JsonResult> GetByIdAsync(int id)
        {
            Models.Todo todo = await _todosRepository.FindByIdAsync(id);
            return Json(todo);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(Models.Todo todo)
        {
            return await base.Edit(todo, _todosRepository, nameof(TodoList));
        }

        [Route("Delete")]
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
                return NotFound();
            }

            return NotFound();
        }
    }
}
