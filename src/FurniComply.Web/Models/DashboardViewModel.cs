using System.Collections.Generic;
using FurniComply.Application.Models;

namespace FurniComply.Web.Models;

public record WeatherWidget(
    string City,
    decimal TemperatureC,
    string Description,
    int Humidity,
    decimal WindSpeed,
    string? IconUrl,
    decimal? UvIndex,
    string? UvAlert
);

public record ExchangeRateWidget(string BaseCurrency, IReadOnlyList<ExchangeRateItem> Rates);

public record ExchangeRateItem(string Currency, decimal Rate);

public record HolidayWidget(string CountryCode, IReadOnlyList<HolidayItem> Holidays);

public record HolidayItem(string Name, string Date);

public record DashboardViewModel(
    AnalyticsSnapshot Analytics,
    WeatherWidget? Weather,
    ExchangeRateWidget? ExchangeRates,
    HolidayWidget? Holidays
);

public record RoleDashboardMetric(
    string Title,
    string Value,
    string Subtitle,
    string? Url = null
);

public record RoleDashboardItem(
    string Title,
    string Detail,
    string Severity = "neutral",
    string? Url = null
);

public record RoleDashboardViewModel(
    string RoleName,
    string Focus,
    IReadOnlyList<RoleDashboardMetric> Metrics,
    IReadOnlyList<RoleDashboardItem> ActionItems,
    IReadOnlyList<RoleDashboardItem> Alerts
);
