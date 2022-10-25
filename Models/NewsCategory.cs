using System.ComponentModel.DataAnnotations;

public class NewsCategory
{
    [Key]
    public Guid Id { get; set; }
    public Guid? NewsId { get; set; }
    public Guid? CategoryId { get; set; }
}