$lines = Get-Content 'QLTK\logs\c01dfd0c-1bdf-4e97-a4f8-38a6a29373d5.jsonl'
$times = @()
foreach ($line in $lines) {
    if ($line -match '"opcode":207') {
        $j = $line | ConvertFrom-Json
        $times += [DateTime]::Parse($j.time)
    }
}
Write-Output "Total opcode 207: $($times.Count)"
for ($i = 0; $i -lt [Math]::Min(15, $times.Count - 1); $i++) {
    $diff = ($times[$i+1] - $times[$i]).TotalMilliseconds
    Write-Output "[$i -> $($i+1)] $($times[$i].ToString('HH:mm:ss.fff')) -> $($times[$i+1].ToString('HH:mm:ss.fff')) : diff = ${diff}ms"
}

