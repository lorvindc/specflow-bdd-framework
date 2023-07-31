using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    /// <summary>
    /// Database context for the ToDoItems
    /// </summary>
    public class ToDoItemContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemContext"/> class.                                                         
        /// </summary>
        /// <param name="options">DB context options</param>
        public ToDoItemContext(DbContextOptions<ToDoItemContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the ToDoItems from Database
        /// </summary>
        public DbSet<ToDoItem> ToDoItems { get; set; } = null!;
    }
}
