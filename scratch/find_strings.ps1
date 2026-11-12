$path = 'TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat'
$bytes = [System.IO.File]::ReadAllBytes($path)
$strUtf8 = [System.Text.Encoding]::UTF8.GetString($bytes)

$keywords = @('Vứt', 'vứt', 'Bỏ', 'bỏ', 'Bán', 'bán', 'Hủy', 'hủy', 'vật phẩm', 'hành trang', 'túi đồ', 'trang bị')
foreach ($kw in $keywords) {
    $idx = 0
    $count = 0
    while (($idx = $strUtf8.IndexOf($kw, $idx)) -ge 0) {
        $start = [Math]::Max(0, $idx - 30)
        $len = [Math]::Min(80, $strUtf8.Length - $start)
        $sub = $strUtf8.Substring($start, $len) -replace "[^\x20-\x7E\u00A0-\u024F\u1EA0-\u1EF9]", " "
        Write-Output "[$kw] at $idx : $sub"
        $idx += $kw.Length
        $count++
        if ($count -ge 10) { break }
    }
}

