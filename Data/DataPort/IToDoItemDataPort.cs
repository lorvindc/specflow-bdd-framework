using Data.Models;

namespace Data.DataPort
{
    /// <summary>
    /// Interface for the ToDoItem data port
    /// </summary>
    public interface IToDoItemDataPort
    {
        /// <summary>
        /// Gets the ToDoItems
        /// </summary>
        /// <returns>ToDoItems</returns>
        IEnumerable<ToDoItem> GetToDoItems();

        /// <summary>
        /// Gets the ToDoItem based on specified ID
        /// </summary>
        /// <param name="id">ID of the ToDoItem</param>
        /// <returns>ToDoItem</returns>
        ToDoItem GetToDoItem(long id);

        /// <summary>
        /// Create a ToDoItem
        /// </summary>
        /// <param name="toDoItem">ToDoItem</param>
        /// <returns>Created ToDoItem</returns>
        ToDoItem CreateToDoItem(ToDoItem toDoItem);

        /// <summary>
        /// Register list of ToDoItems
        /// </summary>
        /// <param name="toDoItems">List of ToDoItem</param>
        void RegisterToDoItems(List<ToDoItem> toDoItems);
    }
}
