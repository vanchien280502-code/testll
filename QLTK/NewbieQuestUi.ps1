$newbiePage = New-Object Windows.Forms.TabPage
$newbiePage.Text = 'Auto Tan thu'
$newbiePage.BackColor = $form.BackColor
$tabs.TabPages.Add($newbiePage)

$script:loadingNewbie = $false
$script:newbieApplied = @{}

$newbieAccount = Ui-Label 'Chon tai khoan trong bang.' $newbiePage 16 15 630 25
$newbieCheck = New-Object Windows.Forms.CheckBox
$newbieCheck.Text = 'Tu dong lam nhiem vu chinh tuyen len Lv 10'
$newbieCheck.SetBounds(16, 50, 450, 27)
$newbiePage.Controls.Add($newbieCheck)

$null = Ui-Label 'So tai khoan mo cung luc:' $newbiePage 16 84 250 24
$newbieMax = New-Object Windows.Forms.NumericUpDown
$newbieMax.Minimum = 1
$newbieMax.Maximum = 1000
$newbieMax.Increment = 1
$newbieMax.SetBounds(275, 82, 80, 24)
$newbieMax.Value = 6
if ($script:settings.newbieMaxOnline) {
    $newbieMax.Value = [Math]::Max(1, [Math]::Min(1000, [int]$script:settings.newbieMaxOnline))
}
$newbiePage.Controls.Add($newbieMax)

$newbieCloseAfter = New-Object Windows.Forms.CheckBox
$newbieCloseAfter.Text = 'Tu tat game sau khi dat Lv 10'
$newbieCloseAfter.SetBounds(16, 112, 620, 25)
if ($null -ne $script:settings.newbieCloseAfter) {
    $newbieCloseAfter.Checked = [bool]$script:settings.newbieCloseAfter
} else {
    $newbieCloseAfter.Checked = $true
}
$newbiePage.Controls.Add($newbieCloseAfter)

$newbieQueueStart = Ui-NewButton 'Xep hang tan thu da chon' {
    try {
        Add-NewbieQueue @((Get-LaunchRows) | ForEach-Object { $_.Tag })
    } catch {
        Show-Error $_.Exception.Message
    }
}
Ui-Place $newbieQueueStart $newbiePage 16 144 220 26

$newbieQueueStop = Ui-NewButton 'Huy hang cho tan thu' {
    Reset-NewbieQueue
    Update-NewbieQueueStatus
    $status.Text = 'Da huy hang cho tan thu; cac game da mo van giu nguyen.'
}
Ui-Place $newbieQueueStop $newbiePage 248 144 220 26

$newbieQueueHint = Ui-Label '' $newbiePage 16 176 635 40
$newbieHint = Ui-Label '' $newbiePage 16 222 635 120
$newbiePage.AutoScroll = $true

. (Join-Path $PSScriptRoot 'NewbieQuestData.ps1')
. (Join-Path $PSScriptRoot 'NewbieQuestQueue.ps1')

function Save-NewbieSettings {
    $script:settings.newbieMaxOnline = [int]$newbieMax.Value
    $script:settings.newbieCloseAfter = $newbieCloseAfter.Checked
    if (!$UiTest) { Write-JsonAtomic $script:configPath $script:settings }
    Update-NewbieQueueStatus
}
$newbieMax.Add_ValueChanged({ Save-NewbieSettings })
$newbieCloseAfter.Add_CheckedChanged({ Save-NewbieSettings })
Update-NewbieQueueStatus

function Update-NewbieUi {
    $script:loadingNewbie = $true
    try {
        if (!$grid.SelectedRows.Count) {
            $newbieCheck.Enabled = $false
            $newbieHint.Text = ''
            return
        }
        $a = $grid.SelectedRows[0].Tag
        if ($null -eq $a) { return }
        $newbieCheck.Enabled = $true
        $newbieCheck.Checked = [bool]$a.autoNewbie
        $newbieAccount.Text = 'Tai khoan: ' + $a.username
        
        $lvl = 1
        if ($script:accountJobs.ContainsKey([string]$a.id)) {
            $j = Get-AccountJob $script:accountJobs[[string]$a.id]
            if ($j -and $j.CharacterLevel) { $lvl = $j.CharacterLevel }
        }
        
        $newbieHint.Text = "Cap hien tai: Lv $lvl / Yeu cau: Lv 10`r`n"
        $newbieHint.Text += "Lo trinh tu dong: 9 nhiem vu chinh tuyen (Task 0 -> 8)`r`n"
        $newbieHint.Text += "Dich den: Dat Lv 10 & xin nhap hoc tai Truong Konoha."
    } finally {
        $script:loadingNewbie = $false
    }
}

$newbieCheck.Add_CheckedChanged({
    if ($script:loadingNewbie -or !$grid.SelectedRows.Count) { return }
    $a = $grid.SelectedRows[0].Tag
    if ($null -eq $a) { return }
    $a | Add-Member NoteProperty autoNewbie $newbieCheck.Checked -Force
    if (!$UiTest) { Save-Accounts }
    Update-NewbieUi
})

$grid.Add_SelectionChanged({ Update-NewbieUi })
Update-NewbieUi