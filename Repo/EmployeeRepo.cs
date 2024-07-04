using connectingDB.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace connectingDB.Repo
{
    public class EmployeeRepo
    {
        SqlConnection connection;
        SqlCommand command;
        public EmployeeRepo(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            command = new SqlCommand(null, connection);

        }

        public void AddEmployee(Employee employee)
        {   
            connection.Open();
            command.CommandText =
                     "INSERT INTO Employees (Name, Salary, Status) " +
                     "VALUES (@Name, @Salary, @Status)";
            command.CommandType = CommandType.Text;
            command.Parameters.Clear();

            SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
            SqlParameter salaryParam = new SqlParameter("@Salary", SqlDbType.Decimal);
            salaryParam.Precision = 18;
            salaryParam.Scale = 2;
            SqlParameter statusParam = new SqlParameter("@Status", SqlDbType.NVarChar, 50);

            nameParam.Value = employee.Name;
            salaryParam.Value = employee.Salary;
            statusParam.Value = employee.Status;

            command.Parameters.Add(nameParam);
            command.Parameters.Add(salaryParam);
            command.Parameters.Add(statusParam);

            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DeleteEmployee(long id) 
        {
            connection.Open();
            command.CommandText =
             "DELETE FROM Employees WHERE ID = @id";
            command.CommandType = CommandType.Text;
            command.Parameters.Clear();

            SqlParameter idParam = new SqlParameter("@id", SqlDbType.BigInt);

            idParam.Value = id;

            command.Parameters.Add(idParam);

            command.ExecuteNonQuery();
            connection.Close();

        }

        public void EditEmployee(Employee emp) 
        {

            connection.Open();
            command.CommandText =
                "UPDATE Employees SET Name = @Name, Salary = @Salary, Status = @Status WHERE Id = @Id";
            command.CommandType = CommandType.Text;
            command.Parameters.Clear();

            SqlParameter idParam = new SqlParameter("@Id", SqlDbType.BigInt);
            SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
            SqlParameter salaryParam = new SqlParameter("@Salary", SqlDbType.Decimal);
            salaryParam.Precision = 18;
            salaryParam.Scale = 2;
            SqlParameter statusParam = new SqlParameter("@Status", SqlDbType.NVarChar, 50);

            idParam.Value = emp.Id;
            nameParam.Value = emp.Name;
            salaryParam.Value = emp.Salary;
            statusParam.Value = emp.Status;

            command.Parameters.Add(idParam);
            command.Parameters.Add(nameParam);
            command.Parameters.Add(salaryParam);
            command.Parameters.Add(statusParam);

            command.ExecuteNonQuery();
            connection.Close();

        }

    }
}
