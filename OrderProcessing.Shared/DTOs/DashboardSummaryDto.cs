namespace OrderProcessing.Shared.DTOs;

public class DashboardSummaryDto
{
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public int CompletedOrders { get; set; }
    public int FailedOrders { get; set; }
    public decimal TotalRevenue { get; set; }
}
