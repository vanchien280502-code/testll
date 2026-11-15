Get-ChildItem -Path 'QLTK\logs' -Filter '*.jsonl' | ForEach-Object {
    $m = Select-String -Path $_.FullName -Pattern '"event":"task_packet_sent"' | Select-Object -First 2
    if ($m) {
        Write-Output "$($_.Name):"
        foreach ($line in $m) {
            Write-Output "   $($line.Line)"
        }
    }
}

