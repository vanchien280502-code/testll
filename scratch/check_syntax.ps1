$tokens = $null
$errors = $null
[System.Management.Automation.Language.Parser]::ParseFile((Resolve-Path "QLTK\NewbieQuestQueue.ps1"), [ref]$tokens, [ref]$errors)
if ($errors.Count -eq 0) {
    Write-Output "SYNTAX OK"
} else {
    $errors | ForEach-Object { Write-Error $_.Message }
}

