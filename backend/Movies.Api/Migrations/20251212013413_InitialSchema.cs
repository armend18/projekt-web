using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Movies.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearOfRelease = table.Column<int>(type: "int", nullable: false),
                    RunTime = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Director = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CastList = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    Cover = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Age", "CastList", "Country", "Cover", "Description", "Director", "Genres", "Rating", "RunTime", "Title", "VideoLink", "YearOfRelease" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 13, "[\"Leonardo DiCaprio\",\"Joseph Gordon-Levitt\",\"Elliot Page\"]", "USA", "https://filmartgallery.com/cdn/shop/files/Inception-Vintage-Movie-Poster-Original.jpg?v=1738912645", "A skilled thief is offered a chance to have his past crimes forgiven by implanting another person's idea into a target's subconscious.", "Christopher Nolan", "[\"Action\",\"Sci-Fi\",\"Thriller\"]", 8.8f, 148, "Inception", "https://www.youtube.com/embed/YoHD9XEInc0", 2010 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 16, "[\"Tim Robbins\",\"Morgan Freeman\",\"Bob Gunton\"]", "USA", "https://image.tmdb.org/t/p/original/5OWFF1DhvYVQiX1yUrBE9CQqO5t.jpg", "Two imprisoned men bond over years, finding solace and eventual redemption through acts of common decency.", "Frank Darabont", "[\"Drama\"]", 3.3f, 142, "The Shawshank Redemption", "https://www.youtube.com/embed/PLl99DlL6b4", 1994 },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 12, "[\"Matthew McConaughey\",\"Anne Hathaway\",\"Jessica Chastain\"]", "USA", "https://m.media-amazon.com/images/I/61ASebTsLpL._AC_UF1000,1000_QL80_.jpg", "A group of explorers travels through a wormhole in space in an attempt to ensure humanity's survival.", "Christopher Nolan", "[\"Adventure\",\"Drama\",\"Sci-Fi\"]", 8.6f, 169, "Interstellar", "https://www.youtube.com/embed/zSWdZVtXT7E", 2014 },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 13, "[\"Christian Bale\",\"Heath Ledger\",\"Aaron Eckhart\"]", "USA", "https://storage.googleapis.com/pod_public/750/257216.jpg", "Batman faces the Joker, a criminal mastermind who plunges Gotham City into chaos.", "Christopher Nolan", "[\"Action\",\"Crime\",\"Drama\"]", 9f, 152, "The Dark Knight", "https://www.youtube.com/embed/EXeTwQWrcwY", 2008 },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 16, "[\"Song Kang-ho\",\"Lee Sun-kyun\",\"Cho Yeo-jeong\"]", "South Korea", "https://image.tmdb.org/t/p/original/7IiTTgloJzvGI1TAYymCfbfl3vT.jpg", "Greed and class discrimination threaten the newly formed symbiotic relationship between the wealthy Park family and the destitute Kim clan.", "Bong Joon Ho", "[\"Comedy\",\"Drama\",\"Thriller\"]", 8.6f, 132, "Parasite", "https://www.youtube.com/embed/5xH0HfJHsaY", 2019 },
                    { new Guid("66666666-6666-6666-6666-666666666666"), 16, "[\"Russell Crowe\",\"Joaquin Phoenix\",\"Connie Nielsen\"]", "USA", "https://i.ebayimg.com/images/g/OcEAAOSwHgdmFp-N/s-l1200.jpg", "A betrayed Roman general seeks revenge against the corrupt emperor who murdered his family and sent him into slavery.", "Ridley Scott", "[\"Action\",\"Adventure\",\"Drama\"]", 8.5f, 155, "Gladiator", "https://www.youtube.com/embed/P5ieIbInFpg", 2000 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
