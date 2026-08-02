# $Process = Get-Process -Name explorer
# Write-Host $Process
# Write-Host $Process.Handle

$shell = New-Object -ComObject Shell.Application
# $shell | Get-Member

$windowList = $shell.Windows()
Write-Output $windowList
foreach ($window in $windowList)
{
    Write-Output "In loop"
    $window | Get-Member
    Write-Output $window.Name
    break
}

