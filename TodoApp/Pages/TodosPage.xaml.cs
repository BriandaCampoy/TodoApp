using TodoApp.Models;
using TodoApp.ViewModels;

namespace TodoApp.Pages;

/// <summary>
/// Page that displays a list of todos based on the selected category.
/// </summary>
[QueryProperty("Category","category")]
public partial class TodosPage : ContentPage
{
	TodosViewModel todosViewModel;
	public TodosPage()
	{
		InitializeComponent();

        todosViewModel= new TodosViewModel();
		BindingContext = todosViewModel;

    }


    /// <summary>
    /// Gets or sets the selected category passed as a query parameter.
    /// </summary>
    Category _category { get; set; }

	public Category Category
	{
		get => _category;

		set
		{
			if (_category == value)
				return;

			_category = value;
			todosViewModel.CategoryId = _category.CategoryId;
			todosViewModel.CategoryName = _category.Name;
		}
	}

    /// <summary>
    /// Event handler for adding a new todo item.
    /// </summary>
    private async void handleAddTodo(object sender, EventArgs e)
	{
        string result = await DisplayPromptAsync("New Todo", "Enter the new todo");

        if (result !=null && BindingContext is TodosViewModel viewModel)
        {
            await viewModel.AddTodo(result, Category.CategoryId);
        }
    }


}