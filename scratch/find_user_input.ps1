$lines = Get-Content 'C:\Users\chien\.gemini\antigravity\brain\545e266f-14a1-48a1-bac0-d8a931d6c4b8\.system_generated\logs\transcript.jsonl'
foreach ($l in $lines) {
    if ($l -like '*USER_INPUT*' -and $l -like '*làm người tốt bụng*') {
        $obj = $l | ConvertFrom-Json
        Write-Host $obj.content
    }
}

