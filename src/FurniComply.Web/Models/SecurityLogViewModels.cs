using System;
using System.Collections.Generic;
using FurniComply.Domain.Entities;

namespace FurniComply.Web.Models;

public class SecurityLogsIndexViewModel
{
    public IReadOnlyList<SecurityLog> Logs { get; init; } = Array.Empty<SecurityLog>();
    public IReadOnlyList<SecurityLogTopIpViewModel> TopIpAddresses { get; init; } = Array.Empty<SecurityLogTopIpViewModel>();
    public int TotalEvents { get; init; }
    public int SuccessfulLoginsToday { get; init; }
    public int FailedLoginsToday { get; init; }
    public int UniqueIpCount { get; init; }
    public bool IsScopedToCurrentUser { get; init; }
    public bool CanViewSensitiveFields { get; init; }
}

public record SecurityLogTopIpViewModel(string IpAddress, int Count);
