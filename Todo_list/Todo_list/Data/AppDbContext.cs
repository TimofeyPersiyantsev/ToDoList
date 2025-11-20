namespace Todo_list.Data;
using Microsoft.EntityFrameworkCore;
using Todo_list.Models;
using Todo_list.Models;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists { get; set; }
    public DbSet<TodoItem> TodoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoList>()
            .Property(tl => tl.Title)
            .IsRequired();

        modelBuilder.Entity<TodoItem>()
            .Property(ti => ti.Title)
            .IsRequired();

        modelBuilder.Entity<TodoItem>()
            .HasOne(ti => ti.TodoList)
            .WithMany(tl => tl.Items)
            .HasForeignKey(ti => ti.TodoListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
