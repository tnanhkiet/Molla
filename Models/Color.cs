using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Color
{
    [Key]
    public Guid Id { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(50)]
    public string? Name { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(10)]
    public string? Code { get; set; }
}