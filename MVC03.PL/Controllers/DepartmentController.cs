using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC03.BLL.Interfaces;
using MVC03.BLL.Repositories;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;


namespace MVC03.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
       // private readonly IDepartmentRepository _deptRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(/*IDepartmentRepository departmentRepository*/ IUnitOfWork unitOfWork)
        {
          //  _deptRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]  // GET ://Department//Index
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.departmentRepository.GetAllAsync();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // server side validation
            {
                var department = new Department()
                {
                    Name = model.Name,
                    Code = model.Code,
                    CreateAt = model.CreateAt
                };
               await _unitOfWork.departmentRepository.AddAsync(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewname = "Details")
        {
            if (id is null) return BadRequest("Invalid Id ");

            var deprtment = await _unitOfWork.departmentRepository.GetAsync(id.Value);

            if (deprtment == null) return NotFound(new { statusCode = 400, messege = $"Department With Id:{id} is Not Found" });

            return View(viewname, deprtment);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            /*  //if (id is null) return BadRequest("Invalid Id ");

              //var deprtment = _deptRepository.Get(id.Value);

              //if (deprtment == null) return NotFound(new { statusCode = 400, messege = $"Department With Id:{id} is Not Found" });

              //return View(deprtment);*/
            return await Details(id, "Edit");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id,Department department)
        //{
        //    if (ModelState.IsValid) // server side validation
        //    {
        //        if (id == department.Id)   // ---> defensive code 
        //        {
        //            var count = _deptRepository.Update(department);
        //            if (count > 0)
        //            {
        //                return RedirectToAction(nameof(Index));
        //            }
        //        }

        //    }

        //    return View(department);
        //}


        [HttpPost]    //-----> Another way for Edit 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // server side validation
            {
                var department = await _unitOfWork.departmentRepository.GetAsync(id);

                if (department == null) return NotFound(new { statusCode = 400, messege = $"Department With Id:{id} is Not Found" });

                department.Name = model.Name;
                department.Code = model.Code;
                department.CreateAt = model.CreateAt;

                _unitOfWork.departmentRepository.Update(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            /*//if (id is null) return BadRequest("Invalid Id ");

            //var deprtment = _deptRepository.Get(id.Value);

            //if (deprtment == null) return NotFound(new { statusCode = 400, messege = $"Department With Id:{id} is Not Found" });
*/

            return await Details(id, "Delete");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // server side validation
            {
                var department = await _unitOfWork.departmentRepository.GetAsync(id);

                if (department == null) return NotFound(new { statusCode = 400, messege = $"Department With Id:{id} is Not Found" });

                _unitOfWork.departmentRepository.Delete(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

    }
}
