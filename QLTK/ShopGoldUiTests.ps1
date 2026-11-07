if($null -eq $shopGoldPage -or $shopGoldPage.Text -notlike '*Auto Shop*'){throw "ShopGold tab missing: $($shopGoldPage.Text)"}
if($shopGoldGrid.Rows.Count -lt 25){throw 'ShopGold grid rows not populated'}
if($shopGoldSilver.Value -lt 0){throw 'ShopGold silver value invalid'}
if($null -eq $shopGoldCodes){throw 'ShopGold codes box missing'}
if($null -eq $shopGoldReload -or $null -eq $shopGoldAddManual -or $null -eq $shopGoldOpenJson -or $null -eq $shopGoldReloadJson){throw 'ShopGold scan/json buttons missing'}

# Test item selection
$shopGoldGrid.Rows[0].Cells['pick'].Value=$true
$shopGoldGrid.Rows[0].Cells['quantity'].Value=5
$selectedItems=@(Get-ShopGoldSelectedItems)
if($selectedItems.Count -ne 1 -or $selectedItems[0].quantity -ne 5){throw 'Get-ShopGoldSelectedItems failed'}
$shopGoldGrid.Rows[0].Cells['pick'].Value=$false

# Test Start-ShopGold validation
$err=''
try{Start-ShopGold}catch{$err=$_.Exception.Message}
if($err -notmatch 'acc clone|váº­t pháº©m|vat pham'){throw "Unexpected Start-ShopGold validation: $err"}

# Test Stop-ShopGold
Stop-ShopGold
if($script:shopGoldRun -and $script:shopGoldRun.active){throw 'Stop-ShopGold failed'}

# Test Catalog persistence functions
$catalog = Get-ShopGoldCatalog
if($catalog.Count -lt 25){throw 'Get-ShopGoldCatalog returned insufficient items'}

Write-Output 'PASS: Auto Shop Vang unit tests.'