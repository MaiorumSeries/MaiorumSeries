$location = Get-Location

Set-Location $PSScriptRoot

$releaseTag = "v1.0.1"
$releaseNotes = "Working on first release"

$sourceLocation = "$PSScriptRoot\..\src\gc2book\bin\Release\net6.0"

&gh auth status

if ($?)
{
     #&dir "$sourceLocation\*.*"
     $files = Get-ChildItem -Path "$sourceLocation" -Filter "*.*" -File
     gh release create $releaseTag -t $releaseTag -n "$releaseNotes"
     foreach ($file in $files)
     {
          Write-Host $file
          gh release upload $releaseTag "$sourceLocation\$file"
     }

}
else {
     Write-Error "gh is not logged in Github. Perform gh auth login."   
     Exit 1
}

Set-Location $location