using System.Collections.Generic;
using System.Linq;
using Xunit;
using Euroland.NetCore.ToolsFramework.Data.Test.Models;
using System;

namespace Euroland.NetCore.ToolsFramework.Data.Test
{
    public class DapperDatabaseContextTest
    {
        string serverName = "(localdb)\\mssqllocaldb";
        string databaseName = "School";
        /// <summary>
        /// option to login MS Sql Server
        /// </summary>
        string userID = "123";
        string password = "";
        string connectionString => $"Server={serverName};Database={databaseName};Trusted_Connection=True;MultipleActiveResultSets=true";
        public SchoolContext context => new SchoolContext(connectionString);
        DapperDatabaseContext DatabaseConnection => new DapperDatabaseContext(connectionString);
        public DapperDatabaseContextTest()
        {
            DbInitializer.Initialize(context);
        }

        [Fact]
        public void CanReturnIEnumerable()
        {
            IEnumerable<Student> students = DatabaseConnection.Exec<Student>("spStudentSelect");
            Assert.True(students.Count() > 0);
        }

        [Fact]
        public void CanReturnIObject()
        {
            Student student = DatabaseConnection.ExecSingle<Student>("spStudentSelectByID", new { ID = 1 });
            Assert.True(student != null);
        }

        [Fact]
        public int ExNonQuery()
        {
            Student studentobj = new Student();
            int student = DatabaseConnection.ExecNonQuery("spStudentInsert", new { ID = 1, LastName="Nguyen", FirstMidName="Cuong", EnrollmentDate = DateTime.Now });
            return student;
        }
    }
}
