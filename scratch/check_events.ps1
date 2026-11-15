$lines = Get-Content 'QLTK\logs\8ff917ec-a696-41e3-b9a3-8d5098265002.jsonl'
$grouped = @{}
foreach ($l in $lines) {
    try {
        $j = $l | ConvertFrom-Json
        $ev = $j.event
        if (!$grouped.ContainsKey($ev)) { $grouped[$ev] = 0 }
        $grouped[$ev]++
    } catch {}
}
$grouped.GetEnumerator() | Sort-Object Value -Descending | ForEach-Object { "$($_.Key): $($_.Value)" }

