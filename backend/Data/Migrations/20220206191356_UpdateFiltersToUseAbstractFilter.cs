using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Data.Migrations
{
    public partial class UpdateFiltersToUseAbstractFilter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Directors");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Directors",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Directors");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Directors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
