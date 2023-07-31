using Data.Models;
using Data.Models.Mapper;
using FluentAssertions;
using NUnit.Framework;

namespace Data.Tests.Unit.Models.Mapper
{
    public class DtoMapperTests
    {
        [Test]
        public void ToDto_NullToDoItem_ReturnsNull()
        {
            var sut = (null as ToDoItem)!.ToDto();
            sut.Should().BeNull();
        }

        [Test]
        public void ToDto_ValidToDoItem_ReturnsToDoItemDto()
        {
            var toDoItem = new ToDoItem
            {
                Id = 1,
                Name = "Sample Name",
                IsComplete = false
            };

            var sut = toDoItem.ToDto();
            sut.Should().NotBeNull();
            sut.Id.Should().Be(toDoItem.Id);
            sut.Name.Should().Be(toDoItem.Name);
            sut.IsComplete.Should().BeFalse();

        }
    }
}
