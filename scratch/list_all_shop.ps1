$json = Get-Content Data/shop_gold.json -Raw | ConvertFrom-Json
foreach ($item in $json) {
    Write-Output ("TemplateId: " + $item.TemplateId + ", SaleId: " + $item.SaleId + ", Name: " + $item.Name + ", Price: " + $item.GoldPrice)
}

