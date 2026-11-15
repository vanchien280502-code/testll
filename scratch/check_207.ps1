$file = 'QLTK\logs\c01dfd0c-1bdf-4e97-a4f8-38a6a29373d5.jsonl'
$lines = Get-Content $file
$targetPackets = @()
for ($i = 0; $i -lt $lines.Count; $i++) {
    if ($lines[$i] -match '"opcode":207') {
        $start = [Math]::Max(0, $i - 3)
        $end = [Math]::Min($lines.Count - 1, $i + 3)
        for ($k = $start; $k -le $end; $k++) {
            try {
                $j = $lines[$k] | ConvertFrom-Json
                if ($j.event -eq 'user_packet_sent' -and $j.opcode -ne 0) {
                    $targetPackets += "$($j.time) [line $k] opcode=$($j.opcode) len=$($j.len) payload=$($j.payload)"
                }
            } catch {}
        }
        if ($targetPackets.Count -ge 25) { break }
    }
}
$targetPackets | Select-Object -Unique | ForEach-Object { Write-Output $_ }
