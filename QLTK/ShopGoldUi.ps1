$shopGoldPage=New-Object Windows.Forms.TabPage;$shopGoldPage.Text='Auto Shop Vàng';$shopGoldPage.BackColor=$form.BackColor;$shopGoldPage.AutoScroll=$true;$tabs.TabPages.Add($shopGoldPage)
$script:loadingShopGold=$false;$script:shopGoldRun=$null
$script:shopGoldCatalogFile=Join-Path $script:dataDir 'shop_gold.json'

$null=Ui-Label 'Acc dòng 1 là người nhận đồ & cấp Bạc cho clone làm cước gửi thư.' $shopGoldPage 14 10 750 22
$shopGoldReceiver=Ui-Label 'Acc nhận (dòng 1): [Chưa xác định]' $shopGoldPage 14 34 230 22

$null=Ui-Label 'Số clone mở cùng lúc:' $shopGoldPage 250 34 130 22
$shopGoldMax=New-Object Windows.Forms.NumericUpDown;$shopGoldMax.Minimum=1;$shopGoldMax.Maximum=1000;$shopGoldMax.Value=2;$shopGoldMax.SetBounds(385,32,55,24);$shopGoldPage.Controls.Add($shopGoldMax)
if($script:settings.ContainsKey('shopGoldMaxOnline')){$shopGoldMax.Value=[Math]::Max(1,[Math]::Min(1000,[int]$script:settings.shopGoldMaxOnline))}

$null=Ui-Label 'Bạc cấp:' $shopGoldPage 450 34 60 22
$shopGoldSilver=New-Object Windows.Forms.NumericUpDown;$shopGoldSilver.Minimum=0;$shopGoldSilver.Maximum=2000000000;$shopGoldSilver.Value=10000;$shopGoldSilver.SetBounds(515,32,95,24);$shopGoldPage.Controls.Add($shopGoldSilver)

$null=Ui-Label 'Cách thư (s):' $shopGoldPage 620 34 85 22
$shopGoldDelay=New-Object Windows.Forms.NumericUpDown;$shopGoldDelay.Minimum=1;$shopGoldDelay.Maximum=3600;$shopGoldDelay.Value=5;$shopGoldDelay.SetBounds(710,32,60,24);$shopGoldPage.Controls.Add($shopGoldDelay)

$null=Ui-Label 'Mã Gift Code (clone tự nhập trước để nhận Vàng):' $shopGoldPage 14 62 310 22
$shopGoldCodes=New-Object Windows.Forms.TextBox;$shopGoldCodes.Multiline=$true;$shopGoldCodes.ScrollBars='Vertical';$shopGoldCodes.SetBounds(14,86,305,84);$shopGoldPage.Controls.Add($shopGoldCodes)
if($script:settings.ContainsKey('shopGoldCodes')){$shopGoldCodes.Text=$script:settings.shopGoldCodes}
if($script:settings.ContainsKey('shopGoldSilver')){$shopGoldSilver.Value=[Math]::Max(0,[Math]::Min(2000000000,[int]$script:settings.shopGoldSilver))}
if($script:settings.ContainsKey('shopGoldDelay')){$shopGoldDelay.Value=[Math]::Max(1,[Math]::Min(3600,[int]$script:settings.shopGoldDelay))}

$null=Ui-Label 'Danh mục vật phẩm Shop Vàng (Tích chọn & nhập số lượng mua mỗi clone):' $shopGoldPage 330 62 455 22
$shopGoldGrid=New-Object Windows.Forms.DataGridView;$shopGoldGrid.SetBounds(330,86,455,160);$shopGoldGrid.AllowUserToAddRows=$false;$shopGoldGrid.RowHeadersVisible=$false;$shopGoldGrid.SelectionMode='FullRowSelect';$shopGoldGrid.MultiSelect=$false;$shopGoldPage.Controls.Add($shopGoldGrid)

$colPick=New-Object Windows.Forms.DataGridViewCheckBoxColumn;$colPick.Name='pick';$colPick.HeaderText='Chọn';$colPick.Width=45;$null=$shopGoldGrid.Columns.Add($colPick)
$colName=New-Object Windows.Forms.DataGridViewTextBoxColumn;$colName.Name='name';$colName.HeaderText='Tên vật phẩm';$colName.ReadOnly=$true;$colName.Width=195;$null=$shopGoldGrid.Columns.Add($colName)
$colPrice=New-Object Windows.Forms.DataGridViewTextBoxColumn;$colPrice.Name='price';$colPrice.HeaderText='Giá Vàng';$colPrice.ReadOnly=$true;$colPrice.Width=75;$null=$shopGoldGrid.Columns.Add($colPrice)
$colQty=New-Object Windows.Forms.DataGridViewTextBoxColumn;$colQty.Name='quantity';$colQty.HeaderText='Số lượng';$colQty.Width=75;$null=$shopGoldGrid.Columns.Add($colQty)
$colTpl=New-Object Windows.Forms.DataGridViewTextBoxColumn;$colTpl.Name='templateId';$colTpl.Visible=$false;$null=$shopGoldGrid.Columns.Add($colTpl)
$colSale=New-Object Windows.Forms.DataGridViewTextBoxColumn;$colSale.Name='saleId';$colSale.Visible=$false;$null=$shopGoldGrid.Columns.Add($colSale)

