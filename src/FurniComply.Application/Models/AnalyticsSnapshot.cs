using System.Collections.Generic;

namespace FurniComply.Application.Models;

public record MonthlyCount(string Label, int Count);
public record ComplianceAlertItem(System.Guid Id, string Title, string Message, string Severity, bool IsAcknowledged, System.DateTime TriggeredAtUtc);

public record AnalyticsSnapshot(
    int ActivePolicies,
    int ComplianceChecksThisMonth,
    int OpenNonCompliance,
    int SuppliersOnHold,
    int PendingSupplierApprovals,
    int PendingPolicyApprovals,
    int PendingReportApprovals,
    int PendingProcurementApprovals,
    int OpenReports,
    int ProcurementOrdersOpen,
    int CompliantChecks,
    int NonCompliantChecks,
    IReadOnlyList<MonthlyCount> ComplianceChecksByMonth,
    IReadOnlyList<ComplianceAlertItem> Alerts,
    IReadOnlyList<string> Insights
);


