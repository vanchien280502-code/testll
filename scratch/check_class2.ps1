$classFile = (Get-ChildItem -Path 'C:\Users\chien\Downloads' -Recurse -Filter 'TaskHandler.class' -ErrorAction SilentlyContinue)[0].FullName
Write-Host "Found class: $classFile"
$bytes = [System.IO.File]::ReadAllBytes($classFile)
$str = [System.Text.Encoding]::UTF8.GetString($bytes)
$regex = [regex]'[^\x00-\x1F\x7F-\x9F]{3,40}'
$all = $regex.Matches($str)
foreach ($m in $all) {
    if ($m.Value -match 'chuy|Anko|biệt|Tìm|quen|mặt|Rokusho') {
        Write-Host $m.Value
    }
}

