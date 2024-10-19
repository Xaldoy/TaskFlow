using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TaskFlow.Model.Migrations
{
    /// <inheritdoc />
    public partial class FriendRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendRelation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    User1Id = table.Column<string>(type: "text", nullable: false),
                    User2Id = table.Column<string>(type: "text", nullable: false),
                    Accepted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendRelation_AspNetUsers_User1Id",
                        column: x => x.User1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRelation_AspNetUsers_User2Id",
                        column: x => x.User2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendRelation_User1Id",
                table: "FriendRelation",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRelation_User2Id",
                table: "FriendRelation",
                column: "User2Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendRelation");
        }
    }
}
