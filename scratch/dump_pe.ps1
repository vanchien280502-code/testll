$path = 'TGLL_minCPU_v2\GameAssembly.dll'
$bytes = [System.IO.File]::ReadAllBytes($path)

function Get-Int32($offset) {
    return [BitConverter]::ToInt32($bytes, $offset)
}
function Get-Int16($offset) {
    return [BitConverter]::ToInt16($bytes, $offset)
}

$pe = Get-Int32 0x3c
$numSections = Get-Int16 ($pe + 6)
$optHeaderSize = Get-Int16 ($pe + 20)
$secStart = $pe + 24 + $optHeaderSize

Write-Output "PE: 0x$($pe.ToString('X')), Sections: $numSections, OptHeaderSize: $optHeaderSize, SecStart: 0x$($secStart.ToString('X'))"

$rva = 0x4fb6d0
$fileOffset = -1

for ($i = 0; $i -lt $numSections; $i++) {
    $entry = $secStart + ($i * 40)
    $name = [System.Text.Encoding]::ASCII.GetString($bytes, $entry, 8).TrimEnd("`0")
    $vSize = Get-Int32 ($entry + 8)
    $vAddr = Get-Int32 ($entry + 12)
    $rSize = Get-Int32 ($entry + 16)
    $rAddr = Get-Int32 ($entry + 20)
    Write-Output "Section $name : VA=0x$($vAddr.ToString('X')), VSize=0x$($vSize.ToString('X')), RawOffset=0x$($rAddr.ToString('X'))"
    if ($rva -ge $vAddr -and $rva -lt ($vAddr + $vSize)) {
        $fileOffset = $rAddr + ($rva - $vAddr)
        Write-Output ">>> Found RVA 0x$($rva.ToString('X')) in section $name at FileOffset 0x$($fileOffset.ToString('X'))"
    }
}

if ($fileOffset -gt 0) {
    # Dump 256 bytes from 0x4fb6d0
    $sub = New-Object byte[] 256
    [Array]::Copy($bytes, $fileOffset, $sub, 0, 256)
    Write-Output "Disassembly hint around 0x4fb6d0:"
    Write-Output ([BitConverter]::ToString($sub) -replace '-', ' ')
}

