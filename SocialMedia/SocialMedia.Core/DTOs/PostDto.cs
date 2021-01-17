using System;

namespace SocialMedia.Core.DTOs
{
  /// <summary>
  /// Data transfer object use by the API
  /// </summary>
  public class PostDto
  {
    /// <summary>
    /// Auto-generated id for Post entity
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Valid user id
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// A valid date
    /// </summary>
    public DateTime Date { get; set; }
    
    /// <summary>
    /// A description for the post
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// A valid path for the image
    /// </summary>
    public string Image { get; set; }
  }
}