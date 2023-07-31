using Data.Context;
using Data.Models;
using Data.Models.DTO;
using Data.Models.Mapper;
using Data.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using RestApi.Controllers;

namespace RestApi.Tests.Unit.Controllers
{
    public class ToDoItemsControllerTests
    {
        private DbContextOptions<ToDoItemContext> contextOptions = null!;
        private readonly Mock<IToDoItemDbService> mockToDoItemDbService = new();

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

        [TearDown]
        public void TearDown()
        {
            mockToDoItemDbService.Invocations.Clear();
        }

        [Test]
        public void NewToDoItemsController_ValidService_ReturnsInstance()
        {
            var sut = MakeSut();
            sut.Should().NotBeNull();
        }

        [Test]
        public void NewToDoItemsController_NullService_ThrowsArgumentNullException()
        {
            Action act = () => _ = new ToDoItemsController(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void GetToDoItems_ReturnsAllToDoItemsDTO()
        {
            mockToDoItemDbService
                .Setup(x => x!.GetToDoItems())
                .Returns(toDoItems.Select(x => x.ToDto()));

            var sut = MakeSut();

            var actualResult = sut.GetToDoItems().Result;

            var toDoItemDTOs = ((OkObjectResult)actualResult!).Value;
            toDoItemDTOs.Should().NotBeNull();
            toDoItemDTOs.Should().BeAssignableTo<IEnumerable<ToDoItemDto>>();
            toDoItemDTOs.Should().BeEquivalentTo(toDoItems.Select(x => x.ToDto()));

            mockToDoItemDbService.Verify(
                x => x!.GetToDoItems(),
                Times.Once);
        }

        [Test]
        public void GetToDoItems_DbServiceThrowsException_ThrowsException()
        {
            mockToDoItemDbService
                .Setup(x => x!.GetToDoItems())
                .Throws<Exception>();

            var sut = MakeSut();
            var actualResult = sut.GetToDoItems().Result;

            var toDoItemDTOs = ((ObjectResult)actualResult!).Value;
            toDoItemDTOs.Should().NotBeNull();
            toDoItemDTOs.Should().BeAssignableTo<Exception>();
            (actualResult as ObjectResult)!.StatusCode.Should().Be(500);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void GetToDoItem_ValidItemId_ReturnsToDoItem(long id)
        {
            mockToDoItemDbService
                .Setup(x => x!.GetToDoItem(It.IsAny<long>()))
                .Returns(toDoItems.FirstOrDefault(x => x.Id.Equals(id))!.ToDto());

            var sut = MakeSut();

            var actualResult = sut.GetToDoItem(id).Result;
            var toDoItemDTO = ((OkObjectResult)actualResult!).Value as ToDoItemDto;

            toDoItemDTO.Should().NotBeNull();
            toDoItemDTO.Should().BeAssignableTo<ToDoItemDto>();
            toDoItemDTO?.Id.Should().Be(id);
            toDoItemDTO?.Name.Should().Be(toDoItems
                .FirstOrDefault(x => x.Id.Equals(id))?.Name);
            toDoItemDTO?.IsComplete.Should().Be(toDoItems
                .FirstOrDefault(x => x.Id.Equals(id))!.IsComplete);


            mockToDoItemDbService.Verify(
                x => x!.GetToDoItem(id),
                Times.Once);
        }

        [Test]
        public void GetToDoItem_DbServiceThrowsException_ThrowsException()
        {
            mockToDoItemDbService
                .Setup(x => x!.GetToDoItem(It.IsAny<long>()))
                .Throws<Exception>();

            var sut = MakeSut();
            var actualResult = sut.GetToDoItem(1).Result;

            var toDoItemDTO = ((ObjectResult)actualResult!).Value;
            toDoItemDTO.Should().NotBeNull();
            toDoItemDTO.Should().BeAssignableTo<Exception>();
            (actualResult as ObjectResult)!.StatusCode.Should().Be(500);
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

            mockToDoItemDbService
                .Setup(x => x!.CreateToDoItem(It.IsAny<ToDoItem>()))
                .Returns(someToDoItem.ToDto());

            var sut = MakeSut();

            var actualResult = sut.CreateToDoItem(someToDoItem).Result;
            var toDoItemDTO = ((OkObjectResult)actualResult!).Value as ToDoItemDto;

            toDoItemDTO.Should().NotBeNull();
            toDoItemDTO.Should().BeAssignableTo<ToDoItemDto>();
            toDoItemDTO.Should().BeEquivalentTo(someToDoItem.ToDto());

            mockToDoItemDbService.Verify(
                x => x!.CreateToDoItem(someToDoItem),
                Times.Once);
        }
        [Test]
        public void CreateToDoItem_DbServiceThrowsException_ThrowsException()
        {
            mockToDoItemDbService
                .Setup(x => x!.CreateToDoItem(It.IsAny<ToDoItem>()))
                .Throws<Exception>();

            var sut = MakeSut();
            var actualResult = sut.CreateToDoItem(new ToDoItem()).Result;

            var toDoItemDTO = ((ObjectResult)actualResult!).Value;
            toDoItemDTO.Should().NotBeNull();
            toDoItemDTO.Should().BeAssignableTo<Exception>();
            (actualResult as ObjectResult)!.StatusCode.Should().Be(500);
        }

        private ToDoItemsController MakeSut()
        {
            return new ToDoItemsController(mockToDoItemDbService.Object);
        }
    }
}
