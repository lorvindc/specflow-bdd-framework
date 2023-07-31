using Data.Context;
using Data.Models;

namespace Data.DataPort
{
    /// <summary>
    /// Data Port for ToDoItem
    /// </summary>
    public class ToDoItemDataPort : IToDoItemDataPort
    {
        private readonly ToDoItemContext toDoItemContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemDataPort"/> class.
        /// </summary>
        /// <param name="context">Database context</param>
        public ToDoItemDataPort(ToDoItemContext context)
        {
            toDoItemContext = context;
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoItem> GetToDoItems() => toDoItemContext.ToDoItems.ToList();

        /// <inheritdoc/>
        public ToDoItem GetToDoItem(long id) => toDoItemContext.ToDoItems
            .FirstOrDefault(x => x.Id.Equals(id))!;

        /// <inheritdoc/>
        public ToDoItem CreateToDoItem(ToDoItem toDoItem)
        {
            if (toDoItem == null)
            {
                throw new ArgumentNullException(nameof(toDoItem));
            }

            var toDoItemEntry = toDoItemContext.ToDoItems
                .FirstOrDefault(x => x.Id.Equals(toDoItem.Id));

            if (toDoItemEntry != null)
            {
                throw new Exception("Item already exist");
            }

            toDoItemContext.Add(toDoItem);
            toDoItemContext.SaveChanges();

            return GetToDoItem(toDoItem.Id);
        }

        /// <inheritdoc/>
        public void RegisterToDoItems(List<ToDoItem> toDoItems)
        {
            toDoItemContext.ToDoItems.RemoveRange(toDoItemContext.ToDoItems);
            toDoItemContext.ToDoItems.AddRange(toDoItems);
            toDoItemContext.SaveChanges();
        }
    }
}
