using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Data.Migrations
{
    public partial class UpdateMoviePersonsRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Person_Movies_MovieId",
                table: "Movie_Person");

            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Person_Person_PersonId",
                table: "Movie_Person");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Person",
                table: "Person");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movie_Person",
                table: "Movie_Person");

            migrationBuilder.RenameTable(
                name: "Person",
                newName: "Persons");

            migrationBuilder.RenameTable(
                name: "Movie_Person",
                newName: "Movie_Persons");

            migrationBuilder.RenameIndex(
                name: "IX_Movie_Person_PersonId",
                table: "Movie_Persons",
                newName: "IX_Movie_Persons_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Movie_Person_MovieId",
                table: "Movie_Persons",
                newName: "IX_Movie_Persons_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Persons",
                table: "Persons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movie_Persons",
                table: "Movie_Persons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Persons_Movies_MovieId",
                table: "Movie_Persons",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Persons_Persons_PersonId",
                table: "Movie_Persons",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Persons_Movies_MovieId",
                table: "Movie_Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Persons_Persons_PersonId",
                table: "Movie_Persons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Persons",
                table: "Persons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movie_Persons",
                table: "Movie_Persons");

            migrationBuilder.RenameTable(
                name: "Persons",
                newName: "Person");

            migrationBuilder.RenameTable(
                name: "Movie_Persons",
                newName: "Movie_Person");

            migrationBuilder.RenameIndex(
                name: "IX_Movie_Persons_PersonId",
                table: "Movie_Person",
                newName: "IX_Movie_Person_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Movie_Persons_MovieId",
                table: "Movie_Person",
                newName: "IX_Movie_Person_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Person",
                table: "Person",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movie_Person",
                table: "Movie_Person",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Person_Movies_MovieId",
                table: "Movie_Person",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Person_Person_PersonId",
                table: "Movie_Person",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