function Save-ShopGoldCatalogFile($offers){
    try{
        $arr=@(foreach($o in $offers){
            [ordered]@{
                SaleId=[int]$o.SaleId
                TemplateId=[int]$o.TemplateId
                Name=[string]$o.Name
                GoldPrice=[int]$o.GoldPrice
            }
        })
        $json=$arr | ConvertTo-Json -Depth 3
        if(!$UiTest){[System.IO.File]::WriteAllText($script:shopGoldCatalogFile,$json,[System.Text.UTF8Encoding]::new($false))}
    }catch{}
}

function Get-ShopGoldCatalog {
    if(Test-Path $script:shopGoldCatalogFile){
        try{
            $json=[System.IO.File]::ReadAllText($script:shopGoldCatalogFile,[System.Text.Encoding]::UTF8)
            $data=$json | ConvertFrom-Json
            if($data -and @($data).Count -gt 0){
                $list=New-Object Collections.Generic.List[TGLL.GoldShopOffer]
                foreach($item in @($data)){
                    $o=New-Object TGLL.GoldShopOffer
                    $o.SaleId=[int]$item.SaleId
                    $o.TemplateId=[int]$item.TemplateId
                    $o.GoldPrice=[int]$item.GoldPrice
                    $o.Name=[string]$item.Name
                    $list.Add($o)
                }
                return $list.ToArray()
            }
        }catch{}
    }
    $def=[TGLL.NativeBackend]::DefaultGoldShopOffers()
    Save-ShopGoldCatalogFile $def
    return $def
}

function Populate-ShopGoldGrid($offers){
    $shopGoldGrid.Rows.Clear()
    $saved=@{}
    if($script:settings.ContainsKey('shopGoldItems') -and $script:settings.shopGoldItems){
        foreach($entry in ($script:settings.shopGoldItems -split ';')){
            if($entry -match '^(\d+):(\d+)$'){$saved[[int]$Matches[1]]=[int]$Matches[2]}
        }
    }
    foreach($o in $offers){
        $picked=$saved.ContainsKey($o.TemplateId)
        $qty=if($picked){$saved[$o.TemplateId]}else{1}
        $null=$shopGoldGrid.Rows.Add($picked,$o.Name,$o.GoldPrice,$qty,$o.TemplateId,$o.SaleId)
    }
}
Populate-ShopGoldGrid (Get-ShopGoldCatalog)

function Refresh-ShopGoldOffers {
    $targetJobId=$null
    if($grid.SelectedRows.Count -gt 0){
        $a=$grid.SelectedRows[0].Tag
        if($a -and $script:accountJobs.ContainsKey([string]$a.id)){
            $j=[TGLL.NativeBackend]::Get($script:accountJobs[[string]$a.id])
            if($j -and $j.TrainReady -and $j.ProcessId -gt 0){$targetJobId=$j.Id}
        }
    }
    if(!$targetJobId){
        foreach($kv in $script:accountJobs.GetEnumerator()){
            $j=[TGLL.NativeBackend]::Get($kv.Value)
            if($j -and $j.TrainReady -and $j.ProcessId -gt 0){$targetJobId=$j.Id;break}
        }
    }
    if(!$targetJobId){
        Show-Error 'Hãy mở ít nhất 1 tài khoản (acc chính hoặc clone) vào game đứng trong làng, rồi bấm "Quét từ game".'
        return
    }
    try{
        $status.Text='Đang gửi lệnh mở Shop Vàng và quét danh mục từ game...'
        $form.Refresh()
        $offers=[TGLL.NativeBackend]::FetchGoldShopOffers($targetJobId)
        if($offers -and $offers.Count -gt 0){
            Save-ShopGoldCatalogFile $offers
            Populate-ShopGoldGrid $offers
            $msg="Đã quét thành công $($offers.Count) vật phẩm từ Shop Vàng trên server hiện tại!"
            $status.Text=$msg
            [System.Windows.Forms.MessageBox]::Show($msg,'Quét Shop Vàng',[System.Windows.Forms.MessageBoxButtons]::OK,[System.Windows.Forms.MessageBoxIcon]::Information)
        }else{
            Show-Error 'Không tìm thấy vật phẩm nào trong Shop Vàng từ server.'
        }
    }catch{
        Show-Error $_.Exception.Message
    }
}

function Open-ShopGoldJson {
    if(!(Test-Path $script:shopGoldCatalogFile)){Save-ShopGoldCatalogFile ([TGLL.NativeBackend]::DefaultGoldShopOffers())}
    try{Start-Process notepad.exe $script:shopGoldCatalogFile}catch{Show-Error $_.Exception.Message}
}

