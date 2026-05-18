# FurniComply

Web-based **compliance and procurement management** system for furniture-industry operations. Built for **IT 16/L – Information Security 1** (capstone).

**Local URL:** [http://localhost:5280](http://localhost:5280)

---

## What it does

- Policy governance, compliance checks, and CAPA workflow  
- Supplier onboarding (documents, approval, compliance scoring)  
- Procurement orders with approval gates  
- Regulatory reports (export PDF/Excel)  
- Role-based dashboards, security logs, and audit trails  
- Admin: users, roles, backups, access requests  

Full security design: **[docs/Information-Security-Documentation.md](docs/Information-Security-Documentation.md)** (print-friendly: [docs/Information-Security-Documentation.html](docs/Information-Security-Documentation.html)).

---

## Tech stack

| Layer | Technology |
| ----- | ---------- |
| Backend | C# / .NET 8, ASP.NET Core MVC |
| Data | Entity Framework Core, **SQLite** (local) |
| Auth | ASP.NET Core Identity, **PBKDF2** (Identity V3), optional TOTP 2FA |
| Secrets | **`.env`** (gitignored), mapped in `EnvironmentVariablesConfiguration.cs` |

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)  
- Git  
- (Optional) [DB Browser for SQLite](https://sqlitebrowser.org/) — to inspect `App_Data/FurniComply.local.db`  
- (Optional) Visual Studio 2022 or VS Code / Cursor  

---

## Quick start

### 1. Clone and open

```powershell
cd d:\Furniture
```

Open **`FurniComply.sln`** in Visual Studio. Use the solution **Configuration** folder for quick access to key files (see below).

### 2. Environment file (required)

```powershell
copy .env.example src\FurniComply.Web\.env
```

Edit **`src/FurniComply.Web/.env`**:

| Variable | Required? | Notes |
| -------- | --------- | ----- |
| `ConnectionStrings__DefaultConnection` | Yes | Default: `Data Source=App_Data/FurniComply.local.db` |
| `APP_ENCRYPTION_KEY` | Yes | Long random string (field encryption). Generate once and keep stable for your DB. |
| `RECAPTCHA_SITE_KEY` / `RECAPTCHA_SECRET_KEY` | Optional | Google reCAPTCHA v2, or set `RECAPTCHA_DISABLE=true` for local testing |
| `MAIL_*` | Optional | Only if demoing password-reset email |
| `OPENWEATHER_API_KEY` | **No** | Paid plans on OpenWeather; leave **empty** — weather shows `--`, exchange rate still works |
| `APP_IDLE_LOGOUT_SECONDS` | Optional | e.g. `10` for short idle logout demo |

Confirm secrets are not tracked:

```powershell
git check-ignore -v src/FurniComply.Web/.env
```

### 3. Run the app

```powershell
cd src\FurniComply.Web
dotnet run
```

Or in Visual Studio: profile **Capstone** or **http** → F5.

Browse to **http://localhost:5280** → **Login**.

### 4. First-time users (if database is empty)

- Use **Request Access** on the home page, then an admin approves and **creates a user** under **Admin → Users**, or  
- Run **`scripts/New-FurniComplyUser.ps1`** (see [Scripts](#scripts)), or  
- Set `SEED_IDENTITY_ON_STARTUP=true` and seed passwords in configuration (advanced; see `IdentitySeeder.cs`).

---

## Configuration folder (Solution Explorer)

The solution **Configuration** folder links important files (shortcuts only):

| File | Purpose |
| ---- | ------- |
| [`.gitignore`](.gitignore) | Excludes `.env`, `Keys/`, `App_Data/*.db`, `appsettings.json`, etc. |
| [`.env.example`](.env.example) | Committed template — **no real secrets** |
| `src/FurniComply.Web/.env` | Your local secrets — **do not commit** |
| `src/FurniComply.Web/App_Data/FurniComply.local.db` | Local SQLite database — **do not commit** |

Opening the `.db` in Visual Studio shows **hex/binary** view — that is normal. Use **DB Browser for SQLite** to browse tables (e.g. `AspNetUsers` → `PasswordHash` for hashing proof).

---

## Project structure

```
FurniComply/
├── .env.example          # Secrets template (committed)
├── .gitignore
├── docs/                 # Information security documentation
├── scripts/              # Security audit, user setup, reCAPTCHA helper
├── src/
│   ├── FurniComply.Domain/
│   ├── FurniComply.Application/
│   ├── FurniComply.Infrastructure/   # EF Core, encryption, seeders
│   └── FurniComply.Web/              # MVC app, .env, App_Data/
└── FurniComply.sln
```

---

## Security highlights (for defense)

| Topic | Where to show |
| ----- | ------------- |
| Password hashing (PBKDF2) | `Program.cs` → `PasswordHasherCompatibilityMode.IdentityV3`, `IterationCount = 210_000` |
| Login | `/Account/Login` → `PasswordSignInAsync` in `AccountController.cs` |
| Hashed passwords in DB | DB Browser → `AspNetUsers.PasswordHash` (not plain text) |
| RBAC | `Program.cs` policies; try pages with a low-privilege role |
| Field encryption | `EncryptedStringValueConverter.cs`; supplier email/phone encrypted at rest |
| Duplicate email/phone | `SupplierContactUniquenessService.cs`; test on **Suppliers → Create** |
| Secrets not in Git | `.gitignore` + `git check-ignore` on `.env` |
| Security audit | `powershell -File scripts/Run-SecurityAudit.ps1` |
| CI | `.github/workflows/security.yml` |

---

## Scripts

| Script | Purpose |
| ------ | ------- |
| `scripts/Run-SecurityAudit.ps1` | Local security build + vulnerable package report |
| `scripts/New-FurniComplyUser.ps1` | Create a login user from the command line |
| `scripts/Configure-Recaptcha.ps1` | Write reCAPTCHA keys to Web `.env` |
| `scripts/Configure-GmailSmtp.ps1` | SMTP settings helper |
| `scripts/Sync-SecurityDocHtml.ps1` | Regenerate HTML doc from `docs/*.md` |

---

## Optional configuration files

Copy **`src/FurniComply.Web/appsettings.Example.json`** to **`appsettings.json`** if you need non-secret defaults (also gitignored).

---

## Documentation

| Document | Use |
| -------- | --- |
| [Information-Security-Documentation.md](docs/Information-Security-Documentation.md) | Full IT Security 1 paper: policies, RBAC, audit, incident response |
| [Information-Security-Documentation.html](docs/Information-Security-Documentation.html) | Same content — open in Chrome → Print / Save as PDF |

---

## License / academic use

Capstone project for educational purposes. Replace placeholder names in documentation before submission.
