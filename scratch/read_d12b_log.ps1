$file = 'QLTK\logs\d12b0b70-437c-4524-a54c-41e7072f51f7.jsonl'
$lines = Get-Content $file
Write-Output "Total lines in d12b: $($lines.Count)"
$lines | ForEach-Object {
    try {
        $j = $_ | ConvertFrom-Json
        if ($j.event -ne 'task_packet_sent' -and ($j.event -ne 'user_packet_sent' -or $j.opcode -ne 0)) {
            Write-Output "$($j.time) | $($j.event) | opcode=$($j.opcode) len=$($j.len) payload=$($j.payload) count=$($j.count) $($j.status)"
        }
    } catch {}
}

