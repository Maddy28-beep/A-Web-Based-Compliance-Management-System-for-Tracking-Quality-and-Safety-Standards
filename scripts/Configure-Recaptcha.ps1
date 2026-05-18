param(
    [Parameter(Mandatory = $true)]
    [string] $SiteKey,

    [Parameter(Mandatory = $true)]
    [string] $SecretKey
)

$ErrorActionPreference = "Stop"
$repoRoot = Split-Path -Parent $PSScriptRoot
$envFile = Join-Path $repoRoot "src\FurniComply.Web\.env"
$webProject = Join-Path $repoRoot "src\FurniComply.Web"

$site = $SiteKey.Trim()
$secret = $SecretKey.Trim()

function Set-EnvLine {
    param([string]$Path, [string]$Key, [string]$Value)
    $line = "$Key=$Value"
    $pattern = "(?m)^$([regex]::Escape($Key))=.*$"
    if (Test-Path $Path) {
        $content = Get-Content $Path -Raw
        if ($content -match $pattern) {
            $content = [regex]::Replace($content, $pattern, $line)
        } else {
            $content = $content.TrimEnd() + "`r`n$line`r`n"
        }
        Set-Content -Path $Path -Value $content -NoNewline
    } else {
        Set-Content -Path $Path -Value "$line`r`n"
    }
}

foreach ($pair in @{
    "RECAPTCHA_SITE_KEY" = $site
    "RECAPTCHA_SECRET_KEY" = $secret
    "RECAPTCHA_DISABLE" = "false"
    "Recaptcha__SiteKey" = $site
    "Recaptcha__SecretKey" = $secret
    "Recaptcha__Disable" = "false"
}.GetEnumerator()) {
    Set-EnvLine -Path $envFile -Key $pair.Key -Value $pair.Value
}

Push-Location $webProject
try {
    dotnet user-secrets set "Recaptcha:SiteKey" $site | Out-Null
    dotnet user-secrets set "Recaptcha:SecretKey" $secret | Out-Null
    dotnet user-secrets set "Recaptcha:Disable" "false" | Out-Null
}
finally {
    Pop-Location
}

Write-Host "Saved: $envFile"
