using BoDi;
using Data.Context;
using Data.DataPort;
using Microsoft.EntityFrameworkCore;
using Specs.Drivers;
using Specs.Utils;

namespace Specs
{
    /// <summary>
    /// Class for managing the tests
    /// </summary>
    [Binding]
    public class TestsManager
    {
        private static IClientDriver clientDriver = null!;
        private static DbContextOptions<ToDoItemContext> toDoItemContextOptions = null!;
        private static IToDoItemDataPort toDoItemDataPort = null!;

        /// <summary>
        /// Setup method runs once before all tests are run
        /// </summary>
        /// <param name="objContainer">Object container</param>
        [BeforeTestRun]
        public static void TestSetup(IObjectContainer objContainer)
        {
            toDoItemContextOptions = new DbContextOptionsBuilder<ToDoItemContext>()
                .UseInMemoryDatabase(TestConfiguration.InMemoryDbName)
                .Options;
            
            var toDoItemContext = new ToDoItemContext(toDoItemContextOptions);

            toDoItemDataPort = new ToDoItemDataPort(toDoItemContext);
            objContainer.RegisterInstanceAs(toDoItemDataPort);

            clientDriver = TestConfiguration.TestMode switch
            {
                "API" => new ApiClientDriver(),
                "WebAPI" => new WebApiClientDriver(),
                "WebPortal" => new WebPortalClientDriver(),
                _ => throw new Exception("Invalid test mode")
            };
            
            clientDriver.SetUp(toDoItemDataPort);
            objContainer.RegisterInstanceAs(clientDriver);
        }

        /// <summary>
        /// Teardown method runs once after all tests are run
        /// </summary>
        [AfterTestRun]
        public static void TestTearDown()
        {
            clientDriver.TearDown();
        }
    }
}
