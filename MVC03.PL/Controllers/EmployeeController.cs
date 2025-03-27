using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC03.BLL.Interfaces;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;
using MVC03.PL.Helpers;

namespace MVC03.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepo;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
            //IEmployeeRepository employeeRepository,
            // IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_employeeRepo = employeeRepository;
            // _departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.employeeRepository.GetAllAsync();
            }
            else
            {
                employees =await _unitOfWork.employeeRepository.GetByNameAsync(SearchInput);
            }

            //  // ViewData : 
            ////  ViewData["Message"] = "Hello From ViewData";

            //  ViewBag.Message = new { Message = "Hello From ViewBag" };

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.departmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Create(EmployeeDto model)
        {
            if (ModelState.IsValid)
            {


                if (model.Image is not null)
                {
                   model.ImageName = DocumentSetting.UploadFile(model.Image, "images");
                }
                var employee = _mapper.Map<Employee>(model);

                   await  _unitOfWork.employeeRepository.AddAsync(employee);
                   var count = await _unitOfWork.CompleteAsync();

                 if (count > 0)
                 {

                    TempData["Message "] = " Employee is Created !!";
                        return RedirectToAction(nameof(Index));
                 }
              
            }


            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewname = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = await _unitOfWork.employeeRepository.GetAsync(id.Value);

            if (employee == null) return NotFound(new { statusCode = 404, messege = $"Employee With Id:{id} is Not Found" });

            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {


            //var departments = _unitOfWork.departmentRepository.GetAll();
            //ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id");
            var employee = await _unitOfWork.employeeRepository.GetAsync(id.Value);

            if (employee == null) return NotFound(new { statusCode = 404, messege = $"Employee With Id:{id} is Not Found" });
            var dto = _mapper.Map<EmployeeDto>(employee);

            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageName is not null && model.Image is not null)
                {
                    DocumentSetting.DeleteFile(model.ImageName, "images");
                }

                 if (model.Image is not null)
                 {
                    model.ImageName = DocumentSetting.UploadFile(model.Image, "images");

                 }

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
                    ImageName = model.ImageName,
                  //  DepartmentId = model.DepartmentId,


                };
                {
                     _unitOfWork.employeeRepository.Update(employee);
                    var count = await _unitOfWork.CompleteAsync();



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
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee =await _unitOfWork.employeeRepository.GetAsync(id);

                if (employee == null) return NotFound(new { statusCode = 400, messege = $"Employee With Id:{id} is Not Found" });


               _unitOfWork.employeeRepository.Delete(employee);
                var count = await _unitOfWork.CompleteAsync();

                if (count > 0)
                {
                    if (model.ImageName is not null) 
                    {
                        DocumentSetting.DeleteFile(model.ImageName, "images");
                    }
                    return RedirectToAction(nameof(Index));
                }


            }
            return View(model);
        }



    }
}