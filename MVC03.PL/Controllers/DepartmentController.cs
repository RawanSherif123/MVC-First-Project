using Microsoft.AspNetCore.Mvc;
using MVC03.BLL.Interfaces;
using MVC03.BLL.Repositories;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;


namespace MVC03.PL.Controllers
{
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
        public IActionResult Index()
        {
            var departments = _unitOfWork.departmentRepository.GetAll();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // server side validation
            {
                var department = new Department()
                {
                    Name = model.Name,
                    Code = model.Code,
                    CreateAt = model.CreateAt
                };
                _unitOfWork.departmentRepository.Add(department);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Details(int? id, string viewname = "Details")
        {
            if (id is null) return BadRequest("Invalid Id ");

            var deprtment = _unitOfWork.departmentRepository.Get(id.Value);

            if (deprtment == null) return NotFound(new { statusCode = 400, messege = $"Department With Id:{id} is Not Found" });

            return View(viewname, deprtment);
        }



        [HttpGet]
        public IActionResult Edit(int? id)
        {

            /*  //if (id is null) return BadRequest("Invalid Id ");

              //var deprtment = _deptRepository.Get(id.Value);

              //if (deprtment == null) return NotFound(new { statusCode = 400, messege = $"Department With Id:{id} is Not Found" });

              //return View(deprtment);*/
            return Details(id, "Edit");
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
        public IActionResult Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // server side validation
            {
                var department = _unitOfWork.departmentRepository.Get(id);

                if (department == null) return NotFound(new { statusCode = 400, messege = $"Department With Id:{id} is Not Found" });

                department.Name = model.Name;
                department.Code = model.Code;
                department.CreateAt = model.CreateAt;

                _unitOfWork.departmentRepository.Update(department);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            /*//if (id is null) return BadRequest("Invalid Id ");

            //var deprtment = _deptRepository.Get(id.Value);

            //if (deprtment == null) return NotFound(new { statusCode = 400, messege = $"Department With Id:{id} is Not Found" });
*/

            return Details(id, "Delete");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // server side validation
            {
                var department = _unitOfWork.departmentRepository.Get(id);

                if (department == null) return NotFound(new { statusCode = 400, messege = $"Department With Id:{id} is Not Found" });

                _unitOfWork.departmentRepository.Delete(department);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

    }
}
