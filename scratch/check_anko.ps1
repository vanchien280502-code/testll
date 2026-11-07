$path = (Get-ChildItem -Path 'C:\Users\chien\Downloads' -Recurse -Filter '75.json' | Where-Object { $_.FullName -like '*Npc*' })[0].FullName
$c = Get-Content -LiteralPath $path -Raw | ConvertFrom-Json
for ($i = 0; $i -lt $c.Count; $i++) {
    if ($c[$i].id -eq 99) {
        Write-Host ("Index: " + $i + ", ID: " + $c[$i].id + ", Name: " + $c[$i].name)
    }
}
