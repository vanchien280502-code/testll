$script:newbieBatches = @()
$script:newbieQueuePaused = $false
$script:newbieWaiting = @{}
$script:newbieAccountIndex = @{}

function Get-NewbieQueuedMember($id) {
    if ($script:newbieWaiting.ContainsKey([string]$id)) { return $script:newbieWaiting[[string]$id] }
    foreach ($b in $script:newbieBatches) {
        if ($b.state -notin @('done', 'cancelled')) {
            foreach ($m in $b.members) {
                if ($m.id -eq [string]$id) { return $m }
            }
        }
    }
    return $null
}

function Get-NewbieMemberJob($member) {
    if ($member.job) { return Get-AccountJob $member.job }
    return $null
}

function Start-NewbieMember($member) {
    $a = $script:newbieAccountIndex[$member.id]
    if (!$a) { throw 'Tai khoan trong hang cho da bi xoa.' }
    if (Test-AccountOpenBlocked $a) {
        if (!$script:accountJobs.ContainsKey($member.id)) { throw 'Game mo thu cong chua ho tro tai khoan tu dong.' }
        $member.job = $script:accountJobs[$member.id]
        $j = Get-AccountJob $member.job
        $j.QuestQueueOnly = $false
        [TGLL.NativeBackend]::SetReceiveKey($member.job, $false)
        [TGLL.NativeBackend]::SetDungeonHigh($member.job, $false)
        [TGLL.NativeBackend]::SetCloseAfterKey($member.job, $false)
        [TGLL.NativeBackend]::SetIzanami($member.job, $false)
        [TGLL.NativeBackend]::SetAutoQuest($member.job, $false)
        $member.status = 'Dang ket noi lai game...'
        return
    }
    $script:newbieLaunchingAccountId = $member.id
    try { Open-Account $a } finally { $script:newbieLaunchingAccountId = '' }
    if (!$script:accountJobs.ContainsKey($member.id)) { throw 'Can bat dang nhap truc tiep de chay hang cho.' }
    $member.job = $script:accountJobs[$member.id]
    $member.status = 'Dang mo game...'
}

