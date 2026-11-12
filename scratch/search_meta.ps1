$path = 'TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat'
$bytes = [System.IO.File]::ReadAllBytes($path)
$text = [System.Text.Encoding]::UTF8.GetString($bytes)
$pattern = '(?i)[a-zA-Z0-9_\.]{0,25}(vứt|bỏ|drop|throw|discard|destroy|bán|vut|bo)[a-zA-Z0-9_\.]{0,25}'
$matches = [regex]::Matches($text, $pattern)
$results = @($matches | ForEach-Object { $_.Value } | Select-Object -Unique)
$results | Out-File -FilePath 'scratch/metadata_matches.txt' -Encoding utf8
Write-Output "Found $($results.Count) matches"

