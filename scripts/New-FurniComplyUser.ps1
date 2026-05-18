# Creates a FurniComply login user in Development (local SQLite).
# Usage:
#   .\scripts\New-FurniComplyUser.ps1 -Email "you@gmail.com" -Password "YourPass12!@" -Role SuperAdmin -FullName "Your Name"

param(
    [Parameter(Mandatory = $true)]
    [string] $Email,

    [Parameter(Mandatory = $true)]
    [string] $Password,

    [Parameter(Mandatory = $true)]
    [ValidateSet("SuperAdmin", "Admin", "ComplianceManager", "DepartmentHead", "Auditor", "Procurement")]
    [string] $Role,

    [string] $FullName = ""
)

$ErrorActionPreference = "Stop"
$repoRoot = Split-Path -Parent $PSScriptRoot
$webProject = Join-Path $repoRoot "src\FurniComply.Web"
$nameArg = if ([string]::IsNullOrWhiteSpace($FullName)) { $Email } else { $FullName }

$env:ASPNETCORE_ENVIRONMENT = "Development"
Push-Location $webProject
try {
    dotnet run -- create-user $Email $Password $Role $nameArg
    exit $LASTEXITCODE
}
finally {
    Pop-Location
}
