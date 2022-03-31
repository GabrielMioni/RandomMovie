using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Data.Migrations
{
    public partial class AddMoviePerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OriginalName = table.Column<string>(nullable: true),
                    KnownFor = table.Column<string>(nullable: true),
                    ProfilePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movie_Person",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(nullable: false),
                    PersonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie_Person", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movie_Person_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movie_Person_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movie_Person_MovieId",
                table: "Movie_Person",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_Person_PersonId",
                table: "Movie_Person",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movie_Person");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
