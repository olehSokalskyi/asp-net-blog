using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Messages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chat_user_chat_chat_id",
                table: "chat_user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_chat",
                table: "chat");

            migrationBuilder.RenameTable(
                name: "chat",
                newName: "chats");

            migrationBuilder.AddPrimaryKey(
                name: "pk_chats",
                table: "chats",
                column: "id");

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "varchar(255)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_messages_chats_chat_id",
                        column: x => x.chat_id,
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_messages_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_messages_chat_id",
                table: "messages",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "ix_messages_user_id",
                table: "messages",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_chat_user_chats_chat_id",
                table: "chat_user",
                column: "chat_id",
                principalTable: "chats",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chat_user_chats_chat_id",
                table: "chat_user");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_chats",
                table: "chats");

            migrationBuilder.RenameTable(
                name: "chats",
                newName: "chat");

            migrationBuilder.AddPrimaryKey(
                name: "pk_chat",
                table: "chat",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_chat_user_chat_chat_id",
                table: "chat_user",
                column: "chat_id",
                principalTable: "chat",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
