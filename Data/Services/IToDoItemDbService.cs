using Data.Models;
using Data.Models.DTO;

namespace Data.Services
{
    /// <summary>
    /// Interface for ToDoItem Database Service
    /// </summary>
    public interface IToDoItemDbService
    {
        /// <summary>
        /// Gets all ToDoItemsDTO
        /// </summary>
        /// <returns>List of all ToDoItemDto</returns>
        IEnumerable<ToDoItemDto> GetToDoItems();

        /// <summary>
        /// Get ToDoItemDTO based on specified ID
        /// </summary>
        /// <param name="id">ToDoItem identifier</param>
        /// <returns>ToDoItemDto</returns>
        ToDoItemDto GetToDoItem(long id);

        /// <summary>
        /// Get ToDoItemDTO based on specified ID
        /// </summary>
        /// <param name="toDoItem">ToDoItem</param>
        /// <returns>ToDoItemDto</returns>
        ToDoItemDto CreateToDoItem(ToDoItem? toDoItem);
    }
}