function Drive-NewbieMember($member, $j) {
    if ($null -eq $j -or $j.CloseRequested -or $j.CancelRequested -or $j.State -in @('failed', 'closed', 'cancelled')) {
        return
    }

    $lvl = [int]$j.CharacterLevel
    $mTask = [int]$j.MainTaskId
    $mStep = [int]$j.MainStepId
    $mReq = [int]$j.MainRequire
    $now = [DateTime]::UtcNow

    # Dam bao ReceiveFlowActive khong can tro Auto Item hoac Farm (chi khi chua den Map 86 / Truong Konoha)
    if ($j.FarmPositionMap -ne 86 -and $mTask -lt 8) {
        try { [TGLL.NativeBackend]::ResetReceiveFlow($j.Id) } catch {}
    }

    # 1. Kiem tra neu da den Truong Konoha (Map 86) hoac Task >= 8 o Map 86: Chay chu trinh Kinkaku & Gui thu
    if ($j.FarmPositionMap -eq 86 -or $mTask -ge 9 -or ($mTask -eq 8 -and $mStep -ge 2 -and $j.FarmPositionMap -eq 86)) {
        if ($j.FarmEnabled) {
            try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
        }
        try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $false) } catch {}
        try { [TGLL.NativeBackend]::ConfigureAutoItem($j.Id, $false, 0, 5000) } catch {}

        # Khoi tao bien tracking Kinkaku
        if (!$member.PSObject.Properties['kinkakuStep']) {
            $member | Add-Member NoteProperty kinkakuStep 'exchanging' -Force
            $member | Add-Member NoteProperty kinkakuPhase 'idle' -Force
            $member | Add-Member NoteProperty kinkakuPhaseAt ([DateTime]::MinValue) -Force
            $member | Add-Member NoteProperty kinkakuSilverBefore -1 -Force
            $member | Add-Member NoteProperty kinkakuTargetSilver 64350000 -Force
            $member | Add-Member NoteProperty kinkakuExchangeCount 0 -Force
            $member | Add-Member NoteProperty kinkakuLastSilver -1 -Force
            $member | Add-Member NoteProperty kinkakuLastActionAt ([DateTime]::MinValue) -Force
            $member | Add-Member NoteProperty kinkakuStallCount 0 -Force
            $member | Add-Member NoteProperty kinkakuNoChangeAttempts 0 -Force
            $member | Add-Member NoteProperty mailSent $false -Force
            $member | Add-Member NoteProperty mailStartedAt ([DateTime]::MinValue) -Force
            $member | Add-Member NoteProperty silverSentAmount 0 -Force
        }

        $now = [DateTime]::UtcNow

        # Giai doan 1: Doi Bac tai NPC Kinkaku (State Machine tuan tu, khong bi xung dot menu)
        if ($member.kinkakuStep -eq 'exchanging') {
            $curSilver = [TGLL.NativeBackend]::GetPlayerSilver($j.Id)

            # Kiem tra neu da dat du moc 64.350.000 bac
            if ($curSilver -ge [long]$member.kinkakuTargetSilver) {
                try { [TGLL.NativeBackend]::ResetReceiveFlow($j.Id) } catch {}
                $member.kinkakuStep = 'mailing'
                $member.status = 'NV Tan thu: Da doi du ' + ($curSilver / 1000000).ToString('F1') + 'tr bac! Chuyen sang gui thu...'
                return
            }

            if (!$member.PSObject.Properties['kinkakuPhase']) {
                $member | Add-Member NoteProperty kinkakuPhase 'idle' -Force
                $member | Add-Member NoteProperty kinkakuPhaseAt $now -Force
                $member | Add-Member NoteProperty kinkakuSilverBefore $curSilver -Force
            }

            if (($now - $member.kinkakuLastActionAt).TotalMilliseconds -ge 200) {
                $member.kinkakuLastActionAt = $now

                switch ($member.kinkakuPhase) {
                    'idle' {
                        # Cho it nhat 400ms giua cac luot doi de game dong menu cu on dinh
                        if (($now - $member.kinkakuPhaseAt).TotalMilliseconds -lt 400) {
                            return
                        }
                        $opts = @($j.MenuOptions)
                        $isMenu1 = ($opts.Count -ge 5 -and ($opts | Where-Object { $_ -match 'tinh thạch' }))
                        if ($isMenu1) {
                            try { [TGLL.NativeBackend]::SelectDynamicMenuIndex($j.Id, 0) } catch {}
                            $member.kinkakuPhase = 'waiting_menu2'
                            $member.kinkakuPhaseAt = $now
                            $member.status = 'NV Tan thu: Chon muc Doi Bac tai Kinkaku (Menu 1)...'
                            return
                        }
                        try { [TGLL.NativeBackend]::TalkToKinkaku($j.Id) } catch {}
                        $member.kinkakuPhase = 'waiting_menu1'
                        $member.kinkakuPhaseAt = $now
                        $member.status = 'NV Tan thu: Dang mo Menu Kinkaku (' + ($curSilver / 1000000).ToString('F1') + 'tr bac)...'
                        return
                    }

                    'waiting_menu1' {
                        $opts = @($j.MenuOptions)
                        $isMenu1 = ($opts.Count -ge 5 -and ($opts | Where-Object { $_ -match 'tinh thạch' }))
                        if ($isMenu1) {
                            try { [TGLL.NativeBackend]::SelectDynamicMenuIndex($j.Id, 0) } catch {}
                            $member.kinkakuPhase = 'waiting_menu2'
                            $member.kinkakuPhaseAt = $now
                            $member.status = 'NV Tan thu: Chon muc Doi Bac tai Kinkaku (Menu 1)...'
                            return
                        }

                        # Neu cho qua 3 giay ma Menu 1 chua mo: reset va thu lai
                        if (($now - $member.kinkakuPhaseAt).TotalSeconds -ge 3.0) {
                            try { [TGLL.NativeBackend]::ResetReceiveFlow($j.Id) } catch {}
                            $member.kinkakuStallCount++
                            if ($member.kinkakuStallCount -ge 6) {
                                $member.kinkakuStep = 'mailing'
                                $member.status = 'NV Tan thu: Khong the mo Menu Kinkaku sau nhieu lan thu. Chuyen sang gui thu...'
                                return
                            }
                            $member.kinkakuPhase = 'idle'
                            $member.kinkakuPhaseAt = $now
                            return
                        }
                        return
                    }

                    'waiting_menu2' {
                        # TUYET DOI KHONG GOI TalkToKinkaku VA KHONG BAM ESC O DAY!
                        $opts = @($j.MenuOptions)
                        $isMenu2 = ($opts.Count -eq 4 -and ($opts | Where-Object { $_ -match '6\.5tr' }) -and !($opts | Where-Object { $_ -match 'tinh thạch' }))

                        if ($isMenu2) {
                            # Tim moc 2000 Vang - 6.5tr bac (uu tien cao nhat)
                            $targetIndex = -1
                            for ($idx = 0; $idx -lt $opts.Count; $idx++) {
                                if ($opts[$idx] -match '2000.*6\.5tr' -or $opts[$idx] -match '6\.5tr') {
                                    $targetIndex = $idx
                                    break
                                }
                            }
                            # Neu khong co moc 2000 vang, tim moc 200 vang - 600k bac
                            if ($targetIndex -lt 0) {
                                for ($idx = 0; $idx -lt $opts.Count; $idx++) {
                                    if ($opts[$idx] -match '200.*600k' -or $opts[$idx] -match '600k') {
                                        $targetIndex = $idx
                                        break
                                    }
                                }
                            }

                            if ($targetIndex -ge 0) {
                                $member.kinkakuSilverBefore = $curSilver
                                try { [TGLL.NativeBackend]::SelectDynamicMenuIndex($j.Id, $targetIndex) } catch {}
                                $member.kinkakuPhase = 'waiting_result'
                                $member.kinkakuPhaseAt = $now
                                $member.status = 'NV Tan thu: Da chon moc doi vang, cho server cong bac (Lan ' + ($member.kinkakuExchangeCount + 1) + ')...'
                                return
                            }
                        }

                        # Cho toi da 3 giay cho Menu 2 xuat hien tu server
                        if (($now - $member.kinkakuPhaseAt).TotalSeconds -ge 3.0) {
                            try { [TGLL.NativeBackend]::ResetReceiveFlow($j.Id) } catch {}
                            $member.kinkakuPhase = 'idle'
                            $member.kinkakuPhaseAt = $now
                            return
                        }
                        return
                    }

                    'waiting_result' {
                        # Kiem tra xem bac da tang chua
                        if ($curSilver -gt $member.kinkakuSilverBefore) {
                            $member.kinkakuExchangeCount++
                            $member.kinkakuLastSilver = $curSilver
                            $member.kinkakuNoChangeAttempts = 0
                            $member.kinkakuStallCount = 0

                            # Kiem tra neu da dat du moc yeu cau
                            if ($curSilver -ge [long]$member.kinkakuTargetSilver) {
                                try { [TGLL.NativeBackend]::ResetReceiveFlow($j.Id) } catch {}
                                $member.kinkakuStep = 'mailing'
                                $member.status = 'NV Tan thu: Da doi du ' + ($curSilver / 1000000).ToString('F1') + 'tr bac! Chuyen sang gui thu...'
                                return
                            }

                            # Chua du bac -> tiep tuc doi lan tiep theo
                            $member.kinkakuPhase = 'idle'
                            $member.kinkakuPhaseAt = $now
                            $member.status = 'NV Tan thu: Doi thanh cong lan ' + $member.kinkakuExchangeCount + ' (' + ($curSilver / 1000000).ToString('F1') + 'tr bac)...'
                            return
                        }

                        # Cho server phan hoi trong vong toi da 3.0 giay
                        if (($now - $member.kinkakuPhaseAt).TotalSeconds -ge 3.0) {
                            $member.kinkakuNoChangeAttempts++
                            try { [TGLL.NativeBackend]::ResetReceiveFlow($j.Id) } catch {}

                            # Chi khi da gui 2 lan vao Menu 2 ma bac khong tang -> Chac chan da het vang trong tui!
                            if ($member.kinkakuNoChangeAttempts -ge 2) {
                                $member.kinkakuStep = 'mailing'
                                $member.status = 'NV Tan thu: Da het vang trong tui (' + ($curSilver / 1000000).ToString('F1') + 'tr bac). Chuyen sang gui thu...'
                                return
                            }

                            # Thu lai lan nua de dam bao khong bo sot do lag mang
                            $member.kinkakuPhase = 'idle'
                            $member.kinkakuPhaseAt = $now
                            return
                        }
                        return
                    }
                }
            }
            return
        }

        # Giai doan 3: Gui Thu Bac ve acc chinh (dong 1)
        if ($member.kinkakuStep -eq 'mailing') {
            $receiver = Get-MailReceiverName
            if ([string]::IsNullOrWhiteSpace($receiver)) {
                $member.status = 'NV Tan thu: Cho lay ten nhan vat acc dong 1...'
                return
            }

            if (!$member.mailSent) {
                $totalSilver = [TGLL.NativeBackend]::GetPlayerSilver($j.Id)
                if ($totalSilver -le 10) {
                    $member.kinkakuStep = 'done'
                    $member.status = 'Hoan thanh: Khong co du bac de gui'
                    return
                }

                # Tinh so bac toi da co the gui sao cho: silverToSend + 1% phi + 10 <= totalSilver
                $maxSendable = [Math]::Floor(($totalSilver - 10) / 1.01001)
                $silverToSend = [long]$maxSendable
                if ($silverToSend -gt [long]$member.kinkakuTargetSilver) {
                    $targetNeeded = [long]$member.kinkakuTargetSilver + [long]([long]$member.kinkakuTargetSilver * 0.01) + 10
                    if ($totalSilver -ge $targetNeeded) {
                        $silverToSend = [long]$member.kinkakuTargetSilver
                    }
                }

                if ($silverToSend -le 0) {
                    $member.kinkakuStep = 'done'
                    $member.status = 'Hoan thanh: Khong co bac de gui'
                    return
                }

                try {
                    $member.silverSentAmount = $silverToSend
                    [TGLL.NativeBackend]::ConfigureMail($j.Id, $receiver, [int[]]@(), [int[]]@(), 5000, "", [int]$silverToSend)
                    $member.mailSent = $true
                    $member.mailStartedAt = $now
                    $member.status = 'NV Tan thu: Dang gui ' + ($silverToSend / 1000000).ToString('F1') + 'tr bac ve ' + $receiver + '...'
                } catch {
                    $member.status = 'Loi gui thu: ' + $_.Exception.Message
                }
                return
            }

            # Cho thu gui xong
            if ($j.MailDone -and !$j.MailBusy) {
                $member.kinkakuStep = 'done'
                $amtStr = if ($member.silverSentAmount -gt 0) { ($member.silverSentAmount / 1000000).ToString('F1') + 'tr bac' } else { 'bac' }
                $member.status = 'Hoan thanh: Da gui ' + $amtStr + ' ve ' + $receiver
                return
            }
            else {
                if (!$j.MailEnabled -and !$j.MailBusy) {
                    $member.kinkakuStep = 'done'
                    $amtStr = if ($member.silverSentAmount -gt 0) { ($member.silverSentAmount / 1000000).ToString('F1') + 'tr bac' } else { 'bac' }
                    $member.status = 'Hoan thanh: Da gui ' + $amtStr + ' ve ' + $receiver
                    return
                }
                if (($now - $member.mailStartedAt).TotalSeconds -gt 90) {
                    $member.kinkakuStep = 'done'
                    $member.status = 'Hoan thanh: Qua thoi gian gui thu, tiep tuc clone sau'
                    return
                }
                $member.status = 'NV Tan thu: Dang gui thu (' + $j.MailStatus + ')...'
            }
            return
        }

        # Giai doan 4: Hoan tat 100%
        if ($member.kinkakuStep -eq 'done') {
            if ($member.status -notlike 'Hoan thanh*') {
                $member.status = 'Hoan thanh: Da hoan tat va gui bac ve acc chinh'
            }
            return
        }
        return
    }
    
    # 2. Cho nhan vat vao game va san sang
    if (!$j.GameSettingsReady -or !$j.TrainReady) {
        $member.status = 'Dang doi vao game...'
        return
    }

    # 2.0. Kiem tra nhan vat bi chet (do quai/boss danh chet): tu dong xac nhan hoi sinh / ve lang va reset dan duong
    $isDead = ($j.Vitals -and $j.Vitals.HP -le 0)
    if ($isDead) {
        try { [TGLL.NativeBackend]::CheckAndDismissDeathDialog($j.Id) } catch {}
        try { [TGLL.NativeBackend]::SendKeyPress($j.Id, 13) } catch {} # VK_RETURN (Enter)
        try { [TGLL.NativeBackend]::SendKeyPress($j.Id, 74) } catch {} # VK_J (Tan cong/Hoi sinh)
        try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $false) } catch {}
        if ($j.FarmEnabled) {
            try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
        }
        $member.currentTargetKey = ''
        $member.status = 'NV Tan thu: Nhan vat bi thuong/chet, dang xac nhan hoi sinh de lam tiep...'
        return
    }

    # 2.1. Giai doan Chuan bi truoc khi bat dau lam NV tan thu:
    #      - Nhap Gift Code "gopservers1s10"
    #      - Nhan tat ca thu trong Hop thu (Inbox)
    #      - Mua Shop Vang khoa: 1 Nhan sam thuong (ID 159), 1 Bua bao ho (ID 404)
    #      - Su dung 2 vat pham vua mua
    #      - Bat "Dung vang hoi sinh" trong Cai dat Tu dong
    if (!$member.PSObject.Properties['prepDone']) {
        $member | Add-Member NoteProperty prepStep 'gift' -Force
        $member | Add-Member NoteProperty prepStartedAt $now -Force
        $member | Add-Member NoteProperty prepActionAt ([DateTime]::MinValue) -Force
        $member | Add-Member NoteProperty prepItemsUsed @{} -Force
        $member | Add-Member NoteProperty prepDone $false -Force
    }

    if (!$member.prepDone) {
        if ($j.FarmEnabled) {
            try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
        }
        try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $false) } catch {}

        switch ($member.prepStep) {
            'gift' {
                if (!$j.GiftEnabled -and !$j.GiftWaiting -and !$j.GiftResultWaiting -and $j.GiftRequests -eq 0) {
                    try { [TGLL.NativeBackend]::ConfigureGiftCodes($j.Id, $true, @('gopservers1s10')) } catch {}
                    $member.prepActionAt = $now
                    $member.status = 'NV Tan thu: Dang gui Gift Code gopservers1s10...'
                    return
                }

                $giftFinished = ($j.GiftStatus -match 'Đã có phản hồi|Đã xử lý hết|Không gửi được') -or 
                                ($j.GiftRequests -gt 0 -and !$j.GiftWaiting -and !$j.GiftResultWaiting) -or 
                                (($now - $member.prepActionAt).TotalSeconds -ge 8)

                if ($giftFinished) {
                    try { [TGLL.NativeBackend]::ConfigureGiftCodes($j.Id, $false, @()) } catch {}
                    $member.prepStep = 'inbox'
                    $member.prepActionAt = $now
                    $member.status = 'NV Tan thu: Da nhap Gift Code, dang kiem tra hop thu...'
                    return
                }
                $member.status = 'NV Tan thu: Dang gui Gift Code (' + $j.GiftStatus + ')...'
                return
            }

            'inbox' {
                if (!$j.InboxEnabled) {
                    try { [TGLL.NativeBackend]::ConfigureInbox($j.Id, $true) } catch {}
                    $member.prepActionAt = $now
                    $member.status = 'NV Tan thu: Dang mo hop thu de nhan qua...'
                    return
                }

                $inboxFinished = ($j.InboxObservedAt -ne [DateTime]::MinValue -and $j.InboxPendingAttachments -eq 0 -and !$j.InboxBusy) -or 
                                 (($now - $member.prepActionAt).TotalSeconds -ge 8)

                if ($inboxFinished) {
                    try { [TGLL.NativeBackend]::ConfigureInbox($j.Id, $false) } catch {}
                    $member.prepStep = 'buy_shop'
                    $member.prepActionAt = $now
                    $member.status = 'NV Tan thu: Da nhan thu xong. Chuyen sang mua Shop Vang Khoa...'
                    return
                }
                $member.status = 'NV Tan thu: Dang nhan thu (' + $j.InboxStatus + ')...'
                return
            }

            'buy_shop' {
                $buyId159 = [TGLL.NativeBackend]::GetShopBuyId($j.Id, 159, $true)
                if ($buyId159 -le 0) { $buyId159 = 0 }
                $buyId404 = [TGLL.NativeBackend]::GetShopBuyId($j.Id, 404, $true)
                if ($buyId404 -le 0) { $buyId404 = 0 }

                if (!$j.GoldShopEnabled) {
                    try { [TGLL.NativeBackend]::ScanInventory($j.Id) } catch {}
                    $has159 = $j.Inventory -and $j.Inventory.Items -and ($j.Inventory.Items | Where-Object { $_.TemplateId -eq 159 })
                    $has404 = $j.Inventory -and $j.Inventory.Items -and ($j.Inventory.Items | Where-Object { $_.TemplateId -eq 404 })
                    if ($has159 -and $has404) {
                        $member.prepStep = 'use_items'
                        $member.prepActionAt = $now
                        $member.status = 'NV Tan thu: Da co Nhan sam & Bua bao ho, chuyen sang su dung...'
                        return
                    }

                    $sIds = @()
                    $tIds = @()
                    $qList = @()
                    if (!$has159) { $sIds += $buyId159; $tIds += 159; $qList += 1 }
                    if (!$has404) { $sIds += $buyId404; $tIds += 404; $qList += 1 }

                    try {
                        [TGLL.NativeBackend]::StartGoldShopPurchase($j.Id, [int[]]$sIds, [int[]]$tIds, [int[]]$qList, $true)
                        $member.prepActionAt = $now
                        $member.status = 'NV Tan thu: Dang mua 1 Nhan sam thuong & 1 Bua bao ho tu Shop Vang Khoa...'
                        return
                    } catch {
                        $member.prepStep = 'use_items'
                        $member.prepActionAt = $now
                        return
                    }
                }

                $shopFinished = (!$j.GoldShopEnabled) -or (($now - $member.prepActionAt).TotalSeconds -ge 15)
                if ($shopFinished) {
                    try { [TGLL.NativeBackend]::StopGoldShopPurchase($j.Id) } catch {}
                    $member.prepStep = 'use_items'
                    $member.prepActionAt = $now
                    $member.status = 'NV Tan thu: Da mua Shop Vang Khoa xong, dang su dung vat pham...'
                    return
                }
                $member.status = 'NV Tan thu: Dang mua Shop Vang Khoa (' + $j.GoldShopStatus + ')...'
                return
            }

            'use_items' {
                if (($now - $member.prepActionAt).TotalMilliseconds -ge 600) {
                    $member.prepActionAt = $now
                    try { [TGLL.NativeBackend]::ScanInventory($j.Id) } catch {}

                    $item159 = if ($j.Inventory -and $j.Inventory.Items) { $j.Inventory.Items | Where-Object { $_.TemplateId -eq 159 } | Select-Object -First 1 } else { $null }
                    $item404 = if ($j.Inventory -and $j.Inventory.Items) { $j.Inventory.Items | Where-Object { $_.TemplateId -eq 404 } | Select-Object -First 1 } else { $null }

                    if ($item159 -and !$member.prepItemsUsed.ContainsKey(159)) {
                        try { [TGLL.NativeBackend]::SendTaskPacket($j.Id, 116, [int]$item159.BagIndex, 2) } catch {}
                        $member.prepItemsUsed[159] = $true
                        $member.status = 'NV Tan thu: Dang su dung Nhan sam (thuong)...'
                        return
                    }
                    elseif ($item404 -and !$member.prepItemsUsed.ContainsKey(404)) {
                        try { [TGLL.NativeBackend]::SendTaskPacket($j.Id, 116, [int]$item404.BagIndex, 2) } catch {}
                        $member.prepItemsUsed[404] = $true
                        $member.status = 'NV Tan thu: Dang su dung Bua bao ho...'
                        return
                    }
                    else {
                        $member.prepStep = 'config_revive'
                        $member.prepActionAt = $now
                        $member.status = 'NV Tan thu: Da dung xong Nhan sam & Bua bao ho. Dang bat Dung vang hoi sinh...'
                        return
                    }
                }
                return
            }

            'config_revive' {
                if ($j.GameOptions) {
                    foreach ($opt in @($j.GameOptions)) {
                        if ($opt.Label -and ($opt.Label -match 'hồi sinh' -or $opt.Label -match 'hoi sinh')) {
                            if ($opt.Value -eq 0) {
                                try { [TGLL.NativeBackend]::SetGameOption($j.Id, [int]$opt.Code, 1) } catch {}
                            }
                        }
                    }
                }
                $member.prepDone = $true
                $member.status = 'NV Tan thu: Chuan bi xong. Bat dau lam nhiem vu!'
                return
            }
        }
        return
    }

    # 3. Cho doc du lieu nhiem vu tu bo nho game
    if ($mTask -lt 0) {
        $member.status = 'Dang doi doc nhiem vu tu game...'
        return
    }

    # 4. Lay thong tin nhiem vu hien tai
    $task = Get-NewbieTaskDef $mTask
    if ($null -eq $task) {
        if ($j.FarmEnabled) {
            try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
        }
        $member.status = 'Hoan thanh: Nhiem vu ' + $mTask + ' (Level ' + $lvl + ')'
        return
    }

    # 5. Khoi tao hoac reset tracking khi Task hoac Step thay doi
    if (!$member.PSObject.Properties['lastTaskId']) {
        $member | Add-Member NoteProperty lastTaskId -1 -Force
        $member | Add-Member NoteProperty lastStepId -999 -Force
        $member | Add-Member NoteProperty lastPacketAt ([DateTime]::MinValue) -Force
        $member | Add-Member NoteProperty currentTargetKey '' -Force
        $member | Add-Member NoteProperty waitPickupUntil ([DateTime]::MinValue) -Force
    }

    if ($member.lastTaskId -ne $mTask -or $member.lastStepId -ne $mStep) {
        $member.lastTaskId = $mTask
        $member.lastStepId = $mStep
        $member.lastPacketAt = [DateTime]::MinValue
        $member.currentTargetKey = ''
        $member.waitPickupUntil = [DateTime]::MinValue
        if ($j.FarmEnabled) {
            try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
        }
        try { [TGLL.NativeBackend]::ConfigureAutoItem($j.Id, $false, 0, 5000) } catch {}
        try { [TGLL.NativeBackend]::ScanInventory($j.Id) } catch {}
    }

    $mapNameForId = {
        param($mId)
        if ($mId -eq 75) { return 'Làng Lá' }
        if ($mId -eq 82) { return 'Rừng lá phong' }
        if ($mId -eq 83) { return 'Thung lũng Tận Cùng' }
        if ($mId -eq 86) { return 'Trường Konoha' }
        return 'Làng Lá'
    }

    $now = [DateTime]::UtcNow

    # Neu dang o NV 8 (Nhan Gia hoc vien) ma chua dat Level 10 (do server yeu cau Level 10 moi cho nhan/lam tiep NV 8):
    # Bot se tu dong di farm quai tai Thung lung Tan Cung (Khi rung) de up Cap 10 truoc!
    if ($mTask -eq 8 -and $lvl -lt 10) {
        $farmKey = 'grind_task8:83:568:730'
        if ($member.currentTargetKey -ne $farmKey -or !$j.FarmEnabled) {
            if ($j.FarmEnabled) {
                try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
            }
            $keepZone = if ($j.FarmPositionMap -eq 83 -and $j.ZoneId -ge 0) { [int]$j.ZoneId } else { -1 }
            $spot = [TGLL.FarmSpot]@{
                MapId   = 83
                MapName = (& $mapNameForId 83)
                MobId   = 157
                MobName = 'Khi rung'
                Level   = [int]$lvl
                X       = 568
                Y       = 730
                Zone    = $keepZone
            }
            try {
                [TGLL.NativeBackend]::StartMapFarm($j.Id, $false, [int]$lvl, [TGLL.FarmSpot[]]@($spot), ($keepZone -lt 0))
                $member.currentTargetKey = $farmKey
            } catch {
                $member.status = 'Loi dan duong farm: ' + $_.Exception.Message
            }
        }
        else {
            if ($j.FarmArrived -and $j.FarmPositionMap -eq 83 -and $j.FarmPositionReadAt -ne [DateTime]::MinValue) {
                $dx = [int]$j.FarmPositionX - 568
                $dy = [int]$j.FarmPositionY - 730
                $dist = [Math]::Sqrt($dx * $dx + $dy * $dy)
                if ($dist -gt 450) {
                    try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                    $member.currentTargetKey = ''
                    $member.status = 'NV 8: Lech bai khi ' + [int]$dist + 'px, dang quay ve bai 568:730...'
                    return
                }
            }
        }
        if (!$j.GameAutoWanted) {
            try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $true) } catch {}
        }
        if (($now - $member.lastPacketAt).TotalSeconds -ge 1.0) {
            $member.lastPacketAt = $now
            try {
                [TGLL.NativeBackend]::SendKeyPress($j.Id, 74)
            } catch {}
        }
        $member.status = 'NV 8: Yeu cau Cap 10, dang danh khi tai Thung lung Tan Cung de up Cap 10 (Hien tai Cap ' + $lvl + ')...'
        return
    }

    # Neu truoc do dang farm up Cap 10 ma gio da dat Cap 10: dung farm de bat dau lam NV 8
    if ($mTask -eq 8 -and $lvl -ge 10 -and $member.currentTargetKey -like 'grind_task8*') {
        if ($j.FarmEnabled) {
            try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
        }
        try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $false) } catch {}
        $member.currentTargetKey = ''
    }

    # Truong hop 1: idStep < 0 (Chua nhan nhiem vu -> Di gap NPC nhan NV)
    if ($mStep -lt 0) {
        $npcKey = 'npc_accept:' + $task.Id + ':' + $task.NpcId + ':' + $task.MapId + ':' + $task.NpcX + ':' + $task.NpcY
        $dx = [int]$j.FarmPositionX - [int]$task.NpcX
        $dy = [int]$j.FarmPositionY - [int]$task.NpcY
        $dist = [Math]::Sqrt($dx * $dx + $dy * $dy)
        $nearNpc = ($j.FarmPositionMap -eq $task.MapId) -and ($dist -le 120 -or $j.FarmArrived)

        if ($nearNpc) {
            if ($j.FarmEnabled) {
                try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
            }
            try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $false) } catch {}
            try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $false) } catch {}
            $member.currentTargetKey = $npcKey
            $member.status = 'Da den ' + $task.NpcName + ', dang nhan NV ' + $task.Name + '...'
            if (($now - $member.lastPacketAt).TotalSeconds -ge 1.5) {
                $member.lastPacketAt = $now
                try {
                    [TGLL.NativeBackend]::SendTaskPacket($j.Id, 11)
                }
                catch {}
            }
        }
        else {
            if ($member.currentTargetKey -ne $npcKey -or !$j.FarmEnabled) {
                if ($j.FarmEnabled) {
                    try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                }
                $spot = [TGLL.FarmSpot]@{
                    MapId   = [int]$task.MapId
                    MapName = (& $mapNameForId $task.MapId)
                    MobId   = -1
                    MobName = $task.NpcName
                    Level   = 1
                    X       = [int]$task.NpcX
                    Y       = [int]$task.NpcY
                    Zone    = -1
                }
                try {
                    [TGLL.NativeBackend]::StartMapFarm($j.Id, $false, 1, [TGLL.FarmSpot[]]@($spot), $false)
                    $member.currentTargetKey = $npcKey
                    $member.status = 'Di toi ' + $task.NpcName + ' de nhan: ' + $task.Name
                }
                catch {
                    $member.status = 'Loi dan duong: ' + $_.Exception.Message
                }
            }
            else {
                $member.status = 'Dang di toi ' + $task.NpcName + ' de nhan NV: ' + $task.Name
            }
        }
        return
    }

    # Truong hop 2: idStep >= Steps.Count (Da xong tat ca cac buoc -> Di gap NPC tra NV)
    if ($mStep -ge $task.Steps.Count) {
        # Neu la NV 7 (Xua duoi hau tu) ma chua dat Level 10: o lai bai khi danh tiep cho den khi dat Level 10 moi ve tra NV!
        # Dieu nay giup tiet kiem 2 lan chay di chay lai giua Lang La va Thung lung Tan Cung
        if ($task.Id -eq 7 -and $lvl -lt 10) {
            $farmKey = 'grind_task7:83:568:730'
            if ($member.currentTargetKey -ne $farmKey -or !$j.FarmEnabled) {
                if ($j.FarmEnabled) {
                    try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                }
                $keepZone = if ($j.FarmPositionMap -eq 83 -and $j.ZoneId -ge 0) { [int]$j.ZoneId } else { -1 }
                $spot = [TGLL.FarmSpot]@{
                    MapId   = 83
                    MapName = (& $mapNameForId 83)
                    MobId   = 157
                    MobName = 'Khi rung'
                    Level   = [int]$lvl
                    X       = 568
                    Y       = 730
                    Zone    = $keepZone
                }
                try {
                    [TGLL.NativeBackend]::StartMapFarm($j.Id, $false, [int]$lvl, [TGLL.FarmSpot[]]@($spot), ($keepZone -lt 0))
                    $member.currentTargetKey = $farmKey
                } catch {
                    $member.status = 'Loi dan duong: ' + $_.Exception.Message
                }
            }
            if (!$j.GameAutoWanted) {
                try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $true) } catch {}
            }
            if (($now - $member.lastPacketAt).TotalSeconds -ge 1.0) {
                $member.lastPacketAt = $now
                try {
                    [TGLL.NativeBackend]::SendKeyPress($j.Id, 74)
                } catch {}
            }
            $member.status = 'NV 7: Da xong 20 khi, tiep tuc danh khi up Cap 10 tai bai (Hien tai Cap ' + $lvl + ')...'
            return
        }

        # Khi da dat Level 10: dung farm cu de bat dau quay ve Lang La tra NV 7
        if ($task.Id -eq 7 -and $lvl -ge 10) {
            if ($member.currentTargetKey -like 'grind_task7*') {
                if ($j.FarmEnabled) {
                    try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                }
                try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $false) } catch {}
                $member.currentTargetKey = ''
            }
        }

        $npcKey = 'npc_turnin:' + $task.Id + ':' + $task.NpcId + ':' + $task.MapId + ':' + $task.NpcX + ':' + $task.NpcY
        $dx = [int]$j.FarmPositionX - [int]$task.NpcX
        $dy = [int]$j.FarmPositionY - [int]$task.NpcY
        $dist = [Math]::Sqrt($dx * $dx + $dy * $dy)
        $nearNpc = ($j.FarmPositionMap -eq $task.MapId) -and ($dist -le 120 -or $j.FarmArrived)

        # Xac dinh vat pham can nop cho nhiem vu (vi du NV 1 can nop cuu con 378, NV 2 can nop cay thuoc duoc 194)
        $turnInItemId = $null
        $requiredCount = 1
        foreach ($s in $task.Steps) {
            if ($s.DropItem) {
                $turnInItemId = [int]$s.DropItem
                if ($s.Target) { $requiredCount = [int]$s.Target }
                break
            }
        }

        # Neu la NV can nop vat pham roi tu quai: dam bao nhat du so luong tren mat dat truoc khi ve NPC
        if ($turnInItemId -and $requiredCount -gt 1) {
            $hasEnough = $false
            if ($j.Inventory -and $j.Inventory.Items) {
                $itemInBag = $j.Inventory.Items | Where-Object { $_.TemplateId -eq $turnInItemId } | Select-Object -First 1
                if ($itemInBag -and $itemInBag.Quantity -ge $requiredCount) {
                    $hasEnough = $true
                }
            }
            if (!$hasEnough) {
                # Neu chua du vat pham trong tui (dang tren mat dat vua roi ra): cho them toi da 2s de auto game nhat do
                if ($member.waitPickupUntil -eq [DateTime]::MinValue) {
                    $member.waitPickupUntil = $now.AddSeconds(2.0)
                }
                if ($now -lt $member.waitPickupUntil) {
                    try { [TGLL.NativeBackend]::ScanInventory($j.Id) } catch {}
                    $member.status = 'NV ' + $task.Id + ': Cho nhat du ' + $requiredCount + ' vat pham...'
                    return
                }
            }
        }

        if ($nearNpc) {
            if ($j.FarmEnabled) {
                try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
            }
            try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $false) } catch {}
            $member.currentTargetKey = $npcKey
            $member.status = 'Da den ' + $task.NpcName + ', dang tra NV: ' + $task.Name
            if (($now - $member.lastPacketAt).TotalSeconds -ge 1.5) {
                $member.lastPacketAt = $now
                try {
                    if ($turnInItemId) {
                        $bagSlot = -1
                        if ($j.Inventory -and $j.Inventory.Items) {
                            $itemInBag = $j.Inventory.Items | Where-Object { $_.TemplateId -eq $turnInItemId } | Select-Object -First 1
                            if ($itemInBag) { $bagSlot = [int]$itemInBag.BagIndex }
                        }
                        if ($bagSlot -ge 0) {
                            # Giao vat pham: Opcode 10 voi payloadType = 3 (slvp = 1, short indexBag = $bagSlot)
                            [TGLL.NativeBackend]::SendTaskPacket($j.Id, 10, $bagSlot, 3)
                        }
                        else {
                            # Quet lai tui do va gui packet 10
                            try { [TGLL.NativeBackend]::ScanInventory($j.Id) } catch {}
                            [TGLL.NativeBackend]::SendTaskPacket($j.Id, 10)
                        }
                    }
                    else {
                        [TGLL.NativeBackend]::SendTaskPacket($j.Id, 10)
                    }
                }
                catch {}
            }
        }
        else {
            # Khi dang di chuyen ve tra NV, chu dong quet tui do de san sang vi tri vat pham
            if ($turnInItemId -and (!$j.Inventory -or !$j.Inventory.Items)) {
                try { [TGLL.NativeBackend]::ScanInventory($j.Id) } catch {}
            }
            if ($member.currentTargetKey -ne $npcKey -or !$j.FarmEnabled) {
                if ($j.FarmEnabled) {
                    try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                }
                $spot = [TGLL.FarmSpot]@{
                    MapId   = [int]$task.MapId
                    MapName = (& $mapNameForId $task.MapId)
                    MobId   = -1
                    MobName = $task.NpcName
                    Level   = 1
                    X       = [int]$task.NpcX
                    Y       = [int]$task.NpcY
                    Zone    = -1
                }
                try {
                    [TGLL.NativeBackend]::StartMapFarm($j.Id, $false, 1, [TGLL.FarmSpot[]]@($spot), $false)
                    $member.currentTargetKey = $npcKey
                    $member.status = 'Di toi ' + $task.NpcName + ' de tra NV: ' + $task.Name
                }
                catch {
                    $member.status = 'Loi dan duong: ' + $_.Exception.Message
                }
            }
            else {
                $member.status = 'Dang di toi ' + $task.NpcName + ' de tra NV: ' + $task.Name
            }
        }
        return
    }

    # Truong hop 3: 0 <= idStep < Steps.Count (Dang thuc hien buoc idStep)
    $step = $task.Steps[$mStep]
    if ($step.Type -notin @('equip_weapon', 'use_item', 'use_item_at') -and $j.AutoItemEnabled) {
        try { [TGLL.NativeBackend]::ConfigureAutoItem($j.Id, $false, [int]$j.AutoItemId, 1000) } catch {}
    }
    switch ($step.Type) {
        'equip_weapon' {
            $member.status = 'NV ' + $task.Id + ': Mac vu khi...'
            $weaponIds = if ($step.WeaponIds) { [int[]]$step.WeaponIds } else { @([int]$step.ItemId) }
            $bagSlot = 0
            if ($j.Inventory -and $j.Inventory.Items) {
                $weaponInBag = $j.Inventory.Items | Where-Object { $_.Type -eq 1 -or $_.Equipment -or $_.TemplateId -in @(28, 29, 30, 31, 32) } | Select-Object -First 1
                if ($weaponInBag) {
                    $bagSlot = [int]$weaponInBag.BagIndex
                    $weaponIds = @([int]$weaponInBag.TemplateId) + $weaponIds
                }
            }
            else {
                try { [TGLL.NativeBackend]::ScanInventory($j.Id) } catch {}
            }
            if (($now - $member.lastPacketAt).TotalSeconds -ge 1.0) {
                $member.lastPacketAt = $now
                try {
                    # Opcode 116 (useItem) gui short indexBag len server de mac vu khi
                    [TGLL.NativeBackend]::SendTaskPacket($j.Id, 116, $bagSlot, 2)
                    [TGLL.NativeBackend]::ConfigureAutoItems($j.Id, $true, $weaponIds, 1000)
                }
                catch {}
            }
        }

        'config_auto' {
            $member.status = 'NV ' + $task.Id + ': Cau hinh tu dong...'
            if (($now - $member.lastPacketAt).TotalSeconds -ge 1.0) {
                $member.lastPacketAt = $now
                try {
                    [TGLL.NativeBackend]::SetGameAuto($j.Id, $true)
                    [TGLL.NativeBackend]::SendTaskPacket($j.Id, 122, 92, 1)
                }
                catch {}
            }
        }

        'buy_item' {
            $stepKey = 'buy:' + $step.ItemId + ':' + $step.NpcId + ':' + $step.MapId + ':' + $step.X + ':' + $step.Y
            $dx = [int]$j.FarmPositionX - [int]$step.X
            $dy = [int]$j.FarmPositionY - [int]$step.Y
            $dist = [Math]::Sqrt($dx * $dx + $dy * $dy)
            $nearNpc = ($j.FarmPositionMap -eq $step.MapId) -and ($dist -le 120 -or $j.FarmArrived)

            if ($nearNpc) {
                if ($j.FarmEnabled) {
                    try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                }
                try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $false) } catch {}
                $member.currentTargetKey = $stepKey
                $member.status = 'NV ' + $task.Id + ': Dang mua ' + $step.Name + '...'
                if (($now - $member.lastPacketAt).TotalSeconds -ge 1.5) {
                    $member.lastPacketAt = $now
                    try {
                        # Mua truc tiep mi xao qua Opcode 121 (buyId = 66, quantity = 1) ma khong mo giao dien shop 122
                        # dieu nay giup man hinh khong bi ket shop Anko va nhan vat tu do thao tac buoc tiep theo
                        $buyId = [TGLL.NativeBackend]::GetShopBuyId($j.Id, [int]$step.ItemId)
                        if ($buyId -le 0) { $buyId = 66 }
                        # Opcode 121: payloadType = 4 (writeShort(idbuy), writeShort(slbuy = 1))
                        [TGLL.NativeBackend]::SendTaskPacket($j.Id, 121, $buyId, 4, 1)
                    }
                    catch {}
                }
            }
            else {
                if ($member.currentTargetKey -ne $stepKey -or !$j.FarmEnabled) {
                    if ($j.FarmEnabled) {
                        try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                    }
                    $spot = [TGLL.FarmSpot]@{
                        MapId   = [int]$step.MapId
                        MapName = (& $mapNameForId $step.MapId)
                        MobId   = -1
                        MobName = 'NPC ' + $step.NpcId
                        Level   = 1
                        X       = [int]$step.X
                        Y       = [int]$step.Y
                        Zone    = -1
                    }
                    try {
                        [TGLL.NativeBackend]::StartMapFarm($j.Id, $false, 1, [TGLL.FarmSpot[]]@($spot), $false)
                        $member.currentTargetKey = $stepKey
                        $member.status = 'NV ' + $task.Id + ': Di toi mua ' + $step.Name
                    }
                    catch {
                        $member.status = 'Loi dan duong: ' + $_.Exception.Message
                    }
                }
                else {
                    $member.status = 'NV ' + $task.Id + ': Dang di toi mua ' + $step.Name + '...'
                }
            }
        }

        'use_item' {
            $member.status = 'NV ' + $task.Id + ': Dung item (' + $step.ItemId + ')...'
            if (!$j.AutoItemEnabled) {
                try { [TGLL.NativeBackend]::ConfigureAutoItems($j.Id, $true, @([int]$step.ItemId), 1000) } catch {}
            }
            if (($now - $member.lastPacketAt).TotalSeconds -ge 1.5) {
                $member.lastPacketAt = $now
                try {
                    [TGLL.NativeBackend]::ConfigureAutoItems($j.Id, $true, @([int]$step.ItemId), 1000)
                    # Tich hop tu dong dung thuc an trong game neu co tuy chon
                    if ($j.GameOptions) {
                        foreach ($opt in @($j.GameOptions)) {
                            if ($opt.Label -and ($opt.Label -like '*thức ăn*' -or $opt.Label -like '*thuc an*') -and $opt.Value -eq 0) {
                                try { [TGLL.NativeBackend]::SetGameOption($j.Id, [int]$opt.Code, 1) } catch {}
                            }
                        }
                    }
                }
                catch {}
            }
        }

        'talk_npc' {
            $stepKey = 'talk:' + $step.NpcId + ':' + $step.MapId + ':' + $step.X + ':' + $step.Y
            $dx = [int]$j.FarmPositionX - [int]$step.X
            $dy = [int]$j.FarmPositionY - [int]$step.Y
            $dist = [Math]::Sqrt($dx * $dx + $dy * $dy)
            $nearNpc = ($j.FarmPositionMap -eq $step.MapId) -and ($dist -le 120 -or $j.FarmArrived)

            if ($nearNpc) {
                if ($j.FarmEnabled) {
                    try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                }
                try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $false) } catch {}
                $member.currentTargetKey = $stepKey
                $member.status = 'NV ' + $task.Id + ': Dang noi chuyen (' + $step.Name + ')...'
                if (($now - $member.lastPacketAt).TotalSeconds -ge 1.2) {
                    $member.lastPacketAt = $now
                    try {
                        # Gui packet nhiem vu chinh tuyen (Opcode 11) de click tiep tuc doi thoai voi NPC
                        [TGLL.NativeBackend]::SendTaskPacket($j.Id, 11)
                    }
                    catch {}
                }
            }
            else {
                if ($member.currentTargetKey -ne $stepKey -or !$j.FarmEnabled) {
                    if ($j.FarmEnabled) {
                        try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                    }
                    $spot = [TGLL.FarmSpot]@{
                        MapId   = [int]$step.MapId
                        MapName = (& $mapNameForId $step.MapId)
                        MobId   = -1
                        MobName = 'NPC ' + $step.NpcId
                        Level   = 1
                        X       = [int]$step.X
                        Y       = [int]$step.Y
                        Zone    = -1
                    }
                    try {
                        [TGLL.NativeBackend]::StartMapFarm($j.Id, $false, 1, [TGLL.FarmSpot[]]@($spot), $false)
                        $member.currentTargetKey = $stepKey
                        $member.status = 'NV ' + $task.Id + ': Di toi noi chuyen (' + $step.Name + ')'
                    }
                    catch {
                        $member.status = 'Loi dan duong: ' + $_.Exception.Message
                    }
                }
                else {
                    $member.status = 'NV ' + $task.Id + ': Dang di toi noi chuyen...'
                }
            }
        }

        'goto_map' {
            # Khi da den dung map yeu cau (vi du Map 86 Truong Konoha)
            if ($j.FarmPositionMap -eq [int]$step.MapId) {
                if ($j.FarmEnabled) {
                    try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                }
                try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $false) } catch {}
                $member.currentTargetKey = ''
                if ($j.FarmPositionMap -eq 86) {
                    $member.status = 'Da den Truong Konoha, dang tim NPC Kinkaku...'
                    return
                }
                $member.status = 'Hoan thanh: Da den ' + $step.MapName
                return
            }

            $stepKey = 'goto:' + $step.MapId
            if ($member.currentTargetKey -ne $stepKey -or !$j.FarmEnabled) {
                if ($j.FarmEnabled) {
                    try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                }
                $spot = [TGLL.FarmSpot]@{
                    MapId   = [int]$step.MapId
                    MapName = (& $mapNameForId $step.MapId)
                    MobId   = -1
                    MobName = [string]$step.Name
                    Level   = 1
                    X       = [int]$step.X
                    Y       = [int]$step.Y
                    Zone    = -1
                }
                try {
                    [TGLL.NativeBackend]::StartMapFarm($j.Id, $false, 1, [TGLL.FarmSpot[]]@($spot), $false)
                    $member.currentTargetKey = $stepKey
                    $member.status = 'NV ' + $task.Id + ': Dang di toi ' + $step.MapName + '...'
                }
                catch {
                    $member.status = 'Loi dan duong: ' + $_.Exception.Message
                }
            }
            else {
                $member.status = 'NV ' + $task.Id + ': Dang di chuyen sang ' + $step.MapName + '...'
            }
        }

        'use_item_at' {
            $stepKey = 'item_at:' + $step.ItemId + ':' + $step.MapId + ':' + $step.X + ':' + $step.Y
            $dx = [int]$j.FarmPositionX - [int]$step.X
            $dy = [int]$j.FarmPositionY - [int]$step.Y
            $dist = [Math]::Sqrt($dx * $dx + $dy * $dy)
            $nearSpot = ($j.FarmPositionMap -eq $step.MapId) -and ($dist -le 50 -or $j.FarmArrived)

            if ($nearSpot) {
                if ($j.FarmEnabled) {
                    try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                }
                try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $false) } catch {}
                $member.currentTargetKey = $stepKey
                $member.status = 'NV ' + $task.Id + ': Dang cam co tai vi tri ' + ($mStep + 1) + '...'
                if (($now - $member.lastPacketAt).TotalSeconds -ge 1.2) {
                    $member.lastPacketAt = $now
                    try {
                        # 1. Quet lai tui do de cap nhat danh sach vat pham va vi tri o chua co (Item 380)
                        try { [TGLL.NativeBackend]::ScanInventory($j.Id) } catch {}

                        # 2. Tim o chua vat pham co trong tui do (de xu ly triet de truong hop co 2, 3 can refresh tui)
                        $flagSlot = -1
                        if ($j.Inventory -and $j.Inventory.Items) {
                            $flagItem = $j.Inventory.Items | Where-Object { $_.TemplateId -eq [int]$step.ItemId } | Select-Object -First 1
                            if ($flagItem) { $flagSlot = [int]$flagItem.BagIndex }
                        }

                        # 3. Su dung truc tiep co tu o tui qua Opcode 116 (useItem)
                        if ($flagSlot -ge 0) {
                            [TGLL.NativeBackend]::SendTaskPacket($j.Id, 116, $flagSlot, 2)
                        }

                        # 4. Gui dong thoi Opcode 11 (click nhiem vu tai toa do cam co)
                        [TGLL.NativeBackend]::SendTaskPacket($j.Id, 11)

                        # 5. Duy tri AutoItem nhu co che du phong
                        [TGLL.NativeBackend]::ConfigureAutoItems($j.Id, $true, @([int]$step.ItemId), 1000)
                    }
                    catch {}
                }
            }
            else {
                if ($member.currentTargetKey -ne $stepKey -or !$j.FarmEnabled) {
                    if ($j.FarmEnabled) {
                        try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                    }
                    $spot = [TGLL.FarmSpot]@{
                        MapId   = [int]$step.MapId
                        MapName = (& $mapNameForId $step.MapId)
                        MobId   = -1
                        MobName = 'Diem cam co'
                        Level   = 1
                        X       = [int]$step.X
                        Y       = [int]$step.Y
                        Zone    = -1
                    }
                    try {
                        [TGLL.NativeBackend]::StartMapFarm($j.Id, $false, 1, [TGLL.FarmSpot[]]@($spot), $false)
                        $member.currentTargetKey = $stepKey
                        $member.status = 'NV ' + $task.Id + ': Di toi ' + $step.Name
                    }
                    catch {
                        $member.status = 'Loi dan duong: ' + $_.Exception.Message
                    }
                }
                else {
                    $member.status = 'NV ' + $task.Id + ': Dang di toi vi tri cam co ' + ($mStep + 1) + '...'
                }
            }
        }

        'grind_level' {
            if ($lvl -lt [int]$step.TargetLevel) {
                $farmKey = 'grind:' + $step.FarmMapId + ':' + $step.X + ':' + $step.Y
                if ($member.currentTargetKey -ne $farmKey -or !$j.FarmEnabled) {
                    if ($j.FarmEnabled) {
                        try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                    }
                    $keepZone = if ($j.FarmPositionMap -eq [int]$step.FarmMapId -and $j.ZoneId -ge 0) { [int]$j.ZoneId } else { -1 }
                    $spot = [TGLL.FarmSpot]@{
                        MapId   = [int]$step.FarmMapId
                        MapName = (& $mapNameForId $step.FarmMapId)
                        MobId   = [int]$step.FarmMobId
                        MobName = $step.FarmMobName
                        Level   = [int]$lvl
                        X       = [int]$step.X
                        Y       = [int]$step.Y
                        Zone    = $keepZone
                    }
                    try {
                        [TGLL.NativeBackend]::StartMapFarm($j.Id, $false, [int]$lvl, [TGLL.FarmSpot[]]@($spot), ($keepZone -lt 0))
                        $member.currentTargetKey = $farmKey
                    } catch {
                        $member.status = 'Loi dan duong farm: ' + $_.Exception.Message
                    }
                }
                if (!$j.GameAutoWanted) {
                    try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $true) } catch {}
                }
                if (($now - $member.lastPacketAt).TotalSeconds -ge 1.0) {
                    $member.lastPacketAt = $now
                    try {
                        [TGLL.NativeBackend]::SendKeyPress($j.Id, 74)
                    } catch {}
                }
                $member.status = 'NV ' + $task.Id + ': Danh quai len Cap ' + $step.TargetLevel + ' (Hien tai Cap ' + $lvl + ')...'
            } else {
                if ($j.FarmEnabled) {
                    try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                }
                try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $false) } catch {}
                $member.currentTargetKey = ''
                $member.status = 'NV ' + $task.Id + ': Da dat Cap ' + $lvl + ', dang chuyen buoc tiep theo...'
                if (($now - $member.lastPacketAt).TotalSeconds -ge 1.5) {
                    $member.lastPacketAt = $now
                    try {
                        [TGLL.NativeBackend]::SendTaskPacket($j.Id, 11)
                    } catch {}
                }
            }
        }

        'mob' {
            # Ho tro nhieu diem toa do (vi du NV 2: cay thuoc duoc phan bo tai cac vi tri khac nhau)
            $targetX = [int]$step.X
            $targetY = [int]$step.Y
            if ($step.Spots -and $step.Spots.Count -gt 0) {
                $spotIndex = [Math]::Max(0, [int]$mReq) % $step.Spots.Count
                $targetX = [int]$step.Spots[$spotIndex].X
                $targetY = [int]$step.Spots[$spotIndex].Y
            }

            $mobKey = 'mob:' + $step.MobId + ':' + $step.MapId + ':' + $targetX + ':' + $targetY
            if ($member.currentTargetKey -ne $mobKey -or !$j.FarmEnabled) {
                if ($j.FarmEnabled) {
                    try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                }
                $keepZone = if ($j.FarmPositionMap -eq [int]$step.MapId -and $j.ZoneId -ge 0) { [int]$j.ZoneId } else { -1 }
                $spot = [TGLL.FarmSpot]@{
                    MapId   = [int]$step.MapId
                    MapName = (& $mapNameForId $step.MapId)
                    MobId   = [int]$step.MobId
                    MobName = [string]$step.MobName
                    Level   = 1
                    X       = $targetX
                    Y       = $targetY
                    Zone    = $keepZone
                }
                try {
                    [TGLL.NativeBackend]::StartMapFarm($j.Id, $false, 1, [TGLL.FarmSpot[]]@($spot), ($keepZone -lt 0))
                    $member.currentTargetKey = $mobKey
                    $member.status = 'NV ' + $task.Id + ': Di toi bai ' + $step.MobName
                }
                catch {
                    $member.status = 'Loi dan duong: ' + $_.Exception.Message
                }
            }
            else {
                if ($j.FarmArrived) {
                    if ($j.FarmPositionMap -eq $step.MapId -and $j.FarmPositionReadAt -ne [DateTime]::MinValue) {
                        $dx = [int]$j.FarmPositionX - [int]$targetX
                        $dy = [int]$j.FarmPositionY - [int]$targetY
                        $dist = [Math]::Sqrt($dx * $dx + $dy * $dy)
                        if ($dist -gt 450) {
                            try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                            $member.currentTargetKey = ''
                            $member.status = 'NV ' + $task.Id + ': Lech bai ' + [int]$dist + 'px, dang quay ve bai ' + $step.MobName + '...'
                            return
                        }
                    }

                    $progressStr = ''
                    if ($mReq -ge 0 -and $step.Target) {
                        $progressStr = ' (' + $mReq + '/' + $step.Target + ')'
                    }
                    $member.status = 'NV ' + $task.Id + ': Dang danh ' + $step.MobName + $progressStr + ' (Lv ' + $j.CharacterLevel + ')'
                    if (!$j.GameAutoWanted) {
                        try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $true) } catch {}
                    }

                    # Bam tan cong 'J' (74 = VK_J) dinh ky moi 1s de nhan vat danh/tuong tac lien tuc voi cay/quai
                    if (($now - $member.lastPacketAt).TotalSeconds -ge 1.0) {
                        $member.lastPacketAt = $now
                        try {
                            [TGLL.NativeBackend]::SendKeyPress($j.Id, 74)
                        }
                        catch {}
                    }
                }
                else {
                    $member.status = 'NV ' + $task.Id + ': Dang di toi bai ' + $step.MobName
                }
            }
        }

        default {
            $member.status = 'NV ' + $task.Id + ': Dang thuc hien buoc ' + $mStep
        }
    }
}

