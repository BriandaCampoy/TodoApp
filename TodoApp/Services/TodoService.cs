using Newtonsoft.Json;
using System.Net.Http.Json;
using System;
using System.Text;
using TodoApp.Models;

namespace TodoApp.Services;

/// <summary>
/// Service for managing Todo items.
/// </summary>
public class TodoService : ITodoService
{
    static HttpClient client;
    string url = Environment.GetEnvironmentVariable("URL_BASE");

    /// <summary>
    /// Initializes a new instance of the TodoService class.
    /// </summary>
    public TodoService()
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
    /// Retrieves a list of all Todo items from the server.
    /// </summary>
    public async Task<IEnumerable<Todo>> GetTodos()
    {
        try
        {
            HttpClient client = await GetClient();
            var response = await client.GetAsync($"{url}/Todo");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Todo>>(content);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    /// <summary>
    /// Retrieves a list of Todo items that belong to the specified category from the server.
    /// </summary>
    /// <param name="categoryId">The ID of the category.</param>
    public async Task<IEnumerable<Todo>> GetTodosByCategory(Guid categoryId)
    {
        try
        {
            HttpClient client = await GetClient();
            var response = await client.GetAsync($"{url}/Todo/Category/{categoryId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Todo>>(content);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    /// <summary>
    /// Adds a new Todo item to the server.
    /// </summary>
    /// <param name="todoText">The text of the new Todo item.</param>
    /// <param name="CategoryId">The ID of the category to which the Todo item belongs.</param>
    public async Task<Todo> AddTodo(string todoText, Guid CategoryId)
    {
        Todo todo = new Todo()
        {
            TodoText = todoText,
            done = false,
            CategoryId = CategoryId
        };
        HttpClient client = await GetClient();
        var msg = new HttpRequestMessage(HttpMethod.Post, $"{url}/Todo");
        string json = JsonConvert.SerializeObject(todo);
        msg.Content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var returnedJson = await response.Content.ReadAsStringAsync();

        var insertedPart = JsonConvert.DeserializeObject<Todo>(returnedJson);
        return insertedPart;
    }

    /// <summary>
    /// Updates an existing Todo item on the server.
    /// </summary>
    /// <param name="todo">The Todo item with updated values.</param>
    public async Task Update(Todo todo)
    {
        HttpRequestMessage msg = new(HttpMethod.Put, $"{url}/Todo/{todo.Id}");
        msg.Content = JsonContent.Create<Todo>(todo);
        HttpClient client = await GetClient();
        var response = await client.SendAsync(msg);
        response.EnsureSuccessStatusCode();

    }

    /// <summary>
    /// Deletes a Todo item with the specified ID from the server.
    /// </summary>
    /// <param name="idTodo">The ID of the Todo item to delete.</param>
    public async Task Delete(Guid idTodo)
    {
        HttpRequestMessage msg = new(HttpMethod.Delete, $"{url}/Todo/{idTodo}");
        HttpClient client = await GetClient();

        var response = await client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
    }


}
/// <summary>
/// Interface for the TodoService class.
/// </summary>
public interface ITodoService
{
    Task<IEnumerable<Todo>> GetTodos();

    Task<IEnumerable<Todo>> GetTodosByCategory(Guid categoryId);

    Task<Todo> AddTodo(string todoText, Guid CategoryId);

    Task Update(Todo todo);

    Task Delete(Guid idTodo);

}


