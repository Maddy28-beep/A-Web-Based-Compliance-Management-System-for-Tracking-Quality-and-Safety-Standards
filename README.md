# FurniComply — A Web-Based Compliance Management System

A web-based ERP system for tracking quality and safety standards in the furniture industry. Built with ASP.NET Core 8, Entity Framework Core, and a modern responsive UI.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)

---

## About

**FurniComply** helps furniture manufacturers and suppliers manage compliance documentation, supplier approvals, procurement orders, corrective actions (CAPA), and regulatory reporting. The system supports role-based dashboards (Super Admin, Admin, Compliance Officer, Department Head, Procurement Officer) with date-filtered KPI analytics.

### Key Features

- **Supplier Management** — Onboarding, approval workflow, document compliance, performance scoring
- **Procurement Orders** — Order lifecycle, status tracking, supplier linkage
- **CAPA (Corrective Actions)** — Issue tracking, deadlines, evidence submission
- **Compliance Checks** — Policy audits, risk levels, findings
- **Regulatory Reports** — Report submission, status workflow
- **Role Dashboards** — ERP-style KPIs, charts, date filters, operational monitoring
- **Compliance Alerts** — Workflow rules, severity levels, alert management

---

## Tech Stack

- **Backend:** ASP.NET Core 8 (MVC)
- **ORM:** Entity Framework Core 8
- **Database:** SQL Server / SQL Server LocalDB
- **Frontend:** Razor views, Chart.js, vanilla CSS
- **PDF:** QuestPDF
- **Excel:** ClosedXML

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or SQL Server LocalDB

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/Maddy28-beep/A-Web-Based-Compliance-Management-System-for-Tracking-Quality-and-Safety-Standards.git
   cd A-Web-Based-Compliance-Management-System-for-Tracking-Quality-and-Safety-Standards
   ```

2. **Configure settings**
   - Copy `src/FurniComply.Web/appsettings.Example.json` to `src/FurniComply.Web/appsettings.json`
   - Update connection strings, Mail, and API keys (see example file)

3. **Apply migrations**
   ```bash
   cd src/FurniComply.Web
   dotnet ef database update --project ../FurniComply.Infrastructure
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```
   Then open `https://localhost:5001` (or the URL shown in the console).

---

## Project Structure

```
Furniture/
├── src/
│   ├── FurniComply.Web/          # MVC web app
│   ├── FurniComply.Application/   # Business logic, interfaces
│   ├── FurniComply.Infrastructure/# EF Core, services, persistence
│   └── FurniComply.Domain/        # Entities, enums
└── README.md
```

---

## Security

- **Never commit** `appsettings.json` or any file containing real credentials
- Use `appsettings.Example.json` as a template only
- For production, prefer User Secrets, environment variables, or Azure Key Vault

---

## Contact

Got questions or want to collaborate? Reach out:

- **Facebook:** [Maddy Cordova](https://www.facebook.com/maddy.cordova.2025)

---

## License

This project is for educational and portfolio purposes.
