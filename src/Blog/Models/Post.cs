﻿namespace Blog.Models;

public class Post
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int AuthorId { get; set; }
    public string Title { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string Body { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    public Category Category { get; set; } = null!;
    public User Author { get; set; } = null!;
    public List<Tag> Tags { get; set; } = null!;
}
