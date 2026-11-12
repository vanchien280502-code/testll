$path = 'TGLL_minCPU_v2\GameAssembly.dll'
$bytes = [System.IO.File]::ReadAllBytes($path)
$fileOffset = 0x4F9CD0 # 0x4FB6D0 in file

# Dump 1024 bytes starting from 0x4FB6D0
$sub = New-Object byte[] 1024
[Array]::Copy($bytes, $fileOffset, $sub, 0, 1024)

# Tìm các lệnh "mov dl, XX" (B2 XX) hoặc "mov edx, XX" (BA XX 00 00 00)
for ($i = 0; $i -lt 1000; $i++) {
    if ($sub[$i] -eq 0xB2) {
        $op = $sub[$i+1]
        $rva = 0x4FB6D0 + $i
        Write-Output "Found 'mov dl, 0x$($op.ToString('X2'))' at RVA 0x$($rva.ToString('X'))"
    }
    if ($sub[$i] -eq 0xBA -and $sub[$i+3] -eq 0 -and $sub[$i+4] -eq 0) {
        $op = $sub[$i+1]
        $rva = 0x4FB6D0 + $i
        Write-Output "Found 'mov edx, 0x$($op.ToString('X2'))' at RVA 0x$($rva.ToString('X'))"
    }
}

