namespace Gardula.Application.Finance.Reports.DTOs;

public record ReportCategorySpendingResponse(
    int CategoryId,
    string CategoryName,
    decimal Amount,
    decimal Percentage);