$matches = Select-String -Path 'C:\Users\chien\.gemini\antigravity\brain\545e266f-14a1-48a1-bac0-d8a931d6c4b8\.system_generated\logs\transcript.jsonl' -Pattern 'có vẻ là k dc rồi'
foreach ($m in $matches) {
    Write-Output "Line: $($m.LineNumber)"
    $m.Line | Out-String | Write-Output
}