function Reload-ShopGoldJson {
    try{
        $offers=Get-ShopGoldCatalog
        Populate-ShopGoldGrid $offers
        $status.Text="Đã nạp lại $($offers.Count) vật phẩm từ file JSON."
    }catch{Show-Error $_.Exception.Message}
}

function Add-ShopGoldItemDialog {
    $d=New-Object Windows.Forms.Form;$d.Text='Thêm vật phẩm Shop Vàng';$d.Size=New-Object Drawing.Size(320,225);$d.StartPosition='CenterParent';$d.FormBorderStyle='FixedDialog';$d.MaximizeBox=$false;$d.MinimizeBox=$false
    $null=Ui-Label 'Tên vật phẩm:' $d 15 12 95 20
    $txtName=New-Object Windows.Forms.TextBox;$txtName.SetBounds(115,10,170,22);$d.Controls.Add($txtName)
    $null=Ui-Label 'ID Template:' $d 15 42 95 20
    $txtTid=New-Object Windows.Forms.NumericUpDown;$txtTid.Minimum=0;$txtTid.Maximum=10000;$txtTid.Value=1;$txtTid.SetBounds(115,40,170,22);$d.Controls.Add($txtTid)
    $null=Ui-Label 'ID Mua (SaleId):' $d 15 72 95 20
    $txtSale=New-Object Windows.Forms.NumericUpDown;$txtSale.Minimum=-1;$txtSale.Maximum=100000;$txtSale.Value=-1;$txtSale.SetBounds(115,70,170,22);$d.Controls.Add($txtSale)
    $null=Ui-Label 'Giá Vàng:' $d 15 102 95 20
    $txtPrice=New-Object Windows.Forms.NumericUpDown;$txtPrice.Minimum=0;$txtPrice.Maximum=2000000000;$txtPrice.Value=100;$txtPrice.SetBounds(115,100,170,22);$d.Controls.Add($txtPrice)
    $btnOk=Ui-NewButton 'Lưu' {
        if([string]::IsNullOrWhiteSpace($txtName.Text)){Show-Error 'Nhập tên vật phẩm.';return}
        $o=New-Object TGLL.GoldShopOffer;$o.Name=$txtName.Text.Trim();$o.TemplateId=[int]$txtTid.Value;$o.SaleId=[int]$txtSale.Value;$o.GoldPrice=[int]$txtPrice.Value
        $current=@(Get-ShopGoldCatalog)
        $current+=$o
        Save-ShopGoldCatalogFile $current
        Populate-ShopGoldGrid $current
        $d.DialogResult='OK';$d.Close()
    };Ui-Place $btnOk $d 75 142 75 28
    $btnCancel=Ui-NewButton 'Hủy' {$d.Close()};Ui-Place $btnCancel $d 160 142 75 28
    $null=$d.ShowDialog($form)
}

function Save-ShopGoldSettings {
    $script:settings.shopGoldCodes=$shopGoldCodes.Text
    $script:settings.shopGoldSilver=[int]$shopGoldSilver.Value
    $script:settings.shopGoldDelay=[int]$shopGoldDelay.Value
    $script:settings.shopGoldMaxOnline=[int]$shopGoldMax.Value
    $saved=New-Object Collections.Generic.List[string]
    foreach($row in $shopGoldGrid.Rows){
        if($row.Cells['pick'].Value -eq $true){
            $tid=[int]$row.Cells['templateId'].Value;$qty=[int]$row.Cells['quantity'].Value
            if($qty -gt 0){$saved.Add("${tid}:$qty")}
        }
    }
    $script:settings.shopGoldItems=($saved -join ';')
    if(!$UiTest){Write-JsonAtomic $script:configPath $script:settings}
}
$shopGoldCodes.Add_Leave({Save-ShopGoldSettings})
$shopGoldSilver.Add_ValueChanged({Save-ShopGoldSettings})
$shopGoldDelay.Add_ValueChanged({Save-ShopGoldSettings})
$shopGoldMax.Add_ValueChanged({Save-ShopGoldSettings})
$shopGoldGrid.Add_CellValueChanged({Save-ShopGoldSettings})

$shopGoldStart=Ui-NewButton 'Bắt đầu hàng chờ' {try{Start-ShopGold}catch{Show-Error $_.Exception.Message}};Ui-Place $shopGoldStart $shopGoldPage 14 178 130 28
$shopGoldStop=Ui-NewButton 'Dừng' {Stop-ShopGold};Ui-Place $shopGoldStop $shopGoldPage 148 178 65 28
$shopGoldReload=Ui-NewButton 'Quét từ game' {Refresh-ShopGoldOffers};Ui-Place $shopGoldReload $shopGoldPage 217 178 102 28

