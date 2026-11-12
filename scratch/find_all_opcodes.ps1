$path = 'TGLL_minCPU_v2\GameAssembly.dll'
$bytes = [System.IO.File]::ReadAllBytes($path)

# Hàm tính FileOffset từ RVA (il2cpp section: VA=0x237000, Raw=0x235600)
# RVA = FileOffset + 0x1A00 (cho section il2cpp)
$targetRva = 0x2e2aa0

$results = New-Object System.Collections.Generic.List[string]

# Quét section il2cpp (0x235600 đến 0x235600 + 0x10668BC)
$secStart = 0x235600
$secEnd = $secStart + 0x10668BC

for ($offset = $secStart; $offset -lt $secEnd - 5; $offset++) {
    if ($bytes[$offset] -eq 0xE8) {
        $rel = [BitConverter]::ToInt32($bytes, $offset + 1)
        $currRva = ($offset - 0x235600) + 0x237000
        $calledRva = $currRva + 5 + $rel
        if ($calledRva -eq $targetRva) {
            # Tìm lệnh gán edx/dl trước offset (lùi lại tối đa 30 bytes)
            $opFound = "unknown"
            for ($k = 1; $k -le 30; $k++) {
                $pos = $offset - $k
                if ($pos -lt 0) { break }
                # B2 XX : mov dl, XX
                if ($bytes[$pos] -eq 0xB2) {
                    $op = $bytes[$pos + 1]
                    $opFound = "0x$($op.ToString('X2')) ($op) via mov dl"
                    break
                }
                # BA XX 00 00 00 : mov edx, XX
                if ($bytes[$pos] -eq 0xBA -and $bytes[$pos+2] -eq 0 -and $bytes[$pos+3] -eq 0 -and $bytes[$pos+4] -eq 0) {
                    $op = $bytes[$pos + 1]
                    $opFound = "0x$($op.ToString('X2')) ($op) via mov edx"
                    break
                }
                # 33 D2 : xor edx, edx
                if ($bytes[$pos] -eq 0x33 -and $bytes[$pos + 1] -eq 0xD2) {
                    $opFound = "0x00 (0) via xor edx, edx"
                    break
                }
            }
            $results.Add("Call at RVA 0x$($currRva.ToString('X')) -> Opcode: $opFound")
        }
    }
}

$results | Out-File -FilePath 'scratch/all_opcodes.txt' -Encoding utf8
Write-Output "Found $($results.Count) calls to Message ctor (0x2e2aa0)"

