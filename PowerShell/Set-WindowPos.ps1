# $Process = Get-Process -Name explorer
# Write-Host $Process
# Write-Host $Process.Handle


# $shell = New-Object -ComObject Shell.Application
# # $shell | Get-Member
#
# $windowList = $shell.Windows()
# Write-Output $windowList.Count
# foreach ($window in $windowList)
# {
#     Write-Output "In loop"
#     # $window | Get-Member
#     Write-Output $window.Name
# }

# Borrowed from https://github.com/microsoft/PowerToys/blob/d2c53bf3861ed2688a1c30aafd66ea0fc0186399/.github/skills/powertoys-verification/scripts/pt-explorer-com.ps1#L14
function Get-PtExplorerWindows {
    <#
    .SYNOPSIS
    Return all open Explorer windows as Shell COM objects (with .LocationName, .Document.Folder, etc.).
    Returns @() if no Explorer windows are open.
    #>
    try {
        $shell = New-Object -ComObject Shell.Application
        return @($shell.Windows() | Where-Object { $_.Name -eq 'File Explorer' -or $_.FullName -match 'explorer\.exe$' })
    } catch { return @() }
}

$windows = Get-PtExplorerWindows
Write-Output $windows | Format-List
