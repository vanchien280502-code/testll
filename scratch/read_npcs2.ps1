$f = (Get-ChildItem -Path 'C:\Users\chien\Downloads' -Recurse -Filter '75.json' | Where-Object { $_.FullName -like '*Npc*' })[0].FullName
Write-Host "File: $f"
$c = Get-Content -LiteralPath $f -Raw | ConvertFrom-Json
for ($i = 0; $i -lt $c.Count; $i++) {
    Write-Host ("Index: " + $i + ", ID: " + $c[$i].id + ", Name: " + $c[$i].name + ", cx: " + $c[$i].cx + ", cy: " + $c[$i].cy)
}

