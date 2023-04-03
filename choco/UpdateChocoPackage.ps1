
$location = Get-Location

Set-Location $PSScriptRoot

Copy-Item -Path "$PSScriptRoot\..\src\gc2book\bin\Release\net6.0\*.*" -Destination "$PSScriptRoot\maiorumseries\bin" -Recurse

Set-Location "$PSScriptRoot\maiorumseries"

choco pack 

Set-Location $location