using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoApp.Models;
using TodoApp.Services;
using Microsoft.Maui.Controls;

namespace TodoApp.ViewModels
{
    /// <summary>
    /// ViewModel for managing Categories and interacting with the UI.
    /// </summary>
    internal class CategoriesViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event that is triggered when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Command to be executed when a Category item is selected.
        /// </summary>
        public ICommand SelectCategoryCommand { get; private set; }

        /// <summary>
        /// Command to be executed when adding a new Category item.
        /// </summary>
        public ICommand NewCategoryCommand { get; private set; }


        /// <summary>
        /// Service for managing Category data.
        /// </summary>
        public ICategoryService CategoryService = DependencyService.Resolve<ICategoryService>();
        
        private ObservableCollection<Category> _categories;

        /// <summary>
        /// Gets or sets the list of Categories.
        /// </summary>
        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set => _categories = value;
        }

        private bool _isRefreshing = false;

        /// <summary>
        /// Gets or sets a value indicating whether the Categories list is being refreshed.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether the ViewModel is busy performing an operation.
        /// </summary>
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

        private Category _selectedCategory;

        /// <summary>
        /// Gets or sets the currently selected Category item.
        /// </summary>
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory == value)
                    return;

                _selectedCategory = value;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCategory)));
            }
        }

        /// <summary>
        /// Initializes a new instance of the CategoriesViewModel class.
        /// </summary>
        public CategoriesViewModel()
        {
            SelectCategoryCommand = new Command(async()=> await CategorySelected());
            _categories = new ObservableCollection<Category>();

            MessagingCenter.Subscribe<TodosViewModel>(this, "refresh", async (sender) => await LoadCategories());
            Task.Run(LoadCategories);
        }


        /// <summary>
        /// Command handler for selecting a Category item.
        /// </summary>
        private async Task CategorySelected()
        {
            if (SelectedCategory == null)
                return;

            var navigationParameter = new Dictionary<string, object>()
            {
                { "category", SelectedCategory }
            };

            await Shell.Current.GoToAsync("category", navigationParameter);

            MainThread.BeginInvokeOnMainThread(() => SelectedCategory = null);
        }
        


        /// <summary>
        /// Loads the list of Categories from the CategoryService.
        /// </summary>
        public async Task LoadCategories()
        {
            if (IsBusy)
                return;

            try
            {
                IsRefreshing = true;
                IsBusy = true;

                var categoryCollection = await CategoryService.GetCategories();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Categories.Clear();

                    foreach(Category category in categoryCollection)
                    {
                        Categories.Add(category);
                    }
                });

            }catch (Exception ex)
            {
                
            }finally {
                IsRefreshing = false;
                IsBusy = false;
            }

        }

        /// <summary>
        /// Adds a new Category item to the CategoryService.
        /// </summary>
        /// <param name="newCategory">The name of the new Category item.</param>
        public async Task AddCategory(string newCategory)
        {
            await CategoryService.Add(newCategory);

            MessagingCenter.Send(this, "refresh");

            Task.Run(LoadCategories);
        }

    }
}
