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
                    PublicationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    { 1, 1985, "avaswani@google.com", "Ашиш", "Васвани", "0000-0001-1111-0001" },
                    { 2, 1976, "noam@google.com", "Ноам", "Шазиър", "0000-0001-1111-0002" },
                    { 3, 1988, "illia@near.org", "Илия", "Полосухин", "0000-0001-1111-0003" },
                    { 4, 1990, "w.wang@se-research.org", "Уентинг", "Уанг", "0000-0001-1111-0004" },
                    { 5, 1989, "x.niu@se-research.org", "Сяотинг", "Ниу", "0000-0001-1111-0005" },
                    { 6, 1952, "unclebob@cleancoder.com", "Робърт С.", "Мартин", "0000-0001-1111-0006" },
                    { 101, 1975, "ivan@su.bg", "Иван", "Петров", "0000-0001-2345-6789" },
                    { 102, 1980, "maria@bas.bg", "Мария", "Иванова", "0000-0002-9876-5432" },
                    { 103, 1990, "g.dimitrov@tu.bg", "Георги", "Димитров", "0000-0003-1111-2222" },
                    { 104, 1978, "jsmith@mit.edu", "Джон", "Смит", "0000-0002-4444-5555" },
                    { 105, 1985, "elena.n@mu-plovdiv.bg", "Елена", "Николова", "0000-0001-7777-8888" },
                    { 106, 1982, "alice.j@oxford.ac.uk", "Алис", "Джонсън", "0000-0002-3333-6666" },
                    { 107, 1970, "p.todorov@unwe.bg", "Петър", "Тодоров", "0000-0003-9999-0000" },
                    { 108, 1965, "hmuller@mpg.de", "Ханс", "Мюлер", "0000-0001-5555-4444" },
                    { 109, 1988, "smiteva@cern.ch", "Силвия", "Митева", "0000-0002-1234-5678" },
                    { 110, 1992, "rbrown@stanford.edu", "Робърт", "Браун", "0000-0002-8888-1111" }
                });

            migrationBuilder.InsertData(
                table: "Institutions",
                columns: new[] { "Id", "City", "Country", "Name", "Type", "Website" },
                values: new object[,]
                {
                    { 1, "Маунтин Вю", "САЩ", "Google Brain", "Изследователски център", "https://research.google" },
                    { 101, "София", "България", "Софийски университет", "Университет", "https://www.uni-sofia.bg" },
                    { 102, "Кеймбридж", "САЩ", "МИТ (MIT)", "Университет", "https://www.mit.edu" },
                    { 103, "София", "България", "БАН", "Изследователски център", "https://www.bas.bg" },
                    { 104, "Оксфорд", "Великобритания", "Оксфордски университет", "Университет", "https://www.ox.ac.uk" },
                    { 105, "София", "България", "Технически университет", "Университет", "https://tu-sofia.bg" },
                    { 106, "Женева", "Швейцария", "ЦЕРН", "Изследователски център", "https://home.cern" },
                    { 107, "Станфорд", "САЩ", "Станфордски университет", "Университет", "https://www.stanford.edu" },
                    { 108, "Пловдив", "България", "Медицински университет", "Университет", "https://mu-plovdiv.bg" },
                    { 109, "Мюнхен", "Германия", "Институт 'Макс Планк'", "Изследователски център", "https://www.mpg.de" },
                    { 110, "София", "България", "УНСС", "Университет", "https://www.unwe.bg" }
                });

            migrationBuilder.InsertData(
                table: "Publications",
                columns: new[] { "Id", "Abstract", "Doi", "FilePath", "Journal", "PublicationType", "Title", "Year" },
                values: new object[,]
                {
                    { 1, "Доминиращите модели за преобразуване на последователности се основават на сложни рекурентни или конволюционни невронни мрежи. Ние предлагаме нова проста мрежова архитектура, наречена Transformer (Трансформатор), базирана изцяло на механизми за внимание, като напълно премахва необходимостта от рекурентност и конволюции.", "10.48550/arXiv.1706.03762", null, "31-ва Конференция по системи за обработка на невронна информация (NIPS)", "Доклад от конференция", "Вниманието е всичко, от което имате нужда", 2017 },
                    { 2, "Ние систематично анализираме скорошни публикации по софтуерно инженерство, за да изследваме наличието, местоположението, форматите и дълготрайността на споделените артефакти. Нашите открития предоставят изчерпателен преглед на текущите практики и насоки за бъдещи възпроизводими изследвания.", "10.1016/j.jss.2024.112032", null, "Журнал за системи и софтуер", "Научна статия", "Изследователски артефакти в публикациите по софтуерно инженерство: Състояние и тенденции", 2024 },
                    { 3, "Дори лошият код може да функционира. Но ако кодът не е чист, той може да срине една организация за разработка. Тази книга ви учи как да пишете чист, лесен за поддръжка и четим код, превръщайки ви в истински софтуерен майстор.", "9780132350884", null, "Издателство Prentice Hall", "Книга", "Чист код: Наръчник за гъвкаво софтуерно майсторство", 2008 },
                    { 101, "Изследване на приложението на невронни мрежи за ранна диагностика.", "10.1038/s41586-022-00001", null, "Природа", "Научна статия", "Машинно обучение в здравеопазването", 2022 },
                    { 102, "Анализ на новите методи за стабилизиране на кубити.", "10.1126/science.abn0002", null, "Наука", "Научна статия", "Напредък в квантовите изчисления", 2023 },
                    { 103, null, "10.1016/j.ecotod.2021.0003", null, "Икономика днес", "Доклад от конференция", "Изкуствен интелект във финансите", 2021 },
                    { 104, "Картографиране на генома при редки заболявания.", "10.1016/j.cell.2020.0004", null, "Клетка", "Научна статия", "Генетика и наследствени заболявания", 2020 },
                    { 105, null, "10.5555/nano-book-2023", null, "Издателство 'Наука'", "Монография", "Нови материали в нанотехнологиите", 2023 }
                });

            migrationBuilder.InsertData(
                table: "ResearchFields",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Изкуствен интелект" },
                    { 2, null, "Машинно обучение" },
                    { 3, null, "Софтуерно инженерство" },
                    { 4, null, "Обектно-ориентирано програмиране" },
                    { 101, null, "Информатика" },
                    { 102, null, "Физика" },
                    { 103, null, "Математика" },
                    { 104, null, "Биология" },
                    { 105, null, "Химия" },
                    { 106, null, "Медицина" },
                    { 107, null, "Икономика" }
                });

            migrationBuilder.InsertData(
                table: "AuthorInstitutions",
                columns: new[] { "AuthorId", "InstitutionId", "EndYear", "Role", "StartYear" },
                values: new object[,]
                {
                    { 1, 1, null, "Старши изследовател", 2016 },
                    { 2, 1, null, "Главен инженер", 2015 },
                    { 3, 1, null, "Изследовател", 2017 },
                    { 4, 104, null, "Доцент", 2019 },
                    { 5, 104, null, "Изследовател", 2020 },
                    { 6, 102, null, "Софтуерен консултант", 2001 },
                    { 101, 101, null, "Професор", 2010 },
                    { 101, 103, null, "Гост-изследовател", 2015 },
                    { 102, 103, null, "Главен асистент", 2012 },
                    { 103, 105, null, "Доцент", 2018 },
                    { 104, 102, null, "Професор", 2005 },
                    { 105, 108, null, "Асистент", 2020 },
                    { 106, 104, null, "Изследовател", 2016 },
                    { 107, 110, null, "Професор", 2008 },
                    { 108, 109, null, "Директор", 2000 },
                    { 109, 106, null, "Физик", 2019 },
                    { 110, 107, null, "Докторант", 2021 }
                });

            migrationBuilder.InsertData(
                table: "AuthorPublications",
                columns: new[] { "AuthorId", "PublicationId", "AuthorOrder", "ContributionRole" },
                values: new object[,]
                {
                    { 1, 1, 1, "Водещ автор" },
                    { 2, 1, 2, "Съавтор" },
                    { 3, 1, 3, "Съавтор" },
                    { 4, 2, 1, "Водещ автор" },
                    { 5, 2, 2, "Съавтор" },
                    { 6, 3, 1, "Единствен автор" },
                    { 101, 101, 1, "Водещ автор" },
                    { 103, 105, 1, "Водещ автор" },
                    { 104, 102, 2, "Съавтор" },
                    { 105, 101, 2, "Съавтор" },
                    { 105, 104, 1, "Единствен автор" },
                    { 107, 103, 1, "Водещ автор" },
                    { 108, 105, 2, "Съавтор" },
                    { 109, 102, 1, "Водещ автор" },
                    { 110, 103, 2, "Съавтор" }
                });

            migrationBuilder.InsertData(
                table: "PublicationFields",
                columns: new[] { "PublicationId", "ResearchFieldId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 3 },
                    { 3, 3 },
                    { 3, 4 },
                    { 101, 101 },
                    { 101, 106 },
                    { 102, 101 },
                    { 102, 102 },
                    { 103, 101 },
                    { 103, 107 },
                    { 104, 104 },
                    { 104, 106 },
                    { 105, 102 },
                    { 105, 105 }
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
