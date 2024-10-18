using StealAllTheCats.Client;
using StealAllTheCats.Models;

namespace StealAllTheCats.Interfaces
{
    public interface IStealTheCatService
    {
        Task<bool> PostCatImages(List<CatClientModel> catClientModel);
        bool CheckIfCatEntityExists(CatEntity cat);
        Task<CatEntity> GetCatEntityById(int id);
        Task<List<CatEntity>> GetCatsWithPagination(int page, int pageSize);
        Task<List<CatEntity>> GetCatsWithTag(string? tag, int page, int pageSize);
    }
}
