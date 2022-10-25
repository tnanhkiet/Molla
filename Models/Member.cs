using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Member
{
    [Key]
    public Guid Id { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(150)]
    public string? Name { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(500)]
    public string? Picture { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(50)]
    public string? LoginName { get; set; }

    [Column(TypeName = "nvarchar")]
    [MaxLength(50)]
    public string? Password { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? Status { get; set; }
}