namespace Gardula.Application.Common.DTOs;

public record PagedResponse<T>(
    List<T> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages,
    bool HasNextPage);