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
        private void Execute(string query, SqlParameter[] parameters)
        {
            connection.Open();
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            command.Parameters.Clear();

            command.Parameters.AddRange(parameters);

            command.ExecuteNonQuery();
            connection.Close();
        }

        public void AddEmployee(Employee employee)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Name", SqlDbType.NVarChar, 50) 
                { 
                    Value = employee.Name 
                },
                new SqlParameter("@Salary", SqlDbType.Decimal) 
                { 
                    Precision = 18, 
                    Scale = 2, 
                    Value = employee.Salary 
                },
                new SqlParameter("@Status", SqlDbType.NVarChar, 50)
                { 
                    Value = employee.Status 
                }
            };

            Execute( "INSERT INTO Employees (Name, Salary, Status) VALUES (@Name, @Salary, @Status)", parameters);
        }

        public void DeleteEmployee(long id) 
        {

            SqlParameter[] parameters =
            {
                new SqlParameter("@id", SqlDbType.BigInt, 50)
                {
                    Value=id
                }
            };

            Execute("DELETE FROM Employees WHERE ID = @id", parameters);

        }

        public void EditEmployee(Employee emp) 
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", SqlDbType.BigInt)
                {
                    Value = emp.Id
                },
                new SqlParameter("@Name", SqlDbType.NVarChar, 50)
                {
                    Value = emp.Name
                },
                new SqlParameter("@Salary", SqlDbType.Decimal)
                {
                    Value = emp.Salary,
                    Precision = 18,
                    Scale = 2,
                },
                new SqlParameter("@Status", SqlDbType.NVarChar, 50)
                {
                   Value = emp.Status
                }

            }; 
            
            Execute("UPDATE Employees SET Name = @Name, Salary = @Salary, Status = @Status WHERE Id = @Id", parameters);

        }


    }
}
