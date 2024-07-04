using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAuthenticationApp.Migrations
{
    /// <inheritdoc />
    public partial class AddForenameAndSurnameToKieranProjectUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Forename",
                table: "KieranProjectUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "KieranProjectUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Forename",
                table: "KieranProjectUsers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "KieranProjectUsers");
        }
    }
}
