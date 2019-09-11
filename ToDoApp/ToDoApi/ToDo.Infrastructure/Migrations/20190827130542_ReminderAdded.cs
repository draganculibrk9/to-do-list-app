using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDo.Infrastructure.Migrations
{
    public partial class ReminderAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Reminded",
                table: "ToDoLists",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reminded",
                table: "ToDoLists");
        }
    }
}
