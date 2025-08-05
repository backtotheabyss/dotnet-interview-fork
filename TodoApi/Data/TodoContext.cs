using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options) { }

    public DbSet<TodoList> TodoList { get; set; } = default!;
    public DbSet<TodoListItem> TodoListItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoListItem>()
            .HasKey(e => new { e.Id, e.ItemId });

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TodoList>()
        .HasMany(t => t.Items)
        .WithOne()
        .HasForeignKey(i => i.Id);
    }

}