$shopGoldAddManual=Ui-NewButton '+ Thêm món' {Add-ShopGoldItemDialog};Ui-Place $shopGoldAddManual $shopGoldPage 14 212 90 25
$shopGoldOpenJson=Ui-NewButton 'Mở file JSON' {Open-ShopGoldJson};Ui-Place $shopGoldOpenJson $shopGoldPage 108 212 98 25
$shopGoldReloadJson=Ui-NewButton 'Nạp lại JSON' {Reload-ShopGoldJson};Ui-Place $shopGoldReloadJson $shopGoldPage 210 212 109 25

$shopGoldHint=Ui-Label 'Tích Chọn các clone từ dòng 2 trở đi. Acc dòng 1 phát Bạc, clone vào game nhận Bạc -> nhập Code -> mua Shop -> gửi đồ về acc dòng 1.' $shopGoldPage 14 243 771 32
$shopGoldReport=New-Object Windows.Forms.TextBox;$shopGoldReport.Multiline=$true;$shopGoldReport.ReadOnly=$true;$shopGoldReport.ScrollBars='Vertical';$shopGoldReport.SetBounds(14,278,771,139);$shopGoldPage.Controls.Add($shopGoldReport)

function Get-ShopGoldSelectedItems {
    $null=$shopGoldGrid.EndEdit()
    $items=New-Object Collections.Generic.List[pscustomobject]
    foreach($row in $shopGoldGrid.Rows){
        if($row.Cells['pick'].Value -eq $true){
            [int]$qty=0;if(![int]::TryParse($row.Cells['quantity'].Value,[ref]$qty) -or $qty -le 0){$qty=1}
            $items.Add([pscustomobject]@{
                saleId=[int]$row.Cells['saleId'].Value
                templateId=[int]$row.Cells['templateId'].Value
                name=[string]$row.Cells['name'].Value
                quantity=$qty
                goldPrice=[int]$row.Cells['price'].Value
            })
        }
    }
    return $items.ToArray()
}

function Open-ShopGoldAccount($id){
    $a=@($script:accounts | Where-Object {[string]$_.id -eq $id}) | Select-Object -First 1
    if(!$a){throw 'Tài khoản clone đã bị xóa.'}
    $script:distributionLaunchingId=$id;$script:mailLaunchingAccountId=$id
    try{Open-Account $a $true}finally{$script:mailLaunchingAccountId='';$script:distributionLaunchingId=''}
    $j=Get-DistributionJob $id
    if(!$j){throw 'Cần bật Đăng nhập trực tiếp để mở tài khoản clone.'}
    return $j
}

function Prepare-ShopGoldJob($j){
    if(!$j.AutoLoginEnabled){throw 'Cần mở acc bằng đăng nhập trực tiếp.'}
    if($j.MailBusy -or $j.MailEnabled -or $j.InboxBusy -or $j.ReceiveFlowActive){throw 'Acc đang thao tác; đợi thao tác cũ xong trước khi tiếp tục.'}
    [TGLL.NativeBackend]::SetGameAuto($j.Id,$false);[TGLL.NativeBackend]::SetSonCap($j.Id,$false)
    $j.MailQueueOnly=$false;$j.CloakQueueOnly=$false;$j.GiftEnabled=$false;$j.AutoItemEnabled=$false;$j.ChestEnabled=$false;$j.CloakEnabled=$false
    [TGLL.NativeBackend]::SetReceiveKey($j.Id,$false);[TGLL.NativeBackend]::SetDungeonHigh($j.Id,$false);[TGLL.NativeBackend]::SetIzanami($j.Id,$false);[TGLL.NativeBackend]::SetAutoQuest($j.Id,$false);[TGLL.NativeBackend]::SetCloseAfterKey($j.Id,$false)
}

