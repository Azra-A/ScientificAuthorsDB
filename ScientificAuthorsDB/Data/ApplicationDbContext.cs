using Microsoft.EntityFrameworkCore;
using ScientificAuthorsDB.Models;

namespace ScientificAuthorsDB.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Тези редове създават самите таблици
        public DbSet<Author> Authors { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<ResearchField> ResearchFields { get; set; }
        public DbSet<AuthorInstitution> AuthorInstitutions { get; set; }
        public DbSet<AuthorPublication> AuthorPublications { get; set; }
        public DbSet<PublicationField> PublicationFields { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Дефиниране на сложните ключове за Many-to-Many таблиците
            modelBuilder.Entity<AuthorInstitution>()
                .HasKey(ai => new { ai.AuthorId, ai.InstitutionId });

            modelBuilder.Entity<AuthorPublication>()
                .HasKey(ap => new { ap.AuthorId, ap.PublicationId });

            modelBuilder.Entity<PublicationField>()
                .HasKey(pf => new { pf.PublicationId, pf.ResearchFieldId });

            // 2. Добавяне на първоначални (Seed) данни за тест - ПРЕВЕДЕНИ НА БЪЛГАРСКИ
            modelBuilder.Entity<ResearchField>().HasData(
                new ResearchField { Id = 1, Name = "Информатика" },
                new ResearchField { Id = 2, Name = "Физика" },
                new ResearchField { Id = 3, Name = "Математика" },
                new ResearchField { Id = 4, Name = "Биология" },
                new ResearchField { Id = 5, Name = "Химия" }
            );

            modelBuilder.Entity<Institution>().HasData(
                new Institution { Id = 1, Name = "Софийски университет", Country = "България", City = "София", Type = "Университет" },
                new Institution { Id = 2, Name = "МИТ", Country = "САЩ", City = "Кеймбридж", Type = "Университет" },
                new Institution { Id = 3, Name = "БАН", Country = "България", City = "София", Type = "Изследователски център" }
            );

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FirstName = "Иван", LastName = "Петров", Email = "ivan@su.bg", BirthYear = 1975 },
                new Author { Id = 2, FirstName = "Мария", LastName = "Иванова", Email = "maria@bas.bg", BirthYear = 1980 }
            );

            modelBuilder.Entity<Publication>().HasData(
                new Publication { Id = 1, Title = "Машинно обучение в здравеопазването", Year = 2022, Journal = "Nature", PublicationType = "Научна статия" },
                new Publication { Id = 2, Title = "Напредък в квантовите изчисления", Year = 2023, Journal = "Science", PublicationType = "Научна статия" }
            );

            modelBuilder.Entity<AuthorInstitution>().HasData(
                new AuthorInstitution { AuthorId = 1, InstitutionId = 1, Role = "Професор" },
                new AuthorInstitution { AuthorId = 2, InstitutionId = 3, Role = "Изследовател" }
            );

            modelBuilder.Entity<AuthorPublication>().HasData(
                new AuthorPublication { AuthorId = 1, PublicationId = 1, ContributionRole = "Водещ автор", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 2, PublicationId = 1, ContributionRole = "Съавтор", AuthorOrder = 2 },
                new AuthorPublication { AuthorId = 2, PublicationId = 2, ContributionRole = "Водещ автор", AuthorOrder = 1 }
            );

            modelBuilder.Entity<PublicationField>().HasData(
                new PublicationField { PublicationId = 1, ResearchFieldId = 1 },
                new PublicationField { PublicationId = 2, ResearchFieldId = 2 }
            );
        }
    }
}