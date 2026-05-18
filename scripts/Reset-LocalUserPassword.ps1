# Resets a FurniComply user's password on the local Development database (no email).
# Usage: .\scripts\Reset-LocalUserPassword.ps1 -Email "you@example.com" -NewPassword "YourNewPass12!"

param(
    [Parameter(Mandatory = $true)]
    [string] $Email,

    [Parameter(Mandatory = $true)]
    [string] $NewPassword
)

$ErrorActionPreference = "Stop"
$repoRoot = Split-Path -Parent $PSScriptRoot
$webProject = Join-Path $repoRoot "src\FurniComply.Web"

$env:ASPNETCORE_ENVIRONMENT = "Development"
Push-Location $webProject
try {
    dotnet run --no-build -- reset-password $Email $NewPassword
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}
finally {
    Pop-Location
}
