using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Infrastructure
{
    class ToDoListShareConfiguration : IEntityTypeConfiguration<ToDoListShare>
    {
        public void Configure(EntityTypeBuilder<ToDoListShare> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.ExpiresOn)
                .IsRequired();

            builder.HasOne(s => s.ToDoList)
                .WithMany(l => l.ToDoListShares)
                .HasForeignKey(s => s.ToDoListId);
        }
    }
}
