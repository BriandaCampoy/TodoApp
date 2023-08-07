using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoApp.Services;
using TodoApp.Models;


namespace TodoApp.ViewModels
{

    /// <summary>
    /// ViewModel for managing individual Todo items and interacting with the UI.
    /// </summary>
    internal class TodoViewModel: INotifyPropertyChanged
    {
        /// <summary>
        /// Event that is triggered when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Service for managing Todo data.
        /// </summary>
        public ITodoService TodoService = DependencyService.Resolve<ITodoService>();

        /// <summary>
        /// Command to be executed when deleting a Todo item.
        /// </summary>
        public ICommand deleteTodoCommand { get; private set; }

        /// <summary>
        /// Command to be executed when editing a Todo item.
        /// </summary>
        public ICommand editTodoCommand { get; private set; }

        /// <summary>
        /// Command to be executed when marking a Todo item as done.
        /// </summary>
        public ICommand doneCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the TodoViewModel class.
        /// </summary>
        public TodoViewModel()
        {
            deleteTodoCommand = new Command(async () => await deleteTodo());
            editTodoCommand = new Command(async () => await editTodo());
            doneCommand = new Command(async () => await doneTodo());
        }

        string _todoText;

        /// <summary>
        /// Gets or sets the text of the Todo item.
        /// </summary>
        public string TodoText
        {
            get => _todoText;
            set
            {
                if (_todoText == value)
                    return;

                _todoText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TodoText)));
            }
        }

        bool _done;

        /// <summary>
        /// Gets or sets a value indicating whether the Todo item is done.
        /// </summary>
        public bool Done
        {
            get => _done;
            set
            {
                if (_done  == value)
                    return;

                _done = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Done)));
            }
        }

        Guid _idTodo;
        /// <summary>
        /// Gets or sets the ID of the Todo item.
        /// </summary>
        public Guid IdTodo
        {
            get => _idTodo;
            set
            {
                if (_idTodo == value)
                    return;

                _idTodo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IdTodo)));
            }
        }

        Guid _idCategory;

        /// <summary>
        /// Gets or sets the ID of the Category to which the Todo item belongs.
        /// </summary>
        public Guid IdCategory
        {
            get => _idCategory;
            set
            {
                if (_idCategory == value)
                    return;

                _idCategory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IdCategory)));
            }
        }

        /// <summary>
        /// Marks the Todo item as done and updates it in the TodoService.
        /// </summary>
        public async Task doneTodo()
        {
            Todo todo = new Todo();
            todo.TodoText = _todoText;
            todo.done = true;
            todo.CategoryId = _idCategory;
            todo.Id = _idTodo;

            await TodoService.Update(todo);

            MessagingCenter.Send(this, "refresh");

            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Edits the Todo item and updates it in the TodoService.
        /// </summary>
        public async Task editTodo()
        {
            Todo todo = new Todo();
            todo.TodoText = _todoText;
            todo.done = _done;
            todo.CategoryId = _idCategory;
            todo.Id = _idTodo;

            await TodoService.Update(todo);

            MessagingCenter.Send(this, "refresh");

            await Shell.Current.GoToAsync("..");

        }

        /// <summary>
        /// Deletes the Todo item from the TodoService.
        /// </summary>
        public async Task deleteTodo()
        {
            await TodoService.Delete(IdTodo);

            MessagingCenter.Send(this, "refresh");

            await Shell.Current.GoToAsync("..");
        }



    }
}
