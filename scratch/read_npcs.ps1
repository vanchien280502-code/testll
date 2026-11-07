$f = 'C:\Users\chien\Downloads\Làng Lá - Server\data\Npc\75.json'
$c = Get-Content $f -Raw | ConvertFrom-Json
for ($i = 0; $i -lt $c.Count; $i++) {
    Write-Host ("Index: " + $i + ", ID: " + $c[$i].id + ", Name: " + $c[$i].name + ", cx: " + $c[$i].cx + ", cy: " + $c[$i].cy)
}

