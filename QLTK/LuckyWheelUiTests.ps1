$lwA=$script:accounts[0];$lwKey=[string]$lwA.id;$lwHad=$script:accountJobs.ContainsKey($lwKey);$lwOld=$script:accountJobs[$lwKey]
$lwJobs=[TGLL.NativeBackend].GetField('jobs',[Reflection.BindingFlags]'Static,NonPublic').GetValue($null)
$lwCtor=[TGLL.NativeJob].GetConstructors([Reflection.BindingFlags]'Instance,NonPublic')[0]
$lwJ=$lwCtor.Invoke(@('C:\synthetic.exe','wheel-test','unused',$null,$true))
$lwJ.State='opened';$lwJ.GameSettingsReady=$true;$lwJ.TrainReady=$true
$null=$lwJobs.TryAdd($lwJ.Id,$lwJ);$script:accountJobs[$lwKey]=$lwJ.Id
try{
    $grid.ClearSelection();$grid.Rows[0].Selected=$true;$grid.Rows[0].Cells['pick'].Value=$true
    $lwP=Get-LuckyWheelPreference $null;if($lwP.interval -ne 1200 -or $lwP.count -ne 0){throw 'Wheel defaults invalid'}
    $wheelInterval.Value=1500;$wheelCount.Value=50;Save-LuckyWheelUi;Update-LuckyWheelUi
    if($lwA.luckyWheelInterval -ne 1500 -or $lwA.luckyWheelCount -ne 50 -or $lwJ.LuckyWheelEnabled){throw 'Preferences must not start auto-spin'}
    Start-LuckyWheelSelected
    if(!$lwJ.LuckyWheelEnabled -or $lwJ.LuckyWheelIntervalMs -ne 1500 -or $lwJ.LuckyWheelTargetCount -ne 50){throw 'Selected spin start failed'}
    Stop-LuckyWheelSelected;if($lwJ.LuckyWheelEnabled){throw 'Stop failed'}
    $lwJ.TrainReady=$false;Start-LuckyWheelSelected
    if($lwJ.LuckyWheelEnabled){throw 'Not-ready account must not start'}
    if(!$wheelPage.AutoScroll -or $tabs.TabPages.Count -notin @(26,27,28,29,30)){throw 'Wheel page layout invalid'}
    Write-Output 'PASS: lucky wheel UI interval, count, saved preferences without auto-start, selected start/stop and readiness gate.'
}finally{
    $lwRemoved=$null;$null=$lwJobs.TryRemove($lwJ.Id,[ref]$lwRemoved)
    if($lwHad){$script:accountJobs[$lwKey]=$lwOld}else{$script:accountJobs.Remove($lwKey)}
}
