using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDo.Infrastructure.Migrations
{
    public partial class Changedemailtoownerandmadeitmandatory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "ToDoLists");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "ToDoLists",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "ToDoLists");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ToDoLists",
                nullable: true);
        }
    }
}
