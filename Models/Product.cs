using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    [Key]
    public Guid Id { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(150)]
    public string? Name { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "ntext")]
    public string? Information { get; set; }

    [Column(TypeName = "ntext")]
    public string? Additional { get; set; }

    [Column(TypeName = "ntext")]
    public string? Shipping { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(500)]
    public string? Picture { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? BrandId { get; set; }
}