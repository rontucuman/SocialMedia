using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialMedia.Core.CustomEntities
{
  public class PagedList<T> : List<T>
  {
    private PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
      CurrentPage = pageNumber;
      TotalCount = count;
      PageSize = pageSize;
      TotalPages = (int) Math.Ceiling(count / (decimal) pageSize);

      AddRange(items);
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : (int?) null;
    public int? PreviousPageNumber => HasPreviousPage ? CurrentPage - 1 : (int?) null;

    public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
    {
      int count = source.Count();
      List<T> items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

      return new PagedList<T>(items, count, pageNumber, pageSize);
    }
  }
}