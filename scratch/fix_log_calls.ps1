$path = 'QLTK/NativeBackend.cs'
$text = [System.IO.File]::ReadAllText($path, [System.Text.Encoding]::UTF8)

$text = $text.Replace('Log(j,"lucky_wheel_stopped_no_tickets",","\"spun\":"+j.LuckyWheelSpunCount);', 'Log(j,"lucky_wheel_stopped_no_tickets",",\"spun\":"+j.LuckyWheelSpunCount);')
$text = $text.Replace('Log(j,"lucky_wheel_menu_selected",","\"index\":"+targetIdx);', 'Log(j,"lucky_wheel_menu_selected",",\"index\":"+targetIdx);')
$text = $text.Replace('Log(j,"lucky_wheel_spun",","\"type\":"+j.LuckyWheelType+","\"batch\":"+batch+","\"sub_opcode\":"+subOpcode+","\"remaining_tickets\":"+ticketCount);', 'Log(j,"lucky_wheel_spun",",\"type\":"+j.LuckyWheelType+",\"batch\":"+batch+",\"sub_opcode\":"+subOpcode+",\"remaining_tickets\":"+ticketCount);')

[System.IO.File]::WriteAllText($path, $text, [System.Text.UTF8Encoding]::new($true))
Write-Host "Updated Log calls successfully."

