using Data.DataPort;
using Data.Models.DTO;
using Data.Services;

namespace Specs.Drivers
{
    /// <summary>
    /// Client Driver for API testing
    /// </summary>
    public class ApiClientDriver : IClientDriver
    {
        private IToDoItemDbService toDoItemDbService = null!;
        private IEnumerable<ToDoItemDto> requestedToDoItems = new List<ToDoItemDto>();

        /// <inheritdoc/>
        public void SetUp(IToDoItemDataPort dataPort)
        {
            toDoItemDbService = new ToDoItemDbDbService(dataPort);
        }

        /// <inheritdoc/>
        public void RequestToDoItemList()
        {
            requestedToDoItems = toDoItemDbService.GetToDoItems();
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoItemDto> GetRequestedToDoItemList()
        {
            return requestedToDoItems.ToList();
        }

        /// <inheritdoc/>
        public void TearDown()
        {
            requestedToDoItems = null!;
        }
    }
}
