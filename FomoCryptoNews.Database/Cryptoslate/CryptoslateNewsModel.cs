using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FomoCryptoNews.Database.Cryptoslate;

public class CryptoslateNewsModel : AbstractModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Cover { get; set; }

    public Status Status { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }


    public static CryptoslateNewsModel CreateModel(string title, string description, string cover, Status status, DateTimeOffset createdAt)
    {
        return new CryptoslateNewsModel
        {
            Title = title,
            Description = description,
            Cover = cover,
            Status = status,
            CreatedAt = createdAt
        };
    }
}