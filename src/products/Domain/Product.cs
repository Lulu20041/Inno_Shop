namespace Domain
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }

        public bool IsAvailable { get; set; }

        public double Price { get; set; }

        public int CreatedUserId { get; set; }


    }
}
