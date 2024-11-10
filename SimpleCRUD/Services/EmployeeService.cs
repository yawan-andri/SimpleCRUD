using Microsoft.EntityFrameworkCore;
using SimpleCRUD.DataAccess;
using SimpleCRUD.DataAccess.Entities;
using SimpleCRUD.ViewModels;

namespace SimpleCRUD.Services
{
    public class EmployeeService
    {
        private readonly AppDBContext dBContext;

        public EmployeeService(AppDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<List<EmployeeViewModel>> GetAllEmployees()
        {
            return await dBContext.Employees
                .OrderBy(x => x.FullName)
                .Select(x => new EmployeeViewModel
                {
                    EmployeeId = x.EmployeeId,
                    FullName = x.FullName,
                    Department = x.Department,
                    DateOfBirth = x.DateOfBirth,
                    Age = x.Age,
                    PhoneNumber = x.PhoneNumber,
                }).ToListAsync();
        }

        public bool CreateNewEmployee(EmployeeViewModel model)
        {
            try
            {
                Employee employee = new Employee
                {
                    FullName = model.FullName,
                    Department = model.Department,
                    DateOfBirth = model.DateOfBirth,
                    Age = model.Age,
                    PhoneNumber = model.PhoneNumber,
                };

                dBContext.Employees.Add(employee);

                var result = dBContext.SaveChanges();

                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public EmployeeViewModel? FindEmployee(int employeeID)
        {
            var employee = dBContext.Employees.Find(employeeID);
            if (employee == null) return null;

            EmployeeViewModel result = new EmployeeViewModel
            {
                EmployeeId = employee.EmployeeId,
                FullName = employee.FullName,
                DateOfBirth = employee.DateOfBirth,
                Department = employee.Department,
                Age = employee.Age,
                PhoneNumber = employee.PhoneNumber,
            };
            return result;
        }

        public bool UpdateEmployee(EmployeeViewModel model)
        {
            try
            {
                var employee = dBContext.Employees.Find(model.EmployeeId);
                if (employee == null) return false;


                employee.FullName = model.FullName;
                employee.Department = model.Department;
                employee.Age = model.Age;
                employee.PhoneNumber = model.PhoneNumber;
                employee.DateOfBirth = model.DateOfBirth;

                var result = dBContext.SaveChanges();
                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteEmployee(int employeeID)
        {
            try
            {
                var employee = dBContext.Employees.Find(employeeID);
                if (employee == null) return false;

                dBContext.Employees.Remove(employee);
                var result = dBContext.SaveChanges();
                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
