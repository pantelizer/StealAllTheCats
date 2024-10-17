namespace StealAllTheCats.Models
{
    public class CatDto
    {
        public int Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Image { get; set; }
        public List<string> Tags { get; set; }
    }
}
