$enc = New-Object System.Text.UTF8Encoding($true)
Get-ChildItem -Path "QLTK" -Include *.ps1,*.cs -Recurse | ForEach-Object {
    $bytes = [System.IO.File]::ReadAllBytes($_.FullName)
    if ($bytes.Length -lt 3 -or $bytes[0] -ne 0xEF -or $bytes[1] -ne 0xBB -or $bytes[2] -ne 0xBF) {
        $content = [System.IO.File]::ReadAllText($_.FullName, [System.Text.Encoding]::UTF8)
        [System.IO.File]::WriteAllText($_.FullName, $content, $enc)
        Write-Output "Added BOM to $($_.Name)"
    }
}
Write-Output "BOM preserved for all files."
