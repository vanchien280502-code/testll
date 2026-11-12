$metaPath = 'TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat'
if (Test-Path $metaPath) {
    $bytes = [System.IO.File]::ReadAllBytes($metaPath)
    $str = [System.Text.Encoding]::UTF8.GetString($bytes)
    $regex = [System.Text.RegularExpressions.Regex]::new('(?i)[^\x00]{0,30}(vòng quay|may mắn|vong quay|lucky|wheel|quay số)[^\x00]{0,40}')
    $matches = $regex.Matches($str)
    $results = @()
    foreach ($m in $matches) {
        $results += $m.Value
    }
    $results | Select-Object -Unique | ForEach-Object { Write-Output $_ }
} else {
    Write-Output "Not found"
}

