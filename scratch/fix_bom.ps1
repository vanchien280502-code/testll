$enc = New-Object System.Text.UTF8Encoding($true)
foreach ($f in @('QLTK\NewbieQuestQueue.ps1', 'QLTK\NewbieQuestData.ps1', 'QLTK\NativeBackend.cs')) {
    $content = [System.IO.File]::ReadAllText((Resolve-Path $f), [System.Text.Encoding]::UTF8)
    [System.IO.File]::WriteAllText((Resolve-Path $f), $content, $enc)
}
Write-Output "BOM preserved"

