$accs = Get-Content Data/accounts.json -Raw | ConvertFrom-Json
foreach ($a in $accs) {
    if ($a.filterItemIds) {
        Write-Output ("Account: " + $a.username + " filterItemIds: " + ($a.filterItemIds -join ", "))
    }
}

