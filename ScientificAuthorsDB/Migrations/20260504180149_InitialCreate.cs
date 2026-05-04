using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ScientificAuthorsDB.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrcidId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthYear = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Institutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Abstract = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Doi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Journal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicationType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResearchFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorInstitutions",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    InstitutionId = table.Column<int>(type: "int", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: true),
                    EndYear = table.Column<int>(type: "int", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorInstitutions", x => new { x.AuthorId, x.InstitutionId });
                    table.ForeignKey(
                        name: "FK_AuthorInstitutions_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorInstitutions_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorPublications",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    PublicationId = table.Column<int>(type: "int", nullable: false),
                    ContributionRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorPublications", x => new { x.AuthorId, x.PublicationId });
                    table.ForeignKey(
                        name: "FK_AuthorPublications_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorPublications_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PublicationFields",
                columns: table => new
                {
                    PublicationId = table.Column<int>(type: "int", nullable: false),
                    ResearchFieldId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicationFields", x => new { x.PublicationId, x.ResearchFieldId });
                    table.ForeignKey(
                        name: "FK_PublicationFields_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicationFields_ResearchFields_ResearchFieldId",
                        column: x => x.ResearchFieldId,
                        principalTable: "ResearchFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "BirthYear", "Email", "FirstName", "LastName", "OrcidId" },
                values: new object[,]
                {
                    { 1, 1975, "ivan@su.bg", "Иван", "Петров", null },
                    { 2, 1980, "maria@bas.bg", "Мария", "Иванова", null }
                });

            migrationBuilder.InsertData(
                table: "Institutions",
                columns: new[] { "Id", "City", "Country", "Name", "Type", "Website" },
                values: new object[,]
                {
                    { 1, "София", "България", "Софийски университет", "University", null },
                    { 2, "Cambridge", "САЩ", "MIT", "University", null },
                    { 3, "София", "България", "БАН", "Research Center", null }
                });

            migrationBuilder.InsertData(
                table: "Publications",
                columns: new[] { "Id", "Abstract", "Doi", "Journal", "PublicationType", "Title", "Year" },
                values: new object[,]
                {
                    { 1, null, null, "Nature", "Article", "Machine Learning in Healthcare", 2022 },
                    { 2, null, null, "Science", "Article", "Quantum Computing Advances", 2023 }
                });

            migrationBuilder.InsertData(
                table: "ResearchFields",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Computer Science" },
                    { 2, null, "Physics" },
                    { 3, null, "Mathematics" },
                    { 4, null, "Biology" },
                    { 5, null, "Chemistry" }
                });

            migrationBuilder.InsertData(
                table: "AuthorInstitutions",
                columns: new[] { "AuthorId", "InstitutionId", "EndYear", "Role", "StartYear" },
                values: new object[,]
                {
                    { 1, 1, null, "Professor", null },
                    { 2, 3, null, "Researcher", null }
                });

            migrationBuilder.InsertData(
                table: "AuthorPublications",
                columns: new[] { "AuthorId", "PublicationId", "AuthorOrder", "ContributionRole" },
                values: new object[,]
                {
                    { 1, 1, 1, "Lead Author" },
                    { 2, 1, 2, "Co-Author" },
                    { 2, 2, 1, "Lead Author" }
                });

            migrationBuilder.InsertData(
                table: "PublicationFields",
                columns: new[] { "PublicationId", "ResearchFieldId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorInstitutions_InstitutionId",
                table: "AuthorInstitutions",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorPublications_PublicationId",
                table: "AuthorPublications",
                column: "PublicationId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicationFields_ResearchFieldId",
                table: "PublicationFields",
                column: "ResearchFieldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorInstitutions");

            migrationBuilder.DropTable(
                name: "AuthorPublications");

            migrationBuilder.DropTable(
                name: "PublicationFields");

            migrationBuilder.DropTable(
                name: "Institutions");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Publications");

            migrationBuilder.DropTable(
                name: "ResearchFields");
        }
    }
}
