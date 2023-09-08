using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCcrudApp.Data;
using MVCcrudApp.Models;
using MVCcrudApp.Models.Domain;
using System.Numerics;

namespace MVCcrudApp.Controllers
{
    public class EmployeesControler : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;

        public EmployeesControler(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }
        
       
        [HttpGet]

        public async Task<IActionResult> Index()
        {
            var employees = await mvcDemoDbContext.Employees.ToListAsync();
            return View(employees);

        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Phone = addEmployeeRequest.Phone,
                Salary = addEmployeeRequest.Salary,
                Depatment = addEmployeeRequest.Depatment,
                DateOfBirth = addEmployeeRequest.DateOfBirth,

            };

            await mvcDemoDbContext.Employees.AddAsync(employee);   
            await mvcDemoDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
           var employee = await mvcDemoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
           
            if(employee != null)
            {
                var vewModel = new UpdateEmployee()
                {
                    Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Phone = employee.Phone,
                Salary = employee.Salary,
                Depatment = employee.Depatment,
                DateOfBirth = employee.DateOfBirth,
            };
                /*  return await View(vewModel);*/
                return await Task.Run(() => View("View", vewModel));
            }

            return RedirectToAction("Index");
          
        }

        [HttpPost]

        public async Task<IActionResult> View(UpdateEmployee model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if(employee != null)
            {
               employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Phone = model.Phone;   
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Depatment = model.Depatment;

              await  mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }



    }
}
