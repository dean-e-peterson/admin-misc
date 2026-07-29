$Process = Get-Process -Name explorer
Write-Host $Process
Write-Host $Process.Handle
