
$location = Get-Location

Set-Location $PSScriptRoot


if (-not [System.IO.Directory]::Exists("$PSScriptRoot\maiorumseries\bin"))
{
    [System.IO.Directory]::CreateDirectory("$PSScriptRoot\maiorumseries\bin")
}

Copy-Item -Path "$PSScriptRoot\..\src\gc2book\bin\Release\net6.0\*.*" -Destination "$PSScriptRoot\maiorumseries\bin" -Recurse

if (-not [System.IO.Directory]::Exists("$PSScriptRoot\maiorumseries\docs"))
{
    [System.IO.Directory]::CreateDirectory("$PSScriptRoot\maiorumseries\docs")
}
Copy-Item -Path "$PSScriptRoot\..\ChangeLog.md" -Destination "$PSScriptRoot\maiorumseries\docs\ChangeLog.md"  -Force 

Set-Location "$PSScriptRoot\maiorumseries"

[xml]$nuspec = Get-Content "$PSScriptRoot\maiorumseries\maiorumseries.nuspec"   
$changeLog =  Get-Content  "$PSScriptRoot\maiorumseries\docs\ChangeLog.md" | Out-String

$nuspec.package.metadata.releaseNotes = $changeLog.ToString()
$nuspec.Save("$PSScriptRoot\maiorumseries\maiorumseries.nuspec")
#Set-Content $nuspec -Path "$PSScriptRoot\maiorumseries\maiorumseries.nuspec"

choco pack 

Set-Location $location