function Add-NewbieQueue($accounts) {
    if (!$autoLoginCheck.Checked) { throw 'Bat Dang nhap truc tiep truoc khi chay hang cho tan thu.' }
    $known = @{}
    foreach ($b in $script:newbieBatches) { foreach ($m in $b.members) { $known[$m.id] = $true } }
    $fresh = @($accounts | Where-Object { !$known.ContainsKey([string]$_.id) })
    if (!$fresh.Count) { $status.Text = 'Cac tai khoan da co trong hang cho tan thu.'; return }
    
    $tail = $null
    foreach ($a in $fresh) {
        $a | Add-Member NoteProperty autoNewbie $true -Force
        if ($null -eq $tail -or $tail.members.Count -ge 1) {
            $tail = [pscustomobject]@{ members = @(); state = 'waiting'; error = ''; closeOperation = $null }
            $script:newbieBatches += @($tail)
        }
        $tail.members += @([pscustomobject]@{ id = [string]$a.id; job = ''; status = 'Dang cho...'; currentTargetKey = '' })
    }
    if (!$UiTest) { Save-Accounts }
    $script:newbieQueuePaused = $false
    Pump-NewbieQueue
}

function Reset-NewbieQueue {
    $script:newbieQueuePaused = $true
    foreach ($b in $script:newbieBatches) {
        foreach ($m in $b.members) {
            if ($m.job) {
                $j = Get-AccountJob $m.job
                if ($j -and $j.FarmEnabled) {
                    try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {}
                }
            }
        }
    }
    $script:newbieBatches = @()
    $script:newbieWaiting = @{}
}

