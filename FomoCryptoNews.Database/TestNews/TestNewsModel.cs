using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FomoCryptoNews.Database.TestNews;

public class TestNewsModel : AbstractModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string? Cover { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }


    public static TestNewsModel CreateModel(string title, string description, string? cover)
    {
        return new TestNewsModel
        {
            Title = title,
            Description = description,
            Cover = cover,
            CreatedAt = DateTime.Now
        };
    }
}