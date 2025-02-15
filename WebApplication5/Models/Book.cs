namespace WebApplication5.Models
{
    public class Book
    {
        public Book() { }
        public Book(string name, int rating, string description, string jenres)
        {
            Name = name;
            Rating = rating;
            Description = description;
            Jenres = jenres;
        }
        public int Id { set; get; }
        public string? Name { set; get; }
        public string? Description { set; get; }
        public string? Jenres { set; get; }
        public int Rating { set; get; }

    }
}
