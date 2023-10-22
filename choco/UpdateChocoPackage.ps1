
$location = Get-Location

Set-Location $PSScriptRoot

$releaseTag = "v1.0.1"
$sourceLocation = "$PSScriptRoot\..\src\gc2book\bin\Release\net6.0"

&gh auth status

if (-not $?)
{
     Write-Error "gh is not logged in Github. Perform gh auth login."   
     Exit 1
}


if (-not [System.IO.Directory]::Exists("$PSScriptRoot\maiorumseries\bin"))
{
    [System.IO.Directory]::CreateDirectory("$PSScriptRoot\maiorumseries\bin")
}

Set-Location "$PSScriptRoot\maiorumseries\bin"

&gh release download $releaseTag --clobber

#Copy-Item -Path "$PSScriptRoot\..\src\gc2book\bin\Release\net6.0\*.*" -Destination "$PSScriptRoot\maiorumseries\bin" -Recurse
Copy-Item -Path "$PSScriptRoot\VERIFICATION.txt" -Destination "$PSScriptRoot\maiorumseries\tools\VERIFICATION.txt" 

Set-Location "$PSScriptRoot\maiorumseries\bin"

$files = Get-ChildItem -Path "$PSScriptRoot\maiorumseries\bin" -Filter "*.*" -File
foreach ($file in $files)
{
     $hash = Get-FileHash $file
     Add-Content -Path "$PSScriptRoot\maiorumseries\tools\VERIFICATION.txt" -Value $hash.Hash -NoNewline
     Add-Content -Path "$PSScriptRoot\maiorumseries\tools\VERIFICATION.txt" -Value " " -NoNewline
     Add-Content -Path "$PSScriptRoot\maiorumseries\tools\VERIFICATION.txt" -Value $file 
}

#Get-FileHash *.*  | Out-String | Add-Content -Path "$PSScriptRoot\maiorumseries\tools\VERIFICATION.txt" 

if (-not [System.IO.Directory]::Exists("$PSScriptRoot\maiorumseries\docs"))
{
    [System.IO.Directory]::CreateDirectory("$PSScriptRoot\maiorumseries\docs")
}
Copy-Item -Path "$PSScriptRoot\..\ChangeLog.md" -Destination "$PSScriptRoot\maiorumseries\docs\ChangeLog.md"  -Force 
Copy-Item -Path "$PSScriptRoot\..\README.md" -Destination "$PSScriptRoot\maiorumseries\docs\README.md"  -Force 

Set-Location "$PSScriptRoot\maiorumseries"

[xml]$nuspec = Get-Content "$PSScriptRoot\maiorumseries\maiorumseries.nuspec"   
$changeLog =  Get-Content  "$PSScriptRoot\maiorumseries\docs\ChangeLog.md" | Out-String

$nuspec.package.metadata.releaseNotes = $changeLog.ToString()
$nuspec.Save("$PSScriptRoot\maiorumseries\maiorumseries.nuspec")

choco pack 

Set-Location $location