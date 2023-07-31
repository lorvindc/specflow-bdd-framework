namespace Data.Models.DTO
{
    /// <summary>
    /// ToDoItem Response Model
    /// </summary>
    public class ToDoItemDto
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the IsComplete flag
        /// </summary>
        public bool IsComplete { get; set; }
    }
}
