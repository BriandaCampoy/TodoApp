using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Models
{
    /// <summary>
    /// Represents a single Todo item in the application.
    /// </summary>
    public class Todo
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Todo item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the text content of the Todo item.
        /// </summary>
        public string TodoText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Todo item is marked as done (completed) or not.
        /// </summary>
        public bool done { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the category to which this Todo item belongs.
        /// </summary>
        public Guid CategoryId { get; set; }

    }
}
