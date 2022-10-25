using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class News
{
    [Key]
    public Guid Id { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(250)]
    public string? Title { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(500)]
    public string? Picture { get; set;}

    [DataType(DataType.Date)]
    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    [Column(TypeName = "ntext")]
    public string? Content { get; set; }
}