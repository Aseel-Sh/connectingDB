using Microsoft.Data.SqlClient;
using connectingDB.Model;
using System;
using System.Xml;
public class Program
{
    public static void Main(string[] args)
    {
        List<Employee> employees = new List<Employee>();
        employees = GetEmployeeFromDb();
        foreach (var employee in employees)
        {
            Console.WriteLine($"Id = {employee.Id} , Name = {employee.Name} , Salary = {employee.Salary} , Status = {employee.Status}");
        }

        int x;
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine("1. Add\n2. Delete\n3. Edit\n4. Print DB\n5. Exit");
        Console.WriteLine("-------------------------------------------------");
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
        long id;
        string name, status;
        decimal salary;
        bool foundEmployee = false;

        Console.Write("Id of employee you want to edit: ");
        id = Convert.ToInt64(Console.ReadLine());

        foreach (Employee employee in employees)
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
            Console.WriteLine("New Name: ");
            name = Console.ReadLine();

            Console.Write("New Salary: ");
            salary = Convert.ToDecimal(Console.ReadLine());

            Console.Write("New Status: ");
            status = Console.ReadLine();

            string connectionString = "data source=.;Initial Catalog=CompanyDb;Integrated Security=True;TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("UPDATE Employees SET Name = '" + name +"', Salary = " + salary +", Status = '" + status +"' WHERE Id = " + id, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine("Employee edited.");

        }



    }

    private static void DeleteEmployee(List<Employee> employees)
    {
        Console.Write("ID of employee you want to delete: ");
        long id = Convert.ToInt64(Console.ReadLine());

        Employee employeeToDelete = null;
        foreach (var employee in employees)
        {
            if (employee.Id == id)
            {
                employeeToDelete = employee;
                break;
            }
        } 

        if (employeeToDelete == null)
        {
            Console.WriteLine("No such employee in DB");
        }
        else
        {
            string connectionString = "data source=.;Initial Catalog=CompanyDb;Integrated Security=True;TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("DELETE FROM Employees WHERE ID = " + id, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine("Employee deleted.");
        }
    }

    private static void AddEmployee()
    {
        string name, status;
        decimal salary;
        Console.Write("Name of employee you want to add: ");
        name = Console.ReadLine();

        Console.Write("Salary of employee: ");
        salary = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Status of employee: ");
        status = Console.ReadLine();

        string connectionString = "data source=.;Initial Catalog=CompanyDb;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand command = new SqlCommand("INSERT INTO Employees (Name, Salary, Status) VALUES ('" + name + "', " + salary +", '" + status + "')", connection);
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();

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