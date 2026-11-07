$sqlFile = (Get-ChildItem -Path 'C:\Users\chien\Downloads' -Recurse -Filter '*.sql')[0].FullName
$lines = Get-Content -LiteralPath $sqlFile
foreach ($line in $lines) {
    if ($line -match '^\(3,\s*''') {
        Write-Host $line
        break
    }
}

