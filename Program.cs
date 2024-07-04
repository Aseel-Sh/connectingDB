using Microsoft.Data.SqlClient;
using connectingDB.Model;
using System;
using System.Xml;
using connectingDB.Repo;
public class Program
{
    public static EmployeeRepo Repo = new EmployeeRepo("data source=.;Initial Catalog=CompanyDb;Integrated Security=True;TrustServerCertificate=True");
    public static void Main(string[] args)
    {
        List<Employee> employees = new List<Employee>();
        employees = GetEmployeeFromDb();
        foreach (var employee in employees)
        {
            Console.WriteLine($"Id = {employee.Id} , Name = {employee.Name} , Salary = {employee.Salary} , Status = {employee.Status}");
        }

        int x = 0;
        printMenu(x);

        do
        {
             x = Convert.ToInt32(Console.ReadLine());
            printMenu(x);

            switch (x)  
            {
                case 1:
                    AddEmployee();
                    printMenu(x);
                    break;
                case 2:
                    DeleteEmployee(employees);
                    printMenu(x);
                    break;
                case 3:
                    EditEmployee(employees);
                    printMenu(x);
                    break;
                case 4:
                    employees = GetEmployeeFromDb();
                    foreach (var employee in employees)
                    {
                        Console.WriteLine($"Id = {employee.Id} , Name = {employee.Name} , Salary = {employee.Salary} , Status = {employee.Status}");
                    }
                    printMenu(x);
                    break;
                default:
                    break;
            }

        } while (x != 5);

    }

    private static void printMenu(int x)
    {
        if (x != 5)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("1. Add\n2. Delete\n3. Edit\n4. Print DB\n5. Exit");
            Console.WriteLine("-------------------------------------------------");
        }
    }

    private static void EditEmployee(List<Employee> employees)
    {
        bool foundEmployee = false;
        Employee emp = new Employee();

        Console.Write("Id of employee you want to edit: ");
        emp.Id = Convert.ToInt64(Console.ReadLine());

        foreach (Employee employee in employees)
        {
            if (employee.Id == emp.Id)
            {
                foundEmployee = true;
                break;
            }
        }

        if (foundEmployee == false) 
        {
            Console.WriteLine("No such employee in DB");
        }
        else
        {
            Console.Write("New Name: ");
            emp.Name = Console.ReadLine();

            Console.Write("New Salary: ");
            emp.Salary = Convert.ToDecimal(Console.ReadLine());

            Console.Write("New Status: ");
            emp.Status = Console.ReadLine();

            Repo.EditEmployee(emp);

            Console.WriteLine("Employee edited.");

        }



    }

    private static void DeleteEmployee(List<Employee> employees)
    {
        Console.Write("ID of employee you want to delete: ");
        long id = Convert.ToInt64(Console.ReadLine());

        bool foundEmployee = false;
        foreach (var employee in employees)
        {
            if (employee.Id == id)
            {
                foundEmployee = true;
                break;
            }
        } 

        if (foundEmployee == false)
        {
            Console.WriteLine("No such employee in DB");
        }
        else
        {
            Repo.DeleteEmployee(id);
            Console.WriteLine("Employee deleted.");
        }
    }

    private static void AddEmployee()
    {
        Employee emp = new Employee();

        Console.Write("Name of employee you want to add: ");
        emp.Name = Console.ReadLine();

        Console.Write("Salary of employee: ");
        emp.Salary = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Status of employee: ");
        emp.Status = Console.ReadLine();

        Repo.AddEmployee(emp);

        Console.WriteLine("Employee Added.");


    }

    private static List<Employee> GetEmployeeFromDb()
    {
        List<Employee> employees = new List<Employee>();
        string connectionString = "data source=.;Initial Catalog=CompanyDb;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection connection = new SqlConnection(connectionString);
        var query = "Select * from employees";
        SqlCommand command = new SqlCommand(query, connection);
        connection.Open();
        command.ExecuteNonQuery();
        var result = command.ExecuteReader();
        while (result.Read())
        {
            employees.Add(new Employee
            {
                Id = result.GetInt64(0),
                Name = result.GetString(1),
                Salary = result.GetDecimal(2),
                Status = result.GetString(3),
            });
        }
        connection.Close();

        return employees;
    }
}