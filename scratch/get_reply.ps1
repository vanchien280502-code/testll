$lines = Get-Content 'C:\Users\chien\.gemini\antigravity\brain\545e266f-14a1-48a1-bac0-d8a931d6c4b8\.system_generated\logs\transcript.jsonl'
foreach ($line in $lines) {
    try {
        $j = $line | ConvertFrom-Json
        if ($j.step_index -ge 12728 -and $j.step_index -le 12756) {
            Write-Output "[$($j.step_index)] $($j.source) / $($j.type)"
            if ($j.content) {
                Write-Output $j.content
            }
        }
    } catch {}
}

