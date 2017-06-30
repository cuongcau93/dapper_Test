using System.Collections.Generic;
using System.Linq;
using Xunit;
using Euroland.NetCore.ToolsFramework.Data.Test.Models;
using System;
using System.Threading.Tasks;

namespace Euroland.NetCore.ToolsFramework.Data.Test
{
    public class DapperDatabaseContextTest
    {
        string serverName = "(localdb)\\mssqllocaldb";
        string databaseName = "SchoolDB";
        /// <summary>
        /// option to login mssql server
        /// if you usage sql express then do not care about User and Password to login sql server
        /// </summary>
        string userID = "sa";
        string password = "123456sa";
        string connectionString => $"Server={serverName};Database={databaseName};User ID={userID};Password={password};Trusted_Connection=True;MultipleActiveResultSets=true";
        public SchoolContext context => new SchoolContext(connectionString);
        DapperDatabaseContext DatabaseConnection => new DapperDatabaseContext(connectionString);
        public DapperDatabaseContextTest()
        {
            DbInitializer.Initialize(context);

            //If Database had created then we begin create stores
            if (InitialStoreProcedure.CheckExistDB(connectionString))
            {
                InitialStoreProcedure initStore = new InitialStoreProcedure(connectionString);
                initStore.BuildStore("spStudentSelect", "SELECT ID, FirstMidName, LastName, EnrollmentDate FROM Student");
                initStore.BuildStore("spStudentSelectByID", "SELECT ID, FirstMidName, LastName, EnrollmentDate FROM Student WHERE ID = @ID", "@ID INT");
                initStore.BuildStore("spStudentSelectTotalStudent", "SELECT COUNT(ID) FROM Student");
                initStore.BuildStore("spStudentInsert", "DECLARE @StudentID INT \n IF @ID IS NULL OR @ID <= 0 \n BEGIN \n INSERT INTO Student(FirstMidName, LastName, EnrollmentDate) \n VALUES(@FirstName, @LastName, @EnrollmentDate) \n SET @StudentID = @@ROWCOUNT \n END \n ELSE \n BEGIN \n UPDATE Student SET FirstMidName = @FirstName, LastName = @LastName, EnrollmentDate = @EnrollmentDate \n SET @StudentID = @ID \n END \n SELECT @StudentID", "@ID INT = NULL, \n @FirstName NVARCHAR(50), \n  @LastName NVARCHAR(50), \n  @EnrollmentDate DATETIME");

                initStore.BuildStore("spStudentExecNonQueryInsert", "INSERT INTO Student (FirstMidName, LastName, EnrollmentDate) VALUES (@FirstMidName, @LastName, @EnrollmentDate) Select Id from Student", "@FirstMidName NVARCHAR(MAX),@LastName NVARCHAR(MAX),@EnrollmentDate DATETIME ");
                initStore.BuildStore("spStudentExecNonQueryUpdate", "UPDATE Student SET FirstMidName = @FirstMidName WHERE ID = @ID", "@ID INT, @FirstMidName NVARCHAR(MAX)");
                initStore.BuildStore("spStudentExecNonQueryDelete", "DELETE Student WHERE ID = @ID", "@ID INT");
            }
            else
            {
                Console.WriteLine("Database must be created before create stored procedure.");
            }
        }

        [Fact]
        public void CanReturnIEnumerable()
        {
            IEnumerable<Student> students = DatabaseConnection.Exec<Student>("spStudentSelect");
            Assert.True(students.Count() > 0);
        }

        [Fact]
        public async Task DoWorkIEnumerableAsync()
        {
            string message = "Init";
            await Task.Run(async () =>
            {
                IEnumerable<Student> students = await DatabaseConnection.ExecAsync<Student>("spStudentSelect");
                await Task.Delay(2000);
                message += " number of student are: " + students.Count();
            });
            message += " Work";
            string messageResult = message;
            Assert.True(message.Contains("number of student are:"));
        }

        [Fact]
        public async Task SynchronizeTestWithCodeViaAwait()
        {
            string message1 = "Init";
            // Schedule operation to run asynchronously and wait until it is finished.
            IEnumerable<Student> students = await DatabaseConnection.ExecAsync<Student>("spStudentSelect");
            string message2 = "Wait for number of students " + students.Count();
            string mesageResult = message1 + message2;
            // Assert outcome of the operation.
            Assert.True(mesageResult.Contains(students.Count().ToString()));
        }

