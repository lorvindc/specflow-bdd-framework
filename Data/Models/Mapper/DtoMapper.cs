using Data.Models.DTO;

namespace Data.Models.Mapper
{
    /// <summary>
    /// Utility class to convert DB Model to Response DTO Model
    /// </summary>
    public static class DtoMapper
    {
        /// <summary>
        /// Converts ToDoItem to ToDoItemDto
        /// </summary>
        /// <param name="toDoItem">ToDoItem</param>
        /// <returns>ToDoItemDto</returns>
        public static ToDoItemDto ToDto(this ToDoItem toDoItem)
        {
            if (toDoItem == null)
            {
                return null!;
            }

            return new ToDoItemDto
            {
                Id = toDoItem.Id,
                Name = toDoItem.Name,
                IsComplete = toDoItem.IsComplete,
            };
        }
    }
}
