$lines = Get-Content 'QLTK\logs\c01dfd0c-1bdf-4e97-a4f8-38a6a29373d5.jsonl'
for ($i = 16480; $i -le 16525; $i++) {
    Write-Output "[$i] $($lines[$i])"
}

