using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies.Api.Migrations
{
    /// <inheritdoc />
    public partial class ReseedMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Casts_Casts_CastId",
                table: "Movie_Casts");

            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Casts_Movies_MovieId",
                table: "Movie_Casts");

            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Directors_Directors_DirectorId",
                table: "Movie_Directors");

            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Directors_Movies_MovieId",
                table: "Movie_Directors");

            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Genres_Genres_GenreId",
                table: "Movie_Genres");

            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Genres_Movies_MovieId",
                table: "Movie_Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movie_Genres",
                table: "Movie_Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movie_Directors",
                table: "Movie_Directors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movie_Casts",
                table: "Movie_Casts");

            migrationBuilder.RenameTable(
                name: "Movie_Genres",
                newName: "MovieGenres");

            migrationBuilder.RenameTable(
                name: "Movie_Directors",
                newName: "MovieDirectors");

            migrationBuilder.RenameTable(
                name: "Movie_Casts",
                newName: "MovieCasts");

            migrationBuilder.RenameIndex(
                name: "IX_Movie_Genres_MovieId",
                table: "MovieGenres",
                newName: "IX_MovieGenres_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Movie_Genres_GenreId",
                table: "MovieGenres",
                newName: "IX_MovieGenres_GenreId");

            migrationBuilder.RenameIndex(
                name: "IX_Movie_Directors_MovieId",
                table: "MovieDirectors",
                newName: "IX_MovieDirectors_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Movie_Directors_DirectorId",
                table: "MovieDirectors",
                newName: "IX_MovieDirectors_DirectorId");

            migrationBuilder.RenameIndex(
                name: "IX_Movie_Casts_MovieId",
                table: "MovieCasts",
                newName: "IX_MovieCasts_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Movie_Casts_CastId",
                table: "MovieCasts",
                newName: "IX_MovieCasts_CastId");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Directors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Casts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieGenres",
                table: "MovieGenres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieDirectors",
                table: "MovieDirectors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieCasts",
                table: "MovieCasts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCasts_Casts_CastId",
                table: "MovieCasts",
                column: "CastId",
                principalTable: "Casts",
                principalColumn: "CastId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCasts_Movies_MovieId",
                table: "MovieCasts",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDirectors_Directors_DirectorId",
                table: "MovieDirectors",
                column: "DirectorId",
                principalTable: "Directors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDirectors_Movies_MovieId",
                table: "MovieDirectors",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Genres_GenreId",
                table: "MovieGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_MovieId",
                table: "MovieGenres",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieCasts_Casts_CastId",
                table: "MovieCasts");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCasts_Movies_MovieId",
                table: "MovieCasts");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieDirectors_Directors_DirectorId",
                table: "MovieDirectors");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieDirectors_Movies_MovieId",
                table: "MovieDirectors");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Genres_GenreId",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MovieId",
                table: "MovieGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieGenres",
                table: "MovieGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieDirectors",
                table: "MovieDirectors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieCasts",
                table: "MovieCasts");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Directors");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Casts");

            migrationBuilder.RenameTable(
                name: "MovieGenres",
                newName: "Movie_Genres");

            migrationBuilder.RenameTable(
                name: "MovieDirectors",
                newName: "Movie_Directors");

            migrationBuilder.RenameTable(
                name: "MovieCasts",
                newName: "Movie_Casts");

            migrationBuilder.RenameIndex(
                name: "IX_MovieGenres_MovieId",
                table: "Movie_Genres",
                newName: "IX_Movie_Genres_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieGenres_GenreId",
                table: "Movie_Genres",
                newName: "IX_Movie_Genres_GenreId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieDirectors_MovieId",
                table: "Movie_Directors",
                newName: "IX_Movie_Directors_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieDirectors_DirectorId",
                table: "Movie_Directors",
                newName: "IX_Movie_Directors_DirectorId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieCasts_MovieId",
                table: "Movie_Casts",
                newName: "IX_Movie_Casts_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieCasts_CastId",
                table: "Movie_Casts",
                newName: "IX_Movie_Casts_CastId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movie_Genres",
                table: "Movie_Genres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movie_Directors",
                table: "Movie_Directors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movie_Casts",
                table: "Movie_Casts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Casts_Casts_CastId",
                table: "Movie_Casts",
                column: "CastId",
                principalTable: "Casts",
                principalColumn: "CastId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Casts_Movies_MovieId",
                table: "Movie_Casts",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Directors_Directors_DirectorId",
                table: "Movie_Directors",
                column: "DirectorId",
                principalTable: "Directors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Directors_Movies_MovieId",
                table: "Movie_Directors",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Genres_Genres_GenreId",
                table: "Movie_Genres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Genres_Movies_MovieId",
                table: "Movie_Genres",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
