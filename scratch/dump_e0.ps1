$path = 'TGLL_minCPU_v2\GameAssembly.dll'
$bytes = [System.IO.File]::ReadAllBytes($path)
# 0x4FB9D1 - 0x4FB6D0 + 0x4F9CD0
$offset = 0x4F9CD0 + (0x4FB9D1 - 0x4FB6D0)

$start = $offset - 64
$sub = New-Object byte[] 200
[Array]::Copy($bytes, $start, $sub, 0, 200)

Write-Output "Code around 0x4FB9D1:"
Write-Output ([BitConverter]::ToString($sub) -replace '-', ' ')

