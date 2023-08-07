using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    /// <summary>
    /// ViewModel for managing Todos and interacting with the UI.
    /// </summary>
    internal class TodosViewModel: INotifyPropertyChanged
    {
        /// <summary>
        /// Event that is triggered when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Todo> _todos;

        public ITodoService TodoService = DependencyService.Resolve<ITodoService>();
        public ICategoryService categoryService = DependencyService.Resolve<ICategoryService>();

        /// <summary>
        /// Command to be executed when a Todo item is selected.
        /// </summary>
        public ICommand TodoSelectedCommand { get; private set; }

        /// <summary>
        /// Command to be executed when adding a new Todo item.
        /// </summary>
        public ICommand AddTodoCommand { get; private set; }

        /// <summary>
        /// Command to be executed when deleting a category.
        /// </summary>
        public ICommand DeleteCategoryCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the TodosViewModel class.
        /// </summary>
        public TodosViewModel() { 
            _todos = new ObservableCollection<Todo>();
            TodoSelectedCommand = new Command(async () => await TodoSelected());
            DeleteCategoryCommand = new Command(async () => await DeleteCategory());

            MessagingCenter.Subscribe<TodoViewModel>(this, "refresh", async (sender) => await LoadTodos());
        }

        public ObservableCollection<Todo> Todos
        {
            get => _todos;
            set => _todos = value;
        }

        public bool _isEmpty;
        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                if (_isEmpty == value)
                    return;

                _isEmpty = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEmpty)));
            }
        }

        Guid _categoryId;

        public Guid CategoryId
        {
            get => _categoryId;
            set
            {
                if (_categoryId == value)
                    return;

                _categoryId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryId)));
                Task.Run(LoadTodos);
            }
        }

        string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                if (_categoryName == value)
                    return;

                _categoryName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryName)));
            }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (_isRefreshing == value)
                    return;

                _isRefreshing = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRefreshing)));
            }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy == value)
                    return;

                _isBusy = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBusy)));
            }
        }

        
        private Todo _selectedTodo;
        public Todo SelectedTodo
        {
            get => _selectedTodo;
            set
            {
                if (_selectedTodo == value)
                    return;

                _selectedTodo = value;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTodo)));
            }
        }

        /// <summary>
        /// Loads the list of Todos based on the selected category.
        /// </summary>
        public async Task LoadTodos()
        {
            if (IsBusy)
                return;

            try
            {
                IsRefreshing = true;
                IsBusy = true;

                var todosCollection = await TodoService.GetTodosByCategory(CategoryId);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Todos.Clear();

                    foreach (Todo todo in todosCollection)
                    {
                        Todos.Add(todo);
                    }
                    IsEmpty = Todos.Count == 0;
                });
            }
            finally
            {
                IsRefreshing = false;
                IsBusy = false;
            }
        }

        /// <summary>
        /// Command handler for selecting a Todo item.
        /// </summary>
        private async Task TodoSelected()
        {
            if (SelectedTodo == null)
                return;

            var navigationParameter = new Dictionary<string, object>()
            {
                { "todo", SelectedTodo }
            };

            await Shell.Current.GoToAsync("todo", navigationParameter);

            MainThread.BeginInvokeOnMainThread(() => SelectedTodo = null);
        }

        /// <summary>
        /// Adds a new Todo item to the selected category.
        /// </summary>
        /// <param name="newTodo">The text of the new Todo item.</param>
        /// <param name="idCategory">The ID of the category for the new Todo item.</param>
        public async Task AddTodo(string newTodo, Guid idCategory)
        {
            await TodoService.AddTodo(newTodo, idCategory);

            MessagingCenter.Send(this, "refresh");

            Task.Run(LoadTodos);
        }

        /// <summary>
        /// Deletes the selected category and its associated Todos.
        /// </summary>
        public async Task DeleteCategory()
        {
            await categoryService.Delete(CategoryId);

            MessagingCenter.Send(this, "refresh");

            await Shell.Current.GoToAsync("..");

        }

    }

}
