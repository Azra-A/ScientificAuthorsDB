using Microsoft.EntityFrameworkCore;
using ScientificAuthorsDB.Models;

namespace ScientificAuthorsDB.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<ResearchField> ResearchFields { get; set; }
        public DbSet<AuthorInstitution> AuthorInstitutions { get; set; }
        public DbSet<AuthorPublication> AuthorPublications { get; set; }
        public DbSet<PublicationField> PublicationFields { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorInstitution>()
                .HasKey(ai => new { ai.AuthorId, ai.InstitutionId });

            modelBuilder.Entity<AuthorPublication>()
                .HasKey(ap => new { ap.AuthorId, ap.PublicationId });

            modelBuilder.Entity<PublicationField>()
                .HasKey(pf => new { pf.PublicationId, pf.ResearchFieldId });


            modelBuilder.Entity<ResearchField>().HasData(
                new ResearchField { Id = 1, Name = "Изкуствен интелект" },
                new ResearchField { Id = 2, Name = "Машинно обучение" },
                new ResearchField { Id = 3, Name = "Софтуерно инженерство" },
                new ResearchField { Id = 4, Name = "Обектно-ориентирано програмиране" },

                new ResearchField { Id = 101, Name = "Информатика" },
                new ResearchField { Id = 102, Name = "Физика" },
                new ResearchField { Id = 103, Name = "Математика" },
                new ResearchField { Id = 104, Name = "Биология" },
                new ResearchField { Id = 105, Name = "Химия" },
                new ResearchField { Id = 106, Name = "Медицина" },
                new ResearchField { Id = 107, Name = "Икономика" }
            );

            modelBuilder.Entity<Institution>().HasData(
                new Institution { Id = 1, Name = "Google Brain", Country = "САЩ", City = "Маунтин Вю", Type = "Изследователски център", Website = "https://research.google" },

                new Institution { Id = 101, Name = "Софийски университет", Country = "България", City = "София", Type = "Университет", Website = "https://www.uni-sofia.bg" },
                new Institution { Id = 102, Name = "МИТ (MIT)", Country = "САЩ", City = "Кеймбридж", Type = "Университет", Website = "https://www.mit.edu" },
                new Institution { Id = 103, Name = "БАН", Country = "България", City = "София", Type = "Изследователски център", Website = "https://www.bas.bg" },
                new Institution { Id = 104, Name = "Оксфордски университет", Country = "Великобритания", City = "Оксфорд", Type = "Университет", Website = "https://www.ox.ac.uk" },
                new Institution { Id = 105, Name = "Технически университет", Country = "България", City = "София", Type = "Университет", Website = "https://tu-sofia.bg" },
                new Institution { Id = 106, Name = "ЦЕРН", Country = "Швейцария", City = "Женева", Type = "Изследователски център", Website = "https://home.cern" },
                new Institution { Id = 107, Name = "Станфордски университет", Country = "САЩ", City = "Станфорд", Type = "Университет", Website = "https://www.stanford.edu" },
                new Institution { Id = 108, Name = "Медицински университет", Country = "България", City = "Пловдив", Type = "Университет", Website = "https://mu-plovdiv.bg" },
                new Institution { Id = 109, Name = "Институт 'Макс Планк'", Country = "Германия", City = "Мюнхен", Type = "Изследователски център", Website = "https://www.mpg.de" },
                new Institution { Id = 110, Name = "УНСС", Country = "България", City = "София", Type = "Университет", Website = "https://www.unwe.bg" }
            );

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FirstName = "Ашиш", LastName = "Васвани", Email = "avaswani@google.com", BirthYear = 1985, OrcidId = "0000-0001-1111-0001" },
                new Author { Id = 2, FirstName = "Ноам", LastName = "Шазиър", Email = "noam@google.com", BirthYear = 1976, OrcidId = "0000-0001-1111-0002" },
                new Author { Id = 3, FirstName = "Илия", LastName = "Полосухин", Email = "illia@near.org", BirthYear = 1988, OrcidId = "0000-0001-1111-0003" },
                new Author { Id = 4, FirstName = "Уентинг", LastName = "Уанг", Email = "w.wang@se-research.org", BirthYear = 1990, OrcidId = "0000-0001-1111-0004" },
                new Author { Id = 5, FirstName = "Сяотинг", LastName = "Ниу", Email = "x.niu@se-research.org", BirthYear = 1989, OrcidId = "0000-0001-1111-0005" },
                new Author { Id = 6, FirstName = "Робърт С.", LastName = "Мартин", Email = "unclebob@cleancoder.com", BirthYear = 1952, OrcidId = "0000-0001-1111-0006" },

                new Author { Id = 101, FirstName = "Иван", LastName = "Петров", Email = "ivan@su.bg", BirthYear = 1975, OrcidId = "0000-0001-2345-6789" },
                new Author { Id = 102, FirstName = "Мария", LastName = "Иванова", Email = "maria@bas.bg", BirthYear = 1980, OrcidId = "0000-0002-9876-5432" },
                new Author { Id = 103, FirstName = "Георги", LastName = "Димитров", Email = "g.dimitrov@tu.bg", BirthYear = 1990, OrcidId = "0000-0003-1111-2222" },
                new Author { Id = 104, FirstName = "Джон", LastName = "Смит", Email = "jsmith@mit.edu", BirthYear = 1978, OrcidId = "0000-0002-4444-5555" },
                new Author { Id = 105, FirstName = "Елена", LastName = "Николова", Email = "elena.n@mu-plovdiv.bg", BirthYear = 1985, OrcidId = "0000-0001-7777-8888" },
                new Author { Id = 106, FirstName = "Алис", LastName = "Джонсън", Email = "alice.j@oxford.ac.uk", BirthYear = 1982, OrcidId = "0000-0002-3333-6666" },
                new Author { Id = 107, FirstName = "Петър", LastName = "Тодоров", Email = "p.todorov@unwe.bg", BirthYear = 1970, OrcidId = "0000-0003-9999-0000" },
                new Author { Id = 108, FirstName = "Ханс", LastName = "Мюлер", Email = "hmuller@mpg.de", BirthYear = 1965, OrcidId = "0000-0001-5555-4444" },
                new Author { Id = 109, FirstName = "Силвия", LastName = "Митева", Email = "smiteva@cern.ch", BirthYear = 1988, OrcidId = "0000-0002-1234-5678" },
                new Author { Id = 110, FirstName = "Робърт", LastName = "Браун", Email = "rbrown@stanford.edu", BirthYear = 1992, OrcidId = "0000-0002-8888-1111" }
            );

            modelBuilder.Entity<Publication>().HasData(
                new Publication { Id = 1, Title = "Вниманието е всичко, от което имате нужда", Year = 2017, Journal = "Advances in Neural Information Processing Systems (NIPS)", PublicationType = "Доклад от конференция", Doi = "10.48550/arXiv.1706.03762", Abstract = "Доминиращите модели за преобразуване на последователности се основават на сложни рекурентни или конволюционни невронни мрежи. Ние предлагаме нова проста мрежова архитектура, наречена Transformer (Трансформатор), базирана изцяло на механизми за внимание, като напълно премахва необходимостта от рекурентност и конволюции." },
                new Publication { Id = 2, Title = "Изследователски артефакти в публикациите по софтуерно инженерство: Състояние и тенденции", Year = 2024, Journal = "Journal of Systems and Software", PublicationType = "Научна статия", Doi = "10.1016/j.jss.2024.112032", Abstract = "Ние систематично анализираме скорошни публикации по софтуерно инженерство, за да изследваме наличието, местоположението, форматите и дълготрайността на споделените артефакти. Нашите открития предоставят изчерпателен преглед на текущите практики и насоки за бъдещи възпроизводими изследвания." },
                new Publication { Id = 3, Title = "Чист код: Наръчник за гъвкаво софтуерно майсторство", Year = 2008, Journal = "Prentice Hall", PublicationType = "Книга", Doi = "9780132350884", Abstract = "Дори лошият код може да функционира. Но ако кодът не е чист, той може да срине една организация за разработка. Тази книга ви учи как да пишете чист, лесен за поддръжка и четим код, превръщайки ви в истински софтуерен майстор." },

                new Publication { Id = 101, Title = "Машинно обучение в здравеопазването", Year = 2022, Journal = "Nature", PublicationType = "Научна статия", Doi = "10.1038/s41586-022-00001", Abstract = "Изследване на приложението на невронни мрежи за ранна диагностика." },
                new Publication { Id = 102, Title = "Напредък в квантовите изчисления", Year = 2023, Journal = "Science", PublicationType = "Научна статия", Doi = "10.1126/science.abn0002", Abstract = "Анализ на новите методи за стабилизиране на кубити." },
                new Publication { Id = 103, Title = "Изкуствен интелект във финансите", Year = 2021, Journal = "Списание „Икономическа мисъл“", PublicationType = "Доклад от конференция", Doi = "10.1016/j.ecotod.2021.0003" },
                new Publication { Id = 104, Title = "Генетика и наследствени заболявания", Year = 2020, Journal = "Cell", PublicationType = "Научна статия", Doi = "10.1016/j.cell.2020.0004", Abstract = "Картографиране на генома при редки заболявания." },
                new Publication { Id = 105, Title = "Нови материали в нанотехнологиите", Year = 2023, Journal = "Академично издателство „Проф. Марин Дринов“", PublicationType = "Монография", Doi = "10.5555/nano-book-2023" }
            );

            modelBuilder.Entity<AuthorInstitution>().HasData(
                new AuthorInstitution { AuthorId = 1, InstitutionId = 1, Role = "Старши изследовател", StartYear = 2016 },
                new AuthorInstitution { AuthorId = 2, InstitutionId = 1, Role = "Главен инженер", StartYear = 2015 },

                new AuthorInstitution { AuthorId = 3, InstitutionId = 1, Role = "Изследовател", StartYear = 2017 },
                new AuthorInstitution { AuthorId = 4, InstitutionId = 104, Role = "Доцент", StartYear = 2019 },
                new AuthorInstitution { AuthorId = 5, InstitutionId = 104, Role = "Изследовател", StartYear = 2020 },
                new AuthorInstitution { AuthorId = 6, InstitutionId = 102, Role = "Софтуерен консултант", StartYear = 2001 },

                new AuthorInstitution { AuthorId = 101, InstitutionId = 101, Role = "Професор", StartYear = 2010 },
                new AuthorInstitution { AuthorId = 101, InstitutionId = 103, Role = "Гост-изследовател", StartYear = 2015 },
                new AuthorInstitution { AuthorId = 102, InstitutionId = 103, Role = "Главен асистент", StartYear = 2012 },
                new AuthorInstitution { AuthorId = 103, InstitutionId = 105, Role = "Доцент", StartYear = 2018 },
                new AuthorInstitution { AuthorId = 104, InstitutionId = 102, Role = "Професор", StartYear = 2005 },
                new AuthorInstitution { AuthorId = 105, InstitutionId = 108, Role = "Асистент", StartYear = 2020 },
                new AuthorInstitution { AuthorId = 106, InstitutionId = 104, Role = "Изследовател", StartYear = 2016 },
                new AuthorInstitution { AuthorId = 107, InstitutionId = 110, Role = "Професор", StartYear = 2008 },
                new AuthorInstitution { AuthorId = 108, InstitutionId = 109, Role = "Директор", StartYear = 2000 },
                new AuthorInstitution { AuthorId = 109, InstitutionId = 106, Role = "Физик", StartYear = 2019 },
                new AuthorInstitution { AuthorId = 110, InstitutionId = 107, Role = "Докторант", StartYear = 2021 }
            );

            modelBuilder.Entity<AuthorPublication>().HasData(
                new AuthorPublication { AuthorId = 1, PublicationId = 1, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 2, PublicationId = 1, ContributionRole = "Съавтор", AuthorOrder = 2 },
                new AuthorPublication { AuthorId = 3, PublicationId = 1, ContributionRole = "Съавтор", AuthorOrder = 3 },
                new AuthorPublication { AuthorId = 4, PublicationId = 2, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 5, PublicationId = 2, ContributionRole = "Съавтор", AuthorOrder = 2 },
                new AuthorPublication { AuthorId = 6, PublicationId = 3, ContributionRole = "Единствен автор", AuthorOrder = 1 },

                new AuthorPublication { AuthorId = 101, PublicationId = 101, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 105, PublicationId = 101, ContributionRole = "Съавтор", AuthorOrder = 2 },
                new AuthorPublication { AuthorId = 109, PublicationId = 102, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 104, PublicationId = 102, ContributionRole = "Съавтор", AuthorOrder = 2 },
                new AuthorPublication { AuthorId = 107, PublicationId = 103, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 110, PublicationId = 103, ContributionRole = "Съавтор", AuthorOrder = 2 },
                new AuthorPublication { AuthorId = 105, PublicationId = 104, ContributionRole = "Единствен автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 103, PublicationId = 105, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 108, PublicationId = 105, ContributionRole = "Съавтор", AuthorOrder = 2 }
            );

            modelBuilder.Entity<PublicationField>().HasData(
                new PublicationField { PublicationId = 1, ResearchFieldId = 1 },
                new PublicationField { PublicationId = 1, ResearchFieldId = 2 },
                new PublicationField { PublicationId = 2, ResearchFieldId = 3 },
                new PublicationField { PublicationId = 3, ResearchFieldId = 3 },
                new PublicationField { PublicationId = 3, ResearchFieldId = 4 },

                new PublicationField { PublicationId = 101, ResearchFieldId = 101 },
                new PublicationField { PublicationId = 101, ResearchFieldId = 106 },
                new PublicationField { PublicationId = 102, ResearchFieldId = 102 },
                new PublicationField { PublicationId = 102, ResearchFieldId = 101 },
                new PublicationField { PublicationId = 103, ResearchFieldId = 101 },
                new PublicationField { PublicationId = 103, ResearchFieldId = 107 },
                new PublicationField { PublicationId = 104, ResearchFieldId = 104 },
                new PublicationField { PublicationId = 104, ResearchFieldId = 106 },
                new PublicationField { PublicationId = 105, ResearchFieldId = 105 },
                new PublicationField { PublicationId = 105, ResearchFieldId = 102 }
            );
        }
    }
}