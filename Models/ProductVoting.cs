using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ProductVoting
{
    [Key]
    public Guid Id { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? CreatedBy { get; set; }

    public int? Star { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(500)]
    public string? Content { get; set; }

    public int? Helpful { get; set; }

    public int? UnHelpful { get; set; }

    public DateTime? CreatedDate { get; set; }
}  