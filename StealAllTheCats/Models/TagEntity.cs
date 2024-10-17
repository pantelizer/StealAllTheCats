using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StealAllTheCats.Models
{
    public class TagEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

        [JsonIgnore]
        public List<CatEntity>? Cats { get; set; } = new();

    }
}
