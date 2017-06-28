using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace Euroland.NetCore.ToolsFramework.Data.Test
{
    public class MethodUtilsTest
    {
        string connectionString = "Server=localhost;Database=DapperDemo;user id=sa;password=123456;Trusted_Connection=true;";

        [Fact]
        public void CanReturnNoneQuery()
        {
            DapperDatabaseContext dbContext = new DapperDatabaseContext(connectionString);
            int products = dbContext.ExecNonQuery("spStudentInsert", new { Name = "Euroland", Quantity = 154, Price = 90 });
            Console.Write("Value is: ", products.ToString());
            Assert.True(products > 0);
        }

    }
}
