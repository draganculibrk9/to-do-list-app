﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ToDo.Infrastructure;

namespace ToDo.Infrastructure.Migrations
{
    [DbContext(typeof(ToDoDbContext))]
    [Migration("20190910124513_Added ToDoListShare")]
    partial class AddedToDoListShare
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.ToDoItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Completed");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("Position");

                    b.Property<Guid>("ToDoListId");

                    b.HasKey("Id");

                    b.HasIndex("ToDoListId");

                    b.ToTable("ToDoItems");
                });

            modelBuilder.Entity("Core.ToDoList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Owner")
                        .IsRequired();

                    b.Property<int>("Position");

                    b.Property<bool>("Reminded");

                    b.Property<DateTime>("ReminderDate");

                    b.Property<string>("Title")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("ToDoLists");
                });

            modelBuilder.Entity("Core.ToDoListShare", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ExpiresOn");

                    b.Property<Guid>("ToDoListId");

                    b.HasKey("Id");

                    b.HasIndex("ToDoListId");

                    b.ToTable("ToDoListShares");
                });

            modelBuilder.Entity("Core.ToDoItem", b =>
                {
                    b.HasOne("Core.ToDoList", "ToDoList")
                        .WithMany("Items")
                        .HasForeignKey("ToDoListId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Core.ToDoListShare", b =>
                {
                    b.HasOne("Core.ToDoList", "ToDoList")
                        .WithMany("ToDoListShares")
                        .HasForeignKey("ToDoListId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
