using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models;

[Table("Tag")]
public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;

    public IList<Post> Posts { get; set; } = null!;
}
