using System;

namespace SocialMedia.Core.QueryFilters
{
  /// <summary>
  /// Entity to save all parameters coming from the client
  /// </summary>
  public class PostQueryFilter
  {
    /// <summary>
    /// A valid User Id to apply to the search filter
    /// </summary>
    public int? UserId { get; set; }
    
    /// <summary>
    /// A valid Date to apply to the search filter
    /// </summary>
    public DateTime? Date { get; set; }
    
    /// <summary>
    /// A valid description to apply to the search filter, search will be done based on contains logic
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Number of records per page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Number of the page to be loaded
    /// </summary>
    public int PageNumber { get; set; }
  }
}