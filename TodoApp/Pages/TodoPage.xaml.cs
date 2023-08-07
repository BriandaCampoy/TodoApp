using TodoApp.ViewModels;
using TodoApp.Models;

namespace TodoApp.Pages;

// Attribute to allow passing the Todo object as a query parameter
[QueryProperty("Todo", "todo")]
public partial class TodoPage : ContentPage
{
    TodoViewModel todoViewModel;

    // Constructor for the TodoPage
    public TodoPage()
	{
		InitializeComponent();
        todoViewModel = new TodoViewModel();
        BindingContext = todoViewModel;
    }

    // Private property to store the Todo object received as a query parameter
    Todo _todo { get; set; }

    // Property to receive the Todo object as a query parameter and update the ViewModel
    public Todo Todo
    {
        get => _todo;

        set
        {
            if (_todo == value)
                return;

            _todo = value;
            todoViewModel.TodoText = _todo.TodoText;
            todoViewModel.Done = _todo.done;
            todoViewModel.IdCategory = _todo.CategoryId;
            todoViewModel.IdTodo = _todo.Id;
        }
    }


}