function Start-ShopGold {
    if(@($script:beastBatches | Where-Object {$_.state -ne 'done'}).Count){throw 'Dừng hàng chờ linh thú trước.'}
    if($script:distributionRun -and $script:distributionRun.active){throw 'Dừng auto chia đồ trước.'}
    if($script:mailRun -and $script:mailRun.active){throw 'Dừng auto gom đồ trước.'}
    if($script:shopGoldRun -and $script:shopGoldRun.active){throw 'Hàng chờ mua shop vàng đang chạy.'}
    if($grid.Rows.Count -lt 2){throw 'Cần acc nhận ở dòng 1 và ít nhất một acc clone ở dòng 2.'}
    if(!$autoLoginCheck.Checked){throw 'Bật Đăng nhập trực tiếp trước.'}
    $items=@(Get-ShopGoldSelectedItems)
    if($items.Count -eq 0){throw 'Tích chọn ít nhất một vật phẩm cần mua trong bảng Shop Vàng.'}
    $silver=[int]$shopGoldSilver.Value
    $codes=[TGLL.NativeBackend]::NormalizeGiftCodes([string[]]($shopGoldCodes.Text -split '\r?\n'))
    $receiverName=Get-MailReceiverName
    if([string]::IsNullOrWhiteSpace($receiverName)){throw 'Mở acc ở dòng 1 để lấy tên nhân vật nhận đồ, rồi bấm Bắt đầu.'}
    $members=@(foreach($row in $grid.Rows){
        if($row.Index -gt 0 -and $row.Cells['pick'].Value -eq $true){
            [pscustomobject]@{
                id=[string]$row.Tag.id
                username=[string]$row.Tag.username
                character=''
                job=''
                state='waiting'
                status='Chờ đến lượt'
                at=[datetime]::UtcNow
                phaseAt=[datetime]::MinValue
                silverSent=$false
                inboxBaseline=0
                readyKey=''
                readyAt=[datetime]::MinValue
                closeOperation=$null
            }
        }
    })
    if($members.Count -eq 0){throw 'Tích chọn các clone ở cột Chọn (từ dòng 2 trở đi).'}

    $script:shopGoldRun=[pscustomobject]@{
        active=$true
        receiverId=[string]$grid.Rows[0].Tag.id
        receiverName=$receiverName
        members=$members
        cursor=0
        silver=$silver
        codes=$codes
        items=$items
        seconds=[int]$shopGoldDelay.Value
        error=''
        readyKey=''
        readyAt=[datetime]::MinValue
        lastSilverSentAt=[datetime]::MinValue
    }
    Update-ShopGoldUi
}

function Stop-ShopGold {
    if($script:shopGoldRun){
        $script:shopGoldRun.active=$false
        foreach($m in $script:shopGoldRun.members){
            if($m.state -notin @('done','waiting')){
                if($m.job){
                    try{[TGLL.NativeBackend]::StopGoldShopPurchase($m.job)}catch{}
                    try{[TGLL.NativeBackend]::ConfigureMail($m.job,"",@(),@(),0,"",0)}catch{}
                    try{[TGLL.NativeBackend]::ConfigureGiftCodes($m.job,$false,@())}catch{}
                    try{[TGLL.NativeBackend]::ConfigureInbox($m.job,$false)}catch{}
                    $j=Get-AccountJob $m.job;if($j){$j.MailQueueOnly=$false}
                }
                $m.status='Đã dừng'
            }
        }
        try{
            $source=Get-DistributionJob $script:shopGoldRun.receiverId
            if($source){[TGLL.NativeBackend]::ConfigureMail($source.Id,"",@(),@(),0,"",0);$source.MailQueueOnly=$false}
        }catch{}
    }
    Update-ShopGoldUi
}

