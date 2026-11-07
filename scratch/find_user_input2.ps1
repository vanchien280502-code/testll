$lines = [System.IO.File]::ReadAllLines('C:\Users\chien\.gemini\antigravity\brain\545e266f-14a1-48a1-bac0-d8a931d6c4b8\.system_generated\logs\transcript.jsonl')
for ($i = 0; $i -lt $lines.Length; $i++) {
    if ($lines[$i] -match '"type":"USER_INPUT"' -and $lines[$i] -match 'Anko') {
        $obj = ConvertFrom-Json $lines[$i]
        [System.IO.File]::WriteAllText("scratch/user_inko_$i.txt", $obj.content, [System.Text.Encoding]::UTF8)
        Write-Host "Wrote scratch/user_inko_$i.txt"
    }
}

