using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC03.BLL.Interfaces;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;

namespace MVC03.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;
       // private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
            IEmployeeRepository employeeRepository, 
           // IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            _employeeRepo = employeeRepository;
           // _departmentRepository = departmentRepository;
           _mapper = mapper;
        }
        public IActionResult Index( string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = _employeeRepo.GetAll();
            }
           else
            {
                 employees = _employeeRepo.GetByName(SearchInput);
            }

            //  // ViewData : 
            ////  ViewData["Message"] = "Hello From ViewData";

            //  ViewBag.Message = new { Message = "Hello From ViewBag" };

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
          //  var departments = _departmentRepository.GetAll();
           // ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //var employee = new Employee()
                    //{
                    //    Name = model.Name,
                    //    Salary = model.Salary,
                    //    Address = model.Address,
                    //    IsActive = model.IsActive,
                    //    IsDeleted = model.IsDeleted,
                    //    Age = model.Age,

                    //    HiringDate = model.HiringDate,
                    //    Phone = model.Phone,
                    //    CreateAt = model.CreateAt,
                    //    Email = model.Email,
                    //    DepartmentId = model.DepartmentId,

                    //};

                   var employee= _mapper.Map<Employee>(model);

                    var count = _employeeRepo.Add(employee);
                    if (count > 0)
                    {
                        TempData["Message "] = " Employee is Created !!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(" ", ex.Message);
                }
            }


            return View(model);
        }


        [HttpGet]
        public IActionResult Details(int? id, string viewname = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepo.Get(id.Value);

            if (employee == null) return NotFound(new { statusCode = 404, messege = $"Employee With Id:{id} is Not Found" });

            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {

            
           // var departments = _departmentRepository.GetAll();
           // ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepo.Get(id.Value);

            if (employee == null) return NotFound(new { statusCode = 404, messege = $"Employee With Id:{id} is Not Found" });
            var dto = _mapper.Map<EmployeeDto>(employee);
           
            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee model)
        {
            if (ModelState.IsValid)
            {
                // if (id !=model.Id) return BadRequest();

                var employee = new Employee()
                {
                    Id = id,
                    Name = model.Name,
                    Salary = model.Salary,
                    Address = model.Address,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    Age = model.Age,

                    HiringDate = model.HiringDate,
                    Phone = model.Phone,
                    CreateAt = model.CreateAt,
                    Email = model.Email,

                };
                {
                    var count = _employeeRepo.Update(employee);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                }
            }
            return View(model);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, EmployeeDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var employee = _employeeRepo.Get(id);

        //        if (employee == null) return NotFound(new { statusCode = 400, messege = $"Employee With Id:{id} is Not Found" });


        //        employee.Email = model.Email;
        //        employee.Phone = model.Phone;
        //        employee.Address = model.Address;
        //        employee.Age = model.Age;
        //        employee.HiringDate = model.HiringDate;
        //        employee.Name = model.Name;
        //        employee.IsActive = model.IsActive;
        //        employee.IsDeleted = model.IsDeleted;
        //        employee.CreateAt = model.CreateAt;
        //        employee.Salary = model.Salary;

        //        var count = _employeeRepo.Update(employee);
        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }

        //    }
        //    return View(model);
        //}


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, EmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee = _employeeRepo.Get(id);

                if (employee == null) return NotFound(new { statusCode = 400, messege = $"Employee With Id:{id} is Not Found" });


                var count = _employeeRepo.Delete(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }


            }
            return View(model);
        }



    }
}