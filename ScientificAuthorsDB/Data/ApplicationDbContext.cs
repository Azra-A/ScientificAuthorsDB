using Microsoft.EntityFrameworkCore;
using ScientificAuthorsDB.Models;

namespace ScientificAuthorsDB.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // таблиците
        public DbSet<Author> Authors { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<ResearchField> ResearchFields { get; set; }
        public DbSet<AuthorInstitution> AuthorInstitutions { get; set; }
        public DbSet<AuthorPublication> AuthorPublications { get; set; }
        public DbSet<PublicationField> PublicationFields { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  many-to-many таблиците
            modelBuilder.Entity<AuthorInstitution>()
                .HasKey(ai => new { ai.AuthorId, ai.InstitutionId });

            modelBuilder.Entity<AuthorPublication>()
                .HasKey(ap => new { ap.AuthorId, ap.PublicationId });

            modelBuilder.Entity<PublicationField>()
                .HasKey(pf => new { pf.PublicationId, pf.ResearchFieldId });

            // данни за тестване
            modelBuilder.Entity<ResearchField>().HasData(
                new ResearchField { Id = 1, Name = "Информатика" },
                new ResearchField { Id = 2, Name = "Физика" },
                new ResearchField { Id = 3, Name = "Математика" },
                new ResearchField { Id = 4, Name = "Биология" },
                new ResearchField { Id = 5, Name = "Химия" },
                new ResearchField { Id = 6, Name = "Медицина" },
                new ResearchField { Id = 7, Name = "Икономика" }
            );

            modelBuilder.Entity<Institution>().HasData(
                new Institution { Id = 1, Name = "Софийски университет", Country = "България", City = "София", Type = "Университет", Website = "https://www.uni-sofia.bg" },
                new Institution { Id = 2, Name = "МИТ (MIT)", Country = "САЩ", City = "Кеймбридж", Type = "Университет", Website = "https://www.mit.edu" },
                new Institution { Id = 3, Name = "БАН", Country = "България", City = "София", Type = "Изследователски център", Website = "https://www.bas.bg" },
                new Institution { Id = 4, Name = "Оксфордски университет", Country = "Великобритания", City = "Оксфорд", Type = "Университет", Website = "https://www.ox.ac.uk" },
                new Institution { Id = 5, Name = "Технически университет", Country = "България", City = "София", Type = "Университет", Website = "https://tu-sofia.bg" },
                new Institution { Id = 6, Name = "ЦЕРН", Country = "Швейцария", City = "Женева", Type = "Изследователски център", Website = "https://home.cern" },
                new Institution { Id = 7, Name = "Станфордски университет", Country = "САЩ", City = "Станфорд", Type = "Университет", Website = "https://www.stanford.edu" },
                new Institution { Id = 8, Name = "Медицински университет", Country = "България", City = "Пловдив", Type = "Университет", Website = "https://mu-plovdiv.bg" },
                new Institution { Id = 9, Name = "Институт 'Макс Планк'", Country = "Германия", City = "Мюнхен", Type = "Изследователски център", Website = "https://www.mpg.de" },
                new Institution { Id = 10, Name = "УНСС", Country = "България", City = "София", Type = "Университет", Website = "https://www.unwe.bg" }
            );

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FirstName = "Иван", LastName = "Петров", Email = "ivan@su.bg", BirthYear = 1975, OrcidId = "0000-0001-2345-6789" },
                new Author { Id = 2, FirstName = "Мария", LastName = "Иванова", Email = "maria@bas.bg", BirthYear = 1980, OrcidId = "0000-0002-9876-5432" },
                new Author { Id = 3, FirstName = "Георги", LastName = "Димитров", Email = "g.dimitrov@tu.bg", BirthYear = 1990, OrcidId = "0000-0003-1111-2222" },
                new Author { Id = 4, FirstName = "Джон", LastName = "Смит", Email = "jsmith@mit.edu", BirthYear = 1978, OrcidId = "0000-0002-4444-5555" },
                new Author { Id = 5, FirstName = "Елена", LastName = "Николова", Email = "elena.n@mu-plovdiv.bg", BirthYear = 1985, OrcidId = "0000-0001-7777-8888" },
                new Author { Id = 6, FirstName = "Алис", LastName = "Джонсън", Email = "alice.j@oxford.ac.uk", BirthYear = 1982, OrcidId = "0000-0002-3333-6666" },
                new Author { Id = 7, FirstName = "Петър", LastName = "Тодоров", Email = "p.todorov@unwe.bg", BirthYear = 1970, OrcidId = "0000-0003-9999-0000" },
                new Author { Id = 8, FirstName = "Ханс", LastName = "Мюлер", Email = "hmuller@mpg.de", BirthYear = 1965, OrcidId = "0000-0001-5555-4444" },
                new Author { Id = 9, FirstName = "Силвия", LastName = "Митева", Email = "smiteva@cern.ch", BirthYear = 1988, OrcidId = "0000-0002-1234-5678" },
                new Author { Id = 10, FirstName = "Робърт", LastName = "Браун", Email = "rbrown@stanford.edu", BirthYear = 1992, OrcidId = "0000-0002-8888-1111" }
            );

            modelBuilder.Entity<Publication>().HasData(
                new Publication { Id = 1, Title = "Машинно обучение в здравеопазването", Year = 2022, Journal = "Природа", PublicationType = "Научна статия", Doi = "10.1038/s41586-022-00001", Abstract = "Изследване на приложението на невронни мрежи за ранна диагностика." },
                new Publication { Id = 2, Title = "Напредък в квантовите изчисления", Year = 2023, Journal = "Наука", PublicationType = "Научна статия", Doi = "10.1126/science.abn0002", Abstract = "Анализ на новите методи за стабилизиране на кубити." },
                new Publication { Id = 3, Title = "Изкуствен интелект във финансите", Year = 2021, Journal = "Икономика днес", PublicationType = "Доклад от конференция", Doi = "10.1016/j.ecotod.2021.0003" },
                new Publication { Id = 4, Title = "Генетика и наследствени заболявания", Year = 2020, Journal = "Клетка", PublicationType = "Научна статия", Doi = "10.1016/j.cell.2020.0004", Abstract = "Картографиране на генома при редки заболявания." },
                new Publication { Id = 5, Title = "Нови материали в нанотехнологиите", Year = 2023, Journal = "Издателство 'Наука'", PublicationType = "Монография", Doi = "10.5555/nano-book-2023" },
                new Publication { Id = 6, Title = "Алгоритми за оптимизация на невронни мрежи", Year = 2019, Journal = "Инженерни науки", PublicationType = "Научна статия", Doi = "10.1109/TNNLS.2019.0006" },
                new Publication { Id = 7, Title = "Тъмна материя и структура на Вселената", Year = 2022, Journal = "Астрофизичен журнал", PublicationType = "Научна статия", Doi = "10.3847/1538-4357/ac0007", Abstract = "Моделиране на разпределението на тъмната материя в близките галактики." },
                new Publication { Id = 8, Title = "Влияние на климата върху биоразнообразието", Year = 2021, Journal = "Екология", PublicationType = "Книга", Doi = "10.1002/ecy.0008" },
                new Publication { Id = 9, Title = "Разработване на ваксини чрез мРНК", Year = 2020, Journal = "Списание Ланцет", PublicationType = "Научна статия", Doi = "10.1016/S0140-6736(20)0009" },
                new Publication { Id = 10, Title = "Криптовалути и икономическа стабилност", Year = 2023, Journal = "Журнал по финанси", PublicationType = "Доклад от конференция", Doi = "10.1111/jofi.0010" },
                new Publication { Id = 11, Title = "Анализ на киберсигурността в държавния сектор", Year = 2023, Journal = "Институт за технологични политики", PublicationType = "Доклад от конференция", Doi = "10.5555/techpol-2023-011" },
                new Publication { Id = 12, Title = "Еволюция на черните дупки", Year = 2018, Journal = "Софийски университет", PublicationType = "Дисертация", Doi = "10.5555/su-thesis-2018-012", Abstract = "Теоретичен модел за изпарението на свръхмасивни черни дупки." },
                new Publication { Id = 13, Title = "Годишен доклад за състоянието на околната среда", Year = 2022, Journal = "Министерство на екологията", PublicationType = "Монография", Doi = "10.5555/moew-report-2022" },
                new Publication { Id = 14, Title = "Съвременни методи за лечение на диабет", Year = 2021, Journal = "Медицински преглед", PublicationType = "Научна статия", Doi = "10.5555/medrev-2021-014", Abstract = "Обзор на най-новите клинични проучвания." },
                new Publication { Id = 15, Title = "Въздействие на инфлацията върху малкия бизнес", Year = 2023, Journal = "Икономически институт", PublicationType = "Доклад от конференция", Doi = "10.5555/bas-econ-2023-015" }
            );

            modelBuilder.Entity<AuthorInstitution>().HasData(
                new AuthorInstitution { AuthorId = 1, InstitutionId = 1, Role = "Професор", StartYear = 2010 },
                new AuthorInstitution { AuthorId = 1, InstitutionId = 3, Role = "Гост-изследовател", StartYear = 2015 },
                new AuthorInstitution { AuthorId = 2, InstitutionId = 3, Role = "Главен асистент", StartYear = 2012 },
                new AuthorInstitution { AuthorId = 3, InstitutionId = 5, Role = "Доцент", StartYear = 2018 },
                new AuthorInstitution { AuthorId = 4, InstitutionId = 2, Role = "Професор", StartYear = 2005 },
                new AuthorInstitution { AuthorId = 5, InstitutionId = 8, Role = "Асистент", StartYear = 2020 },
                new AuthorInstitution { AuthorId = 6, InstitutionId = 4, Role = "Изследовател", StartYear = 2016 },
                new AuthorInstitution { AuthorId = 7, InstitutionId = 10, Role = "Професор", StartYear = 2008 },
                new AuthorInstitution { AuthorId = 8, InstitutionId = 9, Role = "Директор", StartYear = 2000 },
                new AuthorInstitution { AuthorId = 9, InstitutionId = 6, Role = "Физик", StartYear = 2019 },
                new AuthorInstitution { AuthorId = 10, InstitutionId = 7, Role = "Докторант", StartYear = 2021 }
            );

            modelBuilder.Entity<AuthorPublication>().HasData(
                new AuthorPublication { AuthorId = 1, PublicationId = 1, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 5, PublicationId = 1, ContributionRole = "Съавтор", AuthorOrder = 2 },
                new AuthorPublication { AuthorId = 9, PublicationId = 2, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 4, PublicationId = 2, ContributionRole = "Съавтор", AuthorOrder = 2 },
                new AuthorPublication { AuthorId = 7, PublicationId = 3, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 10, PublicationId = 3, ContributionRole = "Съавтор", AuthorOrder = 2 },
                new AuthorPublication { AuthorId = 5, PublicationId = 4, ContributionRole = "Единствен автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 3, PublicationId = 5, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 8, PublicationId = 5, ContributionRole = "Съавтор", AuthorOrder = 2 },
                new AuthorPublication { AuthorId = 1, PublicationId = 6, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 3, PublicationId = 6, ContributionRole = "Съавтор", AuthorOrder = 2 },
                new AuthorPublication { AuthorId = 10, PublicationId = 6, ContributionRole = "Съавтор", AuthorOrder = 3 },
                new AuthorPublication { AuthorId = 9, PublicationId = 7, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 6, PublicationId = 7, ContributionRole = "Съавтор", AuthorOrder = 2 },
                new AuthorPublication { AuthorId = 2, PublicationId = 8, ContributionRole = "Единствен автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 5, PublicationId = 9, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 6, PublicationId = 9, ContributionRole = "Съавтор", AuthorOrder = 2 },
                new AuthorPublication { AuthorId = 7, PublicationId = 10, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 3, PublicationId = 11, ContributionRole = "Главен изследовател", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 9, PublicationId = 12, ContributionRole = "Единствен автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 2, PublicationId = 13, ContributionRole = "Координатор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 8, PublicationId = 13, ContributionRole = "Рецензент", AuthorOrder = 2 },
                new AuthorPublication { AuthorId = 5, PublicationId = 14, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 7, PublicationId = 15, ContributionRole = "Единствен автор", AuthorOrder = 1 }
            );

            modelBuilder.Entity<PublicationField>().HasData(
                new PublicationField { PublicationId = 1, ResearchFieldId = 1 },
                new PublicationField { PublicationId = 1, ResearchFieldId = 6 },
                new PublicationField { PublicationId = 2, ResearchFieldId = 2 },
                new PublicationField { PublicationId = 2, ResearchFieldId = 1 },
                new PublicationField { PublicationId = 3, ResearchFieldId = 1 },
                new PublicationField { PublicationId = 3, ResearchFieldId = 7 },
                new PublicationField { PublicationId = 4, ResearchFieldId = 4 },
                new PublicationField { PublicationId = 4, ResearchFieldId = 6 },
                new PublicationField { PublicationId = 5, ResearchFieldId = 5 },
                new PublicationField { PublicationId = 5, ResearchFieldId = 2 },
                new PublicationField { PublicationId = 6, ResearchFieldId = 1 },
                new PublicationField { PublicationId = 6, ResearchFieldId = 3 },
                new PublicationField { PublicationId = 7, ResearchFieldId = 2 },
                new PublicationField { PublicationId = 8, ResearchFieldId = 4 },
                new PublicationField { PublicationId = 9, ResearchFieldId = 6 },
                new PublicationField { PublicationId = 9, ResearchFieldId = 4 },
                new PublicationField { PublicationId = 10, ResearchFieldId = 7 },
                new PublicationField { PublicationId = 11, ResearchFieldId = 1 },
                new PublicationField { PublicationId = 12, ResearchFieldId = 2 },
                new PublicationField { PublicationId = 13, ResearchFieldId = 4 },
                new PublicationField { PublicationId = 13, ResearchFieldId = 5 },
                new PublicationField { PublicationId = 14, ResearchFieldId = 6 },
                new PublicationField { PublicationId = 15, ResearchFieldId = 7 }
            );
        }
    }
}