using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Comment
{
    [Key]
    public Guid Id { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(500)]
    public string? Content { get; set; }

    public Guid? CreatedBy { get; set;}

    public DateTime? CreatedDate { get; set; }
    
    public Guid? NewsId { get; set; }
}