$gemPage=New-Object Windows.Forms.TabPage;$gemPage.Text='Auto ghép đá 12';$gemPage.BackColor=$form.BackColor;$gemPage.AutoScroll=$true;$tabs.TabPages.Add($gemPage)
$null=Ui-Label 'Ghép đá cấp cao: Đá 12 (16 đá 10 hoặc 4 đá 11), Đá 14 (16 đá 12), Đá 16 (16 đá 14) • tỷ lệ 100%.' $gemPage 16 16 680 25
$null=Ui-Label 'Chỉ dùng bạc khóa. Tự động kiểm tra túi và bạc trước khi ghép. Đủ nguyên liệu mới tạm tắt ghép trên 50% của game.' $gemPage 16 43 680 25
$null=Ui-Label 'Có thể tích chọn 1 hoặc nhiều cấp cùng lúc (ưu tiên ghép cấp cao trước: 14→16, 12→14, 10/11→12).' $gemPage 16 70 680 25

$script:gemLoading=$false;$script:gemApplied=@{}

$gemCheck=New-Object Windows.Forms.CheckBox;$gemCheck.Text='Ghép đá 12 (16 đá cấp 10 hoặc 4 đá cấp 11)';$gemCheck.SetBounds(16,102,420,25);$gemPage.Controls.Add($gemCheck)
$gem14Check=New-Object Windows.Forms.CheckBox;$gem14Check.Text='Ghép đá 14 (16 đá cấp 12)';$gem14Check.SetBounds(16,132,420,25);$gemPage.Controls.Add($gem14Check)
$gem16Check=New-Object Windows.Forms.CheckBox;$gem16Check.Text='Ghép đá 16 (16 đá cấp 14)';$gem16Check.SetBounds(16,162,420,25);$gemPage.Controls.Add($gem16Check)

$gemStart=New-Object Windows.Forms.Button;$gemStart.Text='Bật đã chọn';$gemStart.SetBounds(16,198,150,30);$gemPage.Controls.Add($gemStart)
$gemStop=New-Object Windows.Forms.Button;$gemStop.Text='Dừng đã chọn';$gemStop.SetBounds(176,198,150,30);$gemPage.Controls.Add($gemStop)
$gemApplyAll=New-Object Windows.Forms.Button;$gemApplyAll.Text='Áp dụng cho TẤT CẢ acc';$gemApplyAll.SetBounds(336,198,180,30);$gemPage.Controls.Add($gemApplyAll)

$gemReport=New-Object Windows.Forms.TextBox;$gemReport.Multiline=$true;$gemReport.ReadOnly=$true;$gemReport.ScrollBars='Vertical';$gemReport.SetBounds(16,240,680,190);$gemPage.Controls.Add($gemReport)

function Start-GemCombineJob($j,$g12=$true,$g14=$false,$g16=$false){[TGLL.NativeBackend]::StartGemCombine($j.Id,$g12,$g14,$g16)}
function Stop-GemCombineJob($j){[TGLL.NativeBackend]::StopGemCombine($j.Id)}

function Sync-GemCombineAccount($a,$j){
    if($j -isnot [TGLL.NativeJob] -or $j.CloseRequested -or $j.CancelRequested -or $j.State -in @('failed','closed','cancelled')){return}
    $key=[string]$a.id
    $g12=if($a.PSObject.Properties['gem12Enabled']){[bool]$a.gem12Enabled}else{[bool]$a.gemCombineEnabled}
    $g14=if($a.PSObject.Properties['gem14Enabled']){[bool]$a.gem14Enabled}else{$false}
    $g16=if($a.PSObject.Properties['gem16Enabled']){[bool]$a.gem16Enabled}else{$false}
    $wanted=$g12 -or $g14 -or $g16
    
    if($wanted -and $j.GemSkipped){
        $a | Add-Member NoteProperty gemCombineEnabled $false -Force
        $a | Add-Member NoteProperty gem12Enabled $false -Force
        $a | Add-Member NoteProperty gem14Enabled $false -Force
        $a | Add-Member NoteProperty gem16Enabled $false -Force
        $wanted=$false;if(!$UiTest){Save-Accounts}
    }
    $signature=$j.Id+':'+$g12+':'+$g14+':'+$g16
    if($script:gemApplied[$key] -eq $signature){return}
    if($wanted){
        if(!$j.TrainReady -or !$j.GameSettingsReady){return}
        try{
            [TGLL.NativeBackend]::ConfigureGemCombine($j.Id,$g12,$g14,$g16)
            Start-GemCombineJob $j $g12 $g14 $g16
        }catch{$status.Text=$a.username+': '+$_.Exception.Message;return}
    }elseif($j.GemEnabled){
        Stop-GemCombineJob $j
    }
    $script:gemApplied[$key]=$signature
}

function Set-GemCombinePreference($row,[bool]$enabled){
    Set-GemCombineTiers $row $enabled $false $false
}

