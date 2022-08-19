using Microsoft.AspNetCore.Mvc;
using EmployeesApp.Models;
using System.Text.RegularExpressions;
using EmployeesApp.Repository;
using EmployeesApp.Shared;

namespace EmployeesApp.Areas.Employees.Controllers;

[Area("Employee")]
[Route("Employee")]
public class EmployeeController : BaseController
{
    public int pageSize = 5;
    private IRepository<Employee> _employeesRepository;
    private IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger _logger;
    public EmployeeController(IRepository<Employee> employeesRepository, IWebHostEnvironment webHost, ILogger<EmployeeController> logger)
    {
        _employeesRepository = employeesRepository;
        _webHostEnvironment = webHost;
        _logger = logger;
    }

    [Route("EmployeeList")]
    public IActionResult EmployeeList()
    {
        _logger.LogInformation($"Employee List page - {DateTime.UtcNow.ToLongTimeString()}");
        return View();
    }

    [Route("GetAllEmployeesListAsync")]
    public async Task<JsonResult> GetAllEmployeesListAsync()
    {
        _logger.LogInformation($"Get employee list request - {DateTime.UtcNow.ToLongTimeString()}");
        IEnumerable<Employee> employees = await _employeesRepository.GetAllAsync();
        return Json(employees);
    }

    [Route("GetAllEmployeesAsync")]
    public async Task<JsonResult> GetAllEmployeesAsync(string txtSearch, int? page)
    {
        _logger.LogInformation($"Get employees by page request - {DateTime.UtcNow.ToLongTimeString()}");
        IEnumerable<Employee> employees = await _employeesRepository.GetAllAsync();
        string patternDate = "^([0]?[0-9]|[12][0-9]|[3][01])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$";

        if (!String.IsNullOrEmpty(txtSearch))
        {
            employees = employees.Where(employee =>
                employee.Name.Contains(txtSearch) ||
                employee.Age.ToString().Contains(txtSearch) ||
                employee.Speciality.Contains(txtSearch) ||
                (Regex.IsMatch(txtSearch, patternDate) && employee.EmployementDate == DateTime.Parse(txtSearch)));
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

        int totalPage = employees.Count();
        float totalNumsize = (totalPage / (float)pageSize);
        int numSize = (int)Math.Ceiling(totalNumsize);

        ViewBag.numSize = numSize;

        var dataEmployees = employees.OrderByDescending(x => x.Id).Skip(start).Take(pageSize);
        List<Employee> listEmployees = new List<Employee>();
        listEmployees = dataEmployees.ToList();

        return Json(new { data = listEmployees, pageCurrent = page, numSize });
    }

    [Route("Create")]
    public IActionResult Create()
    {
        _logger.LogInformation($"Create employee page - {DateTime.UtcNow.ToLongTimeString()}");
        return View("CreateEditEmployee");
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(Employee employee, IFormFile file)
    {
        _logger.LogInformation($"Create employee request - {DateTime.UtcNow.ToLongTimeString()}");
        employee.ImageName = "";
        if (file is not null)
        {
            string imgExt = Path.GetExtension(file.FileName);

            string fileName = Guid.NewGuid().ToString() + imgExt;

            if (file.Length > 0)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);
                using (Stream fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            employee.ImageName = fileName;
        }

        return await base.Create(employee, _employeesRepository, nameof(EmployeeList));
        
    }

    [Route("Edit")]

    public IActionResult Edit(int? id)
    {
        _logger.LogInformation($"Edit employee page - {DateTime.UtcNow.ToLongTimeString()}");
        ViewBag.Id = id;
        return View("CreateEditEmployee");
    }

    [Route("GetByIdAsync")]
    public async Task<JsonResult> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Get employee by Id request - {DateTime.UtcNow.ToLongTimeString()}");
        Employee employee = await _employeesRepository.FindByIdAsync(id);
        return Json(employee);
    }

    [HttpPost("Edit")]
    public async Task<IActionResult> Edit(Employee employee, IFormFile file)
    {
        _logger.LogInformation($"Edit employee request - {DateTime.UtcNow.ToLongTimeString()}");
        employee.ImageName = "";
        if (file is not null)
        {
            string imgExt = Path.GetExtension(file.FileName);

            string fileName = Guid.NewGuid().ToString() + imgExt;

            if (file.Length > 0)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);
                using (Stream fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            employee.ImageName = fileName;
        }
        

        return await base.Edit(employee, _employeesRepository, nameof(EmployeeList));
    }

    [Route("Delete")]
    public async Task<IActionResult> Delete(int? id)
    {
        _logger.LogInformation($"Delete employee request - {DateTime.UtcNow.ToLongTimeString()}");
        try
        {
            if (id != null)
            {
                Employee employee = new Employee { Id = id.Value };
                _employeesRepository.Delete(employee);

                await _employeesRepository.SaveAsync();
                return RedirectToAction("EmployeeList");
            }
        } catch
        {
            return RedirectToAction("NotFound", "Error");
        }

        return RedirectToAction("NotFound", "Error");
    }
}
