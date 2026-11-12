$filterPage=New-Object Windows.Forms.TabPage;$filterPage.Text='Lọc đồ';$filterPage.BackColor=$form.BackColor;$tabs.TabPages.Add($filterPage)
$script:loadingItemFilter=$false;$script:itemFilterApplied=@{};$script:itemFilterRaw=@{};$script:itemFilterDrafts=@{};$script:itemFilterUiAccount=$null

$filterAccount=Ui-Label 'Chọn tài khoản trong bảng.' $filterPage 16 15 630 25
$filterEnabled=New-Object Windows.Forms.CheckBox;$filterEnabled.Text='Tự động vứt bỏ vật phẩm (Lọc đồ)';$filterEnabled.SetBounds(16,48,290,27);$filterPage.Controls.Add($filterEnabled)
$filterWhite=New-Object Windows.Forms.CheckBox;$filterWhite.Text='Tự vứt trang bị thường / trắng (không khóa)';$filterWhite.SetBounds(320,48,320,27);$filterPage.Controls.Add($filterWhite)

$null=Ui-Label 'ID vật phẩm vứt' $filterPage 16 88 135 25
$filterIds=New-Object Windows.Forms.TextBox;$filterIds.Multiline=$true;$filterIds.ScrollBars='Vertical';$filterIds.SetBounds(155,85,150,95);$filterPage.Controls.Add($filterIds)

$btnFilterFromBag=Ui-NewButton '+ Thêm từ Hành trang' {Open-AddFilterFromBagDialog};Ui-Place $btnFilterFromBag $filterPage 320 85 160 28
$btnFilterApplyAll=Ui-NewButton 'Áp dụng cho TẤT CẢ acc' {Copy-ItemFilterToAll};Ui-Place $btnFilterApplyAll $filterPage 320 119 160 28
$btnFilterDropNow=Ui-NewButton 'Vứt ngay lập tức' {Invoke-FilterDropNow};Ui-Place $btnFilterDropNow $filterPage 320 153 160 28

$null=Ui-Label 'Quét mỗi (giây)' $filterPage 16 190 135 25
$filterInterval=New-Object Windows.Forms.NumericUpDown;$filterInterval.Minimum=1;$filterInterval.Maximum=300;$filterInterval.Value=1;$filterInterval.SetBounds(155,188,150,27);$filterPage.Controls.Add($filterInterval)

$filterNotice=Ui-Label 'Chế độ siêu tốc: Gom sạch toàn bộ món rác trong túi vứt liền tay (mỗi 70ms), canh túi liên tục (0.3s) chống full khi quay vé.' $filterPage 16 225 630 25
$filterHint=Ui-Label '' $filterPage 16 255 630 65;$filterHint.Anchor='Top,Left,Right'

function Parse-ItemFilterIds([string]$text){
    $ids=@();foreach($line in ($text -split '[\s,;]+')){if(!$line.Trim()){continue};$n=0;if(![int]::TryParse($line.Trim(),[ref]$n) -or $n -lt 0 -or $n -gt 65535){throw 'ID phải là số từ 0 đến 65535; ngăn cách bằng dòng mới hoặc dấu phẩy.'};if($ids -notcontains $n){$ids+= $n}}
    return $ids
}

function Get-ItemFilterPreference($a){
    if($null -eq $a){return [pscustomobject]@{enabled=$false;ids=@();whiteEquip=$false;seconds=1}}
    $ids=@();$seconds=1;$white=$false;$enabled=$false
    if($a.PSObject.Properties['filterItemEnabled']){$enabled=[bool]$a.filterItemEnabled}
    if($a.PSObject.Properties['filterItemWhiteEquip']){$white=[bool]$a.filterItemWhiteEquip}
    if($a.PSObject.Properties['filterItemSeconds'] -and [int]$a.filterItemSeconds -ge 1){$seconds=[int]$a.filterItemSeconds}
    if($a.PSObject.Properties['filterItemIds']){$ids=@($a.filterItemIds | ForEach-Object {[int]$_})}
    [pscustomobject]@{enabled=$enabled;ids=$ids;whiteEquip=$white;seconds=$seconds}
}

