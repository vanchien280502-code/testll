$meta = [System.IO.Path]::Combine([Environment]::GetFolderPath('Desktop'), 'TGLL_minCPU_v2', 'TheGioiLangLa_Data', 'il2cpp_data', 'Metadata', 'global-metadata.dat')
if (-not (Test-Path $meta)) {
    $meta = (Get-ChildItem -Path 'C:\Users\chien' -Recurse -Filter 'global-metadata.dat' -ErrorAction SilentlyContinue)[0].FullName
}
Write-Host "Metadata file: $meta"
$bytes = [System.IO.File]::ReadAllBytes($meta)
$str = [System.Text.Encoding]::UTF8.GetString($bytes)
$regex = [regex]'Anko[^\x00]{1,100}'
$all = $regex.Matches($str)
foreach ($m in $all) {
    Write-Host ('Anko: ' + $m.Value)
}

