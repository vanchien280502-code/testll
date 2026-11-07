@echo off
cd /d "%~dp0"
if exist "%~dp0QLTK.exe" (
  start "" "%~dp0QLTK.exe"
  exit /b
)
powershell.exe -NoProfile -ExecutionPolicy Bypass -Command "Get-ChildItem -LiteralPath '%~dp0QLTK' -Filter '*.ps1' | ForEach-Object { $b=[System.IO.File]::ReadAllBytes($_.FullName); if ($b.Length -ge 3 -and ($b[0] -ne 0xEF -or $b[1] -ne 0xBB -or $b[2] -ne 0xBF)) { [System.IO.File]::WriteAllBytes($_.FullName, [byte[]]@(0xEF,0xBB,0xBF) + $b) } }"
powershell.exe -NoProfile -STA -ExecutionPolicy Bypass -File "%~dp0QLTK\AccountManager.ps1"
if errorlevel 1 pause
