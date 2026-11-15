$lines = Get-Content 'QLTK\logs\c01dfd0c-1bdf-4e97-a4f8-38a6a29373d5.jsonl'
for ($i = 16420; $i -lt 16520; $i++) {
    try {
        $j = $lines[$i] | ConvertFrom-Json
        if ($j.event -ne 'task_packet_sent') {
            Write-Output "[$i] $($j.time) | $($j.event) | opcode=$($j.opcode)"
        }
    } catch {}
}
