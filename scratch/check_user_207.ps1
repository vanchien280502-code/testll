$lines = Get-Content 'QLTK\logs\8ff917ec-a696-41e3-b9a3-8d5098265002.jsonl'
$found207 = 0
foreach ($l in $lines) {
    if ($l -match '"event":"user_packet_sent"') {
        $j = $l | ConvertFrom-Json
        if ($j.opcode -eq 207) {
            $found207++
            Write-Output "FOUND 207: $l"
        }
    }
}
Write-Output "Total user_packet_sent with opcode 207: $found207"