function Sync-ItemFilterPreferences {
    foreach($a in $script:accounts){
        $key=[string]$a.id;if(!$script:accountJobs.ContainsKey($key)){continue}
        $j=Get-AccountJob $script:accountJobs[$key];if($null -eq $j -or $j.CloseRequested -or $j.State -in @('failed','closed','cancelled','reconnecting')){continue}
        $raw=$j.Id+':'+[bool]$a.filterItemEnabled+':'+[bool]$a.filterItemWhiteEquip+':'+($a.filterItemIds -join ',')+':'+$a.filterItemSeconds
        if($script:itemFilterApplied.ContainsKey($key) -and $script:itemFilterRaw[$key] -eq $raw){continue}
        $p=Get-ItemFilterPreference $a;$signature=$j.Id+':'+$p.enabled+':'+$p.whiteEquip+':'+($p.ids -join ',')+':'+$p.seconds
        if($script:itemFilterApplied[$key] -eq $signature){continue}
        try{
            [TGLL.NativeBackend]::ConfigureFilterItemPlan($j.Id,$p.enabled,[int[]]$p.ids,$p.whiteEquip,($p.seconds*1000))
            $script:itemFilterApplied[$key]=$signature;$script:itemFilterRaw[$key]=$raw
        }catch{$status.Text=$a.username+': Lỗi cấu hình lọc đồ: '+$_.Exception.Message}
    }
}

function Remember-ItemFilterDraft {
    if($script:loadingItemFilter -or $null -eq $script:itemFilterUiAccount){return}
    $script:itemFilterDrafts[[string]$script:itemFilterUiAccount.id]=@{ids=$filterIds.Text;error=''}
}

function Update-ItemFilterUi {
    $script:loadingItemFilter=$true
    try{
        $has=$grid.SelectedRows.Count -gt 0
        $filterEnabled.Enabled=$has;$filterWhite.Enabled=$has;$filterIds.Enabled=$has;$filterInterval.Enabled=$has
        $btnFilterFromBag.Enabled=$has;$btnFilterApplyAll.Enabled=$has;$btnFilterDropNow.Enabled=$has
        if(!$has){$script:itemFilterUiAccount=$null;$filterAccount.Text='Chọn tài khoản trong bảng.';$filterHint.Text='';return}
        $a=$grid.SelectedRows[0].Tag;$changed=$null -eq $script:itemFilterUiAccount -or [string]$script:itemFilterUiAccount.id -ne [string]$a.id
        $script:itemFilterUiAccount=$a;$p=Get-ItemFilterPreference $a;$filterAccount.Text='Tài khoản: '+$a.username
        $draft=$script:itemFilterDrafts[[string]$a.id]
        $idText=$p.ids -join "`r`n"
        if($null -ne $draft){$idText=$draft.ids}
        $filterEnabled.Checked=$p.enabled
        $filterWhite.Checked=$p.whiteEquip
        if(($changed -or !$filterIds.ContainsFocus) -and $filterIds.Text -ne $idText){$filterIds.Text=$idText}
        $filterInterval.Value=$p.seconds
        $filterHint.Text=$(if($p.enabled){'Lọc đồ: Đang BẬT • '+$(if($p.ids.Count){'ID '+($p.ids -join ', ')}else{'Chưa chọn ID'})+$(if($p.whiteEquip){' • Kèm trang bị trắng'}else{''})+' • Quét mỗi '+$p.seconds+'s.'}else{'Lọc đồ: Đang TẮT'})
        if($script:accountJobs.ContainsKey([string]$a.id)){
            $j=Get-AccountJob $script:accountJobs[[string]$a.id]
            if($null -ne $j){
                $filterHint.Text+="`r`n"+$j.FilterItemStatus+' • Tổng đã vứt: '+$j.FilterItemDroppedCount
                if($j.FilterItemLastDrop){$filterHint.Text+=' • Món gần nhất: '+$j.FilterItemLastDrop}
            }
        }
        if($null -ne $draft -and $draft.error){$filterHint.Text=$draft.error+' Nội dung được giữ lại, chưa áp dụng.'}
    }finally{$script:loadingItemFilter=$false}
}

