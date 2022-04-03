using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Data.Migrations
{
    public partial class AddBiographyBirthdayAndDeathDayColumnsOnPersonModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Biography",
                table: "Persons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Birthday",
                table: "Persons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Deathday",
                table: "Persons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Biography",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Deathday",
                table: "Persons");
        }
    }
}
