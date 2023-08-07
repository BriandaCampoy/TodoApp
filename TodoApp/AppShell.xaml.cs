using TodoApp.Pages;

namespace TodoApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("category", typeof(TodosPage));
            Routing.RegisterRoute("todo", typeof(TodoPage));
        }
    }
}