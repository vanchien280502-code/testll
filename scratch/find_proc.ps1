Get-Process | Select-Object Id, ProcessName | Where-Object { $_.ProcessName -notmatch 'powershell|conhost|svchost|csrss|lsass|services|wininit|smss' }
