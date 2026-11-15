Get-ChildItem -Path 'QLTK\logs' -Filter '*.jsonl' | ForEach-Object {
    $m = Select-String -Path $_.FullName -Pattern '"opcode":121' | Select-Object -First 1
    if ($m) {
        Write-Output "$($_.Name): $($m.Line)"
    }
}

