$lines = Get-Content 'C:\Users\chien\.gemini\antigravity\brain\545e266f-14a1-48a1-bac0-d8a931d6c4b8\.system_generated\logs\transcript.jsonl'
$obj = $lines[83] | ConvertFrom-Json
[System.IO.File]::WriteAllText('scratch/step84.txt', $obj.content)
Get-Content 'scratch/step84.txt'

