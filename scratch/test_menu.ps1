$opts = @(
    ':-chatĐổi 200 Vàng - 600k bạc',
    ':-chatĐổi 2000 Vàng - 6.5tr bạc',
    ':-chat Đổi 200 Vàng - 5tr bạc khóa',
    ':-chatĐổi 2000 Vàng - 55tr bạc khóa'
)
$isMenu1 = ($opts.Count -ge 5) -or ($opts.Count -gt 1 -and $opts[1] -match 'tinh thạch') -or ($opts[0] -match '^:-chatĐổi Bạc' -or $opts[0] -match '^Đổi Bạc')
$isMenu2 = (!$isMenu1) -and ($opts.Count -eq 4 -or ($opts.Count -ge 2 -and $opts[1] -match '6\.5tr'))

Write-Output "isMenu1=$isMenu1"
Write-Output "isMenu2=$isMenu2"

