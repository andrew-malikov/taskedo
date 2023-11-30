using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taskedo.Tasks.Database.Changes.Migrations
{
    /// <inheritdoc />
    public partial class Add_Task_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: false, defaultValue: ""),
                    DueDateAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.TaskId);
                });

            migrationBuilder.CreateIndex(
                name: "CreatedAtUtc",
                table: "Task",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IsDeleted",
                table: "Task",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Task");
        }
    }
}
