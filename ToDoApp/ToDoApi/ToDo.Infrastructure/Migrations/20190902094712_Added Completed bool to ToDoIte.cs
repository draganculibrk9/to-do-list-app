using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDo.Infrastructure.Migrations
{
    public partial class AddedCompletedbooltoToDoIte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "ToDoItems",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "ToDoItems");
        }
    }
}
