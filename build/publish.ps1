$ErrorActionPreference = "Stop"

$logging_color = "DarkCyan"
$root = Resolve-Path "$PSScriptRoot/.."
$dist = "$root/dist/bin"

Write-Host "Cleaning dist ..." -ForegroundColor $logging_color
Remove-Item $dist -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Path $dist | Out-Null

Write-Host "Publishing Api ..." -ForegroundColor $logging_color
dotnet publish "$root/src/Api/Api.csproj" `
  -c Release `
  -r win-x64 `
  --self-contained true `
  -o $dist

Write-Host "Publishing UI ..." -ForegroundColor $logging_color
dotnet publish "$root/src/UI/UI.csproj" `
  -c Release `
  -r win-x64 `
  --self-contained true `
  -o $dist

Write-Host "Publish finished." -ForegroundColor $logging_color