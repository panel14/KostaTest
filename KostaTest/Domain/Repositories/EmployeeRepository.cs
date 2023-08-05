using KostaTest.Domain.Repositories.Interfaces;
using KostaTest.Extensions;
using KostaTest.Models;
using System.Data.SqlClient;

namespace KostaTest.Domain.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Employee> GetEmployeesByDepartmentId(Guid departmentId)
        {
            List<Employee> employees = new();

            using SqlConnection connection = new (_connectionString);
            using SqlCommand command = new()
            {
                Connection = connection,
                CommandText = $"select * from Empoyee where DepartmentID = '{departmentId}'"
            };
            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                employees.Add(new()
                {
                    Id = reader.GetDecimal(0),
                    FirstName = reader.GetString(1),
                    SurName = reader.GetString(2),
                    Patronymic = reader.SafeGetString(3),
                    DateOfBirth = reader.GetDateTime(4),
                    DocSeries = reader.SafeGetString(5),
                    DocNumber = reader.SafeGetString(6),
                    Position = reader.GetString(7),
                    DepartmentId = reader.GetGuid(8)
                });
            }

            connection.Close();

            return employees;
        }

        public Employee GetEmployeeById(decimal id)
        {
            Employee employee = new();

            using SqlConnection connection = new (_connectionString);
            using SqlCommand command = new()
            {
                Connection = connection,
                CommandText = $"select * from Empoyee where ID = '{id}'"
            };

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                employee = new()
                {
                    Id = reader.GetDecimal(0),
                    FirstName = reader.GetString(1),
                    SurName = reader.GetString(2),
                    Patronymic = reader.SafeGetString(3),
                    DateOfBirth = reader.GetDateTime(4),
                    DocSeries = reader.SafeGetString(5),
                    DocNumber = reader.SafeGetString(6),
                    Position = reader.GetString(7),
                    DepartmentId = reader.GetGuid(8)
                };
            }
            connection.Close();
            return employee;
        }

        public void AddEmployee(Employee emp)
        {
            string? patronymic = (emp.Patronymic != null) ? $"'{emp.Patronymic}'" : "NULL";
            string? docSeries = (emp.DocSeries != null) ? $"'{emp.DocSeries}'" : "NULL";
            string? docNumber = (emp.DocNumber != null) ? $"'{emp.DocNumber}'" : "NULL";

            using SqlConnection connection = new (_connectionString);
            using SqlCommand command = new()
            {
                Connection = connection,
                CommandText = $"insert into Empoyee values('{emp.FirstName}'," +
                $" '{emp.SurName}', {patronymic}, '{emp.DateOfBirth:yyyy-MM-dd}', {docSeries}, {docNumber}, '{emp.Position}', '{emp.DepartmentId}')"
            };
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void UpdateEmployee(Employee emp)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new()
            {
                Connection = connection,
                CommandText = $"update Empoyee set DepartmentId = '{emp.DepartmentId}', " +
                $"SurName = '{emp.SurName}', " +
                $"FirstName = '{emp.FirstName}', " +
                $"Patronymic = '{emp.Patronymic}', " +
                $"DateOfBirth = '{emp.DateOfBirth:yyyy-MM-dd}'," +
                $"DocSeries = '{emp.DocSeries}', " +
                $"DocNumber = '{emp.DocNumber}', " +
                $"Position = '{emp.Position}' where ID = '{emp.Id}'"
            };
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DeleteEmployee(decimal id) 
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new()
            {
                Connection = connection,
                CommandText = $"delete from Empoyee where ID = {id}"
            };
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
