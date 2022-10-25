using System.ComponentModel.DataAnnotations;

public class ProductCategory
{
    [Key]
    public Guid Id { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? CategoryId { get; set; }
}