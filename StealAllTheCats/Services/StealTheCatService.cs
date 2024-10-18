using Microsoft.EntityFrameworkCore;
using StealAllTheCats.Client;
using StealAllTheCats.Interfaces;
using StealAllTheCats.Models;

namespace StealAllTheCats.Services
{
    public class StealTheCatService : IStealTheCatService
    {
        private readonly HttpClient _httpClient;

        private readonly StealCatsDbContext _dbcontext;

        private readonly Serilog.ILogger _logger;

        public StealTheCatService(HttpClient httpclient, StealCatsDbContext dbContext, Serilog.ILogger logger)
        {
            _httpClient = httpclient;
            _dbcontext = dbContext;
            _logger = logger;
        }

        public async Task<bool> PostCatImages(List<CatClientModel> catClientModel)
        {
            try
            {
                if (catClientModel == null) throw new ArgumentNullException(nameof(catClientModel));

                foreach (CatClientModel individualCat in catClientModel)
                {
                    var catEntity = new CatEntity
                    {
                        CatId = individualCat.Id,
                        Width = individualCat.Width,
                        Height = individualCat.Height
                    };
                    catEntity.Image = await _httpClient.GetByteArrayAsync(individualCat.Url);

                    await GetTagsFromApiCat(individualCat, catEntity);

                    await TryAddCatEntityToDb(catEntity);

                    await TryToSaveToDb();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("An exception has occured: {ex}", ex.ToString());
                throw;
            }
        }

        public async Task GetTagsFromApiCat(CatClientModel catClientModel, CatEntity catEntity)
        {
            if (catClientModel.Breeds != null)
            {
                foreach (Breed breed in catClientModel.Breeds)
                {
                    string[] arrayOfTags = breed.Temperament.Split(',');

                    foreach (string tagName in arrayOfTags)
                    {
                        var trimmedTag = tagName.Trim();
                        await AddTagToCatEntity(catEntity, trimmedTag);
                    }
                }
            }
        }

        public async Task AddTagToCatEntity(CatEntity catEntity, string receivingTag)
        {
            try
            {
                var existingTag = await _dbcontext.Tags.FirstOrDefaultAsync(t => t.Name == receivingTag);
                if (existingTag == null)
                {
                    var tag = new TagEntity() { Name = receivingTag };
                    catEntity.Tags.Add(tag);
                }
                else
                {
                    catEntity.Tags.Add(existingTag);
                }

            }
            catch (Exception ex)
            {
                _logger.Warning("Failed to add a tag to a CatEntity with exception message {ex}", ex.ToString());
            }
        }

        public bool CheckIfCatEntityExists(CatEntity cat)
        {
            var catIsInDb = _dbcontext.Cats.Any(c => c.CatId == cat.CatId);
            return catIsInDb;
        }

        public async Task<CatEntity> GetCatEntityById(int id)
        {
            return await _dbcontext.Cats.Where(o => o.Id == id).Include(x => x.Tags).FirstOrDefaultAsync();
        }

        public async Task<List<CatEntity>> GetCatsWithTag(string? tag, int page, int pageSize)
        {
            var cats = await _dbcontext.Cats.Where(t => t.Tags.Any(t => t.Name == tag)).Include(t => t.Tags).Skip((page - 1) * pageSize).OrderBy(t => t.Id).Take(pageSize).ToListAsync();
            return cats;
        }

        public async Task<List<CatEntity>> GetCatsWithPagination(int page = 1, int pageSize = 10)
        {
            return await _dbcontext.Cats.Include(t => t.Tags).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task TryToSaveToDb()
        {
            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Warning("Failed to save to the database with exception message: {ex}", ex.ToString());
            }
        }

        public async Task TryAddCatEntityToDb(CatEntity catEntity)
        {
            if (CheckIfCatEntityExists(catEntity) == false)
            {
                await _dbcontext.Cats.AddAsync(catEntity);
            }
            else
            {
                _logger.Information("Cat with Id of {catEntity} already exists.", catEntity.CatId);
            }
        }
    }
}
