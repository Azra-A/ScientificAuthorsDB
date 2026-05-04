using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScientificAuthorsDB.Migrations
{
    /// <inheritdoc />
    public partial class TranslateSeedDataToBulgarian : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AuthorInstitutions",
                keyColumns: new[] { "AuthorId", "InstitutionId" },
                keyValues: new object[] { 1, 1 },
                column: "Role",
                value: "Професор");

            migrationBuilder.UpdateData(
                table: "AuthorInstitutions",
                keyColumns: new[] { "AuthorId", "InstitutionId" },
                keyValues: new object[] { 2, 3 },
                column: "Role",
                value: "Изследовател");

            migrationBuilder.UpdateData(
                table: "AuthorPublications",
                keyColumns: new[] { "AuthorId", "PublicationId" },
                keyValues: new object[] { 1, 1 },
                column: "ContributionRole",
                value: "Водещ автор");

            migrationBuilder.UpdateData(
                table: "AuthorPublications",
                keyColumns: new[] { "AuthorId", "PublicationId" },
                keyValues: new object[] { 2, 1 },
                column: "ContributionRole",
                value: "Съавтор");

            migrationBuilder.UpdateData(
                table: "AuthorPublications",
                keyColumns: new[] { "AuthorId", "PublicationId" },
                keyValues: new object[] { 2, 2 },
                column: "ContributionRole",
                value: "Водещ автор");

            migrationBuilder.UpdateData(
                table: "Institutions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "Университет");

            migrationBuilder.UpdateData(
                table: "Institutions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "City", "Name", "Type" },
                values: new object[] { "Кеймбридж", "МИТ", "Университет" });

            migrationBuilder.UpdateData(
                table: "Institutions",
                keyColumn: "Id",
                keyValue: 3,
                column: "Type",
                value: "Изследователски център");

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PublicationType", "Title" },
                values: new object[] { "Научна статия", "Машинно обучение в здравеопазването" });

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PublicationType", "Title" },
                values: new object[] { "Научна статия", "Напредък в квантовите изчисления" });

            migrationBuilder.UpdateData(
                table: "ResearchFields",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Информатика");

            migrationBuilder.UpdateData(
                table: "ResearchFields",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Физика");

            migrationBuilder.UpdateData(
                table: "ResearchFields",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Математика");

            migrationBuilder.UpdateData(
                table: "ResearchFields",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Биология");

            migrationBuilder.UpdateData(
                table: "ResearchFields",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Химия");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AuthorInstitutions",
                keyColumns: new[] { "AuthorId", "InstitutionId" },
                keyValues: new object[] { 1, 1 },
                column: "Role",
                value: "Professor");

            migrationBuilder.UpdateData(
                table: "AuthorInstitutions",
                keyColumns: new[] { "AuthorId", "InstitutionId" },
                keyValues: new object[] { 2, 3 },
                column: "Role",
                value: "Researcher");

            migrationBuilder.UpdateData(
                table: "AuthorPublications",
                keyColumns: new[] { "AuthorId", "PublicationId" },
                keyValues: new object[] { 1, 1 },
                column: "ContributionRole",
                value: "Lead Author");

            migrationBuilder.UpdateData(
                table: "AuthorPublications",
                keyColumns: new[] { "AuthorId", "PublicationId" },
                keyValues: new object[] { 2, 1 },
                column: "ContributionRole",
                value: "Co-Author");

            migrationBuilder.UpdateData(
                table: "AuthorPublications",
                keyColumns: new[] { "AuthorId", "PublicationId" },
                keyValues: new object[] { 2, 2 },
                column: "ContributionRole",
                value: "Lead Author");

            migrationBuilder.UpdateData(
                table: "Institutions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "University");

            migrationBuilder.UpdateData(
                table: "Institutions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "City", "Name", "Type" },
                values: new object[] { "Cambridge", "MIT", "University" });

            migrationBuilder.UpdateData(
                table: "Institutions",
                keyColumn: "Id",
                keyValue: 3,
                column: "Type",
                value: "Research Center");

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PublicationType", "Title" },
                values: new object[] { "Article", "Machine Learning in Healthcare" });

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PublicationType", "Title" },
                values: new object[] { "Article", "Quantum Computing Advances" });

            migrationBuilder.UpdateData(
                table: "ResearchFields",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Computer Science");

            migrationBuilder.UpdateData(
                table: "ResearchFields",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Physics");

            migrationBuilder.UpdateData(
                table: "ResearchFields",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Mathematics");

            migrationBuilder.UpdateData(
                table: "ResearchFields",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Biology");

            migrationBuilder.UpdateData(
                table: "ResearchFields",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Chemistry");
        }
    }
}
