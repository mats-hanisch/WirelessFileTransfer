$ErrorActionPreference = "Stop"

$exeName = "Api.exe"
$exePath = Join-Path "$PSScriptRoot/bin" $exeName

$desktop = [Environment]::GetFolderPath("Desktop")
$linkName = "Dateien Hochladen.lnk"
$linkPath = Join-Path $desktop $linkName

$shell = New-Object -ComObject WScript.Shell
$shortcut = $shell.CreateShortcut($linkPath)
$shortcut.TargetPath = $exePath
$shortcut.WorkingDirectory = "$PSScriptRoot/bin"
$shortcut.Save()

Write-Host "Verknüpfung auf dem Desktop erstellt: $linkName"
