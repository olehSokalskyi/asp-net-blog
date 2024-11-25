using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class chat_owner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "chat_owner_id",
                table: "chats",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_chats_chat_owner_id",
                table: "chats",
                column: "chat_owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_chats_users_id",
                table: "chats",
                column: "chat_owner_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chats_users_id",
                table: "chats");

            migrationBuilder.DropIndex(
                name: "ix_chats_chat_owner_id",
                table: "chats");

            migrationBuilder.DropColumn(
                name: "chat_owner_id",
                table: "chats");
        }
    }
}
