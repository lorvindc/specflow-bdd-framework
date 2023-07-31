using Data.Context;
using Data.DataPort;
using Data.Models;
using Data.Models.DTO;
using Data.Models.Mapper;
using Data.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Data.Tests.Unit.Services
{
    public class DtoMapperTests
    {
        private DbContextOptions<ToDoItemContext> contextOptions = null!;
        private readonly Mock<IToDoItemDataPort?> mockToDoItemDataPort = new();

        private static readonly List<ToDoItem> toDoItems = new()
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
            context.ToDoItems.AddRange(toDoItems);
            context.SaveChanges();
        }

        [Test]
        public void NewToDoItemDbDbService_ValidDataPort_ReturnsInstance()
        {
            var sut = MakeSut();

            sut.Should().NotBeNull();
            sut.Should().BeAssignableTo<ToDoItemDbDbService>();
        }

        [Test]
        public void NewToDoItemDbDbService_NullDataPort_ThrowsArgumentNullException()
        {
            Action act = () => _ = new ToDoItemDbDbService(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void GetToDoItems_ReturnsAllToDoItemsDTO()
        {
            mockToDoItemDataPort
                .Setup(x => x!.GetToDoItems())
                .Returns(toDoItems);

            var expectedToDoItemsDTO = new List<ToDoItemDto>
            {
                new()
                {
                    Id = 1,
                    Name = "TODO1",
                    IsComplete = false
                },
                new()
                {
                    Id = 2,
                    Name = "TODO2",
                    IsComplete = true
                }
            };

            var sut = MakeSut();

            var actualResult = sut.GetToDoItems().ToList();

            actualResult.Should().NotBeNull();
            actualResult.Should().HaveCount(2);
            actualResult.Should().BeEquivalentTo(expectedToDoItemsDTO);

            mockToDoItemDataPort.Verify(
                x => x!.GetToDoItems(),
                Times.Once);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void GetToDoItem_ValidItemId_ReturnsToDoItem(long id)
        {
            mockToDoItemDataPort
                .Setup(x => x!.GetToDoItem(It.IsAny<long>()))
                .Returns(toDoItems.FirstOrDefault(x => x.Id.Equals(id))!);
            var expectedToDoItemsDTO = new List<ToDoItemDto>
            {
                new()
                {
                    Id = 1,
                    Name = "TODO1",
                    IsComplete = false
                },
                new()
                {
                    Id = 2,
                    Name = "TODO2",
                    IsComplete = true
                }
            };

            var sut = MakeSut();

            var actualResult = sut.GetToDoItem(id);

            actualResult.Should().NotBeNull();
            actualResult.Id.Should().Be(id);
            actualResult.Name.Should().Be(expectedToDoItemsDTO
                .FirstOrDefault(x => x.Id.Equals(id))?.Name);
            actualResult.IsComplete.Should().Be(expectedToDoItemsDTO
                .FirstOrDefault(x => x.Id.Equals(id))!.IsComplete);

            mockToDoItemDataPort.Verify(
                x => x!.GetToDoItem(id),
                Times.Once);
        }

        [Test]
        public void GetToDoItem_NonExistingItemId_ThrowsException()
        {
            mockToDoItemDataPort
                .Setup(x => x!.GetToDoItem(It.IsAny<long>()))
                .Returns((ToDoItem)null!);
            
            var sut = MakeSut();

            Action act = () => sut.GetToDoItem(3);
            act.Should().Throw<Exception>().WithMessage("ToDoItem ID is not found.");
        }

        [Test]
        public void CreateToDoItem_ValidToDoItem_RegistersToDoItemInDb()
        {
            var someToDoItem = new ToDoItem
            {
                Id = 3,
                Name = "TODO3",
                IsComplete = false
            };

            mockToDoItemDataPort
                .Setup(x => x!.CreateToDoItem(It.IsAny<ToDoItem>()))
                .Returns(someToDoItem);

            var sut = MakeSut();

            var actualResult = sut.CreateToDoItem(someToDoItem);

            actualResult.Should().NotBeNull();
            actualResult.Should().BeEquivalentTo(someToDoItem.ToDto());

            mockToDoItemDataPort.Verify(
                x => x!.CreateToDoItem(someToDoItem),
                Times.Once);
        }

        [Test]
        public void CreateToDoItem_NullParameter_ThrowsException()
        {
            var sut = MakeSut();

            Action act = () => sut.CreateToDoItem(null);
            act.Should().Throw<Exception>().WithMessage("ToDoItem is null.");
        }

        [Test]
        public void CreateToDoItem_DataPortReturnsNull_ReturnsNull()
        {
            var someToDoItem = new ToDoItem
            {
                Id = 10,
                Name = "Sample Name",
                IsComplete = false
            };

            mockToDoItemDataPort
                .Setup(x => x!.CreateToDoItem(It.IsAny<ToDoItem>()))
                .Returns((ToDoItem)null!);

            var sut = MakeSut();

            var actualResult = sut.CreateToDoItem(someToDoItem);

            actualResult.Should().BeNull();

            mockToDoItemDataPort.Verify(
                x => x!.CreateToDoItem(someToDoItem),
                Times.Once);
        }

        private ToDoItemDbDbService MakeSut()
        {
            return new ToDoItemDbDbService(mockToDoItemDataPort.Object);
        }
    }
}
