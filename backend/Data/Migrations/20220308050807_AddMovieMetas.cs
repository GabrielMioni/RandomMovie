using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Data.Migrations
{
    public partial class AddMovieMetas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieMetas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiId = table.Column<int>(nullable: false),
                    OriginalTitle = table.Column<string>(nullable: true),
                    OriginalLanguage = table.Column<string>(nullable: true),
                    Overview = table.Column<string>(nullable: true),
                    PosterPath = table.Column<string>(nullable: true),
                    ReleasedDate = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    MovieId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieMetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieMetas_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieMetas_MovieId",
                table: "MovieMetas",
                column: "MovieId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieMetas");
        }
    }
}
