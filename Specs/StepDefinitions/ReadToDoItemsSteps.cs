using Data.DataPort;
using Data.Models;
using Data.Models.DTO;
using Specs.Drivers;

namespace Specs.StepDefinitions
{
    [Binding]
    public sealed class ReadToDoItemsSteps
    {
        private readonly IClientDriver clientDriver;
        private readonly IToDoItemDataPort dataPort;

        public ReadToDoItemsSteps(IClientDriver clientDriver, IToDoItemDataPort dataPort)
        {
            this.clientDriver = clientDriver;
            this.dataPort = dataPort;
        }

        [Given(@"a list of To Do Items")]
        public void GivenAListOfToDoItems(Table table)
        {
            var toDoItems = table.Rows
                .Select(row =>
                {
                    var toDoItem = new ToDoItem
                    {
                        Id = long.Parse(row["Id"]),
                        Name = row["Name"],
                        IsComplete = bool.Parse(row["Completed"])
                    };
                    return toDoItem;
                })
                .ToList();

            dataPort.RegisterToDoItems(toDoItems);
        }

        [When(@"the list of To Do Items are requested")]
        public void WhenTheListOfToDoItemsAreRequested()
        {
            clientDriver.RequestToDoItemList();
        }

        [Then(@"the list of To Do Items are displayed")]
        public void ThenTheListOfToDoItems(Table table)
        {
            var expectedResult = table.Rows
                .Select(row => new ToDoItemDto()
                {
                    Id = long.Parse(row["Id"]),
                    Name = row["Name"],
                    IsComplete = bool.Parse(row["Completed"])
                })
                .ToList();

            var actualResult = clientDriver.GetRequestedToDoItemList();
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

    }
}