function Sync-ShopGoldMember($run, $m, $source, $now) {
    # 1. State: opening_clone -> Wait for clone to be confirmed in game
    if($m.state -eq 'opening_clone'){
        $clone=Get-DistributionJob $m.id
        if($m.job -and (!$clone -or $m.job -ne $clone.Id)){
            throw 'Phiên clone đã đổi; dừng kiểm tra.'
        }
        if((($now-$m.phaseAt).TotalSeconds) -gt 180){
            throw 'Quá thời gian mở clone và vào game.'
        }
        if(!(Test-DistributionReady $clone)){
            Reset-DistributionConfirmation $m
            if($clone -and $clone.State -in @('failed','closed','cancelled')){
                throw ('Mở clone thất bại: ' + $clone.State)
            }
            $m.status='Đợi clone vào game...'
            return
        }
        if(!(Test-DistributionConfirmed $m $clone)){
            $m.status='Xác nhận clone đã vào game ổn định...'
            return
        }
        if($clone.CharacterName -eq $source.CharacterName){
            throw 'Clone trùng tên với acc chính; kiểm tra lại danh sách tài khoản.'
        }
        $m.job=$clone.Id
        $m.character=$clone.CharacterName
        Prepare-ShopGoldJob $clone

        # Clone online -> Nhập Gift Code trước (nếu có)
        if($run.codes.Count -gt 0){
            [TGLL.NativeBackend]::ConfigureGiftCodes($clone.Id,$true,$run.codes)
            $m.state='waiting_gift';$m.phaseAt=$now
            $m.status='Đang nhập Gift Code nhận Vàng...'
        }elseif($run.silver -gt 0){
            $m.state='awaiting_silver';$m.phaseAt=$now
            $m.status='Đợi lượt acc chính cấp Bạc...'
        }else{
            $m.state='buying_shop';$m.phaseAt=$now;$m.status='Bắt đầu mua Shop Vàng'
        }
        return
    }

    # 2. State: waiting_gift -> Clone enters gift codes to get gold FIRST
    if($m.state -eq 'waiting_gift'){
        $clone=Get-DistributionJob $m.id
        if(!$clone -or !(Test-DistributionSessionLive $clone)){
            throw 'Game clone bị thoát trong lúc nhập Gift Code.'
        }
        $timeoutSeconds=[Math]::Max(60,($run.codes.Count * 15))
        $elapsed=(($now-$m.phaseAt).TotalSeconds)
        if($clone.GiftStatus -match 'Đã xử lý hết' -or (!$clone.GiftEnabled -and $elapsed -ge 3) -or ($elapsed -gt $timeoutSeconds)){
            [TGLL.NativeBackend]::ConfigureGiftCodes($clone.Id,$false,$run.codes)
            # Sau khi nhập code xong -> Chuyển sang gửi & nhận Bạc (nếu có)
            if($run.silver -gt 0){
                $m.state='awaiting_silver';$m.phaseAt=$now
                $m.status='Đã xong Gift Code; đợi lượt acc chính cấp Bạc...'
            }else{
                $m.state='buying_shop';$m.phaseAt=$now;$m.status='Đã xong Gift Code; bắt đầu mua Shop Vàng'
            }
        }else{
            $m.status="Đang nhập Gift Code ($($clone.GiftRequests)/$($run.codes.Count)): $($clone.GiftStatus)"
        }
        return
    }

    # 3. State: awaiting_silver -> Wait for turn to receive silver from main account (only 1 clone at a time)
    if($m.state -eq 'awaiting_silver'){
        $clone=Get-DistributionJob $m.id
        if(!$clone -or !(Test-DistributionSessionLive $clone)){
            throw 'Game clone bị thoát trong lúc chờ nhận Bạc.'
        }
        $otherSending=@($run.members | Where-Object { $_ -ne $m -and $_.state -eq 'sending_silver' }).Count
        if($otherSending -eq 0 -and !$source.MailBusy -and !$source.MailEnabled){
            if($run.lastSilverSentAt -and ($run.lastSilverSentAt -ne [datetime]::MinValue)){
                $gap=($now - $run.lastSilverSentAt).TotalSeconds
                if($gap -lt $run.seconds){
                    $m.status="Đợi giãn cách gửi Bạc từ acc chính ($([Math]::Ceiling($run.seconds - $gap))s)..."
                    return
                }
            }
            if($source.InboxEnabled){
                Set-DistributionInbox $source $false
            }
            Set-DistributionInbox $clone $true
            $m.inboxBaseline=[long]$clone.InboxReceived
            [TGLL.NativeBackend]::ConfigureMail($source.Id,$clone.CharacterName,[int[]]@(),[int[]]@(),($run.seconds*1000),"",$run.silver)
            $m.state='sending_silver';$m.phaseAt=$now
            $m.status="Acc chính đang gửi $($run.silver) Bạc tới $($clone.CharacterName)..."
        }else{
            $m.status="Đợi lượt gửi Bạc từ acc chính (đang cấp cho clone khác)..."
        }
        return
    }

    # 4. State: sending_silver -> Wait for main account to finish mailing silver to clone
    if($m.state -eq 'sending_silver'){
        $clone=Get-DistributionJob $m.id
        if(!$clone -or !(Test-DistributionSessionLive $clone)){
            throw 'Game clone bị thoát trong lúc nhận Bạc.'
        }
        if(!$source.MailDone){
            if(!$source.MailEnabled -and !$source.MailBusy){
                throw ('Gửi Bạc từ acc chính thất bại: ' + $source.MailStatus)
            }
            if((($now-$m.phaseAt).TotalSeconds) -gt 90){
                throw 'Quá thời gian gửi Bạc từ acc chính sang clone.'
            }
            return
        }
        if($source.MailBusy){return}
        $m.silverSent=$true
        $m.state='receiving_silver';$m.phaseAt=$now
        $m.status="Đợi clone $($clone.CharacterName) nhận Bạc..."
        $source.MailDone=$false
        $run.lastSilverSentAt=$now
        return
    }

    # 5. State: receiving_silver -> Wait for clone mailbox to receive silver
    if($m.state -eq 'receiving_silver'){
        $clone=Get-DistributionJob $m.id
        if(!$clone -or !(Test-DistributionSessionLive $clone)){
            throw 'Game clone bị thoát trong lúc nhận Bạc.'
        }
        if(!$clone.InboxEnabled -and !$clone.InboxBusy){
            throw ('Nhận thư trên clone thất bại: ' + $clone.InboxStatus)
        }
        if(([long]$clone.InboxReceived -gt [long]$m.inboxBaseline) -or (($now-$m.phaseAt).TotalSeconds -gt 20)){
            Set-DistributionInbox $clone $false
            $m.state='buying_shop';$m.phaseAt=$now;$m.status='Đã nhận Bạc; bắt đầu mua Shop Vàng'
            return
        }
        return
    }

    # 6. State: buying_shop -> Start gold shop purchase on clone
    if($m.state -eq 'buying_shop'){
        $clone=Get-DistributionJob $m.id
        if(!$clone -or !(Test-DistributionSessionLive $clone)){
            throw 'Game clone bị thoát trước khi mua Shop Vàng.'
        }
        $saleIds=[int[]]@($run.items | ForEach-Object {$_.saleId})
        $templateIds=[int[]]@($run.items | ForEach-Object {$_.templateId})
        $quantities=[int[]]@($run.items | ForEach-Object {$_.quantity})
        [TGLL.NativeBackend]::StartGoldShopPurchase($clone.Id,$saleIds,$templateIds,$quantities)
        $m.state='purchasing';$m.phaseAt=$now;$m.status='Đang mua vật phẩm trong Shop Vàng...'
        return
    }

    # 7. State: purchasing -> Wait for purchase to finish
    if($m.state -eq 'purchasing'){
        $clone=Get-DistributionJob $m.id
        if(!$clone -or !(Test-DistributionSessionLive $clone)){
            throw 'Game clone bị thoát trong lúc mua Shop Vàng.'
        }
        if(!$clone.GoldShopEnabled){
            if($clone.GoldShopBoughtTotal -gt 0){
                $m.state='sending_mail';$m.phaseAt=$now;$m.status="Đã mua đủ $($clone.GoldShopBoughtTotal) món; đang gửi thư về $($run.receiverName)"
            }else{
                throw ('Không mua được đồ Shop Vàng: ' + $clone.GoldShopStatus)
            }
        }else{
            $m.status=$clone.GoldShopStatus
            if((($now-$m.phaseAt).TotalSeconds) -gt 60){
                throw 'Quá thời gian mua Shop Vàng; dừng kiểm tra.'
            }
        }
        return
    }

    # 8. State: sending_mail -> Clone mails purchased items back to main account
    if($m.state -eq 'sending_mail'){
        $clone=Get-DistributionJob $m.id
        if(!$clone -or !(Test-DistributionSessionLive $clone)){
            throw 'Game clone bị thoát trước khi gửi thư.'
        }
        $templateIds=[int[]]@($run.items | ForEach-Object {$_.templateId})
        $quantities=[int[]]@($run.items | ForEach-Object {$_.quantity})
        [TGLL.NativeBackend]::ConfigureMail($clone.Id,$run.receiverName,$templateIds,$quantities,($run.seconds*1000),"",0)
        $m.state='mailing';$m.phaseAt=$now;$m.status="Đang gửi thư về $($run.receiverName)..."
        return
    }

    # 9. State: mailing -> Wait for mail from clone to finish
    if($m.state -eq 'mailing'){
        $clone=Get-DistributionJob $m.id
        if(!$clone -or !(Test-DistributionSessionLive $clone)){
            throw 'Game clone bị thoát trong lúc gửi thư.'
        }
        if($clone.MailDone -and !$clone.MailBusy){
            $m.closeOperation=[TGLL.NativeBackend]::BeginCloseBatch([string[]]@($clone.Id))
            $m.state='closing';$m.phaseAt=$now;$m.status='Đã gửi thư xong; đang tắt game clone'
        }else{
            if(!$clone.MailEnabled -and !$clone.MailBusy){
                throw ('Gửi thư về acc chính thất bại: ' + $clone.MailStatus)
            }
            if((($now-$m.phaseAt).TotalSeconds) -gt 90){
                throw 'Quá thời gian gửi thư về acc chính.'
            }
        }
        return
    }

    # 10. State: closing -> Wait for clone process to exit
    if($m.state -eq 'closing'){
        if($m.closeOperation -and !$m.closeOperation.Completed){return}
        $cloneJob=Get-AccountJob $m.job
        if($null -ne $cloneJob -and $cloneJob.ProcessId -gt 0 -and [TGLL.ManagedGames]::IsRunning([int]$cloneJob.ProcessId)){
            if((($now-$m.phaseAt).TotalSeconds) -le 15){
                $m.status='Đợi game clone tắt hẳn';return
            }
        }
        $m.state='done';$m.status='Hoàn tất'
        return
    }
}

