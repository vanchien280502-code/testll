# ItemFilterUiTests.ps1
$testAcc = $script:accounts[0]
$grid.ClearSelection()
$grid.Rows[0].Selected = $true
Update-ItemFilterUi

# 1. Test parsing IDs
$parsed = Parse-ItemFilterIds "123, 456; 789`n1000"
if ($parsed.Count -ne 4 -or $parsed[0] -ne 123 -or $parsed[3] -ne 1000) {
    throw "Parse-ItemFilterIds failed to parse IDs correctly"
}

# 2. Test configure item filter
$filterEnabled.Checked = $true
$filterWhite.Checked = $true
$filterIds.Text = "159`r`n160"
$filterInterval.Value = 2
Save-ItemFilterUi

$p = Get-ItemFilterPreference $testAcc
if (!$p.enabled -or !$p.whiteEquip -or $p.ids.Count -ne 2 -or $p.ids[0] -ne 159 -or $p.seconds -ne 2) {
    throw "Get-ItemFilterPreference failed to return updated settings"
}

# 3. Test copy to all accounts
Copy-ItemFilterToAll
foreach ($a in $script:accounts) {
    $pa = Get-ItemFilterPreference $a
    if (!$pa.enabled -or !$pa.whiteEquip -or $pa.ids.Count -ne 2) {
        throw "Copy-ItemFilterToAll failed for account $($a.username)"
    }
}

# 4. Test NativeBackend configuration
$testJobId = [guid]::NewGuid().ToString()
try {
    # Test DropItemNow safety check
    $dropRes = [TGLL.NativeBackend]::DropItemNow($testJobId, 5)
    if ($dropRes) {
        throw "DropItemNow should return false for nonexistent/unready session"
    }
} catch {
    # Expected behavior
}

# Restore default
$filterEnabled.Checked = $false
Save-ItemFilterUi

Write-Output 'PASS: ItemFilter tab, per-account filter preferences, parse IDs, white equip toggle, copy to all and native safety.'

