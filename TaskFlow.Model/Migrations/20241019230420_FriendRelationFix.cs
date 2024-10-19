using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskFlow.Model.Migrations
{
    /// <inheritdoc />
    public partial class FriendRelationFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRelation_AspNetUsers_User1Id",
                table: "FriendRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRelation_AspNetUsers_User2Id",
                table: "FriendRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FriendRelation",
                table: "FriendRelation");

            migrationBuilder.RenameTable(
                name: "FriendRelation",
                newName: "FriendRelations");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRelation_User2Id",
                table: "FriendRelations",
                newName: "IX_FriendRelations_User2Id");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRelation_User1Id",
                table: "FriendRelations",
                newName: "IX_FriendRelations_User1Id");

            migrationBuilder.AlterColumn<bool>(
                name: "Accepted",
                table: "FriendRelations",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FriendRelations",
                table: "FriendRelations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRelations_AspNetUsers_User1Id",
                table: "FriendRelations",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRelations_AspNetUsers_User2Id",
                table: "FriendRelations",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRelations_AspNetUsers_User1Id",
                table: "FriendRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRelations_AspNetUsers_User2Id",
                table: "FriendRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FriendRelations",
                table: "FriendRelations");

            migrationBuilder.RenameTable(
                name: "FriendRelations",
                newName: "FriendRelation");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRelations_User2Id",
                table: "FriendRelation",
                newName: "IX_FriendRelation_User2Id");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRelations_User1Id",
                table: "FriendRelation",
                newName: "IX_FriendRelation_User1Id");

            migrationBuilder.AlterColumn<bool>(
                name: "Accepted",
                table: "FriendRelation",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FriendRelation",
                table: "FriendRelation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRelation_AspNetUsers_User1Id",
                table: "FriendRelation",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRelation_AspNetUsers_User2Id",
                table: "FriendRelation",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
