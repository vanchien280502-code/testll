$wheelPage=New-Object Windows.Forms.TabPage;$wheelPage.Text='Auto vòng quay';$wheelPage.BackColor=$form.BackColor;$wheelPage.AutoScroll=$true;$tabs.TabPages.Add($wheelPage)
$script:loadingWheel=$false
$wheelAccount=Ui-Label 'Chọn tài khoản trong bảng.' $wheelPage 16 12 630 24
$null=Ui-Label 'Khoảng cách mỗi lần quay (ms)' $wheelPage 16 48 200 26
$wheelInterval=New-Object Windows.Forms.NumericUpDown;$wheelInterval.Minimum=50;$wheelInterval.Maximum=10000;$wheelInterval.Value=1200;$wheelInterval.Increment=50;$wheelInterval.SetBounds(225,46,120,27);$wheelPage.Controls.Add($wheelInterval)

$null=Ui-Label 'Số lượt quay (0 = không giới hạn)' $wheelPage 16 82 200 26
$wheelCount=New-Object Windows.Forms.NumericUpDown;$wheelCount.Minimum=0;$wheelCount.Maximum=99999;$wheelCount.Value=0;$wheelCount.Increment=10;$wheelCount.SetBounds(225,80,120,27);$wheelPage.Controls.Add($wheelCount)

$null=Ui-Label 'Tự động quay vòng quay (Thử vận may / Vàng / Bạc / May mắn).' $wheelPage 16 118 650 25
$null=Ui-Label 'Lưu ý: Tự mở sẵn giao diện Vòng quay trên màn hình game trước khi bấm Bắt đầu quay.' $wheelPage 16 142 650 25

$wheelStart=New-Object Windows.Forms.Button;$wheelStart.Text='Bắt đầu quay';$wheelStart.SetBounds(16,176,145,29);$wheelPage.Controls.Add($wheelStart)
$wheelStop=New-Object Windows.Forms.Button;$wheelStop.Text='Dừng quay';$wheelStop.SetBounds(170,176,145,29);$wheelPage.Controls.Add($wheelStop)

$wheelHint=Ui-Label '' $wheelPage 16 218 650 90;$wheelHint.Anchor='Top,Left,Right'

function Get-LuckyWheelPreference($a){
    $interval=1200;$count=0
    if($null -ne $a){
        if($a.PSObject.Properties['luckyWheelInterval']){$interval=[Math]::Max(50,[Math]::Min(10000,[int]$a.luckyWheelInterval))}
        if($a.PSObject.Properties['luckyWheelCount']){$count=[Math]::Max(0,[Math]::Min(99999,[int]$a.luckyWheelCount))}
    }
    [pscustomobject]@{interval=$interval;count=$count}
}

function Update-LuckyWheelUi {
    $script:loadingWheel=$true
    try{
        $has=$grid.SelectedRows.Count -gt 0;$wheelInterval.Enabled=$has;$wheelCount.Enabled=$has
        if(!$has){$wheelAccount.Text='Chọn tài khoản trong bảng.';$wheelHint.Text='';return}
        $a=$grid.SelectedRows[0].Tag;$p=Get-LuckyWheelPreference $a;$wheelAccount.Text='Tài khoản: '+$a.username
        if(!$wheelInterval.Focused){$wheelInterval.Value=$p.interval}
        if(!$wheelCount.Focused){$wheelCount.Value=$p.count}
        $wheelHint.Text='Tích chọn tài khoản, mở Vòng quay trong game rồi bấm "Bắt đầu quay".'
        if($script:accountJobs.ContainsKey([string]$a.id)){
            $j=Get-AccountJob $script:accountJobs[[string]$a.id]
            if($null -ne $j -and $j -is [TGLL.NativeJob]){
                $ticketCount=[TGLL.NativeBackend]::LuckyWheelTicketCount($j.Inventory)
                $tgt=if($j.LuckyWheelTargetCount -gt 0){"/"+$j.LuckyWheelTargetCount}else{""}
                $wheelHint.Text=$j.LuckyWheelStatus+"`r`n"+'Đã quay: '+$j.LuckyWheelSpunCount+$tgt+' • Vé vận may (ID 706) trong túi: '+$ticketCount
            }
        }
    }finally{$script:loadingWheel=$false}
}

function Save-LuckyWheelUi {
    if($script:loadingWheel -or !$grid.SelectedRows.Count){return}
    $a=$grid.SelectedRows[0].Tag
    $a | Add-Member NoteProperty luckyWheelInterval ([int]$wheelInterval.Value) -Force
    $a | Add-Member NoteProperty luckyWheelCount ([int]$wheelCount.Value) -Force
    if(!$UiTest){Save-Accounts}
}

function Start-LuckyWheelSelected {
    $rows=@(Get-LaunchRows);if(!$rows.Count){$status.Text='Tích chọn các tài khoản cần quay.';return}
    $interval=[int]$wheelInterval.Value;$count=[int]$wheelCount.Value;$started=0;$errors=New-Object Collections.Generic.List[string]
    foreach($row in $rows){
        $a=$row.Tag
        $a | Add-Member NoteProperty luckyWheelInterval $interval -Force
        $a | Add-Member NoteProperty luckyWheelCount $count -Force
        try{
            if(!$script:accountJobs.ContainsKey([string]$a.id)){throw 'Acc chưa mở.'}
            $j=Get-AccountJob $script:accountJobs[[string]$a.id];if($null -eq $j -or $j -isnot [TGLL.NativeJob]){throw 'Acc chưa sẵn sàng.'}
            [TGLL.NativeBackend]::StartLuckyWheel($j.Id,$interval,$count);$started++
        }catch{$errors.Add($a.username+': '+$_.Exception.Message)}
    }
    if(!$UiTest){Save-Accounts};Update-LuckyWheelUi
    $status.Text='Đã bắt đầu quay '+$started+' acc.';if($errors.Count){$status.Text+=' '+($errors -join ' | ')}
}

function Stop-LuckyWheelSelected {
    foreach($row in @(Get-LaunchRows)){$key=[string]$row.Tag.id;if($script:accountJobs.ContainsKey($key)){[TGLL.NativeBackend]::StopLuckyWheel($script:accountJobs[$key])}}
    Update-LuckyWheelUi
}

$wheelInterval.Add_ValueChanged({Save-LuckyWheelUi})
$wheelCount.Add_ValueChanged({Save-LuckyWheelUi})
$wheelStart.Add_Click({Start-LuckyWheelSelected})
$wheelStop.Add_Click({Stop-LuckyWheelSelected})
$grid.Add_SelectionChanged({Update-LuckyWheelUi});Update-LuckyWheelUi
