$file = 'QLTK\logs\fbd574b1-6e16-4dcd-804b-06f190e5440b.jsonl'
$lines = Get-Content $file
Write-Output "Total lines in fbd574b1: $($lines.Count)"
$lines | ForEach-Object {
    try {
        $j = $_ | ConvertFrom-Json
        if ($j.event -ne 'task_packet_sent' -and ($j.event -ne 'user_packet_sent' -or $j.opcode -ne 0)) {
            Write-Output "$($j.time) | $($j.event) | opcode=$($j.opcode) len=$($j.len) payload=$($j.payload) count=$($j.count) $($j.status)"
        }
    } catch {}
}