function Save-ItemFilterUi {
    if($script:loadingItemFilter -or $null -eq $script:itemFilterUiAccount){return}
    $a=$script:itemFilterUiAccount
    Remember-ItemFilterDraft
    try{
        $ids=@();if($filterIds.Text.Trim()){$ids=@(Parse-ItemFilterIds $filterIds.Text)}
        if($filterEnabled.Checked -and !$ids.Count -and !$filterWhite.Checked){throw 'Nhập ít nhất một ID vật phẩm hoặc tích vứt trang bị trắng.'}
    }catch{
        $script:itemFilterDrafts[[string]$a.id].error=$_.Exception.Message
        $filterHint.Text=$_.Exception.Message+' Nội dung được giữ lại, chưa áp dụng.';return
    }
    $a | Add-Member NoteProperty filterItemEnabled $filterEnabled.Checked -Force
    $a | Add-Member NoteProperty filterItemWhiteEquip $filterWhite.Checked -Force
    $a | Add-Member NoteProperty filterItemIds ([int[]]$ids) -Force
    $a | Add-Member NoteProperty filterItemSeconds ([int]$filterInterval.Value) -Force
    $script:itemFilterDrafts.Remove([string]$a.id)
    if(!$UiTest){Save-Accounts};Sync-ItemFilterPreferences;Update-ItemFilterUi
}

function Copy-ItemFilterToAll {
    if($grid.SelectedRows.Count -eq 0){return}
    Save-ItemFilterUi
    $src=$grid.SelectedRows[0].Tag;$p=Get-ItemFilterPreference $src
    $count=0
    foreach($a in $script:accounts){
        $a | Add-Member NoteProperty filterItemEnabled $p.enabled -Force
        $a | Add-Member NoteProperty filterItemWhiteEquip $p.whiteEquip -Force
        $a | Add-Member NoteProperty filterItemIds ([int[]]$p.ids) -Force
        $a | Add-Member NoteProperty filterItemSeconds $p.seconds -Force
        $count++
    }
    if(!$UiTest){Save-Accounts};Sync-ItemFilterPreferences;Update-ItemFilterUi
    $filterHint.Text="Đã áp dụng cấu hình Lọc đồ cho toàn bộ $count tài khoản."
}

function Invoke-FilterDropNow {
    if($grid.SelectedRows.Count -eq 0){return}
    $a=$grid.SelectedRows[0].Tag;$key=[string]$a.id
    if(!$script:accountJobs.ContainsKey($key)){$filterHint.Text='Tài khoản chưa vào game.';return}
    $j=Get-AccountJob $script:accountJobs[$key]
    if($null -eq $j -or !$j.TrainReady){$filterHint.Text='Đợi nhân vật vào bản đồ game để vứt đồ.';return}
    try{
        $filterEnabled.Checked=$true
        Save-ItemFilterUi
        $j.FilterDropQueue.Clear()
        $j.FilterItemNext=[datetime]::MinValue
        $filterHint.Text='Đã kích hoạt: Đang quét và vứt liền tay toàn bộ món rác trong túi!'
    }catch{$filterHint.Text='Lỗi khi kích hoạt vứt đồ: '+$_.Exception.Message}
}

