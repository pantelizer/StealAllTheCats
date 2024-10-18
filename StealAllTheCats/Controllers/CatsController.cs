using Microsoft.AspNetCore.Mvc;
using StealAllTheCats.Client;
using StealAllTheCats.Interfaces;
using StealAllTheCats.Models;

namespace StealAllTheCats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatsController : ControllerBase
    {
        private readonly IStealTheCatService _stealTheCatService;

        private readonly GetCatsClient _catClient;

        private readonly StealCatsDbContext _dbcontext;

        private readonly Serilog.ILogger _logger;
        public CatsController(IStealTheCatService stealTheCatService, GetCatsClient catClient, StealCatsDbContext dbContext, Serilog.ILogger logger)
        {
            _stealTheCatService = stealTheCatService;
            _catClient = catClient;
            _dbcontext = dbContext;
            _logger = logger;
        }

        [HttpPost("fetch")]
        public async Task<ActionResult> GetRandomCats()
        {
            try
            {
                var response = await _catClient.GetCatImages();

                if (response == null)
                {
                    return NotFound();
                }

                var postedCats = await _stealTheCatService.PostCatImages(response);

                if (postedCats == false)
                {
                    return NotFound();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error("An unexpected error occured. {ex}", ex.ToString());
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CatEntity>> GetCatById(int id)
        {
            if (id.GetType() != typeof(int)) return BadRequest("The Id must be of type integer.");

            if (id < 1) return BadRequest("Id must be bigger than the value of 0.");

            try
            {
                var cat = await _stealTheCatService.GetCatEntityById(id);

                if (cat == null) return NotFound();

                return Ok(cat);
            }
            catch (Exception ex)
            {
                _logger.Error("An unexpected error occured. {ex}", ex.ToString());
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet]
        public async Task<ActionResult<CatEntity>> GetCatsWithPagination(string? tag = null, int page = 1, int pageSize = 10)
        {

            if (page < 1)
            {
                return BadRequest("Page number must be greater than 0.");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Page size must be between 1 and 100.");
            }

            try
            {
                if (tag == null)
                {
                    var cats = await _stealTheCatService.GetCatsWithPagination(page, pageSize);
                    return Ok(cats);
                }

                if (tag.GetType() != typeof(string))
                {
                    return BadRequest("Type of tag must be string");
                }
                else
                {
                    var cats = await _stealTheCatService.GetCatsWithTag(tag, page, pageSize);
                    return Ok(cats);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("An unexpected error occured. {ex}", ex.ToString());
                return StatusCode(500, ex.Message);
            }

        }
    }
}
