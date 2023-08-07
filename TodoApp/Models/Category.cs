using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Models
{
    /// <summary>
    /// Represents a single category in the application.
    /// </summary>
    [Serializable]
    public class Category : INotifyPropertyChanged
    {
         Guid _categoryId;

        /// <summary>
        /// Gets or sets the unique identifier for the category.
        /// </summary>
        public Guid CategoryId
        {
            get => _categoryId;
            set
            {
                if (_categoryId != value)
                {
                    _categoryId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryId)));
                }
            }
        }

        private string _name;

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