function Open-AddFilterFromBagDialog {
    if($grid.SelectedRows.Count -eq 0){return}
    $a=$grid.SelectedRows[0].Tag;$key=[string]$a.id
    if(!$script:accountJobs.ContainsKey($key)){
        [System.Windows.Forms.MessageBox]::Show('Tài khoản chưa vào game. Hãy mở game trước để đọc hành trang.','Lọc đồ',[System.Windows.Forms.MessageBoxButtons]::OK,[System.Windows.Forms.MessageBoxIcon]::Information) | Out-Null
        return
    }
    $j=Get-AccountJob $script:accountJobs[$key]
    try{[TGLL.NativeBackend]::ScanInventory($j.Id)}catch{}
    $snapshot=$j.Inventory
    if($null -eq $snapshot -or !$snapshot.Ready -or $snapshot.Items.Length -eq 0){
        [System.Windows.Forms.MessageBox]::Show('Chưa có dữ liệu hành trang hoặc túi đồ đang trống. Vui lòng mở tab Trang bị và bấm Đọc hành trang trước.','Lọc đồ',[System.Windows.Forms.MessageBoxButtons]::OK,[System.Windows.Forms.MessageBoxIcon]::Information) | Out-Null
        return
    }

    $dlg=New-Object Windows.Forms.Form;$dlg.Text='Chọn vật phẩm từ Hành trang để Lọc đồ';$dlg.Size=New-Object Drawing.Size(480,420);$dlg.StartPosition='CenterParent';$dlg.FormBorderStyle='FixedDialog';$dlg.MaximizeBox=$false;$dlg.MinimizeBox=$false
    $dlgHelp=Ui-Label 'Tích chọn các vật phẩm bạn muốn tự động vứt bỏ:' $dlg 12 10 440 22
    $clb=New-Object Windows.Forms.CheckedListBox;$clb.SetBounds(12,35,440,280);$clb.CheckOnClick=$true;$dlg.Controls.Add($clb)

    $existingIds=@();if($filterIds.Text.Trim()){try{$existingIds=@(Parse-ItemFilterIds $filterIds.Text)}catch{}}
    $seenIds=New-Object 'System.Collections.Generic.HashSet[int]'
    $itemMap=@{}

    foreach($item in $snapshot.Items){
        if($item.Locked){continue}
        if($seenIds.Contains($item.TemplateId)){continue}
        $seenIds.Add($item.TemplateId) | Out-Null
        $desc="[ID "+$item.TemplateId+"] "+$item.Name+" (Số lượng: "+$item.Quantity+")"
        $idx=$clb.Items.Add($desc)
        $itemMap[$idx]=$item.TemplateId
        if($existingIds -contains $item.TemplateId){$clb.SetItemChecked($idx,$true)}
    }

    $btnOk=Ui-NewButton 'Thêm vào danh sách lọc' {$dlg.DialogResult=[Windows.Forms.DialogResult]::OK;$dlg.Close()};Ui-Place $btnOk $dlg 120 330 150 30
    $btnCancel=Ui-NewButton 'Đóng' {$dlg.DialogResult=[Windows.Forms.DialogResult]::Cancel;$dlg.Close()};Ui-Place $btnCancel $dlg 280 330 90 30

    if($dlg.ShowDialog() -eq [Windows.Forms.DialogResult]::OK){
        $newIds=@($existingIds)
        for($i=0;$i -lt $clb.Items.Count;$i++){
            if($clb.GetItemChecked($i)){
                $tid=$itemMap[$i]
                if($newIds -notcontains $tid){$newIds+= $tid}
            }
        }
        $filterIds.Text=$newIds -join "`r`n"
        Save-ItemFilterUi
    }
}

$filterIds.Add_TextChanged({Remember-ItemFilterDraft})
$filterEnabled.Add_CheckedChanged({Save-ItemFilterUi})
$filterWhite.Add_CheckedChanged({Save-ItemFilterUi})
$filterIds.Add_Leave({Save-ItemFilterUi})
$filterInterval.Add_ValueChanged({Save-ItemFilterUi})
$grid.Add_SelectionChanged({Update-ItemFilterUi})
Update-ItemFilterUi

