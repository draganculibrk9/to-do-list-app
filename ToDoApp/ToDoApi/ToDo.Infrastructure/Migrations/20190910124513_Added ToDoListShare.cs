using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDo.Infrastructure.Migrations
{
    public partial class AddedToDoListShare : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDoListShares",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExpiresOn = table.Column<DateTime>(nullable: false),
                    ToDoListId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoListShares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoListShares_ToDoLists_ToDoListId",
                        column: x => x.ToDoListId,
                        principalTable: "ToDoLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoListShares_ToDoListId",
                table: "ToDoListShares",
                column: "ToDoListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDoListShares");
        }
    }
}
