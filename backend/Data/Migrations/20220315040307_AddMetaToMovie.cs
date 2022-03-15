using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Data.Migrations
{
    public partial class AddMetaToMovie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieMeta",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_MovieMeta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieMeta_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieMeta_MovieId",
                table: "MovieMeta",
                column: "MovieId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieMeta");
        }
    }
}
