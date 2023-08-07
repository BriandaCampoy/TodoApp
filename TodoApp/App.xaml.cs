using System.Net.Http;
using TodoApp.Services;

namespace TodoApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyService.Register<ICategoryService, CategoryService>();
            DependencyService.Register<ITodoService, TodoService>();
            MainPage = new AppShell();
        }
    }
}