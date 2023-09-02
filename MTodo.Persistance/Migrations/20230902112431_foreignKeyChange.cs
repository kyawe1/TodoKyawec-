using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTodo.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class foreignKeyChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_todos_users_UserId",
                table: "todos");

            migrationBuilder.DropIndex(
                name: "IX_todos_UserId",
                table: "todos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "todos");

            migrationBuilder.CreateIndex(
                name: "IX_todos_CreatedUser",
                table: "todos",
                column: "CreatedUser");

            migrationBuilder.AddForeignKey(
                name: "FK_todos_users_CreatedUser",
                table: "todos",
                column: "CreatedUser",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_todos_users_CreatedUser",
                table: "todos");

            migrationBuilder.DropIndex(
                name: "IX_todos_CreatedUser",
                table: "todos");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "todos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_todos_UserId",
                table: "todos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_todos_users_UserId",
                table: "todos",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