function Set-GemCombineTiers($row,[bool]$g12,[bool]$g14,[bool]$g16){
    $a=$row.Tag;$key=[string]$a.id
    $enabled=$g12 -or $g14 -or $g16
    $a | Add-Member NoteProperty gemCombineEnabled $enabled -Force
    $a | Add-Member NoteProperty gem12Enabled $g12 -Force
    $a | Add-Member NoteProperty gem14Enabled $g14 -Force
    $a | Add-Member NoteProperty gem16Enabled $g16 -Force
    $script:gemApplied.Remove($key)
    if($enabled){
        $overrides=@($a.gameOverrides | Where-Object {$null -ne $_ -and [int]$_.code -ne 307})
        $overrides+=([pscustomobject]@{code=307;value=1})
        $a | Add-Member NoteProperty gameOverrides $overrides -Force
    }
    if(!$UiTest){Save-Accounts}
    if($script:accountJobs.ContainsKey($key)){
        $j=Get-AccountJob $script:accountJobs[$key]
        if($j -is [TGLL.NativeJob]){$j.GemSkipped=$false}
        Sync-GemCombineAccount $a $j
    }
}

function Update-GemCombineUi {
    $script:gemLoading=$true
    try{
        $has=$grid.SelectedRows.Count -gt 0 -and $null -ne $grid.SelectedRows[0].Tag
        $gemCheck.Enabled=$has
        $gem14Check.Enabled=$has
        $gem16Check.Enabled=$has
        if($has){
            $a=$grid.SelectedRows[0].Tag
            $g12=if($a.PSObject.Properties['gem12Enabled']){[bool]$a.gem12Enabled}else{[bool]$a.gemCombineEnabled}
            $g14=if($a.PSObject.Properties['gem14Enabled']){[bool]$a.gem14Enabled}else{$false}
            $g16=if($a.PSObject.Properties['gem16Enabled']){[bool]$a.gem16Enabled}else{$false}
            $gemCheck.Checked=$g12
            $gem14Check.Checked=$g14
            $gem16Check.Checked=$g16
            $gemCheck.Text='Ghép đá 12 (16 đá 10 hoặc 4 đá 11): '+$a.username
        }else{
            $gemCheck.Checked=$false
            $gem14Check.Checked=$false
            $gem16Check.Checked=$false
            $gemCheck.Text='Ghép đá 12 (16 đá cấp 10 hoặc 4 đá cấp 11)'
        }
    }finally{$script:gemLoading=$false}

    $lines=@(foreach($row in $grid.Rows){
        if($null -eq $row.Tag){continue}
        $key=[string]$row.Tag.id;if(!$script:accountJobs.ContainsKey($key)){continue}
        $j=Get-AccountJob $script:accountJobs[$key]
        if($j -is [TGLL.NativeJob] -and ($j.GemEnabled -or $j.GemStatus -ne 'Tắt')){
            $row.Tag.username+': '+$j.GemStatus+' • Đã ghép: '+$j.GemCompleted
        }
    })
    $text=if($lines.Count){$lines -join "
"}else{'Tích chọn cấp đá muốn ghép (12, 14, 16) rồi bấm Bật đã chọn.'}
    if($gemReport.Text -ne $text){$gemReport.Text=$text}
}

$gemCheck.Add_CheckedChanged({
    if($script:gemLoading -or !$grid.SelectedRows.Count -or $null -eq $grid.SelectedRows[0].Tag){return}
    Set-GemCombineTiers $grid.SelectedRows[0] $gemCheck.Checked $gem14Check.Checked $gem16Check.Checked
    Update-GemCombineUi
})
$gem14Check.Add_CheckedChanged({
    if($script:gemLoading -or !$grid.SelectedRows.Count -or $null -eq $grid.SelectedRows[0].Tag){return}
    Set-GemCombineTiers $grid.SelectedRows[0] $gemCheck.Checked $gem14Check.Checked $gem16Check.Checked
    Update-GemCombineUi
})
$gem16Check.Add_CheckedChanged({
    if($script:gemLoading -or !$grid.SelectedRows.Count -or $null -eq $grid.SelectedRows[0].Tag){return}
    Set-GemCombineTiers $grid.SelectedRows[0] $gemCheck.Checked $gem14Check.Checked $gem16Check.Checked
    Update-GemCombineUi
})

$gemStart.Add_Click({
    $rows=@(Get-LaunchRows)
    foreach($row in $rows){
        $g12=$gemCheck.Checked;$g14=$gem14Check.Checked;$g16=$gem16Check.Checked
        if(!$g12 -and !$g14 -and !$g16){$g12=$true}
        Set-GemCombineTiers $row $g12 $g14 $g16
    }
    $status.Text='Đã bật tự ghép đá cho '+$rows.Count+' acc.'
    Update-GemCombineUi
})

$gemStop.Add_Click({
    foreach($row in @(Get-LaunchRows)){Set-GemCombineTiers $row $false $false $false}
    $status.Text='Đã dừng ghép đá cho các acc đã chọn.'
    Update-GemCombineUi
})

$gemApplyAll.Add_Click({
    $g12=$gemCheck.Checked;$g14=$gem14Check.Checked;$g16=$gem16Check.Checked
    if(!$g12 -and !$g14 -and !$g16){$g12=$true}
    foreach($row in $grid.Rows){Set-GemCombineTiers $row $g12 $g14 $g16}
    $status.Text='Đã áp dụng cấu hình ghép đá cho toàn bộ '+$grid.Rows.Count+' acc.'
    Update-GemCombineUi
})

$grid.Add_SelectionChanged({Update-GemCombineUi})
Update-GemCombineUi