        [Fact]
        public async void CanReturnIEnumerableAsync()
        {
            IEnumerable<Student> students = await DatabaseConnection.ExecAsync<Student>("spStudentSelect");
            Assert.True(students.Count() > 0);
        }

        [Fact]
        public void CanReturnIEnumerableWithWrongStoreName()
        {
            try
            {
                IEnumerable<Student> students = DatabaseConnection.Exec<Student>("spStudentSelect1");
                Assert.True(students.Count() > 0);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Fact]
        public void CanReturnIEnumerableWithoutStorename()
        {
            try
            {
                IEnumerable<Student> students = DatabaseConnection.Exec<Student>("");
                Assert.True(students.Count() > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [Fact]
        public void CanReturnObject()
        {
            Student student = DatabaseConnection.ExecSingle<Student>("spStudentSelectByID", new { ID = 1 });
            Assert.True(student != null);
        }

        [Fact]
        public void CanReturnObjectToManyArgument()
        {
            Student student = DatabaseConnection.ExecSingle<Student>("spStudentSelectByID", new { ID = 1, LastName = "Nam" });
            Assert.True(student != null);
        }

        [Fact]
        public void CanReturnObjectWithStoreHasWhiteSpace()
        {
            try
            {
                Student student = DatabaseConnection.ExecSingle<Student>("spStudentSelectByID ", new { ID = 1 });
                Assert.True(student != null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Fact]
        public async void CanReturnSingleObjectWithAsync()
        {
            Student student = await DatabaseConnection.ExecSingleAsync<Student>("spStudentSelectByID", new { ID = 1 });
            Assert.True(student != null);
        }

        [Fact]
        public async void CanReturnSingleObjectWithExecNonAsync()
        {
            int total = await DatabaseConnection.ExecNonQueryAsync("spStudentInsert", new { ID = 111, FirstName = "Ama", LastName = "Francis", EnrollmentDate = DateTime.Parse("2001-09-03") });
            Assert.True(total > 0);
        }

        //CUONGNM
        [Fact]
        public void CanReturnIntWithExecNonQueryInsert()
        {
            try{
                int student = DatabaseConnection.ExecNonQuery("spStudentExecNonQueryInsert", new { FirstMidName = "Cuong", LastName = "Nguyen", EnrollmentDate = DateTime.Now });
                Assert.True(student > 0);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [Fact]
        public void CanReturnIntWithExecNonQueryUpdate()
        {
            try
            {
                int student = DatabaseConnection.ExecNonQuery("spStudentExecNonQueryUpdate", new { @ID = 1, @FirstMidName = "Nguyen Manh Cuong" });
                Assert.True(student > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        [Fact]
        public void CanReturnIntWithExecNonQueryDelete()
        {
            try
            {
                int student = DatabaseConnection.ExecNonQuery("spStudentExecNonQueryDelete", new { @ID = 11 });
                Assert.True(student > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        [Fact]
        public void CanReturnIntWithStoreHasParamError()
        {
            try
            {
                int student = DatabaseConnection.ExecNonQuery("spStudentExecNonQueryInsert ", new {LastName = "Nguyen", FirstMidName = "Cuong", EnrollmentDate = DateTime.Now });
                Assert.True(student > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Fact]
        public void CanReturnIntWithStoreParaNull()
        {
            try
            {
                int student = DatabaseConnection.ExecNonQuery("spStudentExecNonQueryInsert", new {});
                Assert.True(student > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Fact]
        public async void CanReturnIntWithExecNonQueryAsync()
        {
            int students = 0;
            await Task.Run(async () =>
            {
                students = await DatabaseConnection.ExecNonQueryAsync("spStudentExecNonQueryInsert", new { FirstMidName = "Cuong", LastName = "Nguyen", EnrollmentDate = DateTime.Now });
                await Task.Delay(5000);
            });
            students = students + 1;
            Assert.True(students > 0);
        }

        //[Fact]
        //public async void CanReturnIntWithQuerySingleAsync()
        //{
        //    Student students;
        //    await Task.Run(async () =>
        //    {
        //        students = await DatabaseConnection.QuerySingleAsync<T>("spStudentExecNonQueryInsert", new { FirstMidName = "Cuong", LastName = "Nguyen", EnrollmentDate = DateTime.Now });
        //        await Task.Delay(5000);
        //    });
        //    students = students + 1;
        //    Assert.True(students > 0);
        //}



    }
}
