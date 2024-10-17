using Azure;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using StealAllTheCats;
using StealAllTheCats.Client;
using StealAllTheCats.Models;
using StealAllTheCats.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealTheCats.Tests
{
    public class StealTheCatServiceTests
    {
        private readonly StealCatsDbContext _context;
        private readonly StealTheCatService _service;
        private readonly HttpClient _httpClient;
        private readonly Serilog.ILogger _logger;

        public StealTheCatServiceTests()
        {
            var options = new DbContextOptionsBuilder<StealCatsDbContext>().
                UseInMemoryDatabase("TestCatApiDatabase")
                .Options;

            _context = new StealCatsDbContext(options);
            _service = new StealTheCatService(_httpClient, _context, _logger);
        }

        [Fact]
        public async Task AddTagToCat_ShouldReturnCatWithTag()
        {
            //Arrange
            var catEntity = new CatEntity { CatId = "1m3nfdio", Tags = [] };
            var receivingTag = "Gorgeous";

            // Act   
            var tag = new TagEntity() { Name = receivingTag };
            catEntity.Tags.Add(tag);

            // Assert
            Assert.Contains(catEntity.Tags, t => t.Name == "Gorgeous");

        }

        [Fact]
        public async Task GetTagsFromApiCat_ShouldReturnCatWithTags()
        {
            // Arrange
            var catEntity = new CatEntity() { Id = 5, CatId = "fd4w435de", Tags = [] };
            var temperamentString = "Intelligent, Loyal, Playful";

            // Act
            var arrayOfTemperaments = temperamentString.Split(',');
            foreach (string tag in arrayOfTemperaments)
            {
                var trimmedString = tag.Trim();
                await _service.AddTagToCat(catEntity, trimmedString);
            }

            // Assert
            Assert.Contains(catEntity.Tags, t => t.Name == "Intelligent");
            Assert.Contains(catEntity.Tags, t => t.Name == "Loyal");
            Assert.Contains(catEntity.Tags, t => t.Name == "Playful");

        }

        [Fact]
        public async Task GetCatById_ShouldReturnCatWithId()
        {
            // Arrange
            int Id = 1;
            var catEntity = new CatEntity { Id = Id, CatId = "efd324234", Height = 500, Width = 300, Tags = [] };

            // Act
            await _context.Cats.AddAsync(catEntity);
            await _context.SaveChangesAsync();

            // Assert
            var retrievedCat = await _context.Cats.Include(b => b.Tags).Where(c => c.Id == Id).FirstOrDefaultAsync();
            Assert.Equal(catEntity, retrievedCat);
        }

        [Fact]
        public async Task GetCatsWithTag_ShouldReturnCatsThatHaveASpecificTag()
        {
            // Arrange
            var tag1 = new TagEntity { Id = 9, Name = "Intelligent" };
            var tag2 = new TagEntity { Id = 8, Name = "Loyal" };

            var catEntity1 = new CatEntity { Id = 2, CatId = "gfdijgn39", Height = 500, Width = 500, Tags = [tag1] };
            var catEntity2 = new CatEntity { Id = 3, CatId = "gfdijgn3343", Height = 500, Width = 500, Tags = [tag2] };
            var catEntity3 = new CatEntity { Id = 4, CatId = "gfdijg3434e", Height = 500, Width = 500, Tags = [tag1] };
            List<CatEntity> catList = [catEntity1, catEntity3];

            // Act
            await _context.Cats.AddAsync(catEntity1);
            await _context.Cats.AddAsync(catEntity2);
            await _context.Cats.AddAsync(catEntity3);
            await _context.SaveChangesAsync();

            // Assert
            var cats = await _context.Cats.Where(t => t.Tags.Any(t => t.Name == tag1.Name)).Include(t => t.Tags).ToListAsync();
            Assert.Equal(catList, cats);
        }
    }
}
