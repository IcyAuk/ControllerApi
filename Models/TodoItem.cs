namespace TodoApi.Models;

public class TodoItem
{
    public long Id {get;set;} //property Id is automatically treated like PRIMARY KEY
    public string? Name {get;set;}
    public bool IsComplete {get;set;}
}