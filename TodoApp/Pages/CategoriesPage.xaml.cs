using TodoApp.ViewModels;

namespace TodoApp.Pages;

public partial class CategoriesPage : ContentPage
{
	public CategoriesPage()
	{
		InitializeComponent();

    }

    /// <summary>
    /// Event handler for adding a new category.
    /// </summary>
    private async void handleNewCategory(object sender, EventArgs e)
	{
		string result = await DisplayPromptAsync("New category", "Enter a name for the new category");

		if(result != null && BindingContext is CategoriesViewModel viewModel)
		{
            await viewModel.AddCategory(result);
		}

    }
}