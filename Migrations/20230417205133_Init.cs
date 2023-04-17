using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace l2.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionBook",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    CollectionId = table.Column<int>(type: "int", nullable: false),
                    NumberInSeries = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionBook", x => new { x.BookId, x.CollectionId });
                    table.ForeignKey(
                        name: "FK_CollectionBook_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionBook_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Ladarius Douglas" },
                    { 2, "Fletcher Cormier" },
                    { 3, "Eleonore Luettgen" },
                    { 4, "Nona Kovacek" },
                    { 5, "Dino Lubowitz" },
                    { 6, "Jazmyne Reichel" },
                    { 7, "Franco Friesen" },
                    { 8, "Carlee Schultz" },
                    { 9, "Katlyn Roberts" },
                    { 10, "Demarcus Koelpin" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "ISBN", "Title" },
                values: new object[,]
                {
                    { 1, 7, "935-9-89-148103-0", "Numquam eius a velit porro." },
                    { 2, 4, "488-7-09-056543-9", "Minus dolores dignissimos." },
                    { 3, 3, "082-6-64-577539-1", "Culpa facilis velit delectus aliquid libero sed error aut consequatur." },
                    { 4, 2, "634-9-67-179393-0", "Aut aliquid quia culpa beatae quas dolores." },
                    { 5, 7, "414-4-32-988918-2", "Voluptas aut eos pariatur expedita non necessitatibus et libero." },
                    { 6, 7, "053-4-28-977854-3", "Temporibus similique alias animi doloremque maiores illum veniam commodi." },
                    { 7, 4, "963-3-00-225668-0", "Cumque voluptatem sed dolores aut reiciendis." },
                    { 8, 2, "348-5-97-831318-9", "Eveniet sed in perferendis reiciendis totam sed velit odit." },
                    { 9, 3, "186-2-36-875415-8", "Omnis porro odio neque." },
                    { 10, 10, "883-6-62-994422-0", "Reiciendis est velit expedita eum." },
                    { 11, 1, "193-8-08-366155-0", "Ut nam amet officia voluptates." },
                    { 12, 8, "783-1-28-489034-6", "Ullam perspiciatis aut qui beatae." },
                    { 13, 6, "823-2-86-196966-6", "Hic dolores corrupti dolorum consequatur." },
                    { 14, 4, "135-7-68-193738-0", "Ut ut consectetur laborum." },
                    { 15, 5, "936-8-15-043844-3", "Quia laudantium provident ratione quis provident cum molestias sit." },
                    { 16, 2, "283-7-73-904838-2", "Iure nihil maxime aut sed sit consectetur harum." },
                    { 17, 8, "193-8-98-757738-7", "Pariatur voluptas facere non et." },
                    { 18, 5, "795-0-64-138144-6", "Asperiores quibusdam illum veniam perferendis in." },
                    { 19, 2, "326-5-41-583094-9", "Laudantium totam possimus rerum rerum." },
                    { 20, 1, "186-8-40-205303-3", "Sapiente quo repellendus quia odio natus." },
                    { 21, 2, "081-9-45-036644-0", "Veniam earum qui perspiciatis at eum." },
                    { 22, 2, "778-0-50-868175-1", "Aut minus sit et quis facere." },
                    { 23, 5, "426-9-66-929022-5", "Molestias accusamus sapiente." },
                    { 24, 1, "167-4-68-993580-6", "Harum voluptatem quia veniam repudiandae rerum iste." },
                    { 25, 10, "120-7-46-620589-2", "Necessitatibus architecto voluptatem." },
                    { 26, 4, "119-0-39-773584-5", "Ut accusantium alias." },
                    { 27, 8, "432-0-83-831429-0", "Voluptatem quibusdam optio." },
                    { 28, 3, "208-1-06-314595-8", "Veritatis quisquam et." },
                    { 29, 7, "136-2-08-344414-5", "Voluptatem odio facilis qui." },
                    { 30, 5, "367-7-92-365077-2", "Qui omnis quo quia." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionBook_CollectionId",
                table: "CollectionBook",
                column: "CollectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectionBook");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
