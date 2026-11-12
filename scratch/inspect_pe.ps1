$path = 'TGLL_minCPU_v2\GameAssembly.dll'
$stream = [System.IO.File]::OpenRead($path)
$rva = 0x4fb6d0
# Đọc file PE header để tính File Offset từ RVA
$stream.Seek(0x3c, [System.IO.SeekOrigin]::Begin) | Out-Null
$reader = New-Object System.IO.BinaryReader($stream)
$peOffset = $reader.ReadInt32()
$stream.Seek($peOffset + 4 + 20 + 24 + 68, [System.IO.SeekOrigin]::Begin) | Out-Null # NumberOfRvaAndSizes
$numSectionsOffset = $peOffset + 4 + 2
$stream.Seek($numSectionsOffset, [System.IO.SeekOrigin]::Begin) | Out-Null
$numSections = $reader.ReadInt16()
$optHeaderSize = $reader.ReadInt16()
$sectionHeaderStart = $peOffset + 24 + $optHeaderSize

# Tìm section chứa RVA
$fileOffset = -1
for ($i = 0; $i -lt $numSections; $i++) {
    $stream.Seek($sectionHeaderStart + ($i * 40), [System.IO.SeekOrigin]::Begin) | Out-Null
    $secNameBytes = $reader.ReadBytes(8)
    $secVirtualSize = $reader.ReadInt32()
    $secVirtualAddress = $reader.ReadInt32()
    $secRawSize = $reader.ReadInt32()
    $secRawOffset = $reader.ReadInt32()
    if ($rva -ge $secVirtualAddress -and $rva -lt ($secVirtualAddress + $secVirtualSize)) {
        $fileOffset = $secRawOffset + ($rva - $secVirtualAddress)
        Write-Output "Section: $([System.Text.Encoding]::ASCII.GetString($secNameBytes)) VA: 0x$($secVirtualAddress.ToString('X')) FileOffset: 0x$($fileOffset.ToString('X'))"
        break
    }
}

if ($fileOffset -gt 0) {
    # Đọc 200 bytes trước và sau 0x4fb6d0
    $start = [Math]::Max(0, $fileOffset - 0x200)
    $stream.Seek($start, [System.IO.SeekOrigin]::Begin) | Out-Null
    $bytes = $reader.ReadBytes(0x400)
    $hex = [BitConverter]::ToString($bytes) -replace '-', ' '
    Write-Output "Bytes around 0x4fb6d0 (FileOffset 0x$($fileOffset.ToString('X'))):"
    Write-Output $hex
}
$stream.Close()

