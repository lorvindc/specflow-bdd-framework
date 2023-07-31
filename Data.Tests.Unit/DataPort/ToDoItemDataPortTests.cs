using Data.Context;
using Data.DataPort;
using Data.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Data.Tests.Unit.DataPort
{
    public class ToDoItemDbDbServiceTests
    {
        private DbContextOptions<ToDoItemContext> contextOptions = null!;

        private static readonly List<ToDoItem>? toDoItems = new()
        {
            new ToDoItem
            {
                Id = 1,
                Name = "TODO1",
                IsComplete = false
            },
            new ToDoItem
            {
                Id = 2,
                Name = "TODO2",
                IsComplete = true
            }
        };

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            contextOptions = new DbContextOptionsBuilder<ToDoItemContext>()
                .UseInMemoryDatabase("ReturnsInstance")
                .Options;
        }

        [SetUp]
        public void Setup()
        {
            using var context = new ToDoItemContext(contextOptions);

            context.ToDoItems.RemoveRange(context.ToDoItems);
            context.ToDoItems.AddRange(toDoItems!);
            context.SaveChanges();
        }

        [Test]
        public void NewToDoItemDataPort_ValidContext_ReturnsInstance()
        {
            var sut = MakeSut();

            sut.Should().NotBeNull();
            sut.Should().BeAssignableTo<ToDoItemDataPort>();
        }

        [Test]
        public void GetToDoItems_ReturnsAllToDoItems()
        {
            var sut = MakeSut();

            var actualResult = sut.GetToDoItems().ToList();

            actualResult.Should().NotBeNull();
            actualResult.Should().HaveCount(2);
            actualResult.Should().BeEquivalentTo(toDoItems);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void GetToDoItem_ValidItemId_ReturnsToDoItem(long id)
        {
            var sut = MakeSut();

            var actualResult = sut.GetToDoItem(id);

            actualResult.Should().NotBeNull();
            actualResult.Id.Should().Be(id);
            actualResult.Name.Should().Be(toDoItems!
                .FirstOrDefault(x => x.Id.Equals(id))?.Name);
            actualResult.IsComplete.Should().Be(toDoItems!
                .FirstOrDefault(x => x.Id.Equals(id))!.IsComplete);
        }

        [Test]
        public void GetToDoItem_NonExistingItemId_ReturnsNull()
        {
            var sut = MakeSut();

            var actualResult = sut.GetToDoItem(999);
            actualResult.Should().BeNull();
        }

        [Test]
        public void CreateToDoItem_ValidToDoItem_RegistersToDoItemInDb()
        {
            var newToDoItem = new ToDoItem
            {
                Id = 3,
                Name = "TODO3",
                IsComplete = false
            };

            var sut = MakeSut();

            var actualResult = sut.CreateToDoItem(newToDoItem);

            actualResult.Should().NotBeNull();
            actualResult.Should().BeEquivalentTo(newToDoItem);

            using var context = new ToDoItemContext(contextOptions);
            var toDoItemList = context.ToDoItems.ToList();
            var registeredItem = toDoItemList.FirstOrDefault(x => x.Id.Equals(newToDoItem.Id));
            registeredItem.Should().NotBeNull();
            registeredItem.Should().BeEquivalentTo(newToDoItem);
        }

        [Test]
        public void CreateToDoItem_NullToDoItem_ThrowsException()
        {
            var sut = MakeSut();

            Action act = () => sut.CreateToDoItem(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void CreateToDoItem_ToDoItemAlreadyExist_ThrowsException()
        {
            var existing = new ToDoItem
            {
                Id = 1,
                Name = "TODO1",
                IsComplete = false
            };

            var sut = MakeSut();

            Action act = () => sut.CreateToDoItem(existing);

            act.Should().Throw<Exception>().WithMessage("Item already exist");
        }

        private ToDoItemDataPort MakeSut()
        {
            var context = new ToDoItemContext(contextOptions);
            return new ToDoItemDataPort(context);
        }
    }
}
