namespace _2301B2TempEmbedding.DTOs
{
    public class ItemCreateDto
    {
        public IFormFile image { get; set; }
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Price { get; set; }

        public int? CatId { get; set; }

    }
}
