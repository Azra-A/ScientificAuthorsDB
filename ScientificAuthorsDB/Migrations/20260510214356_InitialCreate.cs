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
                    { 1, 1975, "ivan@su.bg", "Иван", "Петров", "0000-0001-2345-6789" },
                    { 2, 1980, "maria@bas.bg", "Мария", "Иванова", "0000-0002-9876-5432" },
                    { 3, 1990, "g.dimitrov@tu.bg", "Георги", "Димитров", "0000-0003-1111-2222" },
                    { 4, 1978, "jsmith@mit.edu", "Джон", "Смит", "0000-0002-4444-5555" },
                    { 5, 1985, "elena.n@mu-plovdiv.bg", "Елена", "Николова", "0000-0001-7777-8888" },
                    { 6, 1982, "alice.j@oxford.ac.uk", "Алис", "Джонсън", "0000-0002-3333-6666" },
                    { 7, 1970, "p.todorov@unwe.bg", "Петър", "Тодоров", "0000-0003-9999-0000" },
                    { 8, 1965, "hmuller@mpg.de", "Ханс", "Мюлер", "0000-0001-5555-4444" },
                    { 9, 1988, "smiteva@cern.ch", "Силвия", "Митева", "0000-0002-1234-5678" },
                    { 10, 1992, "rbrown@stanford.edu", "Робърт", "Браун", "0000-0002-8888-1111" }
                });

            migrationBuilder.InsertData(
                table: "Institutions",
                columns: new[] { "Id", "City", "Country", "Name", "Type", "Website" },
                values: new object[,]
                {
                    { 1, "София", "България", "Софийски университет", "Университет", "https://www.uni-sofia.bg" },
                    { 2, "Кеймбридж", "САЩ", "МИТ (MIT)", "Университет", "https://www.mit.edu" },
                    { 3, "София", "България", "БАН", "Изследователски център", "https://www.bas.bg" },
                    { 4, "Оксфорд", "Великобритания", "Оксфордски университет", "Университет", "https://www.ox.ac.uk" },
                    { 5, "София", "България", "Технически университет", "Университет", "https://tu-sofia.bg" },
                    { 6, "Женева", "Швейцария", "ЦЕРН", "Изследователски център", "https://home.cern" },
                    { 7, "Станфорд", "САЩ", "Станфордски университет", "Университет", "https://www.stanford.edu" },
                    { 8, "Пловдив", "България", "Медицински университет", "Университет", "https://mu-plovdiv.bg" },
                    { 9, "Мюнхен", "Германия", "Институт 'Макс Планк'", "Изследователски център", "https://www.mpg.de" },
                    { 10, "София", "България", "УНСС", "Университет", "https://www.unwe.bg" }
                });

            migrationBuilder.InsertData(
                table: "Publications",
                columns: new[] { "Id", "Abstract", "Doi", "Journal", "PublicationType", "Title", "Year" },
                values: new object[,]
                {
                    { 1, "Изследване на приложението на невронни мрежи за ранна диагностика.", "10.1038/s41586-022-00001", "Природа", "Научна статия", "Машинно обучение в здравеопазването", 2022 },
                    { 2, "Анализ на новите методи за стабилизиране на кубити.", "10.1126/science.abn0002", "Наука", "Научна статия", "Напредък в квантовите изчисления", 2023 },
                    { 3, null, "10.1016/j.ecotod.2021.0003", "Икономика днес", "Доклад от конференция", "Изкуствен интелект във финансите", 2021 },
                    { 4, "Картографиране на генома при редки заболявания.", "10.1016/j.cell.2020.0004", "Клетка", "Научна статия", "Генетика и наследствени заболявания", 2020 },
                    { 5, null, "10.5555/nano-book-2023", "Издателство 'Наука'", "Монография", "Нови материали в нанотехнологиите", 2023 },
                    { 6, null, "10.1109/TNNLS.2019.0006", "Инженерни науки", "Научна статия", "Алгоритми за оптимизация на невронни мрежи", 2019 },
                    { 7, "Моделиране на разпределението на тъмната материя в близките галактики.", "10.3847/1538-4357/ac0007", "Астрофизичен журнал", "Научна статия", "Тъмна материя и структура на Вселената", 2022 },
                    { 8, null, "10.1002/ecy.0008", "Екология", "Книга", "Влияние на климата върху биоразнообразието", 2021 },
                    { 9, null, "10.1016/S0140-6736(20)0009", "Списание Ланцет", "Научна статия", "Разработване на ваксини чрез мРНК", 2020 },
                    { 10, null, "10.1111/jofi.0010", "Журнал по финанси", "Доклад от конференция", "Криптовалути и икономическа стабилност", 2023 },
                    { 11, null, "10.5555/techpol-2023-011", "Институт за технологични политики", "Доклад от конференция", "Анализ на киберсигурността в държавния сектор", 2023 },
                    { 12, "Теоретичен модел за изпарението на свръхмасивни черни дупки.", "10.5555/su-thesis-2018-012", "Софийски университет", "Дисертация", "Еволюция на черните дупки", 2018 },
                    { 13, null, "10.5555/moew-report-2022", "Министерство на екологията", "Монография", "Годишен доклад за състоянието на околната среда", 2022 },
                    { 14, "Обзор на най-новите клинични проучвания.", "10.5555/medrev-2021-014", "Медицински преглед", "Научна статия", "Съвременни методи за лечение на диабет", 2021 },
                    { 15, null, "10.5555/bas-econ-2023-015", "Икономически институт", "Доклад от конференция", "Въздействие на инфлацията върху малкия бизнес", 2023 }
                });

            migrationBuilder.InsertData(
                table: "ResearchFields",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Информатика" },
                    { 2, null, "Физика" },
                    { 3, null, "Математика" },
                    { 4, null, "Биология" },
                    { 5, null, "Химия" },
                    { 6, null, "Медицина" },
                    { 7, null, "Икономика" }
                });

            migrationBuilder.InsertData(
                table: "AuthorInstitutions",
                columns: new[] { "AuthorId", "InstitutionId", "EndYear", "Role", "StartYear" },
                values: new object[,]
                {
                    { 1, 1, null, "Професор", 2010 },
                    { 1, 3, null, "Гост-изследовател", 2015 },
                    { 2, 3, null, "Главен асистент", 2012 },
                    { 3, 5, null, "Доцент", 2018 },
                    { 4, 2, null, "Професор", 2005 },
                    { 5, 8, null, "Асистент", 2020 },
                    { 6, 4, null, "Изследовател", 2016 },
                    { 7, 10, null, "Професор", 2008 },
                    { 8, 9, null, "Директор", 2000 },
                    { 9, 6, null, "Физик", 2019 },
                    { 10, 7, null, "Докторант", 2021 }
                });

            migrationBuilder.InsertData(
                table: "AuthorPublications",
                columns: new[] { "AuthorId", "PublicationId", "AuthorOrder", "ContributionRole" },
                values: new object[,]
                {
                    { 1, 1, 1, "Водещ автор" },
                    { 1, 6, 1, "Водещ автор" },
                    { 2, 8, 1, "Единствен автор" },
                    { 2, 13, 1, "Координатор" },
                    { 3, 5, 1, "Водещ автор" },
                    { 3, 6, 2, "Съавтор" },
                    { 3, 11, 1, "Главен изследовател" },
                    { 4, 2, 2, "Съавтор" },
                    { 5, 1, 2, "Съавтор" },
                    { 5, 4, 1, "Единствен автор" },
                    { 5, 9, 1, "Водещ автор" },
                    { 5, 14, 1, "Водещ автор" },
                    { 6, 7, 2, "Съавтор" },
                    { 6, 9, 2, "Съавтор" },
                    { 7, 3, 1, "Водещ автор" },
                    { 7, 10, 1, "Водещ автор" },
                    { 7, 15, 1, "Единствен автор" },
                    { 8, 5, 2, "Съавтор" },
                    { 8, 13, 2, "Рецензент" },
                    { 9, 2, 1, "Водещ автор" },
                    { 9, 7, 1, "Водещ автор" },
                    { 9, 12, 1, "Единствен автор" },
                    { 10, 3, 2, "Съавтор" },
                    { 10, 6, 3, "Съавтор" }
                });

            migrationBuilder.InsertData(
                table: "PublicationFields",
                columns: new[] { "PublicationId", "ResearchFieldId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 6 },
                    { 2, 1 },
                    { 2, 2 },
                    { 3, 1 },
                    { 3, 7 },
                    { 4, 4 },
                    { 4, 6 },
                    { 5, 2 },
                    { 5, 5 },
                    { 6, 1 },
                    { 6, 3 },
                    { 7, 2 },
                    { 8, 4 },
                    { 9, 4 },
                    { 9, 6 },
                    { 10, 7 },
                    { 11, 1 },
                    { 12, 2 },
                    { 13, 4 },
                    { 13, 5 },
                    { 14, 6 },
                    { 15, 7 }
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
