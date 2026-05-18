$mdPath = Join-Path $PSScriptRoot "..\docs\Information-Security-Documentation.md"
$htmlPath = Join-Path $PSScriptRoot "..\docs\Information-Security-Documentation.html"
$md = Get-Content -Path $mdPath -Raw -Encoding UTF8
$escaped = $md -replace '\\', '\\' -replace '`', '\`' -replace '</script>', '<\/script>'
$html = Get-Content -Path $htmlPath -Raw -Encoding UTF8
$pattern = '(?s)(<script id="md-source" type="text/plain">).*?(</script>)'
if ($html -notmatch $pattern) { throw "md-source block not found" }
$newHtml = [regex]::Replace($html, $pattern, { param($m) $m.Groups[1].Value + $escaped + $m.Groups[2].Value }, 1)
Set-Content -Path $htmlPath -Value $newHtml -Encoding UTF8 -NoNewline
Write-Host "Synced HTML from MD ($($md.Length) chars)"
