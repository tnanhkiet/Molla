using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Category
{
    [Key]
    public Guid Id { get; set;}

    [Column(TypeName = "nvarchar")]
    [MaxLength(150)]
    public string? Name { get; set;}

    public Guid? ParentId { get; set;}
}