using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StealAllTheCats.Models
{
    public class CatEntity
    {
        public int Id { get; set; }
        public string CatId { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Image { get; set; } = [];
        public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
        public List<TagEntity> Tags { get; set; } = new();
    }

}
