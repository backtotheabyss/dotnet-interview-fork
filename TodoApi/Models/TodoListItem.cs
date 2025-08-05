using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;

public class TodoListItem
{
    public required long Id { get; set; }
    public required long ItemId { get; set; }
    public string Name { get; set; }
    public bool Status { get; set; }
}
