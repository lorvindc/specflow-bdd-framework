using Data.DataPort;
using Data.Models;
using Data.Models.DTO;
using Data.Models.Mapper;

namespace Data.Services
{
    /// <summary>
    /// ToDoItem Database Service
    /// </summary>
    public class ToDoItemDbDbService : IToDoItemDbService
    {
        private readonly IToDoItemDataPort toDoItemDataPort;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemDbDbService"/> class.
        /// </summary>
        /// <param name="dataPort">Data Port</param>
        public ToDoItemDbDbService(IToDoItemDataPort? dataPort)
        {
            toDoItemDataPort = dataPort ?? throw new ArgumentNullException(nameof(dataPort));
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoItemDto> GetToDoItems()
        {
            var toDoItemList = toDoItemDataPort.GetToDoItems();

            return toDoItemList.Select(x => x.ToDto());
        }

        /// <inheritdoc />
        public ToDoItemDto GetToDoItem(long id)
        {
            var toDoItem = toDoItemDataPort.GetToDoItem(id);

            return toDoItem == null
                ? throw new Exception("ToDoItem ID is not found.")
                : toDoItem.ToDto();
        }

        /// <inheritdoc/>
        public ToDoItemDto CreateToDoItem(ToDoItem? toDoItem)
        {
            if (toDoItem == null)
            {
                throw new Exception("ToDoItem is null.");
            }

            var createdToDoItem = toDoItemDataPort.CreateToDoItem(toDoItem);

            return createdToDoItem?.ToDto()!;
        }
    }
}
