﻿using KostaTest.Domain.Repositories.Interfaces;
using KostaTest.Extensions;
using KostaTest.Models;
using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;

namespace KostaTest.Domain.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly string _connectionString;
        public DepartmentRepository(string connectionString) 
        {
            _connectionString = connectionString;
        }

        public List<Department> GetAllDepartments()
        {
            List<Department> result = new();

            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new()
            {
                Connection = connection,
                CommandText = "select * from Department"
            };
            connection.Open();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Department
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Code = reader.SafeGetString(2),
                    ParentDepartmentId = reader.SafeGetGuid(3),
                });
            }
            connection.Close();

            return result;
        }
        
        public Department GetDepartmentById(Guid id) 
        {
            Department department = new();
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new()
            {
                Connection = connection,
                CommandText = $"select * from Department where ID = '{id}'"
            };
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                department = new()
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Code = reader.SafeGetString(2),
                    ParentDepartmentId = reader.SafeGetGuid(3),
                };
            }
            connection.Close();
            return department;
        }

        public Dictionary<string, Guid> GetDepartmentsNames()
        {
            Dictionary<string, Guid> departmentsNames = new();
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new()
            {
                Connection = connection,
                CommandText = "select [Name], ID from Department"
            };
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                departmentsNames.Add(reader.GetString(0), reader.GetGuid(1));
            }
            connection.Close();
            return departmentsNames;
        }

        public void AddDepartment(Department dep)
        {
            string parentDep = (dep.ParentDepartmentId == Guid.Empty) ? "NULL" : $"'{dep.ParentDepartmentId}'";
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new()
            {
                Connection = connection,
                CommandText = $"insert into Department values(NEWID(), '{dep.Name}', '{dep.Code}', {parentDep})"
            };
            connection.Open();
            command.ExecuteNonQuery();

            if (dep.ParentDepartmentId == Guid.Empty)
            {
                using SqlCommand updateId = new()
                {
                    Connection = connection,
                    CommandText = $"update Department set ParentDepartmentId = (select Id from Department where [Name] = '{dep.Name}') where [Name] = '{dep.Name}'"
                };
                updateId.ExecuteNonQuery();
            }

            connection.Close();
        }

        public void UpdateDepartment(Department dep)
        {
            string? code = (dep.Code != null) ? $"'{dep.Code}'" : null;

            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new()
            {
                Connection = connection,
                CommandText = $"update Department set ParentDepartmentId = '{dep.ParentDepartmentId}', Code = {code}, Name = '{dep.Name}' where ID = '{dep.Id}'"
            };

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DeleteDepartment(Guid id)
        {
            using SqlConnection connection = new(_connectionString);

            using SqlCommand deleteEmployes = new()
            {
                Connection = connection,
                CommandText = $"delete from Empoyee where DepartmentID = '{id}'"
            };

            connection.Open();
            deleteEmployes.ExecuteNonQuery();

            Department current = GetDepartmentById(id);

            if (current.Id == current.ParentDepartmentId)
            {
                using SqlCommand update = new()
                {
                    Connection = connection,
                    CommandText = "begin transaction " +
                    "declare @enumerator table (ID uniqueidentifier) " +
                    $"insert into @enumerator select ID from Department where ParentDepartmentID = '{current.Id}' " +
                    "declare @count int = @@ROWCOUNT " +
                    "declare @curid uniqueidentifier " +
                    "while @count <> 0 " +
                    "begin " +
                    "select top 1 @curid = ID from @enumerator " +
                    "update Department set ParentDepartmentID = @curid where ID = @curid " +
                    "delete from @enumerator where ID = @curid " +
                    "set @count = @count - 1 " +
                    "end " +
                    "commit"
                };
                update.ExecuteNonQuery();
            }
            else
            {
                using SqlCommand updateParent = new()
                {
                    Connection = connection,
                    CommandText = $"update Department set ParentDepartmentID = '{current.ParentDepartmentId}' where ParentDepartmentID = '{id}'"
                };
                updateParent.ExecuteNonQuery();
            }

            using SqlCommand deleteDepCommand = new()
            {
                Connection = connection,
                CommandText = $"delete from Department where ID = '{id}'"
            };

            deleteDepCommand.ExecuteNonQuery();
            connection.Close();
        }
    }
}
