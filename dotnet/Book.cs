
  public class Book
  {
      public Guid Id { get; set; }
      [Required]
      public string Author { get; set; }
      [Required]
      public string Title { get; set; }
      public string Description { get; set; }
  }
