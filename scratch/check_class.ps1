$bytes = [System.IO.File]::ReadAllBytes('C:\Users\chien\Downloads\Làng Lá - Server\out\production\pkoolvn\com\langla\real\task\TaskHandler.class')
$str = [System.Text.Encoding]::UTF8.GetString($bytes)
$regex = [regex]'[^\x00-\x1F\x7F-\x9F]{3,40}'
$all = $regex.Matches($str)
foreach ($m in $all) {
    if ($m.Value -match 'chuy|Anko|biệt|Tìm|quen|mặt|Rokusho') {
        Write-Host $m.Value
    }
}

