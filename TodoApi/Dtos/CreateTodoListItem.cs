namespace TodoApi.Dtos;

public class CreateTodoListItem
{
    public long ItemId { get; set; }
    public string Name { get; set; }
    public bool Status { get; set; }
}