function Pump-NewbieQueue {
    if ($script:newbieQueuePaused -or !$script:newbieBatches.Count) { return }
    $script:newbieAccountIndex = @{}
    foreach ($a in $script:accounts) { $script:newbieAccountIndex[[string]$a.id] = $a }
    
    foreach ($batch in $script:newbieBatches) {
        if ($batch.state -eq 'done') { continue }
        
        if ($batch.state -eq 'running') {
            $allComplete = $true
            foreach ($m in $batch.members) {
                if ($m.status -like 'Hoan thanh*') {
                    continue
                }
                $j = Get-NewbieMemberJob $m
                if ($null -ne $j) {
                    Drive-NewbieMember $m $j
                    if ($m.status -notlike 'Hoan thanh*') {
                        $allComplete = $false
                    }
                }
                else {
                    $allComplete = $false
                }
            }
            if ($allComplete) {
                $batch.state = 'finished'
            }
        }

        # Xu ly dong game sau khi batch hoan thanh
        if ($batch.state -eq 'finished') {
            foreach ($m in $batch.members) {
                $j = Get-NewbieMemberJob $m
                if ($j) {
                    if ($j.FarmEnabled) { try { [TGLL.NativeBackend]::StopMapFarm($j.Id) } catch {} }
                    try { [TGLL.NativeBackend]::SetGameAuto($j.Id, $false) } catch {}
                }
            }

            $shouldClose = $true
            if ($newbieCloseAfter -and ($newbieCloseAfter.Checked -eq $false)) {
                $shouldClose = $false
            }

            if ($shouldClose) {
                $jobsToClose = @($batch.members | ForEach-Object { $_.job } | Where-Object { $_ })
                if ($jobsToClose.Count -gt 0) {
                    try {
                        $batch.closeOperation = [TGLL.NativeBackend]::BeginCloseBatch([string[]]$jobsToClose)
                        $batch.state = 'closing'
                        $batch.closingAt = [DateTime]::UtcNow
                    } catch {
                        $batch.state = 'done'
                    }
                } else {
                    $batch.state = 'done'
                }
            } else {
                $batch.state = 'done'
            }
        }

        # Kiem tra tien trinh dong game cho cac batch dang 'closing'
        if ($batch.state -eq 'closing') {
            $allClosed = $true
            if ($batch.closeOperation -and !$batch.closeOperation.Completed) {
                $allClosed = $false
            }
            foreach ($m in $batch.members) {
                $j = Get-NewbieMemberJob $m
                if ($null -ne $j -and $j.ProcessId -gt 0 -and [TGLL.ManagedGames]::IsRunning([int]$j.ProcessId)) {
                    $allClosed = $false
                }
            }
            if (!$batch.closingAt) { $batch.closingAt = [DateTime]::UtcNow }
            if (([DateTime]::UtcNow - $batch.closingAt).TotalSeconds -gt 15) {
                $allClosed = $true
            }
            if ($allClosed) {
                $batch.state = 'done'
                foreach ($m in $batch.members) {
                    if ($m.status -like 'Hoan thanh*' -and $m.status -notlike '*(Da tat game)*') {
                        $m.status = $m.status + ' (Da tat game)'
                    }
                }
            }
        }
    }
    
    # Tu dong phuc hoi / retry cac batch bi blocked sau 25 giay cooldown
    $now = [DateTime]::UtcNow
    foreach ($b in $script:newbieBatches) {
        if ($b.state -eq 'blocked') {
            if (!$b.blockedAt) { $b.blockedAt = $now }
            if (($now - $b.blockedAt).TotalSeconds -ge 25) {
                $b.state = 'waiting'
                $b.error = ''
                $b.blockedAt = $null
                foreach ($m in $b.members) {
                    if ($m.status -notlike 'Hoan thanh*') {
                        $m.status = 'Dang cho thu lai...'
                    }
                }
            }
        }
    }

    # Chi tinh cac batch thuc su dang chay hoac dang dong, khong de batch loi chiem slot
    $active = 0
    foreach ($b in $script:newbieBatches) { 
        if ($b.state -in @('launching', 'running', 'closing')) { 
            $active += $b.members.Count 
        } 
    }
    $limit = [int]$newbieMax.Value
    
    foreach ($batch in $script:newbieBatches) {
        if ($batch.state -ne 'waiting') { continue }
        if ($active + $batch.members.Count -gt $limit) { continue }
        $batch.state = 'launching'
        $active += $batch.members.Count
        foreach ($m in $batch.members) { $script:newbieWaiting.Remove($m.id) }
    }
    
    $concurrencyBudget = if ($launchConcurrency) { [Math]::Max(1, [int]$launchConcurrency.Value) } else { 1 }
    for ($launchIndex = 0; $launchIndex -lt $concurrencyBudget; $launchIndex++) {
        $submitted = $false
        foreach ($batch in $script:newbieBatches) {
            if ($batch.state -ne 'launching') { continue }
            $member = @($batch.members | Where-Object { !$_.job }) | Select-Object -First 1
            if ($member) {
                $submitted = $true
                try { 
                    Start-NewbieMember $member 
                } catch { 
                    $batch.state = 'blocked'
                    $batch.error = [string]$_.Exception.Message
                    $batch.blockedAt = [DateTime]::UtcNow
                    foreach ($m in $batch.members) {
                        $m.status = 'Loi mo: ' + $_.Exception.Message
                    }
                }
                break
            }
            $batch.state = 'running'
        }
        if (!$submitted) { break }
    }
    Update-NewbieQueueStatus
}

function Update-NewbieQueueStatus {
    $script:newbieWaiting = @{}
    foreach ($b in $script:newbieBatches) { if ($b.state -eq 'waiting') { foreach ($m in $b.members) { $script:newbieWaiting[$m.id] = $m } } }
    $active = 0; $waiting = 0; $done = 0
    foreach ($b in $script:newbieBatches) {
        if ($b.state -eq 'waiting') { $waiting += $b.members.Count }
        elseif ($b.state -eq 'done') { $done += $b.members.Count }
        else { $active += $b.members.Count }
    }
    if ($newbieQueueHint) {
        $newbieQueueHint.Text = 'Dang giu luot: ' + $active + ' / ' + [int]$newbieMax.Value + ' - Hang cho: ' + $waiting + ' - Da dat Lv10: ' + $done
    }
}
