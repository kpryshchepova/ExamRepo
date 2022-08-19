using EmployeesApp.Repository;
using Microsoft.AspNetCore.Mvc;
using EmployeesApp.Models;
using EmployeesApp.Shared;

namespace EmployeesApp.Areas.Todo.Controllers;

[Area("Todo")]
[Route("Todo")]
public class TodoController : BaseController
{
    public int pageSize = 5;
    private IRepository<Models.Todo> _todosRepository;
    private IRepository<Employee> _employeesRepository;
    private readonly ILogger _logger;
    public TodoController(IRepository<Models.Todo> todosRepository, IRepository<Employee> employeesRepository, ILogger<TodoController> logger)
    {
        _todosRepository = todosRepository;
        _employeesRepository = employeesRepository;
        _logger = logger;
    }

    [Route("TodoList")]
    public IActionResult TodoList()
    {
        _logger.LogInformation($"Todo List page - {DateTime.UtcNow.ToLongTimeString()}");
        return View();
    }

    [Route("GetAllTodosAsync")]
    public async Task<JsonResult> GetAllTodosAsync(string txtSearch, int? page, int? employeeId)
    {
        _logger.LogInformation($"Get Todo List by page request - {DateTime.UtcNow.ToLongTimeString()}");
        IEnumerable<Models.Todo> todos = await _todosRepository.GetAllAsync();
        IEnumerable<Employee> employees = await _employeesRepository.GetAllAsync();
        if (employeeId > 0)
        {
            todos = todos.Where(todo =>
                todo.EmployeeId.Equals(employeeId));
        }
        if (!String.IsNullOrEmpty(txtSearch))
        {
            todos = todos.Where(todo =>
                todo.Name.Contains(txtSearch) ||
                todo.Description.ToString().Contains(txtSearch));
        }
        if (page > 0)
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
        _logger.LogInformation($"Create Todo page - {DateTime.UtcNow.ToLongTimeString()}");
        ViewData["Employees"] = await _employeesRepository.GetAllAsync();
        return View("CreateEditTodo");
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(Models.Todo todo)
    {
        _logger.LogInformation($"Create Todo request - {DateTime.UtcNow.ToLongTimeString()}");
        return await base.Create(todo, _todosRepository, nameof(TodoList));

    }

    [Route("Edit")]
    public IActionResult Edit(int? id)
    {
        _logger.LogInformation($"Edit Todo page - {DateTime.UtcNow.ToLongTimeString()}");
        ViewBag.Id = id;
        return View("CreateEditTodo");
    }

    [Route("GetByIdAsync")]
    public async Task<JsonResult> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Get Todo by Id request - {DateTime.UtcNow.ToLongTimeString()}");
        Models.Todo todo = await _todosRepository.FindByIdAsync(id);
        return Json(todo);
    }

    [HttpPost("Edit")]
    public async Task<IActionResult> Edit(Models.Todo todo)
    {
        _logger.LogInformation($"Edit Todo request - {DateTime.UtcNow.ToLongTimeString()}");
        return await base.Edit(todo, _todosRepository, nameof(TodoList));
    }

    [Route("Delete")]
    public async Task<IActionResult> Delete(int? id)
    {
        _logger.LogInformation($"Delete Todo request - {DateTime.UtcNow.ToLongTimeString()}");
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
            return RedirectToAction("NotFound", "Error");
        }

        return RedirectToAction("NotFound", "Error");
    }
}
