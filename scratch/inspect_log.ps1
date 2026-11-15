$lines = Get-Content 'QLTK\logs\061d4a20-7bc4-4287-a05b-6c92e2f9ec50.jsonl'
for ($i = 0; $i -lt $lines.Count; $i++) {
    if ($lines[$i] -match '"event":"task_packet_sent"') {
        $start = [Math]::Max(0, $i - 2)
        $end = [Math]::Min($lines.Count - 1, $i + 4)
        for ($k = $start; $k -le $end; $k++) {
            Write-Output "[$k] $($lines[$k])"
        }
        break
    }
}

