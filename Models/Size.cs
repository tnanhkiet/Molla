using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Size
{
    [Key]
    public Guid Id { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(50)]
    public string? Name { get; set; }

    public Guid? ParentId { get; set; }
}