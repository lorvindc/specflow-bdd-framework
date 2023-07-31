using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    /// <summary>
    /// ToDoItem Model
    /// </summary>
    public class ToDoItem
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [Key]
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
