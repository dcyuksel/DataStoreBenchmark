using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStoreBenchmark.Entities;

[Table("Movies", Schema = "dbo")]
public sealed class Movie
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = default!;

    [MaxLength(200)]
    public string? Director { get; set; }

    public int? ReleaseYear { get; set; }

    [MaxLength(50)]
    public string? Genre { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}