function Sync-ShopGold {
    $run=$script:shopGoldRun;if(!$run -or !$run.active){return}
    $now=[datetime]::UtcNow

    # 1. Kiểm tra acc chính (người nhận ở dòng 1)
    $source=Get-DistributionJob $run.receiverId
    if(!$source -or $source.ProcessId -le 0 -or ![TGLL.ManagedGames]::IsRunning([int]$source.ProcessId)){
        $run.error='Acc nhận ở dòng 1 chưa vào game hoặc đã bị thoát.'
        $run.active=$false
        Update-ShopGoldUi
        return
    }
    if(!(Test-DistributionReady $source)){
        Reset-DistributionConfirmation $run
        Update-ShopGoldUi
        return
    }
    if(!(Test-DistributionConfirmed $run $source)){
        Update-ShopGoldUi
        return
    }
    if(![string]::IsNullOrWhiteSpace($source.CharacterName)){
        $run.receiverName=$source.CharacterName
    }

    # Tự động bật nhận thư trên acc chính để thu hoạch đồ khi acc chính rảnh (không bận gửi Bạc)
    $isSendingSilver=@($run.members | Where-Object {$_.state -eq 'sending_silver'}).Count -gt 0
    $hasItemsToHarvest=@($run.members | Where-Object {$_.state -in @('mailing','closing','done')}).Count -gt 0
    if(!$isSendingSilver -and !$source.MailBusy -and !$source.MailEnabled -and $hasItemsToHarvest){
        if(!$source.InboxEnabled){
            Set-DistributionInbox $source $true
        }
    }

    # 2. Xử lý đồng bộ từng member đang chạy
    foreach($m in $run.members){
        if($m.state -notin @('waiting','done','blocked')){
            try{
                Sync-ShopGoldMember $run $m $source $now
            }catch{
                $m.state='blocked'
                $m.status=$_.Exception.Message
            }
        }
    }

    # 3. Cập nhật con trỏ số lượng hoàn tất
    $run.cursor=@($run.members | Where-Object {$_.state -eq 'done'}).Count

    # 4. Kiểm tra hoàn tất toàn bộ hàng chờ
    $remaining=@($run.members | Where-Object {$_.state -notin @('done','blocked')}).Count
    if($remaining -eq 0){
        $run.active=$false
        $status.Text='Hoàn tất toàn bộ hàng chờ mua Shop Vàng cho các clone.'
        Update-ShopGoldUi
        return
    }

    # 5. Khởi chạy các clone mới trong danh sách chờ theo giới hạn maxOnline
    # Tu dong thu lai sau 30 giay cho cac clone bi blocked do timeout / loi tam thoi
    foreach($m in $run.members){
        if($m.state -eq 'blocked' -and $m.phaseAt){
            if((($now - $m.phaseAt).TotalSeconds) -ge 30){
                $m.state='waiting'
                $m.status='Thử lại sau khi bị lỗi/timeout...'
                $m.phaseAt=$now
            }
        }
    }

    # Chi tinh cac clone dang mo hoac dang thao tac, khong de clone bi loi chiem slot lam dung hang cho
    $occupied=@($run.members | Where-Object {$_.state -notin @('waiting','done','blocked')}).Count
    $limit=if($shopGoldMax){[int]$shopGoldMax.Value}else{2}
    $budget=[Math]::Max(0, [int]$limit - $occupied)
    if($launchConcurrency -and [int]$launchConcurrency.Value -gt 0){
        $budget=[Math]::Min($budget, [int]$launchConcurrency.Value)
    }

    foreach($m in $run.members){
        if($occupied -ge $limit -or $budget -le 0){break}
        if($m.state -ne 'waiting'){continue}
        $m.state='opening_clone';$m.at=$now;$m.phaseAt=$now
        $m.status='Đang mở clone; đợi vào game...'
        $occupied++
        $budget--
        try{
            $existing=Get-DistributionJob $m.id
            if(!$existing -or $existing.CloseRequested -or $existing.State -in @('closed','failed','cancelled') -or !(Test-DistributionSessionLive $existing)){
                $null=Open-ShopGoldAccount $m.id
            }
        }catch{
            $m.state='blocked'
            $m.status=$_.Exception.Message
        }
    }

    Update-ShopGoldUi
}

