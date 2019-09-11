using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ToDo.Infrastructure
{
    class ToDoItemConfiguration : IEntityTypeConfiguration<ToDoItem>
    {
        public void Configure(EntityTypeBuilder<ToDoItem> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Description)
                    .IsRequired();

            builder.HasOne(x => x.ToDoList)
                .WithMany(list => list.Items)
                .HasForeignKey(item => item.ToDoListId);
        }
    }
}
