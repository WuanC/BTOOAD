using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OOAD.Migrations
{
    /// <inheritdoc />
    public partial class up1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calenders_Users_UserID",
                table: "Calenders");

            migrationBuilder.DropIndex(
                name: "IX_Calenders_UserID",
                table: "Calenders");

            migrationBuilder.AlterColumn<int>(
                name: "CalenderID",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CalenderID",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Calenders_UserID",
                table: "Calenders",
                column: "UserID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Calenders_Users_UserID",
                table: "Calenders",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
