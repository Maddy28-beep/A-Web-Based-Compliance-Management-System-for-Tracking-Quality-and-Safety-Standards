# Configures FurniComply to send mail through a Gmail account (App Password required).
# Usage:
#   .\scripts\Configure-GmailSmtp.ps1 -GmailAddress "furnicomply.mail@gmail.com" -AppPassword "abcdefghijklmnop"

param(
    [Parameter(Mandatory = $true)]
    [string] $GmailAddress,

    [Parameter(Mandatory = $true)]
    [string] $AppPassword
)

$ErrorActionPreference = "Stop"
$repoRoot = Split-Path -Parent $PSScriptRoot
$envFile = Join-Path $repoRoot "src\FurniComply.Web\.env"
$webProject = Join-Path $repoRoot "src\FurniComply.Web"

$normalizedPassword = ($AppPassword -replace '\s', '')
if ($normalizedPassword.Length -ne 16) {
    Write-Warning "Gmail app passwords are usually 16 characters (spaces are removed automatically). Length is $($normalizedPassword.Length)."
}

function Set-EnvLine {
    param([string]$Path, [string]$Key, [string]$Value)
    $escaped = $Value -replace '\\', '\\'
    $pattern = "(?m)^$([regex]::Escape($Key))=.*$"
    $line = "$Key=$escaped"
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

$mailKeys = @{
    "MAIL_SMTP_HOST"           = "smtp.gmail.com"
    "MAIL_SMTP_PORT"           = "587"
    "MAIL_FROM"                = $GmailAddress
    "MAIL_USERNAME"            = $GmailAddress
    "MAIL_PASSWORD"            = $normalizedPassword
    "MAIL_USE_SSL"             = "true"
    "Mail__SmtpHost"           = "smtp.gmail.com"
    "Mail__SmtpPort"           = "587"
    "Mail__From"               = $GmailAddress
    "Mail__UserName"           = $GmailAddress
    "Mail__Password"           = $normalizedPassword
    "Mail__UseSsl"             = "true"
}

foreach ($pair in $mailKeys.GetEnumerator()) {
    Set-EnvLine -Path $envFile -Key $pair.Key -Value $pair.Value
}

Push-Location $webProject
try {
    dotnet user-secrets set "Mail:SmtpHost" "smtp.gmail.com" | Out-Null
    dotnet user-secrets set "Mail:SmtpPort" "587" | Out-Null
    dotnet user-secrets set "Mail:From" $GmailAddress | Out-Null
    dotnet user-secrets set "Mail:UserName" $GmailAddress | Out-Null
    dotnet user-secrets set "Mail:Password" $normalizedPassword | Out-Null
    dotnet user-secrets set "Mail:UseSsl" "true" | Out-Null
}
finally {
    Pop-Location
}

Write-Host "Gmail SMTP configured for $GmailAddress in .env and user secrets."
Write-Host "Restart FurniComply, sign in as admin, open Settings -> Mail diagnostics -> Send test."
Write-Host "Login email can stay different from Mail:From (e.g. keep madelyncordova12@gmail.com for sign-in)."
