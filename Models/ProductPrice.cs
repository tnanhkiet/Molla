using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ProductPrice
{
    [Key]
    public Guid Id { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? ColorId { get; set; }

    public Guid? SizeId { get; set; }

    public decimal? Price { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(500)]
    public string? Picture { get; set; }

    public int? Amount { get; set; }
}