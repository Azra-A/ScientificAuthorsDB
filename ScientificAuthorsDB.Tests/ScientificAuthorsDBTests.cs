using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ScientificAuthorsDB.Controllers;
using ScientificAuthorsDB.Data;
using ScientificAuthorsDB.Models;
using Xunit;

namespace ScientificAuthorsDB.Tests
{
    public static class DbHelper
    {
        public static ApplicationDbContext CreateInMemoryDb(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted();
            return context;
        }

        public static ApplicationDbContext CreateSeededDb(string dbName)
        {
            var context = CreateInMemoryDb(dbName);

            var su = new Institution { Id = 1, Name = "Софийски университет", Country = "България", City = "София", Type = "University" };
            var bas = new Institution { Id = 2, Name = "БАН", Country = "България", City = "София", Type = "Research Center" };
            context.Institutions.AddRange(su, bas);

            var cs = new ResearchField { Id = 1, Name = "Computer Science" };
            var math = new ResearchField { Id = 2, Name = "Mathematics" };
            context.ResearchFields.AddRange(cs, math);

            var author1 = new Author { Id = 1, FirstName = "Иван", LastName = "Петров", Email = "ivan@su.bg", BirthYear = 1975 };
            var author2 = new Author { Id = 2, FirstName = "Мария", LastName = "Иванова", Email = "maria@bas.bg", BirthYear = 1980 };
            context.Authors.AddRange(author1, author2);

            var pub1 = new Publication { Id = 1, Title = "Machine Learning в медицината", Year = 2022, Journal = "Nature", PublicationType = "Article" };
            var pub2 = new Publication { Id = 2, Title = "Квантови изчисления", Year = 2023, Journal = "Science", PublicationType = "Article" };
            context.Publications.AddRange(pub1, pub2);

            context.AuthorInstitutions.AddRange(
                new AuthorInstitution { AuthorId = 1, InstitutionId = 1, Role = "Professor" },
                new AuthorInstitution { AuthorId = 2, InstitutionId = 2, Role = "Researcher" }
            );

            context.AuthorPublications.AddRange(
                new AuthorPublication { AuthorId = 1, PublicationId = 1, ContributionRole = "Lead Author", AuthorOrder = 1 },
                new AuthorPublication { AuthorId = 2, PublicationId = 1, ContributionRole = "Co-Author", AuthorOrder = 2 },
                new AuthorPublication { AuthorId = 2, PublicationId = 2, ContributionRole = "Lead Author", AuthorOrder = 1 }
            );

            context.PublicationFields.AddRange(
                new PublicationField { PublicationId = 1, ResearchFieldId = 1 },
                new PublicationField { PublicationId = 2, ResearchFieldId = 2 }
            );

            context.SaveChanges();
            return context;
        }
    }



    //  ТЕСТ 1 
    public class Test1_AddAuthor
    {
        [Fact]
        public async Task CreateAuthor_SavesCorrectlyToDatabase()
        {
            var context = DbHelper.CreateInMemoryDb("Test1_AddAuthor");

            var newAuthor = new Author
            {
                FirstName = "Георги",
                LastName = "Димитров",
                Email = "georgi@uni.bg",
                BirthYear = 1985
            };

            context.Authors.Add(newAuthor);
            await context.SaveChangesAsync();

            var savedAuthor = await context.Authors.FirstOrDefaultAsync(a => a.Email == "georgi@uni.bg");
            Assert.NotNull(savedAuthor);
            Assert.Equal("Георги", savedAuthor.FirstName);
            Assert.Equal("Димитров", savedAuthor.LastName);
            Assert.Equal(1985, savedAuthor.BirthYear);
        }
    }

    //  ТЕСТ 2
    public class Test2_SearchAuthorByName
    {
        [Fact]
        public async Task Index_FiltersByName_ReturnsCorrectAuthors()
        {
            var context = DbHelper.CreateSeededDb("Test2_SearchByName");
            var controller = new AuthorsController(context);

            var result = await controller.Index(searchName: "Георги", searchField: null, searchYear: null);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Author>>(viewResult.Model);
            var list = model.ToList();

            Assert.Empty(list);
        }
    }

    //  ТЕСТ 3 
    public class Test3_AuthorInstitutionRelation
    {
        [Fact]
        public async Task AuthorInstitution_ManyToMany_IsStoredCorrectly()
        {
            var context = DbHelper.CreateSeededDb("Test3_ManyToMany");

            var relation = await context.AuthorInstitutions
                .Include(ai => ai.Author)
                .Include(ai => ai.Institution)
                .FirstOrDefaultAsync(ai => ai.AuthorId == 1 && ai.InstitutionId == 1);

            Assert.NotNull(relation);
            Assert.Equal("Иван", relation.Author.FirstName);
            Assert.Equal("Софийски университет", relation.Institution.Name);
            Assert.Equal("Professor", relation.Role);
        }
    }

    //  ТЕСТ 4 
    public class Test4_PublicationHasMultipleAuthors
    {
        [Fact]
        public async Task Publication_CanHaveMultipleAuthors()
        {
            var context = DbHelper.CreateSeededDb("Test4_PubAuthors");

            var publication = await context.Publications
                .Include(p => p.AuthorPublications)
                    .ThenInclude(ap => ap.Author)
                .FirstOrDefaultAsync(p => p.Id == 1);

            Assert.NotNull(publication);
            Assert.Equal(2, publication.AuthorPublications.Count);

            var names = publication.AuthorPublications.Select(ap => ap.Author.FullName).ToList();
            Assert.Contains("Иван Петров", names);
            Assert.Contains("Мария Иванова", names);
        }
    }

    //  ТЕСТ 5 
    public class Test5_DeleteInstitution
    {
        [Fact]
        public async Task DeleteInstitution_RemovesFromDatabase()
        {
            var context = DbHelper.CreateSeededDb("Test5_DeleteInst");
            var controller = new InstitutionsController(context);

            var result = await controller.DeleteConfirmed(1);

            var deleted = await context.Institutions.FindAsync(1);
            Assert.Null(deleted);
            Assert.IsType<RedirectToActionResult>(result);
        }
    }

    //  ТЕСТ 6 
    public class Test6_FilterPublicationsByType
    {
        [Fact]
        public async Task Publications_FilteredByType_ReturnsOnlyMatchingType()
        {
            var context = DbHelper.CreateSeededDb("Test6_FilterType");

            context.Publications.Add(new Publication
            {
                Id = 3,
                Title = "Квантова физика - учебник",
                Year = 2021,
                PublicationType = "Book"
            });
            await context.SaveChangesAsync();

            var controller = new PublicationsController(context);

            var result = await controller.Index(searchTitle: null, searchType: "Article");

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Publication>>(viewResult.Model);
            var list = model.ToList();

            Assert.Equal(2, list.Count);
            Assert.All(list, p => Assert.Equal("Article", p.PublicationType));
        }
    }

   
}
