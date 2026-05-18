# Runs local security/code audits for FurniComply (Criterion 8 evidence).
# Usage: powershell -File scripts/Run-SecurityAudit.ps1

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$outDir = Join-Path $root "security-audit"
$reportPath = Join-Path $outDir "latest-report.txt"
$sln = Join-Path $root "FurniComply.sln"
$webProj = Join-Path $root "src\FurniComply.Web\FurniComply.Web.csproj"

New-Item -ItemType Directory -Force -Path $outDir | Out-Null

$lines = New-Object System.Collections.Generic.List[string]
$lines.Add("FurniComply Security Audit Report")
$lines.Add("Generated (UTC): $(Get-Date -Format o)")
$lines.Add("Machine: $env:COMPUTERNAME")
$lines.Add("")

$lines.Add("=== 1. Tool inventory ===")
$lines.Add("- SecurityCodeScan.VS2019 (Roslyn security analyzer) on all projects via Directory.Build.props")
$lines.Add("- .NET nullable reference types and compiler warnings (CS86xx)")
$lines.Add("- dotnet list package --vulnerable (NuGet dependency audit)")
$lines.Add("- GitHub Actions: .github/workflows/security.yml")
$lines.Add("- IDE: SonarLint (Roslyn-compatible) in Visual Studio / Cursor")
$lines.Add("- N/A: ESLint (no Node frontend), Bandit (no Python app; optional smtp_fallback.py only)")
$lines.Add("")

$lines.Add("=== 2. Vulnerable packages (transitive) ===")
Push-Location $root
try {
    $vuln = dotnet list $webProj package --vulnerable --include-transitive 2>&1 | Out-String
    $lines.Add($vuln.TrimEnd())
}
finally {
    Pop-Location
}
$lines.Add("")

$lines.Add("=== 3. Release build (SecurityCodeScan + compiler analysis) ===")
Push-Location $root
try {
    $build = dotnet build $sln -c Release --no-incremental 2>&1 | Out-String
    $lines.Add($build.TrimEnd())
}
finally {
    Pop-Location
}
$lines.Add("")

$lines.Add("=== 4. Documented resolutions (application code) ===")
$lines.Add("- Removed anonymous PasswordResetController")
$lines.Add("- Removed ProductionBackup and duplicate DebugController")
$lines.Add("- SafeErrorMessages and generic /Home/Error page")
$lines.Add("- Blocked direct /uploads static access")
$lines.Add("- RBAC policies and department-scoped compliance data")
$lines.Add("")

$lines.Add("=== 5. Open / accepted findings ===")
$lines.Add("- Transitive advisories on SqlClient/Azure.Identity from optional SQL Server packages; runtime uses SQLite.")
$lines.Add("- CS86xx nullable warnings in Infrastructure encryption/EF converters (tracked, non-security).")
$lines.Add("")

$text = ($lines -join [Environment]::NewLine) + [Environment]::NewLine
[System.IO.File]::WriteAllText($reportPath, $text, [System.Text.UTF8Encoding]::new($false))
Write-Host "Wrote $reportPath"