function Update-ShopGoldUi {
    $run=$script:shopGoldRun;$active=$run -and $run.active
    $shopGoldStart.Enabled=!$active;$shopGoldSilver.Enabled=!$active;$shopGoldDelay.Enabled=!$active;$shopGoldCodes.Enabled=!$active;$shopGoldGrid.ReadOnly=$active
    $shopGoldReload.Enabled=!$active;$shopGoldAddManual.Enabled=!$active;$shopGoldReloadJson.Enabled=!$active
    if($shopGoldMax){$shopGoldMax.Enabled=!$active}
    $recName=Get-MailReceiverName
    if($grid.Rows.Count -gt 0){$shopGoldReceiver.Text='Acc nhận (dòng 1): '+$(if($recName){$recName}else{'Chưa có tên nhân vật; mở acc dòng 1 trước'})}
    if($run){
        $doneCount=@($run.members | Where-Object {$_.state -eq 'done'}).Count
        $busyCount=@($run.members | Where-Object {$_.state -notin @('waiting','done')}).Count
        $waitCount=@($run.members | Where-Object {$_.state -eq 'waiting'}).Count
        $limit=if($shopGoldMax){[int]$shopGoldMax.Value}else{2}
        $summary="Đang chạy: $busyCount/$limit • Chờ: $waitCount • Hoàn tất: $doneCount/$($run.members.Count)"
        if($run.error){$summary += " • Lỗi: $($run.error)"}
        $lines=@($summary)
        $lines+=@($run.members | ForEach-Object {$_.username+': '+$_.status})
        $text=$lines -join "`r`n"
        if($shopGoldReport.Text -ne $text){$shopGoldReport.Text=$text}
    }
}
