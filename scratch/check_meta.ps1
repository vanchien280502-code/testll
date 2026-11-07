$meta = 'C:\Users\chien\OneDrive\Máy tính\TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat'
if (Test-Path $meta) {
    $bytes = [System.IO.File]::ReadAllBytes($meta)
    $str = [System.Text.Encoding]::UTF8.GetString($bytes)
    $regex = [regex]'Làm người tốt bụng[^\x00]{1,500}'
    $all = $regex.Matches($str)
    foreach ($m in $all) {
        Write-Host ('FOUND: ' + $m.Value)
    }
    if ($all.Count -eq 0) {
        Write-Host "Not found 'Làm người tốt bụng', searching 'Anko'..."
        $regex2 = [regex]'Anko[^\x00]{1,100}'
        $all2 = $regex2.Matches($str)
        foreach ($m in $all2) {
            Write-Host ('FOUND Anko: ' + $m.Value)
        }
    }
} else {
    Write-Host "Metadata not found at $meta"
}

