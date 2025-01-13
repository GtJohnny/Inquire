using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inquire.Data.Migrations
{
    /// <inheritdoc />
    public partial class userincomms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentaries_AspNetUsers_ApplicationUserId",
                table: "Commentaries");

            migrationBuilder.DropIndex(
                name: "IX_Commentaries_ApplicationUserId",
                table: "Commentaries");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Commentaries");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Commentaries",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Commentaries_UserId",
                table: "Commentaries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaries_AspNetUsers_UserId",
                table: "Commentaries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentaries_AspNetUsers_UserId",
                table: "Commentaries");

            migrationBuilder.DropIndex(
                name: "IX_Commentaries_UserId",
                table: "Commentaries");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Commentaries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Commentaries",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Commentaries_ApplicationUserId",
                table: "Commentaries",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaries_AspNetUsers_ApplicationUserId",
                table: "Commentaries",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
