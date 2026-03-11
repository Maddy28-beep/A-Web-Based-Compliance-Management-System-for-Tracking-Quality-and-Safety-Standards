using System;
using System.Collections.Generic;

namespace FurniComply.Application.Models;

public sealed record SupplierComplianceScoreResult(
    Guid SupplierId,
    string SupplierName,
    int Score,
    string Band,
    IReadOnlyList<string> TopReasons,
    int MissingRequiredDocuments,
    int ExpiredRequiredDocuments,
    int PendingRequiredDocuments,
    int HighActiveAlerts,
    int CriticalActiveAlerts,
    int OverdueCapaCount,
    int OpenCapaCount
);
