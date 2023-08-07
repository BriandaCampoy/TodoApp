using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using TodoApp.Models;

namespace TodoApp.Services;

/// <summary>
/// Service for managing Category items.
/// </summary>
public class CategoryService : ICategoryService
{
    static HttpClient client;
    static string url = Environment.GetEnvironmentVariable("URL_BASE");

    /// <summary>
    /// Initializes a new instance of the CategoryService class.
    /// </summary>
    public CategoryService()
    {
    }

    private static async Task<HttpClient> GetClient()
    {
        if (client != null)
            return client;

        client = new HttpClient();
        return client;
    }

    /// <summary>
    /// Retrieves a list of all Category items from the server.
    /// </summary>
    public async Task<IEnumerable<Category>> GetCategories()
    {
        try
        {
            HttpClient client = await GetClient();
            var response = await client.GetAsync($"{url}/Category");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Category>>(content);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    /// <summary>
    /// Adds a new Category item to the server.
    /// </summary>
    /// <param name="CategoryName">The name of the new Category item.</param>
    public async Task<Category> Add(string CategoryName)
    {
        Category category = new Category()
        {
            Name = CategoryName
        };
        HttpClient client = await GetClient();
        var msg = new HttpRequestMessage(HttpMethod.Post, $"{url}/Category");
        string json = JsonConvert.SerializeObject(category);
        msg.Content = new StringContent(json, Encoding.UTF8, "application/json");
        //*JsonContent.Create<Category>(category).ToString();*/
        var response = await client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var returnedJson = await response.Content.ReadAsStringAsync();

        var insertedPart = JsonConvert.DeserializeObject<Category>(returnedJson);
        return insertedPart;
    }

    /// <summary>
    /// Deletes a Category item with the specified ID from the server.
    /// </summary>
    /// <param name="categoryId">The ID of the Category item to delete.</param>
    public async Task Delete(Guid categoryId)
    {
        HttpRequestMessage msg = new(HttpMethod.Delete, $"{url}/Category/{categoryId}");
        HttpClient client = await GetClient();

        var response = await client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
    }


}

/// <summary>
/// Interface for the CategoryService class.
/// </summary>
public interface ICategoryService
{
    Task<IEnumerable<Category>> GetCategories();

    Task<Category> Add(string CategoryName);

    Task Delete(Guid categoryId);
}

