
$location = Get-Location

Set-Location $PSScriptRoot

if (-Not ($env:CHOCOAPIKEY))
{
    Write-Error "ERROR: Environment variable CHOCOAPIKEY not set, providing the API Key for chocolatey site"
    Exit 3
}


$files = Get-ChildItem "$PSScriptRoot\maiorumseries\*.nupkg"

[Int32]$count = 0

foreach ($file in $files)
{
    $count = $count + 1
}


if ($count -ne 1)
{
    Write-Error "ERROR: There is not exactly one nuget package in the folder"
    Exit 4
}

foreach ($file in $files)
{
    Write-Host "Push chocolatey package $file"
    & choco push $file --api-key $env:CHOCOAPIKEY
}

Set-Location $location